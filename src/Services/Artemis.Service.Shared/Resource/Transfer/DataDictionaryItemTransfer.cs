namespace Artemis.Service.Shared.Resource.Transfer;

/// <summary>
/// 数据字典项目信息接口
/// </summary>
public record DataDictionaryItemInfo : DataDictionaryItemPackage, IDataDictionaryItemInfo
{
    /// <summary>
    ///     存储标识
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 数据字典标识
    /// </summary>
    public Guid DataDictionaryId { get; set; }
}

/// <summary>
/// 数据字典项目数据包接口
/// </summary>
public record DataDictionaryItemPackage : IDataDictionaryItemPackage
{
    /// <summary>
    ///     数据项目键
    /// </summary>
    public required string Key { get; set; }

    /// <summary>
    ///     数据项目值
    /// </summary>
    public required string Value { get; set; }

    /// <summary>
    ///     数据项目描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    ///     数据项目是否有效
    /// </summary>
    public bool Valid { get; set; }
}