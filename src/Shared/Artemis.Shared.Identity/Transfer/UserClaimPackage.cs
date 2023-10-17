using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Artemis.Data.Core;

namespace Artemis.Shared.Identity.Transfer;

#region Interface

/// <summary>
///     基本用户凭据接口
/// </summary>
internal interface IUserClaim : IClaim
{
}

/// <summary>
///     用户凭据文档接口
/// </summary>
file interface IUserClaimDocument : IUserClaim
{
    /// <summary>
    ///     用户标识
    /// </summary>
    Guid UserId { get; set; }
}

#endregion

/// <summary>
///     基本用户凭据信息
/// </summary>
[DataContract]
public record UserClaimPackage : ClaimPackage, IUserClaim
{
    #region Implementation of IRoleClaim

    /// <summary>
    ///     凭据类型
    /// </summary>
    [Required]
    [MaxLength(32)]
    [DataMember(Order = 1)]
    public override required string ClaimType { get; set; }

    /// <summary>
    ///     凭据值
    /// </summary>
    [Required]
    [MaxLength(128)]
    [DataMember(Order = 2)]
    public override required string ClaimValue { get; set; }

    /// <summary>
    ///     校验戳
    /// </summary>
    [Required]
    [MaxLength(64)]
    [DataMember(Order = 3)]
    public override required string CheckStamp { get; set; }

    /// <summary>
    ///     凭据描述
    /// </summary>
    [MaxLength(128)]
    [DataMember(Order = 4)]
    public override string? Description { get; set; }

    #endregion
}

/// <summary>
///     用户凭据信息
/// </summary>
[DataContract]
public record UserClaimInfo : UserClaimPackage, IUserClaimDocument, IKeySlot<int>
{
    /// <summary>
    ///     标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public virtual required int Id { get; set; }

    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public virtual required Guid UserId { get; set; }

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