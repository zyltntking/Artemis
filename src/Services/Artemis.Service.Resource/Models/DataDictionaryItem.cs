using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Data.Shared.Resource;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Resource.Models;

/// <summary>
///     数据字典项目模型
/// </summary>
public class DataDictionaryItem : ConcurrencyModel, IDataDictionaryItem
{
    /// <summary>
    ///     数据字典标识
    /// </summary>
    [Required]
    [Comment("数据字典标识")]
    public required Guid DataDictionaryId { get; set; }

    #region Implementation of IDataDictionaryItem

    /// <summary>
    ///     数据项目键
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("数据项目键")]
    public required string Key { get; set; }

    /// <summary>
    ///     数据项目值
    /// </summary>
    [Required]
    [MaxLength(256)]
    [Comment("数据项目值")]
    public required string Value { get; set; }

    /// <summary>
    ///     数据项目描述
    /// </summary>
    [MaxLength(256)]
    [Comment("数据项目描述")]
    public string? Description { get; set; }

    /// <summary>
    ///     数据项目是否有效
    /// </summary>
    [Required]
    [Comment("数据项目是否有效")]
    public required bool Valid { get; set; }

    #endregion
}