namespace Artemis.Data.Shared.School;

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
    ///     教师标识
    /// </summary>
    Guid TeacherId { get; set; }
}