using System.ComponentModel.DataAnnotations;
using Artemis.App.BroadcastApi.Data;
using Artemis.Data.Core;
using Artemis.Data.Store.Extensions;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Artemis.App.BroadcastApi.Controllers;

/// <summary>
///     广播Api
/// </summary>
[ApiController]
[Route("[controller]/[action]")]
public class BroadcastController : ControllerBase
{
    private readonly BroadcastContext _context;

    private readonly ILogger<BroadcastController> _logger;

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="logger">日志依赖</param>
    /// <param name="context">上下文依赖</param>
    public BroadcastController(
        ILogger<BroadcastController> logger,
        BroadcastContext context)
    {
        _logger = logger;
        _context = context;
    }

    /// <summary>
    ///     登录
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns>响应</returns>
    [HttpPost]
    public DataResult<string> SignIn([FromBody] SignInRequest request)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserName == request.UserName);

        if (user == null) return DataResult.Fail<string>("用户不存在");

        return DataResult.Success("token", "成功");
    }

    /// <summary>
    ///     查询订单信息
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<DataResult<PageResult<OrderInfo>>> FetchOrderInfos([FromBody] FetchOrderInfosRequest request)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var filter = request.Filter;

        var licenseSearch = filter.LicenseSearch ?? string.Empty;

        var statusMatch = filter.StatusMatch ?? OrderStatus.All;

        var query = _context.Orders.AsNoTracking();

        var total = await query.CountAsync(cancellationToken);

        var today = DateTime.Today;

        var tomorrow = today.AddDays(1);

        query = query.WhereIf(licenseSearch != string.Empty,
                order => EF.Functions.Like(
                    order.License,
                    $"%{licenseSearch}%"))
            .WhereIf(statusMatch != OrderStatus.All,
                order => order.Status == statusMatch)
            .Where(order => order.CreatedAt >= today)
            .Where(order => order.CreatedAt <= tomorrow);

        var count = await query.CountAsync(cancellationToken);

        var orderInfoList = await query.OrderBy(order => order.WaitFlag)
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
    ///     创建订单
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

        return DataResult.Fail<OrderInfo>("创建失败");
    }

    /// <summary>
    ///     更新订单
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

        if (order == null) return DataResult.Fail<OrderInfo>("订单不存在");

        request.Adapt(order);
        order.License = request.License.ToUpper();

        order.UpdatedAt = DateTime.Now;

        _context.Orders.Update(order);

        var result = await _context.SaveChangesAsync(cancellationToken);

        if (result > 0) return DataResult.Success(order.Adapt<OrderInfo>());

        return DataResult.Fail<OrderInfo>("更新失败");
    }

    /// <summary>
    ///     删除订单
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

        if (order == null) return DataResult.Fail<EmptyRecord>("订单不存在");

        _context.Orders.Remove(order);

        var result = await _context.SaveChangesAsync(cancellationToken);

        if (result > 0)
        {
            var record = new EmptyRecord();

            return DataResult.Success(record);
        }

        return DataResult.Fail<EmptyRecord>("删除失败");
    }
}

/// <summary>
///     登录请求
/// </summary>
public record SignInRequest
{
    /// <summary>
    ///     用户名
    /// </summary>
    [Required]
    public required string UserName { get; set; }

    /// <summary>
    ///     密码
    /// </summary>
    [Required]
    public required string Password { get; set; }
}

/// <summary>
///     订餐信息过滤器
/// </summary>
public record OrderInfoFilter
{
    /// <summary>
    ///     车牌号
    /// </summary>
    public string? LicenseSearch { get; set; }

    /// <summary>
    ///     价格
    /// </summary>
    public string? StatusMatch { get; set; }
}

/// <summary>
///     查询订单信息请求
/// </summary>
public record FetchOrderInfosRequest : PageRequest<OrderInfoFilter>;

/// <summary>
///     创建订单请求
/// </summary>
public record CreateOrderRequest : OrderInfo;

/// <summary>
///     更新订单请求
/// </summary>
public record UpdateOrderRequest : OrderInfo
{
    /// <summary>
    ///     标识
    /// </summary>
    public Guid Id { get; set; }
}

/// <summary>
///     删除订单请求
/// </summary>
public record DeleteOrderRequest
{
    /// <summary>
    ///     标识
    /// </summary>
    public Guid Id { get; set; }
}

/// <summary>
///     订单信息
/// </summary>
public record OrderInfo : IOrder
{
    /// <summary>
    ///     车牌号
    /// </summary>
    [Required]
    public required string License { get; set; } = null!;

    /// <summary>
    ///     人数
    /// </summary>
    [Required]
    public required int Count { get; set; }

    /// <summary>
    ///     价格
    /// </summary>
    [Required]
    public required double Price { get; set; }

    /// <summary>
    ///     用餐日期
    /// </summary>
    [Required]
    public required string MealDate { get; set; }

    /// <summary>
    ///     餐类
    /// </summary>
    [Required]
    public required string MealType { get; set; } = null!;

    /// <summary>
    ///     状态
    /// </summary>
    [Required]
    public required string Status { get; set; } = OrderStatus.Normal;

    /// <summary>
    ///     排序
    /// </summary>
    public int WaitFlag { get; set; } = 0;

    /// <summary>
    ///     备注
    /// </summary>
    public string? Remark { get; set; }
}