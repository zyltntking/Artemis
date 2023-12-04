using System.ComponentModel.DataAnnotations;
using Artemis.App.BroadcastApi.Data;
using Artemis.Data.Core;
using Artemis.Data.Store.Extensions;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Artemis.App.BroadcastApi.Controllers;

/// <summary>
///     �㲥Api
/// </summary>
[ApiController]
[Route("[controller]/[action]")]
public class BroadcastController : ControllerBase
{
    private readonly BroadcastContext _context;

    private readonly ILogger<BroadcastController> _logger;

    /// <summary>
    ///     ���캯��
    /// </summary>
    /// <param name="logger">��־����</param>
    /// <param name="context">����������</param>
    public BroadcastController(
        ILogger<BroadcastController> logger,
        BroadcastContext context)
    {
        _logger = logger;
        _context = context;
    }

    /// <summary>
    ///     ��¼
    /// </summary>
    /// <param name="request">����</param>
    /// <returns>��Ӧ</returns>
    [HttpPost]
    public DataResult<string> SignIn([FromBody] SignInRequest request)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserName == request.UserName);

        return DataResult.Success("token", "�ɹ�");
    }

    /// <summary>
    ///     ��ѯ������Ϣ
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<DataResult<PageResult<OrderInfo>>> FetchOrderInfos([FromBody] FetchOrderInfosRequest request)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var filter = request.Filter;

        var licenseSearch = filter.LicenseSearch ?? string.Empty;

        var statusMatch = filter.StatusMatch.ToUpper();

        var priceSearch = filter.PriceSearch;

        var mealTime = filter.MealTime ?? DateTime.Today;

        var total = await _context.Orders.AsNoTracking().CountAsync(cancellationToken);

        var query = _context.Orders.AsNoTracking()
            .WhereIf(licenseSearch != string.Empty,
                order => EF.Functions.Like(order.License, $"%{licenseSearch}%"))
            .WhereIf(statusMatch != OrderStatus.All.ToUpper(),
                order => order.Status == statusMatch)
            .Where(order => order.Price >= priceSearch)
            .Where(order => order.MealTime >= mealTime);

        var count = await query.CountAsync(cancellationToken);

        var orderInfoList = await query.OrderBy(order => order.MealTime)
            .Skip(request.Skip)
            .Take(request.Size)
            .ProjectToType<OrderInfo>()
            .ToListAsync(cancellationToken);

        var result = new PageResult<OrderInfo>
        {
            Page = request.Page,
            Size = request.Size,
            Total = total,
            Count = count,
            Data = orderInfoList
        };

        return DataResult.Success(result);
    }

    /// <summary>
    ///     ��������
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<DataResult<OrderInfo>> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var order = request.Adapt<Order>();
        order.License = request.License.ToUpper();

        var now = DateTime.Now;
        order.CreatedAt = now;
        order.UpdatedAt = now;

        await _context.Orders.AddAsync(order, cancellationToken);

        var result = await _context.SaveChangesAsync(cancellationToken);

        if (result > 0) return DataResult.Success(order.Adapt<OrderInfo>());

        return DataResult.Fail<OrderInfo>("����ʧ��");
    }

    /// <summary>
    ///     ���¶���
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<DataResult<OrderInfo>> UpdateOrder([FromBody] UpdateOrderRequest request)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var order = await _context.Orders
            .Where(item => item.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (order == null) return DataResult.Fail<OrderInfo>("����������");

        request.Adapt(order);
        order.License = request.License.ToUpper();

        order.UpdatedAt = DateTime.Now;

        _context.Orders.Update(order);

        var result = await _context.SaveChangesAsync(cancellationToken);

        if (result > 0) return DataResult.Success(order.Adapt<OrderInfo>());

        return DataResult.Fail<OrderInfo>("����ʧ��");
    }

    /// <summary>
    ///     ɾ������
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<DataResult<EmptyRecord>> DeleteOrder([FromBody] DeleteOrderRequest request)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var order = await _context.Orders
            .Where(item => item.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (order == null) return DataResult.Fail<EmptyRecord>("����������");

        _context.Orders.Remove(order);

        var result = await _context.SaveChangesAsync(cancellationToken);

        if (result > 0)
        {
            var record = new EmptyRecord();

            return DataResult.Success(record);
        }

        return DataResult.Fail<EmptyRecord>("ɾ��ʧ��");
    }
}

/// <summary>
///     ��¼����
/// </summary>
public record SignInRequest
{
    /// <summary>
    ///     �û���
    /// </summary>
    [Required]
    public required string UserName { get; set; }

    /// <summary>
    ///     ����
    /// </summary>
    [Required]
    public required string Password { get; set; }
}

/// <summary>
///     ������Ϣ������
/// </summary>
public record OrderInfoFilter
{
    /// <summary>
    ///     ���ƺ�
    /// </summary>
    public string? LicenseSearch { get; set; }

    /// <summary>
    ///     �۸�
    /// </summary>
    [Required]
    public required string StatusMatch { get; set; }

    /// <summary>
    ///     �۸�
    /// </summary>
    [Required]
    public required double PriceSearch { get; set; }

    /// <summary>
    ///     �ò�ʱ��
    /// </summary>
    public DateTime? MealTime { get; set; }
}

/// <summary>
///     ��ѯ������Ϣ����
/// </summary>
public record FetchOrderInfosRequest : PageRequest<OrderInfoFilter>;

/// <summary>
///     ������������
/// </summary>
public record CreateOrderRequest : OrderInfo;

/// <summary>
///     ���¶�������
/// </summary>
public record UpdateOrderRequest : OrderInfo
{
    /// <summary>
    ///     ��ʶ
    /// </summary>
    public Guid Id { get; set; }
}

/// <summary>
///     ɾ����������
/// </summary>
public record DeleteOrderRequest
{
    /// <summary>
    ///     ��ʶ
    /// </summary>
    public Guid Id { get; set; }
}

/// <summary>
///     ������Ϣ
/// </summary>
public record OrderInfo : IOrder
{
    /// <summary>
    ///     ���ƺ�
    /// </summary>
    [Required]
    public required string License { get; set; } = null!;

    /// <summary>
    ///     ����
    /// </summary>
    [Required]
    public required int Count { get; set; }

    /// <summary>
    ///     �۸�
    /// </summary>
    [Required]
    public required double Price { get; set; }

    /// <summary>
    ///     �ò�ʱ��
    /// </summary>
    [Required]
    public required DateTime MealTime { get; set; } = DateTime.Now;

    /// <summary>
    ///     ״̬
    /// </summary>
    [Required]
    public required string Status { get; set; } = null!;

    /// <summary>
    ///     ��ע
    /// </summary>
    public string? Remark { get; set; }
}