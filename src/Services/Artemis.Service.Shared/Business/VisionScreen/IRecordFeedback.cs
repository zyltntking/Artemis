using Artemis.Data.Core;

namespace Artemis.Service.Shared.Business.VisionScreen;

/// <summary>
/// 记录反馈接口
/// </summary>
public interface IRecordFeedback : IRecordFeedbackInfo
{
}

/// <summary>
/// 记录反馈信息接口
/// </summary>
public interface IRecordFeedbackInfo : IRecordFeedbackPackage, IKeySlot
{
    /// <summary>
    /// 记录标识
    /// </summary>
    Guid RecordId { get; set; }
}

/// <summary>
/// 记录反馈数据包接口
/// </summary>
public interface IRecordFeedbackPackage
{
    /// <summary>
    /// 反馈用户标识
    /// </summary>
    Guid? UserId { get; set; }

    /// <summary>
    /// 是否已处理
    /// </summary>
    bool IsCheck { get; set; }

    /// <summary>
    /// 处理日期
    /// </summary>
    DateTime? CheckDate { get; set; }

    /// <summary>
    /// 反馈内容
    /// </summary>
    string? Content { get; set; }

    /// <summary>
    /// 反馈时间
    /// </summary>
    DateTime? FeedBackTime { get; set; }

}