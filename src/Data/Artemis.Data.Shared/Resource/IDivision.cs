namespace Artemis.Data.Shared.Resource;

/// <summary>
///     行政区划接口
/// </summary>
public interface IDivision
{
    /// <summary>
    ///     行政区划名称
    /// </summary>
    string Name { get; set; }

    /// <summary>
    ///     行政区划代码
    /// </summary>
    string Code { get; set; }

    /// <summary>
    ///     行政区划级别
    /// </summary>
    int Level { get; set; }
}