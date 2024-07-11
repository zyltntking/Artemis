namespace Artemis.Service.Shared.Transfer;

/// <summary>
///     认证策略
/// </summary>
public class IdentityPolicy
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

    /// <summary>
    ///     需求Token策略
    /// </summary>
    public static readonly List<string> TokenPolicies = [Token, Admin, ActionName, RoutePath];
}