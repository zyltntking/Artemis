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
    ///     创建订单
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

        return DataResult.Fail<OrderData>("创建失败");
    }

    /// <summary>
    ///     更新订单
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

        if (order == null) return DataResult.Fail<OrderData>("订单不存在");

        request.Adapt(order);
        order.License = request.License.ToUpper();

        order.UpdatedAt = DateTime.Now;

        _context.Orders.Update(order);

        var result = await _context.SaveChangesAsync(cancellationToken);

        if (result > 0) return DataResult.Success(order.Adapt<OrderData>());

        return DataResult.Fail<OrderData>("更新失败");
    }

    /// <summary>
    ///     修改订单状态
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
            return DataResult.Fail<OrderData>("订单不存在");

        order.Status = request.Status;

        _context.Orders.Update(order);

        var result = await _context.SaveChangesAsync(cancellationToken);

        if (result > 0)
            return DataResult.Success(order.Adapt<OrderData>());

        return DataResult.Fail<OrderData>("更新失败");
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

    /// <summary>
    ///     订单统计
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

    /// <summary>
    ///     用餐日期
    /// </summary>
    public string? MealDateMatch { get; set; }
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
public record UpdateOrderRequest : OrderData;

/// <summary>
///     修改订单状态请求
/// </summary>
public record ChangeOrderStatusRequest
{
    /// <summary>
    ///     订单标识
    /// </summary>
    [Required]
    public required Guid Id { get; set; }

    /// <summary>
    ///     目标状态
    /// </summary>
    [Required]
    public required string Status { get; set; }
}

/// <summary>
///     删除订单请求
/// </summary>
public record DeleteOrderRequest
{
    /// <summary>
    ///     标识
    /// </summary>
    [Required]
    public Guid Id { get; set; }
}

/// <summary>
///     订单统计请求
/// </summary>
public record OrderStatisticsRequest
{
    /// <summary>
    ///     用餐时间
    /// </summary>
    public string? MealDate { get; set; }
}

/// <summary>
///     订单统计结果
/// </summary>
public record OrderStatisticsResult
{
    /// <summary>
    ///     总车次
    /// </summary>
    public required int TotalNumber { get; set; }

    /// <summary>
    ///     总人数
    /// </summary>
    public required int TotalCount { get; set; }

    /// <summary>
    ///     完成车次
    /// </summary>
    public required int CompleteNumber { get; set; }

    /// <summary>
    ///     完成人数
    /// </summary>
    public required int CompleteCount { get; set; }

    /// <summary>
    ///     剩余车次
    /// </summary>
    public required int NormalNumber { get; set; }

    /// <summary>
    ///     剩余人数
    /// </summary>
    public required int NormalCount { get; set; }
}

/// <summary>
///     订单数据
/// </summary>
public record OrderData : OrderInfo
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