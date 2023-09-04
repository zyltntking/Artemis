using System.ComponentModel;
using Artemis.Data.Core.Fundamental;

namespace Artemis.Extensions.Web.Fundamental;

/// <summary>
///     认证凭据类型
/// </summary>
[Description("认证凭据类型")]
public class IdentityClaimType : Enumeration
{
    /// <summary>
    ///     路由路径凭据
    /// </summary>
    [Description("路由凭据")] public static IdentityClaimType RoutePath = new(1, nameof(RoutePath));

    /// <summary>
    ///     操作名凭据
    /// </summary>
    [Description("操作名凭据")] public static IdentityClaimType ActionName = new(2, nameof(ActionName));

    /// <summary>
    ///     签名凭据
    /// </summary>
    [Description("签名凭据")] public static IdentityClaimType Signature = new(3, nameof(Signature));

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private IdentityClaimType(int id, string name) : base(id, name)
    {
    }
}