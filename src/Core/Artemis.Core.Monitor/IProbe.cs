using Artemis.Core.Monitor.Fundamental.Components.Interface;
using Artemis.Core.Monitor.Fundamental.Interface;

namespace Artemis.Core.Monitor;

/// <summary>
///     探针接口
/// </summary>
public interface IProbe
{
    /// <summary>
    ///     主机信息接口
    /// </summary>
    IHostInfo HostInfo { get; }

    /// <summary>
    /// 内存状态接口
    /// </summary>
    IMemoryStatus MemoryStatus { get; }

    /// <summary>
    /// 电池信息接口
    /// </summary>
    ICollection<IBattery> Batteries { get; }

    /// <summary>
    /// BIOS信息接口
    /// </summary>
    ICollection<IBIOS> BIOSList { get; }
}