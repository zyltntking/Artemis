using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Artemis.Data.Core;
using Microsoft.AspNetCore.Identity;

namespace Artemis.Shared.Identity.Models;

/// <summary>
///     角色凭据
/// </summary>
[DataContract]
public class RoleClaim : IdentityRoleClaim<Guid>, IKeySlot<int>
{
    /// <summary>
    ///     标识
    /// </summary>
    [DataMember(Order = 1)]
    public override int Id { get; set; }

    /// <summary>
    ///     角色标识
    /// </summary>
    [DataMember(Order = 2)]
    public override Guid RoleId { get; set; }

    /// <summary>
    ///     凭据类型
    /// </summary>
    [DataMember(Order = 3)]
    [MaxLength(32)]
    [Required]
    public override string ClaimType { get; set; } = null!;

    /// <summary>
    ///     凭据值
    /// </summary>
    [DataMember(Order = 4)]
    [MaxLength(128)]
    [Required]
    public override string ClaimValue { get; set; } = null!;

    /// <summary>
    ///     校验戳
    /// </summary>
    [DataMember(Order = 5)]
    [MaxLength(64)]
    [Required]
    public virtual string CheckStamp { get; set; } = null!;

    /// <summary>
    ///     凭据描述
    /// </summary>
    [DataMember(Order = 6)]
    [MaxLength(128)]
    public virtual string? Description { get; set; }
}