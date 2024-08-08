using Artemis.Data.Core;

namespace Artemis.Service.Shared.Business.VisionScreen;

/// <summary>
/// 通知消息接口
/// </summary>
public interface INotificationMessage : INotificationMessageInfo
{
}

/// <summary>
/// 通知消息信息接口
/// </summary>
public interface INotificationMessageInfo : INotificationMessagePackage,IKeySlot
{
}

/// <summary>
/// 通知消息数据包接口
/// </summary>
public interface INotificationMessagePackage
{
    /// <summary>
    /// 用户标识
    /// </summary>
    Guid UserId { get; set; }

    /// <summary>
    /// 任务标识
    /// </summary>
    Guid TaskId { get; set; }

    /// <summary>
    /// 端类型
    /// </summary>
    string? EndType { get; set; }

    /// <summary>
    /// 是否已读
    /// </summary>
    bool IsRead { get; set; }

    /// <summary>
    /// 消息读取时间
    /// </summary>
    DateTime? ReadTime { get; set; }

    /// <summary>
    /// 通知标题
    /// </summary>
    string? Title { get; set; }

    /// <summary>
    /// 通知内容
    /// </summary>
    string? Content { get; set; }

    /// <summary>
    /// 绑定标记
    /// </summary>
    string? BindingTag { get; set; }
}