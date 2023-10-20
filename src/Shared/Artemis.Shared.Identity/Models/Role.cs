using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Shared.Identity.Transfer;
using Microsoft.AspNetCore.Identity;

namespace Artemis.Shared.Identity.Models;

/// <summary>
///     角色
/// </summary>
public class Role : IdentityRole<Guid>, IKeySlot, IConcurrencyStamp, IRole
{
    /// <summary>
    ///     并发锁
    /// </summary>
    [MaxLength(64)]
    public override string? ConcurrencyStamp { get; set; }

    /// <summary>
    ///     标识
    /// </summary>
    [Required]
    public override required Guid Id { get; set; }

    /// <summary>
    ///     角色名
    /// </summary>
    [Required]
    [MaxLength(32)]
    public override required string Name { get; set; }

    /// <summary>
    ///     角色描述
    /// </summary>
    [MaxLength(128)]
    public virtual string? Description { get; set; }

    /// <summary>
    ///     规范化角色名
    /// </summary>
    [Required]
    [MaxLength(32)]
    public override required string NormalizedName { get; set; }
}