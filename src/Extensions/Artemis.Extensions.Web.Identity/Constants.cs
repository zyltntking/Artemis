﻿namespace Artemis.Extensions.Web.Identity;

/// <summary>
///     常量
/// </summary>
public static class Constants
{
    /// <summary>
    ///     头Token键
    /// </summary>
    public const string HeaderTokenKey = "Token";

    /// <summary>
    ///     Token缓存键前缀
    /// </summary>
    public const string CacheTokenPrefix = "Artemis:Identity:Token";

    /// <summary>
    ///     用户对Token映射缓存键前缀
    /// </summary>
    public const string UserMapTokenPrefix = "Artemis:Identity:UserMapToken";

    /// <summary>
    ///     上下文项目键
    /// </summary>
    internal const string ContextItemKey = "Token";
}

/// <summary>
///     程序集内共享键
/// </summary>
internal static class SharedKey
{
    /// <summary>
    ///     认证消息
    /// </summary>
    internal const string AuthMessage = nameof(AuthMessage);
}

/// <summary>
///     认证策略
/// </summary>
public static class IdentityPolicy
{
    /// <summary>
    ///     匿名策略名
    /// </summary>
    public const string Anonymous = nameof(Anonymous);

    /// <summary>
    ///     令牌认证策略名
    /// </summary>
    public const string Token = nameof(Token);

    /// <summary>
    ///     管理员角色认证策略名
    /// </summary>
    public const string Admin = nameof(Admin);

    /// <summary>
    ///     操作名凭据认证策略名
    /// </summary>
    public const string ActionName = nameof(ActionName);

    /// <summary>
    ///     路由路径认证凭据策略名
    /// </summary>
    public const string RoutePath = nameof(RoutePath);
}