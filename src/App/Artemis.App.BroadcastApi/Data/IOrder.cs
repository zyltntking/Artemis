namespace Artemis.App.BroadcastApi.Data;

/// <summary>
///     订单接口
/// </summary>
public interface IOrder
{
    /// <summary>
    ///     车牌号
    /// </summary>
    string License { get; set; }

    /// <summary>
    ///     人数
    /// </summary>
    int Count { get; set; }

    /// <summary>
    ///     价格
    /// </summary>
    double Price { get; set; }

    /// <summary>
    ///     用餐时间
    /// </summary>
    DateTime MealTime { get; set; }

    /// <summary>
    ///     状态
    /// </summary>
    string Status { get; set; }

    /// <summary>
    ///     备注
    /// </summary>
    string? Remark { get; set; }
}