using System.ComponentModel;

namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
///     路由类型
/// </summary>
[Description("路由类型")]
public class RouteType : Enumeration
{
    /// <summary>
    ///     gRpc
    /// </summary>
    [Description("gRpc")] public static RouteType gRpc = new(1, nameof(gRpc));

    /// <summary>
    ///     Restful
    /// </summary>
    [Description("Restful")] public static RouteType Restful = new(2, nameof(Restful));

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private RouteType(int id, string name) : base(id, name)
    {
    }
}