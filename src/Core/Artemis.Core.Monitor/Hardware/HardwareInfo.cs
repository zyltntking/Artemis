using Artemis.Core.Monitor.Fundamental.Components.Interface;
using Artemis.Core.Monitor.Fundamental.Types;

namespace Artemis.Core.Monitor.Hardware;

/// <summary>
///     硬件信息
/// </summary>
internal class HardwareInfo : IHardwareInfo
{
    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="platform">平台类型</param>
    public HardwareInfo(PlatformType platform)
    {
        if (platform.Equals(PlatformType.Linux))
            HardwareInfoRetrieval = new Linux.HardwareInfoRetrieval
            {
                GatherMetadata = false
            };

        if (platform.Equals(PlatformType.Windows))
            HardwareInfoRetrieval = new Windows.HardwareInfoRetrieval
            {
                UseAsteriskInWmi = false,
                GatherMetadata = false
            };

        if (platform.Equals(PlatformType.MacOS)) 
            HardwareInfoRetrieval = new Unknown.HardwareInfoRetrieval
            {
                GatherMetadata = false
            };

        if (platform.Equals(PlatformType.Unknown))
            HardwareInfoRetrieval = new Unknown.HardwareInfoRetrieval
            {
                GatherMetadata = false
            };
    }

    /// <summary>
    ///     硬件信息扫描
    /// </summary>
    private IHardwareInfoRetrieval HardwareInfoRetrieval { get; } = null!;

    #region Implementation of IHardwareInfo

    /// <summary>
    ///     操作系统信息
    /// </summary>
    public IOS OperatingSystem => HardwareInfoRetrieval.GetOperatingSystem();

    /// <summary>
    /// 当前内存状态
    /// </summary>
    public IMemoryStatus MemoryStatus => HardwareInfoRetrieval.GetMemoryStatus();

    /// <summary>
    ///  电池信息
    /// </summary>
    public ICollection<IBattery> Batteries => HardwareInfoRetrieval.GetBatteries();

    /// <summary>
    ///  BIOS信息
    /// </summary>
    public ICollection<IBIOS> BIOSList => HardwareInfoRetrieval.GetBIOSList();

    #endregion
}