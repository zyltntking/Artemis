using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Data.Shared.Resource;
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
    [MaxLength(128)]
    [Comment("行政区划代码")]
    public required string Code { get; set; }

    /// <summary>
    ///     行政区划级别
    /// </summary>
    [Required]
    [Comment("行政区划级别")]
    public required int Level { get; set; }

    /// <summary>
    ///     父级行政区划标识
    /// </summary>
    [Comment("父级行政区划标识")]
    public Guid ParentId { get; set; } = Guid.Empty;
}