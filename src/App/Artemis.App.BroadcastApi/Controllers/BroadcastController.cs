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

        if (user == null) return DataResult.Fail<string>("�û�������");

        return DataResult.Success("token", "�ɹ�");
    }

    /// <summary>
    ///     ��ѯ������Ϣ
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<DataResult<PageResult<OrderData>>> FetchOrderInfos([FromBody] FetchOrderInfosRequest request)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var filter = request.Filter;

        var licenseSearch = filter.LicenseSearch ?? string.Empty;

        var statusMatch = filter.StatusMatch ?? OrderStatus.All;

        var mealDateMatch = filter.MealDateMatch ?? DateTime.Today.ToString("yyyy-MM-dd");

        var query = _context.Orders.AsNoTracking();

        var total = await query.CountAsync(cancellationToken);

        query = query.WhereIf(licenseSearch != string.Empty,
                order => EF.Functions.Like(
                    order.License,
                    $"%{licenseSearch}%"))
            .WhereIf(statusMatch != OrderStatus.All,
                order => order.Status == statusMatch)
            .Where(order => order.MealDate == mealDateMatch);

        var count = await query.CountAsync(cancellationToken);

        var orderInfoList = await query.OrderBy(order => order.CreatedAt)
            .Skip(request.Skip)
            .Take(request.Size)
            .ProjectToType<OrderData>()
            .ToListAsync(cancellationToken);

        var result = new PageResult<OrderData>
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
    public async Task<DataResult<OrderData>> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var order = request.Adapt<Order>();
        order.License = request.License.ToUpper();

        var now = DateTime.Now;
        order.CreatedAt = now;
        order.UpdatedAt = now;

        await _context.Orders.AddAsync(order, cancellationToken);

        var result = await _context.SaveChangesAsync(cancellationToken);

        if (result > 0) return DataResult.Success(order.Adapt<OrderData>());

        return DataResult.Fail<OrderData>("����ʧ��");
    }

    /// <summary>
    ///     ���¶���
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<DataResult<OrderData>> UpdateOrder([FromBody] UpdateOrderRequest request)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var order = await _context.Orders
            .Where(item => item.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (order == null) return DataResult.Fail<OrderData>("����������");

        request.Adapt(order);
        order.License = request.License.ToUpper();

        order.UpdatedAt = DateTime.Now;

        _context.Orders.Update(order);

        var result = await _context.SaveChangesAsync(cancellationToken);

        if (result > 0) return DataResult.Success(order.Adapt<OrderData>());

        return DataResult.Fail<OrderData>("����ʧ��");
    }

    /// <summary>
    ///     �޸Ķ���״̬
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<DataResult<OrderData>> ChangeOrderStatus([FromBody] ChangeOrderStatusRequest request)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var order = await _context.Orders
            .Where(item => item.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (order == null)
            return DataResult.Fail<OrderData>("����������");

        order.Status = request.Status;

        _context.Orders.Update(order);

        var result = await _context.SaveChangesAsync(cancellationToken);

        if (result > 0)
            return DataResult.Success(order.Adapt<OrderData>());

        return DataResult.Fail<OrderData>("����ʧ��");
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

    /// <summary>
    ///     ����ͳ��
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<DataResult<OrderStatisticsResult>> OrderStatistics([FromBody] OrderStatisticsRequest request)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var mealDateMatch = request.MealDate ?? DateTime.Today.ToString("yyyy-MM-dd");

        var totalItems = await _context.Orders.AsNoTracking()
            .Where(order => order.MealDate == mealDateMatch)
            .Select(order => new
            {
                order.Count,
                order.Status
            })
            .ToListAsync(cancellationToken);


        var totalNumber = totalItems.Count;

        var totalCount = totalItems
            .Sum(item => item.Count);

        var completeNumber = totalItems
            .Count(item => item.Status == OrderStatus.Complete);

        var completeCount = totalItems
            .Where(item => item.Status == OrderStatus.Complete)
            .Sum(item => item.Count);

        var normalNumber = totalItems
            .Count(item => item.Status == OrderStatus.Normal);

        var normalCount = totalItems
            .Where(item => item.Status == OrderStatus.Normal)
            .Sum(item => item.Count);

        var result = new OrderStatisticsResult
        {
            TotalNumber = totalNumber,
            TotalCount = totalCount,
            CompleteNumber = completeNumber,
            CompleteCount = completeCount,
            NormalNumber = normalNumber,
            NormalCount = normalCount
        };

        return DataResult.Success(result);
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
    public string? StatusMatch { get; set; }

    /// <summary>
    ///     �ò�����
    /// </summary>
    public string? MealDateMatch { get; set; }
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
public record UpdateOrderRequest : OrderData;

/// <summary>
///     �޸Ķ���״̬����
/// </summary>
public record ChangeOrderStatusRequest
{
    /// <summary>
    ///     ������ʶ
    /// </summary>
    [Required]
    public required Guid Id { get; set; }

    /// <summary>
    ///     Ŀ��״̬
    /// </summary>
    [Required]
    public required string Status { get; set; }
}

/// <summary>
///     ɾ����������
/// </summary>
public record DeleteOrderRequest
{
    /// <summary>
    ///     ��ʶ
    /// </summary>
    [Required]
    public Guid Id { get; set; }
}

/// <summary>
///     ����ͳ������
/// </summary>
public record OrderStatisticsRequest
{
    /// <summary>
    ///     �ò�ʱ��
    /// </summary>
    public string? MealDate { get; set; }
}

/// <summary>
///     ����ͳ�ƽ��
/// </summary>
public record OrderStatisticsResult
{
    /// <summary>
    ///     �ܳ���
    /// </summary>
    public required int TotalNumber { get; set; }

    /// <summary>
    ///     ������
    /// </summary>
    public required int TotalCount { get; set; }

    /// <summary>
    ///     ��ɳ���
    /// </summary>
    public required int CompleteNumber { get; set; }

    /// <summary>
    ///     �������
    /// </summary>
    public required int CompleteCount { get; set; }

    /// <summary>
    ///     ʣ�೵��
    /// </summary>
    public required int NormalNumber { get; set; }

    /// <summary>
    ///     ʣ������
    /// </summary>
    public required int NormalCount { get; set; }
}

/// <summary>
///     ��������
/// </summary>
public record OrderData : OrderInfo
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
    ///     �ò�����
    /// </summary>
    [Required]
    public required string MealDate { get; set; }

    /// <summary>
    ///     ����
    /// </summary>
    [Required]
    public required string MealType { get; set; } = null!;

    /// <summary>
    ///     ״̬
    /// </summary>
    [Required]
    public required string Status { get; set; } = OrderStatus.Normal;

    /// <summary>
    ///     ����
    /// </summary>
    public int WaitFlag { get; set; } = 0;

    /// <summary>
    ///     ��ע
    /// </summary>
    public string? Remark { get; set; }
}