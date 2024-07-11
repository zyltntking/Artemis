namespace Artemis.Service.Shared.School;

/// <summary>
///     教师当前所属关系接口
/// </summary>
public interface ITeacherCurrentAffiliation
{
    /// <summary>
    ///     学校标识
    /// </summary>
    Guid SchoolId { get; set; }

    /// <summary>
    ///     班级标识
    /// </summary>
    Guid ClassId { get; set; }

    /// <summary>
    ///     教师标识
    /// </summary>
    Guid TeacherId { get; set; }
}