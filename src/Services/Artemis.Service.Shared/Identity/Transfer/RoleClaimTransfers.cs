﻿namespace Artemis.Service.Shared.Identity.Transfer;

/// <summary>
///     用户凭据信息
/// </summary>
public record RoleClaimInfo : RoleClaimPackage, IRoleClaimInfo
{
    /// <summary>
    ///     存储标识
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     角色标识
    /// </summary>
    public Guid RoleId { get; set; }

    /// <summary>
    ///     校验戳
    /// </summary>
    public required string CheckStamp { get; set; }
}

/// <summary>
///     用户凭据数据包
/// </summary>
public record RoleClaimPackage : ClaimPackage, IRoleClaimPackage;