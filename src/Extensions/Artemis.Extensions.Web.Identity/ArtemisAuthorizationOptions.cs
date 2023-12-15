namespace Artemis.Extensions.Web.Identity;

#region Interface

/// <summary>
///     Artemis认证选项接口
/// </summary>
file interface IArtemisAuthorizationOptions
{
    /// <summary>
    ///     是否启用高级策略
    /// </summary>
    bool EnableAdvancedPolicy { get; set; }

    /// <summary>
    ///     请求头Token键
    /// </summary>
    string HeaderTokenKey { get; set; }

    /// <summary>
    ///     缓存Token前缀
    /// </summary>
    string CacheTokenPrefix { get; set; }

    /// <summary>
    /// 过期时间
    /// </summary>
    int Expire {get; set; }
}

#endregion

/// <summary>
///     内部认证选项
/// </summary>
public class InternalAuthorizationOptions : IArtemisAuthorizationOptions
{
    #region Implementation of IArtemisIdentityOptions

    /// <summary>
    ///     是否启用高级策略
    /// </summary>
    public required bool EnableAdvancedPolicy { get; set; }

    /// <summary>
    ///     请求头Token键
    /// </summary>
    public required string HeaderTokenKey { get; set; } = Constants.HeaderTokenKey;

    /// <summary>
    ///     缓存Token前缀
    /// </summary>
    public required string CacheTokenPrefix { get; set; } = Constants.CacheTokenPrefix;

    /// <summary>
    /// 过期时间
    /// </summary>
    public int Expire { get; set; } = 0;

    #endregion
}

/// <summary>
///     Artemis认证选项
/// </summary>
public class ArtemisAuthorizationOptions : InternalAuthorizationOptions
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
public abstract class PolicyOptions
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
    public required IEnumerable<string> Roles { get; set; }
}

/// <summary>
///     基于凭据的策略配置
/// </summary>
public sealed class ClaimsBasedPolicyOptions : PolicyOptions
{
    /// <summary>
    ///     策略支持的凭据字典
    /// </summary>
    public required IDictionary<string, string> Claims { get; set; }
}