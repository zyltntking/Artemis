using System.Net;
using System.Runtime.InteropServices;
using Artemis.Core.Monitor.Fundamental.Components.Interface;
using Artemis.Core.Monitor.Fundamental.Interface;
using Artemis.Core.Monitor.Fundamental.Model;
using Artemis.Core.Monitor.Fundamental.Types;
using Artemis.Core.Monitor.Hardware;

namespace Artemis.Core.Monitor;

/// <summary>
///     探针实现
/// </summary>
public class Probe : IProbe
{
    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="hostType"></param>
    /// <param name="instanceType"></param>
    public Probe(HostType hostType, InstanceType instanceType)
    {
        HostType = hostType;
        InstanceType = instanceType;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            PlatformType = PlatformType.Windows;
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            PlatformType = PlatformType.Linux;
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            PlatformType = PlatformType.MacOS;
        else
            PlatformType = PlatformType.Unknown;
        HardwareInfo = new HardwareInfo(PlatformType);
    }

    /// <summary>
    ///     平台类型
    /// </summary>
    private PlatformType PlatformType { get; }

    /// <summary>
    ///     主机类型
    /// </summary>
    private HostType HostType { get; }

    /// <summary>
    ///     实例类型
    /// </summary>
    private InstanceType InstanceType { get; }

    /// <summary>
    ///     主机信息
    /// </summary>
    private IHardwareInfo HardwareInfo { get; }

    /// <summary>
    ///     主机信息接口
    /// </summary>
    public IHostInfo HostInfo
    {
        get
        {
            var os = HardwareInfo.OperatingSystem;

            var hostInfo = new HostInfo
            {
                HostName = Dns.GetHostName(),
                HostType = HostType,
                InstanceType = InstanceType,
                OsName = os.Name,
                PlatformType = PlatformType,
                OsVersion = os.Version,
                OsArchitecture = RuntimeInformation.OSArchitecture.ToString()
            };

            return hostInfo;
        }
    }

    /// <summary>
    /// 内存状态接口
    /// </summary>
    public IMemoryStatus MemoryStatus => HardwareInfo.MemoryStatus;
}