using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Artemis.Data.Core;
using Artemis.Shared.Identity.Transfer;
using Microsoft.AspNetCore.Identity;

namespace Artemis.Shared.Identity.Models;

/// <summary>
///     用户凭据
/// </summary>
[DataContract]
public class UserClaim : IdentityUserClaim<Guid>, IKeySlot<int>, IUserClaim
{
    /// <summary>
    ///     标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required int Id { get; set; }

    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public override required Guid UserId { get; set; }

    /// <summary>
    ///     凭据类型
    /// </summary>
    [Required]
    [MaxLength(128)]
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
}