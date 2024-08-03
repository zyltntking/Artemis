namespace Artemis.Service.Shared.Resource.Transfer;

/// <summary>
/// 数据字典信息
/// </summary>
public class DataDictionaryInfo : DataDictionaryPackage, IDataDictionaryInfo
{
    /// <summary>
    ///     存储标识
    /// </summary>
    public Guid Id { get; set; }
}

/// <summary>
/// 数据字典数据包
/// </summary>
public class DataDictionaryPackage : IDataDictionaryPackage
{
    /// <summary>
    ///     字典名称
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    ///     字典代码
    /// </summary>
    public required string Code { get; set; }

    /// <summary>
    ///     字典描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    ///     字典类型
    /// </summary>
    public required string Type { get; set; }

    /// <summary>
    ///     字典是否有效
    /// </summary>
    public bool Valid { get; set; }
}