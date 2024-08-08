namespace Artemis.Service.Shared.Business.VisionScreen.Transfer;

/// <summary>
/// 学生关系绑定信息
/// </summary>
public record StudentRelationBindingInfo : StudentRelationBindingPackage, IStudentRelationBindingInfo
{
    #region Implementation of IKeySlot<Guid>

    /// <summary>
    ///     存储标识
    /// </summary>
    public Guid Id { get; set; }

    #endregion
}

/// <summary>
/// 学生关系绑定数据包
/// </summary>
public record StudentRelationBindingPackage : IStudentRelationBindingPackage
{
    #region Implementation of IStudentRelationBindingPackage

    /// <summary>
    /// 用户标识
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// 学生标识
    /// </summary>
    public Guid StudentId { get; set; }

    /// <summary>
    /// 关系
    /// </summary>
    public required string Relation { get; set; }

    #endregion
}