using Artemis.Data.Core;
using Artemis.Service.Shared.Business.VisionScreen;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Business.VisionScreen.Models;

/// <summary>
/// 记录反馈数据模型
/// </summary>
public class RecordFeedback : ConcurrencyPartition, IRecordFeedback
{
    #region Implementation of IRecordFeedbackPackage

    /// <summary>
    /// 反馈用户标识
    /// </summary>
    [Comment("反馈用户标识")]
    public Guid? UserId { get; set; }

    /// <summary>
    /// 是否已处理
    /// </summary>
    [Comment("是否已处理")]
    public bool IsCheck { get; set; }

    /// <summary>
    /// 处理日期
    /// </summary>
    [Comment("处理日期")]
    public DateTime? CheckDate { get; set; }

    /// <summary>
    /// 反馈内容
    /// </summary>
    [Comment("反馈内容")]
    public string? Content { get; set; }

    /// <summary>
    /// 反馈时间
    /// </summary>
    [Comment("反馈时间")]
    public DateTime? FeedBackTime { get; set; }

    #endregion

    #region Implementation of IRecordFeedbackInfo

    /// <summary>
    /// 记录标识
    /// </summary>
    [Comment("记录标识")]
    public Guid RecordId { get; set; }

    #endregion
}