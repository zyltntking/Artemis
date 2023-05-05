using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Core.Monitor.Store.Entities.Configurations;

/// <summary>
///     元数据模型配置
/// </summary>
public class MetadataItemConfiguration : MonitorConfiguration<MetadataItem>
{
    /// <summary>
    ///     Configures the entity of type MetadataItem.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public override void Configure(EntityTypeBuilder<MetadataItem> builder)
    {
        base.Configure(builder);

        builder.Property(entity => entity.MetadataGroupId).HasComment("元数据组标识");

        builder.Property(entity => entity.Key).HasComment("数据键");

        builder.Property(entity => entity.Value).HasComment("数据值");
    }
}