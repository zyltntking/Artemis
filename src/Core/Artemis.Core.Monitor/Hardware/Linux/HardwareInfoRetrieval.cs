using Artemis.Core.Monitor.Fundamental.Components;
using Artemis.Core.Monitor.Fundamental.Components.Interface;
using Artemis.Core.Monitor.Fundamental.Types;
using Artemis.Data.Core.Fundamental.Model;

namespace Artemis.Core.Monitor.Hardware.Linux;

/// <summary>
///     硬件信息扫描(LINUX)
/// </summary>
/// <remarks><![CDATA[https://www.binarytides.com/linux-commands-hardware-info/]]></remarks>
internal class HardwareInfoRetrieval : HardwareInfoBase, IHardwareInfoRetrieval
{
    #region Implementation of IHardwareInfoRetrieval

    /// <summary>
    ///     获取操作系统信息
    /// </summary>
    /// <returns></returns>
    public IOS GetOperatingSystem()
    {
        var osInfos = File.ReadAllLines("/etc/os-release");

        var os = new OS();

        foreach (var info in osInfos)
        {
            if (info.StartsWith("NAME="))
            {
                os.Name = info.Replace("NAME=", string.Empty).Trim('"');
            }

            if (info.StartsWith("VERSION_ID="))
            {
                os.Version = info.Replace("VERSION_ID=", string.Empty).Trim('"');
            }
        }

        if (GatherMetadata)
        {
            var metadata = new List<MetadataInfo>();

            foreach (var info in osInfos)
            {
                var split = info.Split('=');

                if (split.Length == 1)
                {
                    metadata.Add(new MetadataInfo
                    {
                        Key = split[0],
                        Value = string.Empty
                    });
                }

                if (split.Length == 2)
                {
                    metadata.Add(new MetadataInfo
                    {
                        Key = split[0],
                        Value = split[1]
                    });
                }
            }

            os.Metadata = metadata;
        }

        return os;
    }

    /// <summary>
    ///   获取内存状态
    /// </summary>
    /// <returns></returns>
    public IMemoryStatus GetMemoryStatus()
    {
        var memoryInfo = TryReadLinesFromFile("/proc/meminfo");

        var memoryStatus = new MemoryStatus();

        foreach (var info in memoryInfo)
        {
            var token = "MemTotal:";
            if (info.StartsWith(token) && info.EndsWith(KbToken))
            {
                memoryStatus.TotalPhysical = GetBytesFromMemoryLine(info, token);
            }

            token = "MemAvailable:";
            if (info.StartsWith(token) && info.EndsWith(KbToken))
            {
                memoryStatus.AvailablePhysical = GetBytesFromMemoryLine(info, token);
            }

            token = "SwapTotal:";
            if (info.StartsWith(token) && info.EndsWith(KbToken))
            {
                memoryStatus.TotalVirtual = GetBytesFromMemoryLine(info, token);
            }

            token = "SwapFree:";
            if (info.StartsWith(token) && info.EndsWith(KbToken))
            {
                memoryStatus.AvailableVirtual = GetBytesFromMemoryLine(info, token);
            }
        }

        if (GatherMetadata)
        {
            var metadata = new List<MetadataInfo>();

            foreach (var info in memoryInfo)
            {
                var split = info.Split(':');
                if (split.Length == 1)
                {
                    metadata.Add(new MetadataInfo
                    {
                        Key = split[0],
                        Value = string.Empty
                    });
                }

                if (split.Length == 2)
                {
                    metadata.Add(new MetadataInfo
                    {
                        Key = split[0],
                        Value = split[1].Trim()
                    });
                }
            }

            memoryStatus.Metadata = metadata;
        }

        memoryStatus.StorageUnit = StorageUnit.B;

        return memoryStatus;
    }

    /// <summary>
    ///  获取电池信息
    /// </summary>
    /// <returns></returns>
    /// <remarks><![CDATA[https://stackoverflow.com/questions/26888636/how-to-calculate-the-time-remaining-until-the-end-of-the-battery-charge]]></remarks>
    /// <remarks><![CDATA[https://stackoverflow.com/questions/4858657/how-can-i-obtain-battery-level-inside-a-linux-kernel-module]]></remarks>
    /// <remarks><![CDATA[/sys/class/power_supply/BAT0/charge_now]]></remarks>
    /// <remarks><![CDATA[/sys/class/power_supply/BAT0/charge_full]]></remarks>
    /// <remarks><![CDATA[/sys/class/power_supply/BAT0/name = BAT0]]></remarks>
    /// <remarks><![CDATA[/sys/class/power_supply/BAT0/status = Charging / Discharging]]></remarks>
    /// <remarks><![CDATA[/sys/class/power_supply/BAT0/present = 1]]></remarks>
    /// <remarks><![CDATA[/sys/class/power_supply/BAT0/technology = Li-ion]]></remarks>
    /// <remarks><![CDATA[/sys/class/power_supply/BAT0/cycle_count = 0]]></remarks>
    /// <remarks><![CDATA[/sys/class/power_supply/BAT0/voltage_min_design = 112530000]]></remarks>
    /// <remarks><![CDATA[/sys/class/power_supply/BAT0/power_now = 21240000]]></remarks>
    /// <remarks><![CDATA[/sys/class/power_supply/BAT0/energy_full_design = 50610000]]></remarks>
    /// <remarks><![CDATA[/sys/class/power_supply/BAT0/energy_full = 50610000]]></remarks>
    /// <remarks><![CDATA[/sys/class/power_supply/BAT0/energy_now  = 22460000]]></remarks>
    /// <remarks><![CDATA[/sys/class/power_supply/BAT0/capacity = 44 //44 % full //50610000 / 22460000 = 0,4437858130804189]]></remarks>
    /// <remarks><![CDATA[/sys/class/power_supply/BAT0/capacity_level = Normal]]></remarks>
    /// <remarks><![CDATA[/sys/class/power_supply/BAT0/model_name = ]]></remarks>
    /// <remarks><![CDATA[/sys/class/power_supply/BAT0/manufacturer = Sony Corp.]]></remarks>
    /// <remarks><![CDATA[/sys/class/power_supply/BAT0/serial_number = ]]></remarks>
    public ICollection<IBattery> GetBatteries()
    {
        var batteries = new List<IBattery>();

        var batteryInfoDirs = Directory.GetDirectories("/sys/class/power_supply", "BAT*");

        foreach (var batteryInfoDir in batteryInfoDirs)
        {
            var uEventFilePath = Path.Combine(batteryInfoDir, "uevent");

            var batteryInfo = TryReadLinesFromFile(uEventFilePath);

            var powerNow = TryReadIntegerFromBatteryInfos(batteryInfo, "POWER_SUPPLY_POWER_NOW=");

            if (powerNow == 0)
                powerNow = 1;

            var designCapacity = TryReadIntegerFromBatteryInfos(batteryInfo, "POWER_SUPPLY_ENERGY_FULL_DESIGN=", "POWER_SUPPLY_CHARGE_FULL_DESIGN=");

            var fullCapacity = TryReadIntegerFromBatteryInfos(batteryInfo, "POWER_SUPPLY_ENERGY_FULL=", "POWER_SUPPLY_CHARGE_FULL=");

            if (fullCapacity == 0)
                fullCapacity = 1;

            var energyNow = TryReadIntegerFromBatteryInfos(batteryInfo, "POWER_SUPPLY_ENERGY_NOW=", "POWER_SUPPLY_CHARGE_NOW=");

            var battery = new Battery
            {
                BatteryName = TryReadTextFromBatteryInfos(batteryInfo, "POWER_SUPPLY_NAME="),
                DesignCapacity = designCapacity,
                FullChargeCapacity = fullCapacity,
                BatteryStatusDescription = TryReadTextFromBatteryInfos(batteryInfo, "POWER_SUPPLY_STATUS="),
                EstimatedChargeRemaining = (ushort)(energyNow * 100 / fullCapacity), // 当前剩余电量百分比
                ExpectedLife = fullCapacity / powerNow, // 电池剩余时间
                MaxRechargeTime = fullCapacity / powerNow, // 电池充满时间
                TimeToFullCharge = (fullCapacity - energyNow) / powerNow // 电池充满时间
            };

            if (GatherMetadata)
            {
                var metadata = new List<MetadataInfo>();

                foreach (var info in batteryInfo)
                {
                    var split = info.Split('=');

                    if (split.Length == 1)
                    {
                        metadata.Add(new MetadataInfo
                        {
                            Key = split[0],
                            Value = string.Empty
                        });
                    }

                    if (split.Length == 2)
                    {
                        metadata.Add(new MetadataInfo
                        {
                            Key = split[0],
                            Value = split[1]
                        });
                    }
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
        var biosList = new List<IBIOS>();

        var bios = new BIOS
        {
            ReleaseDate = TryReadTextFromFile("/sys/class/dmi/id/bios_date"),
            Version = TryReadTextFromFile("/sys/class/dmi/id/bios_version"),
            Manufacturer = TryReadTextFromFile("/sys/class/dmi/id/bios_vendor")
        };

        biosList.Add(bios);

        return biosList;
    }

    #endregion
}