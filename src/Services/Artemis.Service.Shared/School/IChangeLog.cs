using Artemis.Data.Core;

namespace Artemis.Service.Shared.School;

/// <summary>
/// 学生变动记录接口
/// </summary>
public interface IStudentChangeLog : IStudentChangeLogInfo
{

}

/// <summary>
/// 学生变动记录信息接口
/// </summary>
public interface IStudentChangeLogInfo : IStudentChangeLogPackage, IKeySlot
{

}

/// <summary>
/// 学生变动记录数据包
/// </summary>
public interface IStudentChangeLogPackage : IClassChangeLogBase
{
    /// <summary>
    /// 学生标识
    /// </summary>
    Guid StudentId { get; set; }

    /// <summary>
    /// 学生姓名
    /// </summary>
    string? StudentName { get; set; }

    /// <summary>
    /// 学生生日
    /// </summary>
    DateTime? Birthday { get; set; }
}

/// <summary>
/// 教师变动记录接口
/// </summary>
public interface ITeacherChangeLog : ITeacherChangeLogInfo
{

}

/// <summary>
/// 教师变动记录信息接口
/// </summary>
public interface ITeacherChangeLogInfo : ITeacherChangeLogPackage, IKeySlot
{

}

/// <summary>
/// 教师变动记录数据包
/// </summary>
public interface ITeacherChangeLogPackage : ISchoolChangeLogBase
{
    /// <summary>
    /// 教师标识
    /// </summary>
    Guid TeacherId { get; set; }

    /// <summary>
    /// 教师姓名
    /// </summary>
    string? TeacherName { get; set; }
}

/// <summary>
/// 班级变更记录接口
/// </summary>
public interface IClassChangeLogBase : ISchoolChangeLogBase
{
    /// <summary>
    /// 班级标识
    /// </summary>
    Guid? ClassId { get; set; }

    /// <summary>
    /// 班级名称
    /// </summary>
    string? ClassName { get; set; }

    /// <summary>
    /// 年级名称
    /// </summary>
    string? GradeName { get; set; }

    /// <summary>
    /// 班级序列
    /// </summary>
    int? SerialNumber { get; set; }
}

/// <summary>
/// 基础学校变动记录接口
/// </summary>
public interface ISchoolChangeLogBase : IChangeLogBase
{
    /// <summary>
    /// 变动学校标识
    /// </summary>
    Guid SchoolId { get; set; }

    /// <summary>
    /// 变动学校名称
    /// </summary>
    string? SchoolName { get; set; }

    /// <summary>
    /// 学校所在行政区划编码
    /// </summary>
    string? DivisionCode { get; set; }
}

/// <summary>
///     基础变动记录接口
/// </summary>
public interface IChangeLogBase
{
    /// <summary>
    /// 变动类型
    /// </summary>
    string ChangeType { get; set; }

    /// <summary>
    ///     变动时间
    /// </summary>
    DateTime ChangeTime { get; set; }

    /// <summary>
    /// 变动原因
    /// </summary>
    string? ChangeReason { get; set; }
}