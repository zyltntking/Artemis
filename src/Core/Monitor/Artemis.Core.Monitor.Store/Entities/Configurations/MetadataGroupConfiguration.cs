using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Core.Monitor.Store.Entities.Configurations;

/// <summary>
///     元数据组模型配置
/// </summary>
public class MetadataGroupConfiguration : MonitorConfiguration<MetadataGroup>
{
    /// <summary>
    ///     Configures the entity of type MetadataGroup.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public override void Configure(EntityTypeBuilder<MetadataGroup> builder)
    {
        base.Configure(builder);

        builder.Property(entity => entity.Key).HasComment("数据键");

        builder.Property(entity => entity.Value).HasComment("数据值");

        builder.HasMany(group => group.MetadataItems)
            .WithOne(item => item.MetadataGroup)
            .HasForeignKey(entity => entity.MetadataGroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}