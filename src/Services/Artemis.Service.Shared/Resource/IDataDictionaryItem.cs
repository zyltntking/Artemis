using Artemis.Data.Core;

namespace Artemis.Service.Shared.Resource;

/// <summary>
///     数据项目接口
/// </summary>
public interface IDataDictionaryItem : IDataDictionaryItemInfo
{
}

/// <summary>
///     数据项目信息接口
/// </summary>
public interface IDataDictionaryItemInfo : IDataDictionaryItemPackage, IKeySlot
{
    /// <summary>
    ///     数据字典标识
    /// </summary>
    Guid DataDictionaryId { get; set; }
}

/// <summary>
///     数据项目数据包接口
/// </summary>
public interface IDataDictionaryItemPackage
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