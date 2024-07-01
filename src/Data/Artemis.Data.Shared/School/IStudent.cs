namespace Artemis.Data.Shared.School;

/// <summary>
///     学生接口
/// </summary>
public interface IStudent
{
    /// <summary>
    ///     学生名称
    /// </summary>
    string StudentName { get; set; }

    /// <summary>
    ///     学生编码
    /// </summary>
    string StudentCode { get; set; }
}