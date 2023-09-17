using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Artemis.Data.Core;

namespace Artemis.Shared.Identity.Models;

/// <summary>
/// 凭据字典
/// </summary>
[DataContract]
public class Claim : IKeySlot<Guid>
{
    /// <summary>
    ///     角色标识
    /// </summary>
    [DataMember(Order = 1)]
    public Guid Id { get; set; }

    /// <summary>
    ///     凭据类型
    /// </summary>
    [DataMember(Order = 2)]
    [MaxLength(32)]
    [Required]
    public string ClaimType { get; set; } = null!;

    /// <summary>
    ///     凭据值
    /// </summary>
    [DataMember(Order = 3)]
    [MaxLength(128)]
    [Required]
    public string ClaimValue { get; set; } = null!;

    /// <summary>
    /// 校验戳
    /// </summary>
    [DataMember(Order = 4)]
    [MaxLength(64)]
    [Required]
    public string CheckStamp { get; set; } = null!;

    /// <summary>
    ///     凭据描述
    /// </summary>
    [DataMember(Order = 5)]
    [MaxLength(128)]
    public string? Description { get; set; }
}