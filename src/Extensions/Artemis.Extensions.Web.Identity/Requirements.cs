using Microsoft.AspNetCore.Authorization;

namespace Artemis.Extensions.Web.Identity;

#region interface

/// <summary>
///     Artemis认证策略接口
/// </summary>
public interface IArtemisIdentityRequirement : IAuthorizationRequirement
{
    /// <summary>
    ///     认证等级
    /// </summary>
    ArtemisIdentityLevel Level => ArtemisIdentityLevel.Anonymous;
}

/// <summary>
///     Artemis Token认证策略接口
/// </summary>
file interface IArtemisIdentityTokenRequirement : IArtemisIdentityRequirement
{
    /// <summary>
    ///     令牌键
    /// </summary>
    string HeaderTokenKey => Constants.HeaderTokenKey;

    /// <summary>
    ///     缓存令牌前缀
    /// </summary>
    string CacheTokenPrefix => Constants.CacheTokenPrefix;
}

#endregion

/// <summary>
///     Artemis认证策略实现
/// </summary>
public abstract class IdentityRequirement : IArtemisIdentityRequirement
{
    /// <summary>
    ///     认证策略构造
    /// </summary>
    /// <param name="level"></param>
    protected IdentityRequirement(ArtemisIdentityLevel level)
    {
        Level = level;
    }

    /// <summary>
    ///     认证等级
    /// </summary>
    public ArtemisIdentityLevel Level { get; }
}

/// <summary>
///     匿名许可要求
/// </summary>
public sealed class AnonymousRequirement : IdentityRequirement
{
    /// <summary>
    ///     匿名认证策略构造
    /// </summary>
    public AnonymousRequirement() : base(ArtemisIdentityLevel.Anonymous)
    {
    }
}

/// <summary>
///     令牌认证策略
/// </summary>
public class TokenRequirement : IdentityRequirement, IArtemisIdentityTokenRequirement
{
    /// <summary>
    ///     认证策略构造
    /// </summary>
    /// <param name="headerKey">header key</param>
    /// <param name="cacheTokenPrefix">cache token prefix</param>
    public TokenRequirement(
        string headerKey = Constants.HeaderTokenKey,
        string cacheTokenPrefix = Constants.CacheTokenPrefix) : base(ArtemisIdentityLevel.Token)
    {
        HeaderTokenKey = headerKey;
        CacheTokenPrefix = cacheTokenPrefix;
    }

    /// <summary>
    ///     HeaderTokenKey
    /// </summary>
    public string HeaderTokenKey { get; }

    /// <summary>
    ///     CacheTokenPrefix
    /// </summary>
    public string CacheTokenPrefix { get; }
}

/// <summary>
///     角色认证要求
/// </summary>
public class RolesRequirement : TokenRequirement
{
    /// <summary>
    ///     认证策略构造
    /// </summary>
    /// <param name="roles">角色</param>
    /// <param name="headerKey">Header Token键</param>
    /// <param name="cacheTokenPrefix">缓存 Token键</param>
    public RolesRequirement(
        IEnumerable<string> roles,
        string headerKey = Constants.HeaderTokenKey,
        string cacheTokenPrefix = Constants.CacheTokenPrefix) : base(headerKey, cacheTokenPrefix)
    {
        Roles = roles;
    }

    /// <summary>
    ///     角色
    /// </summary>
    public IEnumerable<string> Roles { get; }
}

/// <summary>
///     凭据认证要求
/// </summary>
public class ClaimRequirement : TokenRequirement
{
    /// <summary>
    ///     认证策略构造
    /// </summary>
    /// <param name="claims">凭据</param>
    /// <param name="headerKey">Header Token键</param>
    /// <param name="cacheTokenPrefix">缓存 Token键</param>
    public ClaimRequirement(
        IEnumerable<KeyValuePair<string, string>> claims,
        string headerKey = Constants.HeaderTokenKey,
        string cacheTokenPrefix = Constants.CacheTokenPrefix) : base(headerKey, cacheTokenPrefix)
    {
        Claims = claims;
    }

    /// <summary>
    ///     凭据键值对集合
    /// </summary>
    public IEnumerable<KeyValuePair<string, string>> Claims { get; }
}

/// <summary>
///     操作名凭据认证要求
/// </summary>
public sealed class ActionNameClaimRequirement : TokenRequirement
{
    /// <summary>
    ///     认证策略构造
    /// </summary>
    /// <param name="headerKey">Header Token键</param>
    /// <param name="cacheTokenPrefix">缓存 Token键</param>
    public ActionNameClaimRequirement(
        string headerKey = Constants.HeaderTokenKey,
        string cacheTokenPrefix = Constants.CacheTokenPrefix) : base(headerKey, cacheTokenPrefix)
    {
    }
}

/// <summary>
///     路由路径凭据认证要求
/// </summary>
public sealed class RoutePathClaimRequirement : TokenRequirement
{
    /// <summary>
    ///     认证策略构造
    /// </summary>
    /// <param name="headerKey">Header Token键</param>
    /// <param name="cacheTokenPrefix">缓存 Token键</param>
    public RoutePathClaimRequirement(
        string headerKey = Constants.HeaderTokenKey,
        string cacheTokenPrefix = Constants.CacheTokenPrefix) : base(headerKey, cacheTokenPrefix)
    {
    }
}