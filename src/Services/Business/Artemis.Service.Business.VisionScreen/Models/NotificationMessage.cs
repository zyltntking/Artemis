using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Service.Shared.Business.VisionScreen;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Business.VisionScreen.Models;

/// <summary>
/// 通知消息模型
/// </summary>
public class NotificationMessage : ConcurrencyPartition, INotificationMessage
{
    #region Implementation of INotificationMessagePackage

    /// <summary>
    /// 用户标识
    /// </summary>
    [Comment("用户标识")]
    public Guid UserId { get; set; }

    /// <summary>
    /// 任务标识
    /// </summary>
    [Comment("任务标识")]
    public Guid TaskId { get; set; }

    /// <summary>
    /// 端类型
    /// </summary>
    [Comment("端类型")]
    [MaxLength(32)]
    public string? EndType { get; set; }

    /// <summary>
    /// 是否已读
    /// </summary>
    [Comment("是否已读")]
    public bool IsRead { get; set; }

    /// <summary>
    /// 消息读取时间
    /// </summary>
    [Comment("消息读取时间")]
    public DateTime? ReadTime { get; set; }

    /// <summary>
    /// 通知标题
    /// </summary>
    [Comment("通知标题")]
    [MaxLength(128)]
    public string? Title { get; set; }

    /// <summary>
    /// 通知内容
    /// </summary>
    [Comment("通知内容")]
    [MaxLength(512)]
    public string? Content { get; set; }

    /// <summary>
    /// 绑定标记
    /// </summary>
    [Comment("绑定标记")]
    [MaxLength(64)]
    public string? BindingTag { get; set; }

    #endregion
}