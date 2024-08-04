using Artemis.Data.Core;

namespace Artemis.Service.Shared.School;

/// <summary>
/// 班级接口
/// </summary>
public interface IClass : IClassInfo
{
}

/// <summary>
/// 班级信息接口
/// </summary>
public interface IClassInfo : IClassPackage, IKeySlot
{
    /// <summary>
    ///     学校标识
    /// </summary>
    Guid SchoolId { get; set; }
}

/// <summary>
///     班级数据包接口
/// </summary>
public interface IClassPackage
{
    /// <summary>
    ///     班主任标识
    /// </summary>
    Guid? HeadTeacherId { get; set; }

    /// <summary>
    /// 班主任名称
    /// </summary>
    string? HeadTeacherName { get; set; }

    /// <summary>
    ///     班级名称
    /// </summary>
    string? Name { get; set; }

    /// <summary>
    ///     年级名称
    /// </summary>
    string? GradeName { get; set; }

    /// <summary>
    ///     班级类型
    /// </summary>
    string? Type { get; set; }

    /// <summary>
    ///     所学专业
    /// </summary>
    string? Major { get; set; }

    /// <summary>
    ///     班级编码
    /// </summary>
    string? Code { get; set; }

    /// <summary>
    ///     学段
    /// </summary>
    string? StudyPhase { get; set; }

    /// <summary>
    ///     学制
    /// </summary>
    string? SchoolLength { get; set; }

    /// <summary>
    ///     学制长度
    /// </summary>
    int Length { get; set; }

    /// <summary>
    ///     班级序号
    /// </summary>
    int SerialNumber { get; set; }

    /// <summary>
    ///     班级创建时间
    /// </summary>
    DateTime? EstablishTime { get; set; }
}