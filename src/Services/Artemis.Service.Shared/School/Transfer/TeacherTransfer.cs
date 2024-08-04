namespace Artemis.Service.Shared.School.Transfer;

/// <summary>
///     教师信息
/// </summary>
public record TeacherInfo : TeacherPackage, ITeacherInfo
{
    /// <summary>
    ///     存储标识
    /// </summary>
    public Guid Id { get; set; }
}

/// <summary>
///     教师数据包
/// </summary>
public record TeacherPackage : ITeacherPackage
{
    #region Implementation of ITeacherPackage

    /// <summary>
    ///     学校标识
    /// </summary>
    public Guid? SchoolId { get; set; }

    /// <summary>
    ///     教师名称
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    ///     教师编码
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    ///     入职时间
    /// </summary>
    public DateTime? EntryTime { get; set; }

    /// <summary>
    ///     教师性别
    /// </summary>
    public string? Gender { get; set; }

    /// <summary>
    ///     教师职称
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    ///     教师学历
    /// </summary>
    public string? Education { get; set; }

    /// <summary>
    ///     教师身份证号
    /// </summary>
    public string? IdCard { get; set; }

    /// <summary>
    ///     教师籍贯
    /// </summary>
    public string? NativePlace { get; set; }

    /// <summary>
    ///     政治面貌
    /// </summary>
    public string? PoliticalStatus { get; set; }

    /// <summary>
    ///     家庭住址
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    ///     生日
    /// </summary>
    public DateTime? Birthday { get; set; }

    /// <summary>
    ///     联系电话
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    ///     邮箱
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    ///     备注
    /// </summary>
    public string? Remark { get; set; }

    #endregion
}