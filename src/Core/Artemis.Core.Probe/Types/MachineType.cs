using Artemis.Data.Core;

namespace Artemis.Core.Probe.Types;

/// <summary>
/// 机器类型
/// </summary>
public class MachineType : Enumeration
{
    /// <summary>
    /// 未知类型
    /// </summary>
    public static MachineType Unknown = new(-1, nameof(Unknown));

    /// <summary>
    ///     物理机
    /// </summary>
    public static MachineType Physical = new(0, nameof(Physical));

    /// <summary>
    ///     虚拟机
    /// </summary>
    public static MachineType Virtual = new(1, nameof(Virtual));

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private MachineType(int id, string name) : base(id, name)
    {
    }
}