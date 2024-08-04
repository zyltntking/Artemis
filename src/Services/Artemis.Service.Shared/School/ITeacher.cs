using Artemis.Data.Core;

namespace Artemis.Service.Shared.School;

/// <summary>
///     教师接口
/// </summary>
public interface ITeacher : ITeacherInfo;

/// <summary>
///     教师信息接口
/// </summary>
public interface ITeacherInfo : ITeacherPackage, IKeySlot;

/// <summary>
///     教师数据包接口
/// </summary>
public interface ITeacherPackage
{
    /// <summary>
    ///     学校标识
    /// </summary>
    Guid? SchoolId { get; set; }

    /// <summary>
    ///     教师名称
    /// </summary>
    string Name { get; set; }

    /// <summary>
    ///     教师编码
    /// </summary>
    string? Code { get; set; }

    /// <summary>
    ///     入职时间
    /// </summary>
    DateTime? EntryTime { get; set; }

    /// <summary>
    ///     教师性别
    /// </summary>
    string? Gender { get; set; }

    /// <summary>
    ///     教师职称
    /// </summary>
    string? Title { get; set; }

    /// <summary>
    ///     教师学历
    /// </summary>
    string? Education { get; set; }

    /// <summary>
    ///     教师身份证号
    /// </summary>
    string? IdCard { get; set; }

    /// <summary>
    ///     教师籍贯
    /// </summary>
    string? NativePlace { get; set; }

    /// <summary>
    ///     政治面貌
    /// </summary>
    string? PoliticalStatus { get; set; }

    /// <summary>
    ///     家庭住址
    /// </summary>
    string? Address { get; set; }

    /// <summary>
    ///     生日
    /// </summary>
    DateTime? Birthday { get; set; }

    /// <summary>
    ///     联系电话
    /// </summary>
    string? Phone { get; set; }

    /// <summary>
    ///     邮箱
    /// </summary>
    string? Email { get; set; }

    /// <summary>
    ///     备注
    /// </summary>
    string? Remark { get; set; }
}