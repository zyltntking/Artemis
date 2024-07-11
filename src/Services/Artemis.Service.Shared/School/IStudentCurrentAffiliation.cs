namespace Artemis.Service.Shared.School;

/// <summary>
///     学生当前所属关系接口
/// </summary>
public interface IStudentCurrentAffiliation
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
    ///     学生标识
    /// </summary>
    Guid StudentId { get; set; }
}