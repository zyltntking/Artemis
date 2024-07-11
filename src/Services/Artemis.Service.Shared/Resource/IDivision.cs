namespace Artemis.Service.Shared.Resource;

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

    /// <summary>
    ///     行政区划类型
    /// </summary>
    string Type { get; set; }

    /// <summary>
    ///     行政区划全名
    /// </summary>
    string? FullName { get; set; }

    /// <summary>
    ///     行政区划拼音
    /// </summary>
    string? Pinyin { get; set; }

    /// <summary>
    ///     行政区划备注
    /// </summary>
    string? Remark { get; set; }
}