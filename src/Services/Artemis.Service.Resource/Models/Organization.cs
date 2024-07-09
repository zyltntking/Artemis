using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Data.Shared.Resource;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Resource.Models;

/// <summary>
///     组织机构
/// </summary>
public class Organization : ConcurrencyModel, IOrganization
{
    /// <summary>
    ///     机构名称
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("机构名称")]
    public required string Name { get; set; }

    /// <summary>
    ///     机构地址
    /// </summary>
    [MaxLength(256)]
    [Comment("机构地址")]
    public string? Address { get; set; }

    /// <summary>
    ///     父级机构标识
    /// </summary>
    [Comment("父级机构标识")]
    public Guid ParentId { get; set; } = Guid.Empty;
}