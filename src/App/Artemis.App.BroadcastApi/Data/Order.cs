using Artemis.App.BroadcastApi.Data.Configuration;
using Artemis.Data.Core;
using Microsoft.EntityFrameworkCore;

namespace Artemis.App.BroadcastApi.Data;

/// <summary>
///     订单模型
/// </summary>
[EntityTypeConfiguration(typeof(OrderConfiguration))]
public class Order : ModelBase, IOrder
{
    /// <summary>
    ///     车牌号
    /// </summary>
    public string License { get; set; } = null!;

    /// <summary>
    ///     人数
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    ///     价格
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    ///     用餐时间
    /// </summary>
    public DateTime MealTime { get; set; } = DateTime.Now;

    /// <summary>
    ///     状态
    /// </summary>
    public string Status { get; set; } = null!;

    /// <summary>
    ///     备注
    /// </summary>
    public string? Remark { get; set; }
}