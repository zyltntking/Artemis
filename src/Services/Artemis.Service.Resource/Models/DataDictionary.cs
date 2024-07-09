using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Data.Shared.Resource;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Resource.Models;

/// <summary>
///     数据字典模型
/// </summary>
public class DataDictionary : ConcurrencyModel, IDataDictionary
{
    #region Implementation of IDataDictionary

    /// <summary>
    ///     字典名称
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("字典名称")]
    public required string Name { get; set; }

    /// <summary>
    ///     字典代码
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("字典代码")]
    public required string Code { get; set; }

    /// <summary>
    ///     字典描述
    /// </summary>
    [MaxLength(256)]
    [Comment("字典描述")]
    public string? Description { get; set; }

    /// <summary>
    ///     字典类型
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("字典类型")]
    public required string Type { get; set; }

    /// <summary>
    ///     字典是否有效
    /// </summary>
    [Required]
    [Comment("字典是否有效")]
    public bool Valid { get; set; }

    #endregion
}