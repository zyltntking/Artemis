namespace Artemis.Data.Shared.School;

/// <summary>
///     班级接口
/// </summary>
public interface IClass
{
    /// <summary>
    /// 学校标识
    /// </summary>
    Guid SchoolId { get; set; }

    /// <summary>
    ///     班级名称
    /// </summary>
    string ClassName { get; set; }

    /// <summary>
    ///     班级编码
    /// </summary>
    string ClassCode { get; set; }
}