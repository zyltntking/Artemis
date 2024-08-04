namespace Artemis.Service.Shared.School.Transfer;

/// <summary>
///     学生信息
/// </summary>
public record StudentInfo : StudentPackage, IStudentInfo
{
    /// <summary>
    ///     存储标识
    /// </summary>
    public Guid Id { get; set; }
}

/// <summary>
///     学生数据包
/// </summary>
public record StudentPackage : IStudentPackage
{
    #region Implementation of IStudentPackage

    /// <summary>
    ///     学校标识
    /// </summary>
    public Guid? SchoolId { get; set; }

    /// <summary>
    ///     班级标识
    /// </summary>
    public Guid? ClassId { get; set; }

    /// <summary>
    ///     学生名称
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    ///     学生性别
    /// </summary>
    public string? Gender { get; set; }

    /// <summary>
    ///     学生生日
    /// </summary>
    public DateTime? Birthday { get; set; }

    /// <summary>
    ///     民族
    /// </summary>
    public string? Nation { get; set; }

    /// <summary>
    ///     学生编码
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    ///     学籍号
    /// </summary>
    public required string StudentNumber { get; set; }

    /// <summary>
    ///     证件号码
    /// </summary>
    public string? Cert { get; set; }

    /// <summary>
    ///     入学时间
    /// </summary>
    public DateTime? EnrollmentDate { get; set; }

    /// <summary>
    ///     住址区划代码
    /// </summary>
    public string? DivisionCode { get; set; }

    /// <summary>
    ///     住址
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    ///     备注
    /// </summary>
    public string? Remark { get; set; }

    #endregion
}