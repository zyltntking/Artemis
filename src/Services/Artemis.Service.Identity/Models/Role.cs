using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Service.Shared.Identity;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Identity.Models;

/// <summary>
///     角色模型
/// </summary>
public class Role : ConcurrencyModel, IRole
{
    /// <summary>
    ///     角色名
    /// </summary>
    [Required]
    [MaxLength(32)]
    [Comment("角色名")]
    public required string Name { get; set; }

    /// <summary>
    ///     标准化角色名
    /// </summary>
    [Required]
    [MaxLength(32)]
    [Comment("标准化角色名")]
    public required string NormalizedName { get; set; }

    /// <summary>
    ///     角色描述
    /// </summary>
    [MaxLength(128)]
    [Comment("角色描述")]
    public virtual string? Description { get; set; }
}