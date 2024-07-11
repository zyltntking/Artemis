namespace Artemis.Service.Shared.School;

/// <summary>
///     学校学生对应接口
/// </summary>
public interface ISchoolStudent : ITransfer
{
    /// <summary>
    ///     学校标识
    /// </summary>
    Guid SchoolId { get; set; }

    /// <summary>
    ///     学生标识
    /// </summary>
    Guid StudentId { get; set; }
}