﻿using Artemis.Data.Shared.Identity;

namespace Artemis.Data.Shared.Transfer.Identity;

/// <summary>
///     用户登录信息
/// </summary>
public record UserLoginInfo : UserLoginPackage, IUserLoginInfo
{
    #region Implementation of IUserLoginInfo

    /// <summary>
    ///     用户标识
    /// </summary>
    public Guid UserId { get; set; }

    #endregion
}

/// <summary>
///     用户登录数据包
/// </summary>
public record UserLoginPackage : IUserLoginPackage
{
    #region Implementation of IUserLoginPackage

    /// <summary>
    ///     登录提供程序
    /// </summary>
    public required string LoginProvider { get; set; }

    /// <summary>
    ///     提供程序密钥
    /// </summary>
    public required string ProviderKey { get; set; }

    /// <summary>
    ///     提供程序显示名称
    /// </summary>
    public string? ProviderDisplayName { get; set; }

    #endregion
}