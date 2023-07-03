using Artemis.Core.Monitor.Store.Entities.Configurations;
using Artemis.Data.Core;
using Artemis.Data.Core.Fundamental.Interface;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Core.Monitor.Store.Entities;

/// <summary>
///     元数据组
/// </summary>
[EntityTypeConfiguration(typeof(MetadataGroupConfiguration))]
public class MetadataGroup : PartitionBase, IMetadata
{
    /// <summary>
    ///     组内元数据
    /// </summary>
    public virtual ICollection<MetadataItem>? MetadataItems { get; set; }

    #region Implementation of IMeta

    /// <summary>
    ///     数据键
    /// </summary>
    public virtual string Key { get; set; } = null!;

    /// <summary>
    ///     数据值
    /// </summary>
    public virtual string? Value { get; set; }

    #endregion
}