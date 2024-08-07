using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Service.Shared.Resource;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Resource.Models;

/// <summary>
///     标准项目
/// </summary>
public class StandardItem : ConcurrencyModel, IStandardItem
{
    /// <summary>
    ///     标准目录标识
    /// </summary>
    [Required]
    [Comment("标准目录标识")]
    public Guid StandardCatalogId { get; set; }

    /// <summary>
    ///     标准项目名称
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("标准项目名称")]
    public required string Name { get; set; }

    /// <summary>
    ///     标准项目编码
    /// </summary>
    [MaxLength(32)]
    [Comment("标准项目编码")]
    public string? Code { get; set; }

    /// <summary>
    ///     标准项目最小值
    /// </summary>
    [Required]
    [Comment("标准项目最小值")]
    public int Minimum { get; set; }

    /// <summary>
    ///     标准项目最大值
    /// </summary>
    [Required]
    [Comment("标准项目最大值")]
    public int Maximum { get; set; }

    /// <summary>
    ///     标准项目模板
    /// </summary>
    [MaxLength(512)]
    [Comment("标准项目模板")]
    public string? Template { get; set; }

    /// <summary>
    ///     标准项目描述
    /// </summary>
    [MaxLength(128)]
    [Comment("标准项目描述")]
    public string? Description { get; set; }
}