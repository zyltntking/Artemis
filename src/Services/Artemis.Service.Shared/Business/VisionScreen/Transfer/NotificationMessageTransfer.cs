namespace Artemis.Service.Shared.Business.VisionScreen.Transfer;

/// <summary>
/// 通知消息信息
/// </summary>
public record NotificationMessageInfo : NotificationMessagePackage, INotificationMessageInfo
{
    #region Implementation of IKeySlot<Guid>

    /// <summary>
    ///     存储标识
    /// </summary>
    public Guid Id { get; set; }

    #endregion
}


/// <summary>
/// 通知消息数据包
/// </summary>
public record NotificationMessagePackage : INotificationMessagePackage
{
    #region Implementation of INotificationMessagePackage

    /// <summary>
    /// 用户标识
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// 任务标识
    /// </summary>
    public Guid TaskId { get; set; }

    /// <summary>
    /// 端类型
    /// </summary>
    public string? EndType { get; set; }

    /// <summary>
    /// 是否已读
    /// </summary>
    public bool IsRead { get; set; }

    /// <summary>
    /// 消息读取时间
    /// </summary>
    public DateTime? ReadTime { get; set; }

    /// <summary>
    /// 通知标题
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// 通知内容
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// 绑定标记
    /// </summary>
    public string? BindingTag { get; set; }

    #endregion
}
