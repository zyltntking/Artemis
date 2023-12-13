namespace Artemis.App.BroadcastApi.Data;

/// <summary>
///     订单状态
/// </summary>
public static class OrderStatus
{
    /// <summary>
    ///     全部
    /// </summary>
    public const string All = nameof(All);

    /// <summary>
    ///     正常
    /// </summary>
    public const string Normal = "正常";

    /// <summary>
    ///     完成
    /// </summary>
    public const string Complete = "完成";

    /// <summary>
    ///     等待
    /// </summary>
    public const string Wait = "等待";
}