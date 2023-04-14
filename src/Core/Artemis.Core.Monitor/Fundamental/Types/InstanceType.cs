using Artemis.Data.Core.Fundamental;

namespace Artemis.Core.Monitor.Fundamental.Types;

/// <summary>
///     机器类型
/// </summary>
public class InstanceType : Enumeration
{
    /// <summary>
    ///     未知类型
    /// </summary>
    public static InstanceType Unknown = new(-1, nameof(Unknown));

    /// <summary>
    ///     物理机
    /// </summary>
    public static InstanceType Physical = new(0, nameof(Physical));

    /// <summary>
    ///     虚拟机
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