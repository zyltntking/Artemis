using Microsoft.AspNetCore.Authorization;

namespace Artemis.Extensions.Web.Identity;

#region interface

/// <summary>
///     Artemis认证策略接口
/// </summary>
public interface IArtemisIdentityRequirement : IAuthorizationRequirement
{
}

#endregion

/// <summary>
///     Artemis认证策略实现
/// </summary>
public abstract class IdentityRequirement : IArtemisIdentityRequirement
{
}

/// <summary>
///     匿名许可要求
/// </summary>
public sealed class AnonymousRequirement : IdentityRequirement
{
    /// <summary>
    ///     匿名认证策略构造
    /// </summary>
    public AnonymousRequirement()
    {
    }
}

/// <summary>
///     令牌认证策略
/// </summary>
public abstract class TokenRequirement : IdentityRequirement
{
}

/// <summary>
///     仅Token认证要求
/// </summary>
public sealed class TokenOnlyRequirement : TokenRequirement
{
}

/// <summary>
///     角色认证要求
/// </summary>
public sealed class RolesRequirement : TokenRequirement
{
    /// <summary>
    ///     认证策略构造
    /// </summary>
    /// <param name="roles">角色</param>
    public RolesRequirement(IEnumerable<string> roles)
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
public sealed class ClaimsRequirement : TokenRequirement
{
    /// <summary>
    ///     认证策略构造
    /// </summary>
    /// <param name="claims">凭据</param>
    public ClaimsRequirement(IEnumerable<KeyValuePair<string, string>> claims)
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
}

/// <summary>
///     路由路径凭据认证要求
/// </summary>
public sealed class RoutePathClaimRequirement : TokenRequirement
{
}