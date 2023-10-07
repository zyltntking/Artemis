using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Artemis.Data.Core;
using Artemis.Shared.Identity.Transfer;
using Microsoft.AspNetCore.Identity;

namespace Artemis.Shared.Identity.Models;

/// <summary>
///     角色
/// </summary>
[DataContract]
public class Role : IdentityRole<Guid>, IKeySlot, IConcurrencyStamp, IRole
{
    /// <summary>
    ///     并发锁
    /// </summary>
    [MaxLength(64)]
    [DataMember(Order = 4)]
    public override string? ConcurrencyStamp { get; set; }

    /// <summary>
    ///     标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required Guid Id { get; set; }

    /// <summary>
    ///     角色名
    /// </summary>
    [Required]
    [MaxLength(32)]
    [DataMember(Order = 2)]
    public override required string Name { get; set; }

    /// <summary>
    ///     角色描述
    /// </summary>
    [MaxLength(128)]
    [DataMember(Order = 5)]
    public virtual string? Description { get; set; }

    /// <summary>
    ///     规范化角色名
    /// </summary>
    [Required]
    [MaxLength(32)]
    [DataMember(Order = 3)]
    public override required string NormalizedName { get; set; }
}