using System.ComponentModel.DataAnnotations;

namespace Artemis.Data.Core.Fundamental.Model;

#region Interface

/// <summary>
///     元数据接口
/// </summary>
public interface IMetadata
{
    /// <summary>
    ///     数据键
    /// </summary>
    string Key { get; set; }

    /// <summary>
    ///     数据值
    /// </summary>
    string? Value { get; set; }
}

/// <summary>
///     数据字典
/// </summary>
public interface IDataDict : IMetadata
{
    /// <summary>
    ///     数据标签
    /// </summary>
    string? Label { get; set; }

    /// <summary>
    ///     数据排序
    /// </summary>
    int Order { get; set; }

    /// <summary>
    ///     数据类型
    /// </summary>
    string? Type { get; set; }

    /// <summary>
    ///     数据是否锁定
    /// </summary>
    bool Lock { get; set; }

    /// <summary>
    ///     数据状态
    /// </summary>
    bool Status { get; set; }

    /// <summary>
    ///     数据描述
    /// </summary>
    string? Description { get; set; }
}

#endregion

/// <summary>
///     元数据信息
/// </summary>
file abstract class MetadataInfo : IMetadata
{
    #region Implementation of IMeta

    /// <summary>
    ///     元数据键
    /// </summary>
    [Required]
    public virtual required string Key { get; set; }

    /// <summary>
    ///     元数据值
    /// </summary>
    public virtual string? Value { get; set; }

    #endregion
}

/// <summary>
///     数据字典信息
/// </summary>
file abstract class DataDictInfo : MetadataInfo, IDataDict
{
    #region Implementation of IDataDict

    /// <summary>
    ///     数据标签
    /// </summary>
    public string? Label { get; set; }

    /// <summary>
    ///     数据排序
    /// </summary>
    [Required]
    public required int Order { get; set; }

    /// <summary>
    ///     数据类型
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    ///     数据是否锁定
    /// </summary>
    [Required]
    public required bool Lock { get; set; }

    /// <summary>
    ///     数据状态
    /// </summary>
    [Required]
    public required bool Status { get; set; }

    /// <summary>
    ///     数据描述
    /// </summary>
    public string? Description { get; set; }

    #endregion
}