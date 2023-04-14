using Artemis.Core.Monitor.Fundamental.Interface;
using Artemis.Core.Monitor.Fundamental.Types;
using Artemis.Data.Core;

namespace Artemis.Core.Monitor.Store.Entities;

/// <summary>
/// 监控主机
/// </summary>
public class MonitorHost : PartitionBase, IHost
{
    /// <summary>
    /// 元数据组标识
    /// </summary>
    public Guid? MetadataGroupId { get; set; } = null; 

    #region Implementation of IHost

    /// <summary>
    ///     主机名
    /// </summary>
    public string HostName { get; set; }

    /// <summary>
    ///     主机类型
    /// </summary>
    public HostType HostType { get; set; }

    /// <summary>
    ///     系统名
    /// </summary>
    public string OsName { get; set; }

    /// <summary>
    ///     系统版本
    /// </summary>
    public string OsVersion { get; set; }

    #endregion

    /// <summary>
    /// 具备的元数据组
    /// </summary>
    public virtual MetadataGroup MetadataGroup { get; set; }
}