using Artemis.Core.Monitor.Fundamental.Components.Interface;

namespace Artemis.Core.Monitor.Hardware.Unknown;

/// <summary>
///     硬件信息扫描
/// </summary>
internal class HardwareInfoRetrieval : HardwareInfoBase, IHardwareInfoRetrieval
{
    #region Implementation of IHardwareInfoRetrieval

    /// <summary>
    ///     获取操作系统信息
    /// </summary>
    /// <returns></returns>
    public IOS GetOperatingSystem()
    {
        throw new NotImplementedException();
    }

    #endregion
}