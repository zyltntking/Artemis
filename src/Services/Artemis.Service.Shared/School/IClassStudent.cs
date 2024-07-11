namespace Artemis.Service.Shared.School;

/// <summary>
///     班级学生对应接口
/// </summary>
public interface IClassStudent : ITransfer
{
    /// <summary>
    ///     班级标识
    /// </summary>
    Guid ClassId { get; set; }

    /// <summary>
    ///     学生标识
    /// </summary>
    Guid StudentId { get; set; }
}