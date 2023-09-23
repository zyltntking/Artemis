using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Artemis.Shared.Identity.Transfer.Interface;

namespace Artemis.Shared.Identity.Transfer.Base;

/// <summary>
///     基本角色凭据信息
/// </summary>
[DataContract]
public record RoleClaimBase : IRoleClaim
{
    #region Implementation of IRoleClaim

    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public virtual required Guid RoleId { get; set; }

    /// <summary>
    ///     凭据类型
    /// </summary>
    [Required]
    [MaxLength(32)]
    [DataMember(Order = 2)]
    public virtual string ClaimType { get; set; } = null!;

    /// <summary>
    ///     凭据值
    /// </summary>
    [Required]
    [MaxLength(128)]
    [DataMember(Order = 3)]
    public virtual string ClaimValue { get; set; } = null!;

    /// <summary>
    ///     校验戳
    /// </summary>
    [Required]
    [MaxLength(64)]
    [DataMember(Order = 4)]
    public virtual string CheckStamp { get; set; } = null!;

    /// <summary>
    ///     凭据描述
    /// </summary>
    [MaxLength(128)]
    [DataMember(Order = 5)]
    public virtual string? Description { get; set; }

    #endregion
}