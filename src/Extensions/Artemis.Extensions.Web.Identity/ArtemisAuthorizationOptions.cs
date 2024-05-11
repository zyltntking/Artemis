namespace Artemis.Extensions.Web.Identity;

#region Interface

/// <summary>
///     Artemis认证选项接口
/// </summary>
file interface IIdentityOptions
{
    /// <summary>
    ///     是否启用高级策略
    /// </summary>
    bool EnableAdvancedPolicy { get; set; }

    /// <summary>
    ///     请求头认证Token键
    /// </summary>
    string HeaderIdentityTokenKey { get; set; }

    /// <summary>
    ///     缓存认证Token前缀
    /// </summary>
    string CacheIdentityTokenPrefix { get; set; }

    /// <summary>
    ///     过期时间
    /// </summary>
    int CacheIdentityTokenExpire { get; set; }

    /// <summary>
    ///     是否启用多终端
    /// </summary>
    bool EnableMultiEnd { get; set; }

    /// <summary>
    ///     认证服务对提供者名称
    /// </summary>
    string IdentityServiceProvider { get; set; }
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
public class SharedIdentityOptions : IIdentityOptions
{
    #region Implementation of IArtemisIdentityOptions

    /// <summary>
    ///     是否启用高级策略
    /// </summary>
    public required bool EnableAdvancedPolicy { get; set; }

    /// <summary>
    ///     请求头Token键
    /// </summary>
    public required string HeaderIdentityTokenKey { get; set; } = Constants.HeaderIdentityTokenKey;

    /// <summary>
    ///     缓存Token前缀
    /// </summary>
    public required string CacheIdentityTokenPrefix { get; set; } = Constants.CacheIdentityTokenPrefix;

    /// <summary>
    ///     用户对Token映射缓存键前缀
    /// </summary>
    public required string UserMapTokenPrefix { get; set; } = Constants.UserMapTokenPrefix;

    /// <summary>
    ///     过期时间
    /// </summary>
    public int CacheIdentityTokenExpire { get; set; }

    /// <summary>
    ///     是否启用多终端
    /// </summary>
    public bool EnableMultiEnd { get; set; }

    /// <summary>
    ///     认证服务对提供者名称
    /// </summary>
    public string IdentityServiceProvider { get; set; } = Constants.IdentityServiceProvider;

    #endregion
}

/// <summary>
///     Artemis认证选项
/// </summary>
public class ArtemisIdentityOptions : SharedIdentityOptions
{
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