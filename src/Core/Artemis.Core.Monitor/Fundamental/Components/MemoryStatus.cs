using Artemis.Core.Monitor.Fundamental.Components.Interface;
using Artemis.Core.Monitor.Fundamental.Types;
using Artemis.Data.Core.Fundamental.Model;

namespace Artemis.Core.Monitor.Fundamental.Components;

/// <summary>
///   内存状态
/// </summary>
/// <remarks><![CDATA[https://docs.microsoft.com/en-us/windows/win32/api/sysinfoapi/ns-sysinfoapi-memorystatusex]]></remarks>
public class MemoryStatus : IMemoryStatus
{
    #region Implementation of IMemoryStatus

    /// <summary>
    ///  物理内存总量
    /// </summary>
    public ulong TotalPhysical { get; set; }

    /// <summary>
    ///  可用物理内存
    /// </summary>
    public ulong AvailablePhysical { get; set; }

    /// <summary>
    ///  虚拟内存总量
    /// </summary>
    public ulong TotalVirtual { get; set; }

    /// <summary>
    ///  可用虚拟内存
    /// </summary>
    public ulong AvailableVirtual { get; set; }

    /// <summary>
    /// 存储单位
    /// </summary>
    public StorageUnit StorageUnit { get; set; } = StorageUnit.Unknown;

    #endregion

    /// <summary>
    ///     元数据信息
    /// </summary>
    public ICollection<MetadataInfo>? Metadata { get; set; }

    #region Overrides of Object

    /// <summary>Returns a string that represents the current object.</summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString()
    {
        return
            $"{nameof(TotalPhysical)}: {TotalPhysical}" + Environment.NewLine +
            $"{nameof(AvailablePhysical)}: {AvailablePhysical}" + Environment.NewLine +
            $"{nameof(TotalVirtual)}: {TotalVirtual}" + Environment.NewLine +
            $"{nameof(AvailableVirtual)}: {AvailableVirtual}" + Environment.NewLine +
            $"{nameof(StorageUnit)}: {StorageUnit}";
    }

    #endregion
}