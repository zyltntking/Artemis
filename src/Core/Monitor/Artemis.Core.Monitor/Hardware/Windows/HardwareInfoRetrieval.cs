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

    /// <summary>
    ///   WMI管理范围
    /// </summary>
    private readonly string _managementScope = @"root\cimv2";

    /// <summary>
    ///  WMI枚举选项
    /// </summary>
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

        foreach (var mo in mos.Get())
        {
            os.Name = GetPropertyString(mo["Caption"]);
            os.Version = GetPropertyString(mo["Version"]);

            if (GatherMetadata)
            {
                var metadata = new List<MetadataInfo>();

                foreach (var property in mo.Properties)
                {
                    metadata.Add(new MetadataInfo
                    {
                        Key = property.Name,
                        Value = property.Value as string ?? string.Empty
                    });
                }

                os.Metadata = metadata;
            }
        }

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

        foreach (var mo in mos.Get())
        {
            memoryStatus.TotalPhysical = GetMemoryPropertyBytes(mo["TotalVisibleMemorySize"]);
            memoryStatus.AvailablePhysical = GetMemoryPropertyBytes(mo["FreePhysicalMemory"]);
            memoryStatus.TotalVirtual = GetMemoryPropertyBytes(mo["TotalVirtualMemorySize"]);
            memoryStatus.AvailableVirtual = GetMemoryPropertyBytes(mo["FreeVirtualMemory"]);
            memoryStatus.StorageUnit = StorageUnit.B;

            if (GatherMetadata)
            {
                var metadata = new List<MetadataInfo>();

                foreach (var property in mo.Properties)
                {
                    metadata.Add(new MetadataInfo
                    {
                        Key = property.Name,
                        Value = property.Value as string ?? string.Empty
                    });
                }

                memoryStatus.Metadata = metadata;
            }
        }

        return memoryStatus;
    }

    /// <summary>
    ///  获取电池信息
    /// </summary>
    /// <returns></returns>
    public ICollection<IBattery> GetBatteries()
    {
        var query = $"SELECT {(UseAsteriskInWmi? "*" : "Name, FullChargeCapacity, DesignCapacity, BatteryStatus, EstimatedChargeRemaining, EstimatedRunTime, ExpectedLife, MaxRechargeTime, TimeOnBattery, TimeToFullCharge")} FROM Win32_Battery";

        var batteries = new List<IBattery>();

        using var mos = new ManagementObjectSearcher(_managementScope, query, _enumerationOptions);

        foreach (var mo in mos.Get())
        {
            var battery = new Battery
            {
                BatteryName = GetPropertyString(mo["Name"]),
                FullChargeCapacity = GetPropertyValue<uint>(mo["FullChargeCapacity"]),
                DesignCapacity = GetPropertyValue<uint>(mo["DesignCapacity"]),
                BatteryStatus = GetPropertyValue<ushort>(mo["BatteryStatus"]),
                EstimatedChargeRemaining = GetPropertyValue<ushort>(mo["EstimatedChargeRemaining"]),
                EstimatedRunTime = GetPropertyValue<uint>(mo["EstimatedRunTime"]),
                ExpectedLife = GetPropertyValue<uint>(mo["ExpectedLife"]),
                MaxRechargeTime = GetPropertyValue<uint>(mo["MaxRechargeTime"]),
                TimeOnBattery = GetPropertyValue<uint>(mo["TimeOnBattery"]),
                TimeToFullCharge = GetPropertyValue<uint>(mo["TimeToFullCharge"])
            };

            if (GatherMetadata)
            {
                var metadata = new List<MetadataInfo>();

                foreach (var property in mo.Properties)
                {
                    metadata.Add(new MetadataInfo
                    {
                        Key = property.Name,
                        Value = property.Value as string ?? string.Empty
                    });
                }

                battery.Metadata = metadata;
            }

            batteries.Add(battery);
        }

        return batteries;
    }

    /// <summary>
    /// 获取BIOS信息
    /// </summary>
    /// <returns></returns>
    public ICollection<IBIOS> GetBIOSList()
    {
        var query = $"SELECT {(UseAsteriskInWmi ? "*" : "Caption, Description, Manufacturer, Name, ReleaseDate, SerialNumber, SoftwareElementID, Version")} FROM Win32_BIOS";

        var biosList = new List<IBIOS>();

        using var mos = new ManagementObjectSearcher(_managementScope, query, _enumerationOptions);

        foreach (var mo in mos.Get())
        {
            var bios = new BIOS
            {
                Caption = GetPropertyString(mo["Caption"]),
                Description = GetPropertyString(mo["Description"]),
                Manufacturer = GetPropertyString(mo["Manufacturer"]),
                Name = GetPropertyString(mo["Name"]),
                ReleaseDate = GetPropertyString(mo["ReleaseDate"]),
                SerialNumber = GetPropertyString(mo["SerialNumber"]),
                SoftwareElementId = GetPropertyString(mo["SoftwareElementID"]),
                Version = GetPropertyString(mo["Version"])
            };

            if (GatherMetadata)
            {
                var metadata = new List<MetadataInfo>();

                foreach (var property in mo.Properties)
                {
                    metadata.Add(new MetadataInfo
                    {
                        Key = property.Name,
                        Value = property.Value as string ?? string.Empty
                    });
                }

                bios.Metadata = metadata;
            }

            biosList.Add(bios);
        }

        return biosList;
    }

    #endregion

    
}