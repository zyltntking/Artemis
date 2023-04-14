using Artemis.Core.Monitor.Fundamental.Interface;
using Artemis.Core.Monitor.Fundamental.Types;
using Artemis.Core.Monitor.Store.Entities.Configurations;
using Artemis.Data.Core;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Core.Monitor.Store.Entities;

/// <summary>
/// 监控主机
/// </summary>
[EntityTypeConfiguration(typeof(MonitorHostConfiguration))]
public class MonitorHost : PartitionBase, IHost
{
    /// <summary>
    /// 元数据组标识
    /// </summary>
    public virtual Guid? MetadataGroupId { get; set; } = null; 

    #region Implementation of IHost

    /// <summary>
    ///     主机名
    /// </summary>
    public virtual string HostName { get; set; }

    /// <summary>
    ///     主机类型
    /// </summary>
    public virtual HostType HostType { get; set; }

    /// <summary>
    /// 实例类型
    /// </summary>
    public virtual InstanceType InstanceType { get; set; }

    /// <summary>
    ///     系统名
    /// </summary>
    public virtual string OsName { get; set; }

    /// <summary>
    ///    平台类型
    /// </summary>
    public virtual PlatformType PlatformType { get; set; }

    /// <summary>
    ///     系统版本
    /// </summary>
    public virtual string OsVersion { get; set; }

    /// <summary>
    /// 进程架构
    /// </summary>
    public virtual string ProcessArchitecture { get; set; }

    #endregion

    /// <summary>
    /// 具备的元数据组
    /// </summary>
    public virtual MetadataGroup MetadataGroup { get; set; }
}