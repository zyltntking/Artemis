using System.ComponentModel.DataAnnotations;
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
    [MaxLength(10)]
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
    ///     用餐日期
    /// </summary>
    [MaxLength(32)]
    public string MealDate { get; set; } = null!;

    /// <summary>
    ///     用餐类型
    /// </summary>
    [MaxLength(32)]
    public string MealType { get; set; } = null!;

    /// <summary>
    ///     状态
    /// </summary>
    [MaxLength(32)]
    public string Status { get; set; } = null!;

    /// <summary>
    ///     等待序列
    /// </summary>
    public int WaitFlag { get; set; }

    /// <summary>
    ///     备注
    /// </summary>
    [MaxLength(256)]
    public string? Remark { get; set; }
}