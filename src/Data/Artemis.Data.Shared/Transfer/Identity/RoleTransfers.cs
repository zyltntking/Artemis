﻿using Artemis.Data.Shared.Identity;

namespace Artemis.Data.Shared.Transfer.Identity;

/// <summary>
///     基本角色信息
/// </summary>
public record RolePackage : IRoleInfo
{
    #region Implementation of IRole

    /// <summary>
    ///     角色名
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    ///     角色描述
    /// </summary>
    public string? Description { get; set; }

    #endregion
}

/// <summary>
///     角色信息
/// </summary>
public sealed record RoleInfo : RolePackage
{
    /// <summary>
    ///     角色标识
    /// </summary>
    public required Guid Id { get; set; }
}