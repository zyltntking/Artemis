namespace Artemis.Service.Shared.School.Transfer;

/// <summary>
/// 教师变动记录信息
/// </summary>
public class TeacherChangeLogInfo : ITeacherChangeLogInfo
{
    #region Implementation of IChangeLogBase

    /// <summary>
    /// 变动类型
    /// </summary>
    public required string ChangeType { get; set; }

    /// <summary>
    ///     变动时间
    /// </summary>
    public DateTime ChangeTime { get; set; }

    /// <summary>
    /// 变动原因
    /// </summary>
    public string? ChangeReason { get; set; }

    #endregion

    #region Implementation of ISchoolChangeLogBase

    /// <summary>
    /// 变动学校标识
    /// </summary>
    public Guid SchoolId { get; set; }

    /// <summary>
    /// 变动学校名称
    /// </summary>
    public string? SchoolName { get; set; }

    /// <summary>
    /// 学校所在行政区划编码
    /// </summary>
    public string? DivisionCode { get; set; }

    #endregion

    #region Implementation of ITeacherChangeLogPackage

    /// <summary>
    /// 教师标识
    /// </summary>
    public Guid TeacherId { get; set; }

    /// <summary>
    /// 教师姓名
    /// </summary>
    public string? TeacherName { get; set; }

    #endregion

    #region Implementation of IKeySlot<Guid>

    /// <summary>
    ///     存储标识
    /// </summary>
    public Guid Id { get; set; }

    #endregion
}


/// <summary>
/// 学生变动记录信息
/// </summary>
public class StudentChangeLogInfo : IStudentChangeLogInfo
{
    #region Implementation of IChangeLogBase

    /// <summary>
    /// 变动类型
    /// </summary>
    public required string ChangeType { get; set; }

    /// <summary>
    ///     变动时间
    /// </summary>
    public DateTime ChangeTime { get; set; }

    /// <summary>
    /// 变动原因
    /// </summary>
    public string? ChangeReason { get; set; }

    #endregion

    #region Implementation of ISchoolChangeLogBase

    /// <summary>
    /// 变动学校标识
    /// </summary>
    public Guid SchoolId { get; set; }

    /// <summary>
    /// 变动学校名称
    /// </summary>
    public string? SchoolName { get; set; }

    /// <summary>
    /// 学校所在行政区划编码
    /// </summary>
    public string? DivisionCode { get; set; }

    #endregion

    #region Implementation of IClassChangeLogBase

    /// <summary>
    /// 班级标识
    /// </summary>
    public Guid? ClassId { get; set; }

    /// <summary>
    /// 班级名称
    /// </summary>
    public string? ClassName { get; set; }

    /// <summary>
    /// 年级名称
    /// </summary>
    public string? GradeName { get; set; }

    /// <summary>
    /// 班级序列
    /// </summary>
    public int? SerialNumber { get; set; }

    #endregion

    #region Implementation of IStudentChangeLogPackage

    /// <summary>
    /// 学生标识
    /// </summary>
    public Guid StudentId { get; set; }

    /// <summary>
    /// 学生姓名
    /// </summary>
    public string? StudentName { get; set; }

    /// <summary>
    /// 学生生日
    /// </summary>
    public DateTime? Birthday { get; set; }

    #endregion

    #region Implementation of IKeySlot<Guid>

    /// <summary>
    ///     存储标识
    /// </summary>
    public Guid Id { get; set; }

    #endregion
}