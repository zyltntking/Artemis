using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Artemis.Data.Core;
using Artemis.Shared.Identity.Transfer;
using Microsoft.AspNetCore.Identity;

namespace Artemis.Shared.Identity.Models;

/// <summary>
///     角色凭据
/// </summary>
[DataContract]
public class RoleClaim : IdentityRoleClaim<Guid>, IKeySlot<int>, IRoleClaim
{
    /// <summary>
    ///     标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required int Id { get; set; }

    /// <summary>
    ///     凭据类型
    /// </summary>
    [Required]
    [MaxLength(32)]
    [DataMember(Order = 3)]
    public override required string ClaimType { get; set; }

    /// <summary>
    ///     凭据值
    /// </summary>
    [Required]
    [MaxLength(128)]
    [DataMember(Order = 4)]
    public override required string ClaimValue { get; set; }

    /// <summary>
    ///     校验戳
    /// </summary>
    [Required]
    [MaxLength(64)]
    [DataMember(Order = 5)]
    public virtual required string CheckStamp { get; set; }

    /// <summary>
    ///     凭据描述
    /// </summary>
    [MaxLength(128)]
    [DataMember(Order = 6)]
    public virtual string? Description { get; set; }

    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public override required Guid RoleId { get; set; }
}