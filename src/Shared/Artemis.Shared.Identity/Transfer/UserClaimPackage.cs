using System.ComponentModel.DataAnnotations;
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
public record UserClaimPackage : ClaimPackage, IUserClaim;

/// <summary>
///     用户凭据信息
/// </summary>
public sealed record UserClaimInfo : UserClaimPackage, IUserClaimDocument, IKeySlot<int>
{
    /// <summary>
    ///     标识
    /// </summary>
    [Required]
    public required int Id { get; set; }

    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    public required Guid UserId { get; set; }
}