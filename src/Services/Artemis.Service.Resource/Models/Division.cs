using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Service.Shared.Resource;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Resource.Models;

/// <summary>
///     行政区划模型
/// </summary>
public class Division : ConcurrencyModel, IDivision
{
    /// <summary>
    ///     行政区划名称
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("行政区划名称")]
    public required string Name { get; set; }

    /// <summary>
    ///     行政区划代码
    /// </summary>
    [Required]
    [MaxLength(32)]
    [Comment("行政区划代码")]
    public required string Code { get; set; }

    /// <summary>
    ///     行政区划级别
    /// </summary>
    [Required]
    [Comment("行政区划级别")]
    public required int Level { get; set; }

    /// <summary>
    ///     行政区划类型
    /// </summary>
    [Required]
    [MaxLength(32)]
    [Comment("行政区划类型")]
    public required string Type { get; set; }

    /// <summary>
    ///     行政区划全名
    /// </summary>
    [MaxLength(128)]
    [Comment("行政区划全名")]
    public string? FullName { get; set; }

    /// <summary>
    ///     行政区划拼音
    /// </summary>
    [MaxLength(128)]
    [Comment("行政区划拼音")]
    public string? Pinyin { get; set; }

    /// <summary>
    ///     行政区划备注
    /// </summary>
    [MaxLength(512)]
    [Comment("行政区划备注")]
    public string? Remark { get; set; }

    /// <summary>
    ///     上级行政区划标识
    /// </summary>
    [Comment("上级行政区划标识")]
    public Guid? ParentId { get; set; }
}