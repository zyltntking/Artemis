namespace Artemis.Data.Shared.School;

/// <summary>
///     教师接口
/// </summary>
public interface ITeacher
{
    /// <summary>
    ///     教师名称
    /// </summary>
    string TeacherName { get; set; }

    /// <summary>
    ///     教师编码
    /// </summary>
    string TeacherCode { get; set; }
}