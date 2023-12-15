using System.ComponentModel;

namespace Artemis.Shared.Identity;

/// <summary>
///     预设凭据类型
/// </summary>
public static class ArtemisClaimTypes
{
    /// <summary>
    ///     路由路径凭据
    /// </summary>
    [Description("路由凭据")] public static string RoutePath = nameof(RoutePath);

    /// <summary>
    ///     操作名凭据
    /// </summary>
    [Description("操作名凭据")] public static string ActionName = nameof(ActionName);

    /// <summary>
    ///     签名凭据
    /// </summary>
    [Description("签名凭据")] public static string Signature = nameof(Signature);
}