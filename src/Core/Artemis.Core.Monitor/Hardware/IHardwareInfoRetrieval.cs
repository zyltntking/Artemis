using Artemis.Core.Monitor.Fundamental.Components.Interface;

namespace Artemis.Core.Monitor.Hardware;

/// <summary>
///     硬件信息扫描接口
/// </summary>
internal interface IHardwareInfoRetrieval
{
    /// <summary>
    ///     获取操作系统信息
    /// </summary>
    /// <returns></returns>
    IOS GetOperatingSystem();

    /// <summary>
    ///   获取内存状态
    /// </summary>
    /// <returns></returns>
    IMemoryStatus GetMemoryStatus();

    /// <summary>
    ///  获取电池信息
    /// </summary>
    /// <returns></returns>
    ICollection<IBattery> GetBatteries();
}