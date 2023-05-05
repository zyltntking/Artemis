using Artemis.Core.Monitor.Fundamental.Components.Interface;
using System.Management;
using Artemis.Core.Monitor.Fundamental.Components;
using Artemis.Core.Monitor.Fundamental.Types;
using Artemis.Data.Core.Fundamental.Model;
using EnumerationOptions = System.Management.EnumerationOptions;

#pragma warning disable CA1416

namespace Artemis.Core.Monitor.Hardware.Windows;

/// <summary>
///     硬件信息扫描(WINDOWS)
/// </summary>
internal class HardwareInfoRetrieval : HardwareInfoBase, IHardwareInfoRetrieval
{

    /// <summary>
    ///     使用WMI中的星号查询标识
    /// </summary>
    public bool UseAsteriskInWmi { get; init; }

    private readonly string _managementScope = @"root\cimv2";
    private readonly EnumerationOptions _enumerationOptions = new() { ReturnImmediately = true, Rewindable = false, Timeout = ManagementOptions.InfiniteTimeout };

    #region Implementation of IHardwareInfoRetrieval

    /// <summary>
    ///     获取操作系统信息
    /// </summary>
    /// <returns></returns>
    /// <remarks><![CDATA[https://docs.microsoft.com/en-us/windows/win32/api/winnt/ns-winnt-osversioninfoexa]]></remarks>
    public IOS GetOperatingSystem()
    {
        var query = $"SELECT {(UseAsteriskInWmi ? "*" : "Caption, Version")} FROM Win32_OperatingSystem";

        using var mos = new ManagementObjectSearcher(_managementScope, query, _enumerationOptions);

        var os = new OS();

        var metadata = new List<MetadataInfo>();

        foreach (var mo in mos.Get())
        {
            os.Name = GetPropertyString(mo["Caption"]);
            os.Version = GetPropertyString(mo["Version"]);

            if (GatherMetadata)
            {
                foreach (var property in mo.Properties)
                {
                    metadata.Add(new MetadataInfo
                    {
                        Key = property.Name,
                        Value = property.Value is string str ? str : string.Empty
                    });
                }
            }
        }

        os.Metadata = metadata;

        if (string.IsNullOrEmpty(os.Name))
        {
            os.Name = "Windows";
        }

        return os;
    }

    /// <summary>
    ///   获取内存状态
    /// </summary>
    /// <returns></returns>
    /// <remarks><![CDATA[https://docs.microsoft.com/en-us/windows/win32/api/sysinfoapi/nf-sysinfoapi-globalmemorystatusex]]></remarks>
    public IMemoryStatus GetMemoryStatus()
    {
        var query = $"SELECT {(UseAsteriskInWmi ? "*" : "TotalVisibleMemorySize, FreePhysicalMemory, TotalVirtualMemorySize, FreeVirtualMemory")} FROM Win32_OperatingSystem";

        using var mos = new ManagementObjectSearcher(_managementScope, query, _enumerationOptions);

        var memoryStatus = new MemoryStatus();

        var metadata = new List<MetadataInfo>();

        foreach (var mo in mos.Get())
        {
            memoryStatus.TotalPhysical = GetPropertyMemoryBytes(mo["TotalVisibleMemorySize"]);
            memoryStatus.AvailablePhysical = GetPropertyMemoryBytes(mo["FreePhysicalMemory"]);
            memoryStatus.TotalVirtual = GetPropertyMemoryBytes(mo["TotalVirtualMemorySize"]);
            memoryStatus.AvailableVirtual = GetPropertyMemoryBytes(mo["FreeVirtualMemory"]);
            memoryStatus.StorageUnit = StorageUnit.B;

            if (GatherMetadata)
            {
                foreach (var property in mo.Properties)
                {
                    metadata.Add(new MetadataInfo
                    {
                        Key = property.Name,
                        Value = property.Value is string str ? str : string.Empty
                    });
                }
            }
        }

        memoryStatus.Metadata = metadata;

        return memoryStatus;
    }

    /// <summary>
    ///  获取电池信息
    /// </summary>
    /// <returns></returns>
    public ICollection<IBattery> GetBatteries()
    {
        throw new NotImplementedException();
    }

    #endregion

    /// <summary>
    ///   获取属性字符串
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    private static string GetPropertyString(object property)
    {
        return property is string str ? str : string.Empty;
    }

    /// <summary>
    ///  获取内存字节数
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    private static ulong GetPropertyMemoryBytes(object property)
    {
        var mem = property is ulong size ? size : 0;

        return mem * 1024;
    }
}