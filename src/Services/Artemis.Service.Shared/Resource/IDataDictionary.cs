using Artemis.Data.Core;

namespace Artemis.Service.Shared.Resource;

/// <summary>
///     数据字典接口
/// </summary>
public interface IDataDictionary : IDataDictionaryInfo;

/// <summary>
///     数据字典信息接口
/// </summary>
public interface IDataDictionaryInfo : IDataDictionaryPackage, IKeySlot;

/// <summary>
///     数据字典数据包接口
/// </summary>
public interface IDataDictionaryPackage
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