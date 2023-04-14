using Artemis.Core.Monitor.Store.Entities.Configurations;
using Artemis.Data.Core;
using Artemis.Data.Core.Fundamental.Interface;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Core.Monitor.Store.Entities;

/// <summary>
/// 元数据
/// </summary>
[EntityTypeConfiguration(typeof(MetadataItemConfiguration))]
public class MetadataItem : PartitionBase, IMeta
{
    /// <summary>
    /// 元数据组标识
    /// </summary>
    public virtual Guid MetadataGroupId { get; set; }

    #region Implementation of IMeta

    /// <summary>
    ///     数据键
    /// </summary>
    public virtual string Key { get; set; }

    /// <summary>
    ///     数据值
    /// </summary>
    public virtual string Value { get; set; }

    #endregion

    /// <summary>
    /// 所属元数据组
    /// </summary>
    public virtual MetadataGroup MetadataGroup { get; set; }
}