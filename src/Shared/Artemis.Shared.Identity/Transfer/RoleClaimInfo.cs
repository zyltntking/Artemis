using Artemis.Data.Core;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Artemis.Shared.Identity.Transfer;

/// <summary>
/// 角色凭据信息
/// </summary>
[DataContract]
public record RoleClaimInfo : RoleClaimBase, IKeySlot<int>
{
    /// <summary>
    ///     标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public required int Id { get; set; }

    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public override required Guid RoleId { get; set; }

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
    public override required string CheckStamp { get; set; }

    /// <summary>
    ///     凭据描述
    /// </summary>
    [MaxLength(128)]
    [DataMember(Order = 6)]
    public override string? Description { get; set; }
}