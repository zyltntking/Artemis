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

        var metadata = new List<MetadataInfo>();

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

            if (GatherMetadata)
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
        }

        os.Metadata = metadata;

        return os;
    }

    private const string KbToken = "kB";

    /// <summary>
    ///   获取内存状态
    /// </summary>
    /// <returns></returns>
    public IMemoryStatus GetMemoryStatus()
    {
        var memoryInfo = TryReadLinesFromFile("/proc/meminfo");

        var memoryStatus = new MemoryStatus();

        var metadata = new List<MetadataInfo>();

        foreach (var info in memoryInfo)
        {
            var token = "MemTotal:";
            if (info.StartsWith(token) && info.EndsWith(KbToken))
            {
                memoryStatus.TotalPhysical = GetBytesFromLine(info, token);
            }

            token = "MemAvailable:";
            if (info.StartsWith(token) && info.EndsWith(KbToken))
            {
                memoryStatus.AvailablePhysical = GetBytesFromLine(info, token);
            }

            token = "SwapTotal:";
            if (info.StartsWith(token) && info.EndsWith(KbToken))
            {
                memoryStatus.TotalVirtual = GetBytesFromLine(info, token);
            }

            token = "SwapFree:";
            if (info.StartsWith(token) && info.EndsWith(KbToken))
            {
                memoryStatus.AvailableVirtual = GetBytesFromLine(info, token);
            }

            if (GatherMetadata)
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

        }

        memoryStatus.StorageUnit = StorageUnit.B;

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
    ///  获取字节数
    /// </summary>
    /// <param name="memoryLine"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    private static ulong GetBytesFromLine(string memoryLine, string token)
    {
        var mem = memoryLine.Replace(token, string.Empty).Replace(KbToken, string.Empty).Trim();

        if (ulong.TryParse(mem, out var memKb))
            return memKb * 1024;

        return 0;
    }
}