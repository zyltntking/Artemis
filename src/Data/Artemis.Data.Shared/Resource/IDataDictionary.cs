namespace Artemis.Data.Shared.Resource;

/// <summary>
///     数据字典接口
/// </summary>
public interface IDataDictionary
{
    /// <summary>
    ///     字典名称
    /// </summary>
    string Name { get; set; }

    /// <summary>
    ///     字典代码
    /// </summary>
    string Code { get; set; }

    /// <summary>
    ///     字典描述
    /// </summary>
    string? Description { get; set; }

    /// <summary>
    ///     字典类型
    /// </summary>
    string Type { get; set; }

    /// <summary>
    ///     字典是否有效
    /// </summary>
    bool Valid { get; set; }
}