namespace Artemis.Extensions.ServiceConnect.Authorization;

/// <summary>
///     共享键
/// </summary>
internal static class SharedKey
{
    /// <summary>
    ///     授权消息
    /// </summary>
    public const string AuthorizationMessage = nameof(AuthorizationMessage);

    /// <summary>
    ///     令牌
    /// </summary>
    public const string Token = nameof(Token);

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