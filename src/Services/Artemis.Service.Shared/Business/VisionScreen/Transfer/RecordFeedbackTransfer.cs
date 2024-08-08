namespace Artemis.Service.Shared.Business.VisionScreen.Transfer;


/// <summary>
/// 档案反馈信息
/// </summary>
public record RecordFeedbackInfo : RecordFeedbackPackage, IRecordFeedbackInfo
{
    #region Implementation of IKeySlot<Guid>

    /// <summary>
    ///     存储标识
    /// </summary>
    public Guid Id { get; set; }

    #endregion

    #region Implementation of IRecordFeedbackInfo

    /// <summary>
    /// 记录标识
    /// </summary>
    public Guid RecordId { get; set; }

    #endregion
}


/// <summary>
/// 档案反馈数据包
/// </summary>
public record RecordFeedbackPackage : IRecordFeedbackPackage
{
    #region Implementation of IRecordFeedbackPackage

    /// <summary>
    /// 反馈用户标识
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// 是否已处理
    /// </summary>
    public bool IsCheck { get; set; }

    /// <summary>
    /// 处理日期
    /// </summary>
    public DateTime? CheckDate { get; set; }

    /// <summary>
    /// 反馈内容
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// 反馈时间
    /// </summary>
    public DateTime? FeedBackTime { get; set; }

    #endregion
}
