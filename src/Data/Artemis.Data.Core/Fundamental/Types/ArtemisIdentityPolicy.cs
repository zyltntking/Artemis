using System.ComponentModel;

namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
///     Artemis认证策略
/// </summary>
[Description("Artemis认证策略")]
public sealed class ArtemisIdentityPolicy : Enumeration
{
    /// <summary>
    ///     匿名策略
    /// </summary>
    [Description("匿名策略")] public static readonly ArtemisIdentityPolicy Anonymous = new(1, nameof(Anonymous));

    /// <summary>
    ///     令牌策略
    /// </summary>
    [Description("令牌策略")] public static readonly ArtemisIdentityPolicy Token = new(2, nameof(Token));

    /// <summary>
    ///     管理员策略
    /// </summary>
    [Description("管理员策略")] public static readonly ArtemisIdentityPolicy Admin = new(3, nameof(Admin));

    /// <summary>
    ///     操作名策略
    /// </summary>
    [Description("操作名策略")] public static readonly ArtemisIdentityPolicy ActionName = new(4, nameof(ActionName));

    /// <summary>
    ///     路由路径策略
    /// </summary>
    [Description("路由路径策略")] public static readonly ArtemisIdentityPolicy RoutePath = new(5, nameof(RoutePath));

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private ArtemisIdentityPolicy(int id, string name) : base(id, name)
    {
    }
}