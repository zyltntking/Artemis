using Artemis.Core.Monitor.Fundamental.Components.Interface;

namespace Artemis.Core.Monitor.Hardware;

/// <summary>
///     硬件信息接口
/// </summary>
internal interface IHardwareInfo
{
    /// <summary>
    ///     操作系统信息
    /// </summary>
    IOS OperatingSystem { get; }

    /// <summary>
    /// 当前内存状态
    /// </summary>
    IMemoryStatus MemoryStatus { get; }

    /// <summary>
    ///  电池信息
    /// </summary>
    ICollection<IBattery> Batteries { get; }
}