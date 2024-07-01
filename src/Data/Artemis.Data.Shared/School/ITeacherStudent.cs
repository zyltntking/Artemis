namespace Artemis.Data.Shared.School;

/// <summary>
///     教师学生对应接口
/// </summary>
public interface ITeacherStudent : ITransfer
{
    /// <summary>
    ///     教师标识
    /// </summary>
    Guid TeacherId { get; set; }

    /// <summary>
    ///     学生标识
    /// </summary>
    Guid StudentId { get; set; }
}