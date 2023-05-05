using Artemis.Core.Monitor.Fundamental.Components.Interface;

namespace Artemis.Core.Monitor.Hardware.Windows;

/// <summary>
///     硬件信息扫描
/// </summary>
internal class HardwareInfoRetrieval : HardwareInfoBase, IHardwareInfoRetrieval
{
    /// <summary>
    ///     使用WMI中的星号查询标识
    /// </summary>
    public bool UseAsteriskInWmi { get; init; }

    #region Implementation of IHardwareInfoRetrieval

    /// <summary>
    ///     获取操作系统信息
    /// </summary>
    /// <returns></returns>
    public IOS GetOperatingSystem()
    {
        var query = $"SELECT {(UseAsteriskInWmi ? "*" : "Caption, Version")} FROM Win32_OperatingSystem";

        throw new NotImplementedException();
    }

    #endregion
}