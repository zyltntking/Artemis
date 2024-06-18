﻿namespace Artemis.Data.Shared.Identity;

/// <summary>
///     用户登录接口
/// </summary>
public interface IUserLogin
{
    /// <summary>
    ///     用户标识
    /// </summary>
    Guid UserId { get; set; }

    /// <summary>
    ///     登录提供程序
    /// </summary>
    string LoginProvider { get; set; }

    /// <summary>
    ///     提供程序密钥
    /// </summary>
    string ProviderKey { get; set; }

    /// <summary>
    ///     提供程序显示名称
    /// </summary>
    string? ProviderDisplayName { get; set; }
}