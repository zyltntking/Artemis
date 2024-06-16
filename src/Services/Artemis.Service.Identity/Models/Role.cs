using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Data.Shared.Identity;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Identity.Models;

/// <summary>
///     角色模型
/// </summary>
public class Role : ModelBase, IRole, IConcurrencyStamp
{
    /// <summary>
    ///     并发锁
    /// </summary>
    [MaxLength(64)]
    [Comment("并发锁")]
    public string? ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    ///     角色名
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("角色名")]
    public required string Name { get; set; }

    /// <summary>
    ///     标准化角色名
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("标准化角色名")]
    public required string NormalizedName { get; set; }

    /// <summary>
    ///     角色描述
    /// </summary>
    [MaxLength(256)]
    [Comment("角色描述")]
    public virtual string? Description { get; set; }
}