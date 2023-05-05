using Artemis.Core.Monitor.Fundamental.Components;
using Artemis.Core.Monitor.Fundamental.Components.Interface;
using Artemis.Data.Core.Fundamental.Model;

namespace Artemis.Core.Monitor.Hardware.Linux;

/// <summary>
///     硬件信息扫描(LINUX)
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
        var lines = File.ReadAllLines("/etc/os-release");

        var os = new OS();

        var metadata = new List<MetadataInfo>();

        foreach (var line in lines)
        {
            if (line.StartsWith("NAME=")) os.Name = line.Replace("NAME=", string.Empty).Trim('"');

            if (line.StartsWith("VERSION_ID=")) os.Version = line.Replace("VERSION_ID=", string.Empty).Trim('"');

            var split = line.Split('=');

            if (split.Length == 1)
                metadata.Add(new MetadataInfo
                {
                    Key = split[0],
                    Value = string.Empty
                });

            if (split.Length == 2)
                metadata.Add(new MetadataInfo
                {
                    Key = split[0],
                    Value = split[1]
                });
        }

        os.Metadata = metadata;

        return os;
    }

    #endregion
}