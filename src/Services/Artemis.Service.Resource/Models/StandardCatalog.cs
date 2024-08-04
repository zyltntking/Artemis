using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Data.Core.Fundamental.Types;
using Artemis.Service.Shared.Resource;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Resource.Models;

/// <summary>
///     标准目录
/// </summary>
public class StandardCatalog : ConcurrencyModel, IStandardCatalog
{
    /// <summary>
    ///     标准目录名称
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("标准目录名称")]
    public required string Name { get; set; }

    /// <summary>
    ///     标准目录编码
    /// </summary>
    [MaxLength(32)]
    [Comment("标准目录编码")]
    public string? Code { get; set; }

    /// <summary>
    ///     标准目录类型
    /// </summary>
    [MaxLength(32)]
    [Comment("标准目录类型")]
    public string? Type { get; set; } = StandardType.VisualStandard;

    /// <summary>
    ///     标准目录状态
    /// </summary>
    [MaxLength(32)]
    [Comment("标准目录状态")]
    public string? State { get; set; } = StandardState.Current;

    /// <summary>
    ///     是否生效
    /// </summary>
    [Required]
    [Comment("是否生效")]
    public bool Valid { get; set; } = true;

    /// <summary>
    ///     标准目录描述
    /// </summary>
    [MaxLength(128)]
    [Comment("标准目录描述")]
    public string? Description { get; set; }
}