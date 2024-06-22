using Artemis.Data.Shared.Transfer.Identity;

namespace Artemis.Extensions.ServiceConnect.Authorization;

#region Interface

/// <summary>
///     Artemis认证选项接口
/// </summary>
file interface IArtemisAuthorizationConfig
{
    /// <summary>
    /// 上下文项目Token键
    /// </summary>
    string ContextItemTokenKey { get; set; }

    /// <summary>
    ///     请求头认证Token键
    /// </summary>
    string RequestHeaderTokenKey { get; set; }

    /// <summary>
    ///     缓存认证Token前缀
    /// </summary>
    string CacheTokenPrefix { get; set; }

    /// <summary>
    ///     用户对Token映射缓存键前缀
    /// </summary>
    string CacheUserMapTokenPrefix { get; set; }

    /// <summary>
    ///     过期时间
    /// </summary>
    int CacheTokenExpire { get; set; }

    /// <summary>
    ///     是否启用多终端
    /// </summary>
    bool EnableMultiEnd { get; set; }

    /// <summary>
    ///     认证服务对提供者名称
    /// </summary>
    string IdentityServiceProvider { get; set; }

    /// <summary>
    /// 支持的策略
    /// </summary>
    IEnumerable<string> Policies { get; }

    /// <summary>
    ///     是否启用高级策略
    /// </summary>
    bool EnableAdvancedPolicy { get; set; }

    /// <summary>
    ///     基于角色的策略配置
    /// </summary>
    public IEnumerable<RolesBasedPolicyOptions>? RolesBasedPolicyOptions { get; set; }

    /// <summary>
    ///     基于凭据的策略配置
    /// </summary>
    public IEnumerable<ClaimsBasedPolicyOptions>? ClaimsBasedPolicyOptions { get; set; }
}

/// <summary>
///     策略选项接口
/// </summary>
file interface IPolicyOptions
{
    string Name { get; set; }
}

#endregion

/// <summary>
///     传递认证选项
/// </summary>
public class ArtemisAuthorizationConfig : IArtemisAuthorizationConfig
{
    #region Implementation of IArtemisIdentityOptions

    /// <summary>
    /// 上下文项目Token键
    /// </summary>
    public string ContextItemTokenKey { get; set; } = SharedKey.Token;

    /// <summary>
    ///     请求头Token键
    /// </summary>
    public string RequestHeaderTokenKey { get; set; } = SharedKey.Token;

    /// <summary>
    ///     缓存Token前缀
    /// </summary>
    public string CacheTokenPrefix { get; set; } = $"{SharedKey.CacheSpace}:{SharedKey.Token}";

    /// <summary>
    ///     用户对Token映射缓存键前缀
    /// </summary>
    public string CacheUserMapTokenPrefix { get; set; } = $"{SharedKey.CacheSpace}:{SharedKey.UserMapToken}";

    /// <summary>
    ///     过期时间
    /// </summary>
    public int CacheTokenExpire { get; set; } = 60 * 60 * 24 * 7;

    /// <summary>
    ///     是否启用多终端
    /// </summary>
    public bool EnableMultiEnd { get; set; } = true;

    /// <summary>
    ///     认证服务对提供者名称
    /// </summary>
    public string IdentityServiceProvider { get; set; } = SharedKey.DefaultServiceProvider;

    /// <summary>
    /// 支持的策略
    /// </summary>
    public IEnumerable<string> Policies
    {
        get
        {
            var roleBasedPolicies = RolesBasedPolicyOptions?.Select(item => item.Name);

            var claimBasedPolicies = ClaimsBasedPolicyOptions?.Select(item => item.Name);

            var policies = IdentityPolicy.TokenPolicies.AsEnumerable();

            if (roleBasedPolicies != null)
            {
                policies = policies.Concat(roleBasedPolicies);
            }

            if (claimBasedPolicies != null)
            {
                policies = policies.Concat(claimBasedPolicies);
            }

            return policies;
        }
    }

    /// <summary>
    ///     是否启用高级策略
    /// </summary>
    public bool EnableAdvancedPolicy { get; set; }

    /// <summary>
    ///     基于角色的策略配置
    /// </summary>
    public IEnumerable<RolesBasedPolicyOptions>? RolesBasedPolicyOptions { get; set; }

    /// <summary>
    ///     基于凭据的策略配置
    /// </summary>
    public IEnumerable<ClaimsBasedPolicyOptions>? ClaimsBasedPolicyOptions { get; set; }

    #endregion
}

/// <summary>
///     策略配置
/// </summary>
public abstract class PolicyOptions : IPolicyOptions
{
    /// <summary>
    ///     策略名称
    /// </summary>
    public required string Name { get; set; }
}

/// <summary>
///     基于角色的策略配置
/// </summary>
public sealed class RolesBasedPolicyOptions : PolicyOptions
{
    /// <summary>
    ///     策略支持的角色列表
    /// </summary>
    public required IEnumerable<string> Roles { get; set; } = new List<string>();
}

/// <summary>
///     基于凭据的策略配置
/// </summary>
public sealed class ClaimsBasedPolicyOptions : PolicyOptions
{
    /// <summary>
    ///     策略支持的凭据字典
    /// </summary>
    public required IDictionary<string, string> Claims { get; set; } = new Dictionary<string, string>();
}