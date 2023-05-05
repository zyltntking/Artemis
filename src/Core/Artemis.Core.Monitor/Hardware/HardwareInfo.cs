using Artemis.Core.Monitor.Fundamental.Components.Interface;
using Artemis.Core.Monitor.Fundamental.Types;
using Artemis.Core.Monitor.Hardware.Linux;

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
        if (platform.Equals(PlatformType.Linux)) HardwareInfoRetrieval = new HardwareInfoRetrieval();

        if (platform.Equals(PlatformType.Windows))
            HardwareInfoRetrieval = new Windows.HardwareInfoRetrieval
            {
                UseAsteriskInWmi = false
            };

        if (platform.Equals(PlatformType.MacOS)) HardwareInfoRetrieval = new Unknown.HardwareInfoRetrieval();

        if (platform.Equals(PlatformType.Unknown)) HardwareInfoRetrieval = new Unknown.HardwareInfoRetrieval();
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

    #endregion
}