using Artemis.Core.Monitor.Store.Entities.Configurations;
using Artemis.Data.Core;
using Artemis.Data.Core.Fundamental.Interface;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Core.Monitor.Store.Entities;

/// <summary>
///     元数据组
/// </summary>
[EntityTypeConfiguration(typeof(MetadataGroupConfiguration))]
public class MetadataGroup : PartitionBase, IMeta
{
    #region Implementation of IMeta

    /// <summary>
    ///     数据键
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    ///     数据值
    /// </summary>
    public string Value { get; set; }

    #endregion
}