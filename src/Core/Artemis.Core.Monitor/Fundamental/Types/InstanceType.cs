using Artemis.Data.Core.Fundamental;
using System.ComponentModel;

namespace Artemis.Core.Monitor.Fundamental.Types;

/// <summary>
///     实例类型
/// </summary>
[Description("实例类型")]
public class InstanceType : Enumeration
{
    /// <summary>
    ///     未知类型
    /// </summary>
    public static InstanceType Unknown = new(-1, nameof(Unknown));

    /// <summary>
    ///     物理实例
    /// </summary>
    public static InstanceType Physical = new(0, nameof(Physical));

    /// <summary>
    ///     虚拟实例
    /// </summary>
    public static InstanceType Virtual = new(1, nameof(Virtual));

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private InstanceType(int id, string name) : base(id, name)
    {
    }
}