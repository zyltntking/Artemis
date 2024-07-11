namespace Artemis.Service.Shared.School;

/// <summary>
///     学校教师对应接口
/// </summary>
public interface ISchoolTeacher : ITransfer
{
    /// <summary>
    ///     班级标识
    /// </summary>
    Guid SchoolId { get; set; }

    /// <summary>
    ///     教师标识
    /// </summary>
    Guid TeacherId { get; set; }
}