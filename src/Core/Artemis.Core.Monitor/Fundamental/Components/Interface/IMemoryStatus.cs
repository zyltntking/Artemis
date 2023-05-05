using Artemis.Core.Monitor.Fundamental.Types;

namespace Artemis.Core.Monitor.Fundamental.Components.Interface;

/// <summary>
///   内存状态接口
/// </summary>
public interface IMemoryStatus
{
    /// <summary>
    ///  物理内存总量
    /// </summary>
    ulong TotalPhysical { get; set; }

    /// <summary>
    ///  可用物理内存
    /// </summary>
    ulong AvailablePhysical { get; set; }

    /// <summary>
    ///  虚拟内存总量
    /// </summary>
    ulong TotalVirtual { get; set; }

    /// <summary>
    ///  可用虚拟内存
    /// </summary>
    ulong AvailableVirtual { get; set; }

    /// <summary>
    /// 存储单位
    /// </summary>
    StorageUnit StorageUnit { get; set; }

}