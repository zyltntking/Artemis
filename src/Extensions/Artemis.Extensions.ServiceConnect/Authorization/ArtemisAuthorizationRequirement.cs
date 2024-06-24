using Microsoft.AspNetCore.Authorization;

namespace Artemis.Extensions.ServiceConnect.Authorization;

#region interface

/// <summary>
///     Artemis认证策略接口
/// </summary>
internal interface IArtemisAuthorizationRequirement : IAuthorizationRequirement;

#endregion

/// <summary>
///     Artemis认证策略实现
/// </summary>
internal abstract class ArtemisAuthorizationRequirement : IArtemisAuthorizationRequirement;

/// <summary>
///     匿名许可要求
/// </summary>
internal sealed class AnonymousRequirement : ArtemisAuthorizationRequirement
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
internal abstract class TokenRequirement : ArtemisAuthorizationRequirement;

/// <summary>
///     仅Token认证要求
/// </summary>
internal sealed class TokenOnlyRequirement : TokenRequirement;

/// <summary>
///     角色认证要求
/// </summary>
internal class RolesRequirement : TokenRequirement
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
    ///     认证策略构造
    /// </summary>
    /// <param name="roles"></param>
    public RolesRequirement(params string[] roles)
    {
        Roles = roles;
    }

    /// <summary>
    ///     角色
    /// </summary>
    public IEnumerable<string> Roles { get; }
}

/// <summary>
///     角色认证要求
/// </summary>
internal sealed class RoleRequirement : RolesRequirement
{
    /// <summary>
    ///     认证策略构造
    /// </summary>
    /// <param name="role"></param>
    public RoleRequirement(string role) : base(role)
    {
    }
}

/// <summary>
///     凭据认证要求
/// </summary>
internal class ClaimsRequirement : TokenRequirement
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
    ///     认证策略构造
    /// </summary>
    /// <param name="claims"></param>
    public ClaimsRequirement(params KeyValuePair<string, string>[] claims)
    {
        Claims = claims;
    }

    /// <summary>
    ///     凭据键值对集合
    /// </summary>
    public IEnumerable<KeyValuePair<string, string>> Claims { get; }
}

/// <summary>
///     凭据认证要求
/// </summary>
internal sealed class ClaimRequirement : ClaimsRequirement
{
    /// <summary>
    ///     认证策略构造
    /// </summary>
    /// <param name="claim">凭据</param>
    public ClaimRequirement(KeyValuePair<string, string> claim) : base(claim)
    {
    }

    /// <summary>
    ///     认证策略构造
    /// </summary>
    /// <param name="claimKey"></param>
    /// <param name="claimValue"></param>
    public ClaimRequirement(string claimKey, string claimValue) : base(
        new KeyValuePair<string, string>(claimKey, claimValue))
    {
    }
}

/// <summary>
///     操作名凭据认证要求
/// </summary>
internal sealed class ActionNameClaimRequirement : TokenRequirement;

/// <summary>
///     路由路径凭据认证要求
/// </summary>
internal sealed class RoutePathClaimRequirement : TokenRequirement;