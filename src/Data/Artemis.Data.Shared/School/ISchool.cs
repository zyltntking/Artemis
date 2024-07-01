namespace Artemis.Data.Shared.School;

/// <summary>
///     学校接口
/// </summary>
public interface ISchool
{
    /// <summary>
    ///     学校名称
    /// </summary>
    string SchoolName { get; set; }

    /// <summary>
    ///     学校编码
    /// </summary>
    string SchoolCode { get; set; }
}