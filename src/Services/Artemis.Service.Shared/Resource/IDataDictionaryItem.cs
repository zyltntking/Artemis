namespace Artemis.Service.Shared.Resource;

/// <summary>
///     数据项目接口
/// </summary>
public interface IDataDictionaryItem
{
    /// <summary>
    ///     数据项目键
    /// </summary>
    string Key { get; set; }

    /// <summary>
    ///     数据项目值
    /// </summary>
    string Value { get; set; }

    /// <summary>
    ///     数据项目描述
    /// </summary>
    string? Description { get; set; }

    /// <summary>
    ///     数据项目是否有效
    /// </summary>
    bool Valid { get; set; }
}