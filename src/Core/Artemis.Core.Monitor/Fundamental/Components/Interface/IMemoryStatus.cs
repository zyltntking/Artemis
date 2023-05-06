using Artemis.Core.Monitor.Fundamental.Types;

namespace Artemis.Core.Monitor.Fundamental.Components.Interface;

/// <summary>
///   内存状态接口
/// </summary>
/// <remarks><![CDATA[https://docs.microsoft.com/en-us/windows/win32/api/sysinfoapi/ns-sysinfoapi-memorystatusex]]></remarks>
/// <remarks><![CDATA[https://learn.microsoft.com/zh-cn/windows/win32/cimwin32prov/win32-operatingsystem]]></remarks>
/// <remarks><![CDATA[WMI class: Win32_OperatingSystem]]></remarks>
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