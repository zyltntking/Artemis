using System.ComponentModel;

namespace Artemis.Service.Shared.Transfer;

/// <summary>
///     预设凭据类型
/// </summary>
public static class ClaimTypes
{
    /// <summary>
    ///     用户标识凭据
    /// </summary>
    [Description("用户标识凭据")] public static string UserId = nameof(UserId);

    /// <summary>
    ///     用户名凭据
    /// </summary>
    [Description("用户名凭据")] public static string UserName = nameof(UserName);

    /// <summary>
    ///     端类型凭据
    /// </summary>
    [Description("端类型凭据")] public static string EndType = nameof(EndType);

    /// <summary>
    ///     角色标识凭据
    /// </summary>
    [Description("角色标识凭据")] public static string Role = nameof(Role);

    /// <summary>
    ///     认证令牌凭据
    /// </summary>
    [Description("认证令牌凭据")] public static string Authorization = nameof(Authorization);

    /// <summary>
    ///     路由路径凭据
    /// </summary>
    [Description("路由凭据")] public static string RoutePath = nameof(RoutePath);

    /// <summary>
    ///     元路由路径凭据
    /// </summary>
    [Description("元路由路径凭据")] public static string MateRoutePath = nameof(MateRoutePath);

    /// <summary>
    ///     操作名凭据
    /// </summary>
    [Description("操作名凭据")] public static string ActionName = nameof(ActionName);

    /// <summary>
    ///     元操作名凭据
    /// </summary>
    [Description("元操作名凭据")] public static string MateActionName = nameof(MateActionName);

    /// <summary>
    ///     签名凭据
    /// </summary>
    [Description("签名凭据")] public static string Signature = nameof(Signature);
}