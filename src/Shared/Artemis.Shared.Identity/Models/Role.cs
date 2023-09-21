using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Artemis.Data.Core;
using Artemis.Shared.Identity.Transfer.Interface;
using Microsoft.AspNetCore.Identity;

namespace Artemis.Shared.Identity.Models;

/// <summary>
///     角色
/// </summary>
[DataContract]
public class Role : IdentityRole<Guid>, IKeySlot, IRole
{
    /// <summary>
    ///     标识
    /// </summary>
    [DataMember(Order = 1)]
    public override Guid Id { get; set; }

    /// <summary>
    ///     角色名
    /// </summary>
    [DataMember(Order = 2)]
    [Required]
    [MaxLength(32)]
    public override string Name { get; set; } = null!;

    /// <summary>
    ///     角色描述
    /// </summary>
    [DataMember(Order = 5)]
    [MaxLength(128)]
    public virtual string? Description { get; set; }

    /// <summary>
    ///     规范化角色名
    /// </summary>
    [DataMember(Order = 3)]
    [Required]
    [MaxLength(32)]
    public override string NormalizedName { get; set; } = null!;

    /// <summary>
    ///     并发锁
    /// </summary>
    [DataMember(Order = 4)]
    [MaxLength(64)]
    public override string? ConcurrencyStamp { get; set; }
}