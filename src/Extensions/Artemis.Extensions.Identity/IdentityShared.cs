using Artemis.Data.Core.Fundamental.Types;

namespace Artemis.Extensions.Identity;

/// <summary>
///     共享键
/// </summary>
internal static class IdentityShared
{
    /// <summary>
    ///     授权消息
    /// </summary>
    public const string AuthorizationMessage = nameof(AuthorizationMessage);

    /// <summary>
    ///     Token架构
    /// </summary>
    public const string Schema = "Artemis";

    /// <summary>
    ///     令牌
    /// </summary>
    public const string Token = "Authorization";

    /// <summary>
    ///     缓存名空间
    /// </summary>
    public const string CacheSpace = "Artemis:Identity";

    /// <summary>
    ///     用户对Token的映射
    /// </summary>
    public const string UserMapToken = nameof(UserMapToken);

    /// <summary>
    ///     默认服务提供者
    /// </summary>
    public const string DefaultServiceProvider = "ArtemisIdentityService";
}

/// <summary>
///     预定义的Artemis授权策略
/// </summary>
public static class AuthorizePolicy
{
    /// <summary>
    ///     匿名策略
    /// </summary>
    public const string Anonymous = nameof(IdentityPolicy.Anonymous);

    /// <summary>
    ///     令牌策略
    /// </summary>
    public const string Token = nameof(IdentityPolicy.Token);

    /// <summary>
    ///     管理员策略
    /// </summary>
    public const string Admin = nameof(IdentityPolicy.Admin);

    /// <summary>
    ///     操作名策略
    /// </summary>
    public const string ActionName = nameof(IdentityPolicy.ActionName);

    /// <summary>
    ///     路由路径策略
    /// </summary>
    public const string RoutePath = nameof(IdentityPolicy.RoutePath);

    /// <summary>
    ///     需求Token策略
    /// </summary>
    public static readonly string[] TokenPolicies = [Token, Admin, ActionName, RoutePath];
}