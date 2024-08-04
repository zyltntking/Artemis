namespace Artemis.Service.Shared.School.Transfer;

/// <summary>
/// 班级信息
/// </summary>
public record ClassInfo : ClassPackage, IClassInfo
{
    /// <summary>
    ///     存储标识
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     学校标识
    /// </summary>
    public Guid SchoolId { get; set; }
}

/// <summary>
/// 班级数据包
/// </summary>
public record ClassPackage : IClassPackage
{
    #region Implementation of IClassPackage

    /// <summary>
    ///     班主任标识
    /// </summary>
    public Guid? HeadTeacherId { get; set; }

    /// <summary>
    /// 班主任名称
    /// </summary>
    public string? HeadTeacherName { get; set; }

    /// <summary>
    ///     班级名称
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    ///     年级名称
    /// </summary>
    public string? GradeName { get; set; }

    /// <summary>
    ///     班级类型
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    ///     所学专业
    /// </summary>
    public string? Major { get; set; }

    /// <summary>
    ///     班级编码
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    ///     学段
    /// </summary>
    public string? StudyPhase { get; set; }

    /// <summary>
    ///     学制
    /// </summary>
    public string? SchoolLength { get; set; }

    /// <summary>
    ///     学制长度
    /// </summary>
    public int Length { get; set; }

    /// <summary>
    ///     班级序号
    /// </summary>
    public int SerialNumber { get; set; }

    /// <summary>
    ///     班级创建时间
    /// </summary>
    public DateTime? EstablishTime { get; set; }

    #endregion
}