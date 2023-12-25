using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;

namespace Artemis.Shared.Identity.Transfer;

#region Interface

/// <summary>
///     基本角色凭据接口
/// </summary>
internal interface IRoleClaim : IClaim
{
}

/// <summary>
///     角色凭据信息接口
/// </summary>
file interface IRoleClaimInfo : IRoleClaim
{
    /// <summary>
    ///     角色标识
    /// </summary>
    Guid RoleId { get; set; }
}

#endregion

/// <summary>
///     基本角色凭据信息
/// </summary>
public record RoleClaimPackage : ClaimPackage, IRoleClaim;

/// <summary>
///     角色凭据信息
/// </summary>
public sealed record RoleClaimInfo : RoleClaimPackage, IRoleClaimInfo, IKeySlot<int>
{
    /// <summary>
    ///     标识
    /// </summary>
    [Required]
    public required int Id { get; set; }

    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    public required Guid RoleId { get; set; }
}