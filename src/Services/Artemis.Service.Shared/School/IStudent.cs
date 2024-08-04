using Artemis.Data.Core;

namespace Artemis.Service.Shared.School;

/// <summary>
///     学生接口
/// </summary>
public interface IStudent : IStudentInfo
{
}

/// <summary>
///     学生信息接口
/// </summary>
public interface IStudentInfo : IStudentPackage, IKeySlot
{
}

/// <summary>
///     学生数据包接口
/// </summary>
public interface IStudentPackage
{
    /// <summary>
    ///     学校标识
    /// </summary>
    Guid? SchoolId { get; set; }

    /// <summary>
    ///     班级标识
    /// </summary>
    Guid? ClassId { get; set; }

    /// <summary>
    ///     学生名称
    /// </summary>
    string Name { get; set; }

    /// <summary>
    ///     学生性别
    /// </summary>
    string? Gender { get; set; }

    /// <summary>
    ///     学生生日
    /// </summary>
    DateOnly? Birthday { get; set; }

    /// <summary>
    ///     民族
    /// </summary>
    string? Nation { get; set; }

    /// <summary>
    ///     学生编码
    /// </summary>
    string? Code { get; set; }

    /// <summary>
    ///     学籍号
    /// </summary>
    string StudentNumber { get; set; }

    /// <summary>
    ///     证件号码
    /// </summary>
    string? Cert { get; set; }

    /// <summary>
    ///     入学时间
    /// </summary>
    DateTime? EnrollmentDate { get; set; }

    /// <summary>
    ///     住址区划代码
    /// </summary>
    string? DivisionCode { get; set; }

    /// <summary>
    ///     住址
    /// </summary>
    string? Address { get; set; }

    /// <summary>
    ///     备注
    /// </summary>
    string? Remark { get; set; }
}