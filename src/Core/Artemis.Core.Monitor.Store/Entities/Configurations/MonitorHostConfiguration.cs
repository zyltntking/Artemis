using Artemis.Core.Monitor.Fundamental.Types;
using Artemis.Data.Store;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Core.Monitor.Store.Entities.Configurations;

/// <summary>
/// 监控主机模型配置
/// </summary>
public class MonitorHostConfiguration : MonitorConfiguration<MonitorHost>
{
    #region Overrides of ModelBaseTypeConfiguration<MonitorHost>

    /// <summary>
    ///     Configures the entity of type <typeparamref name="TEntity" />.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public override void Configure(EntityTypeBuilder<MonitorHost> builder)
    {
        base.Configure(builder);

        builder.Property(entity => entity.MetadataGroupId).HasComment("元数据组标识");

        builder.Property(entity => entity.HostName).HasComment("主机名");

        builder.Property(entity => entity.HostType)
            .HasComment("主机类型")
            .HasColumnType(DataTypeSet.String)
            .HasMaxLength(50)
            .HasConversion<EnumerationValueConverter<HostType>>();

        builder.Property(entity => entity.OsName).HasComment("系统名");

        builder.Property(entity => entity.OsVersion).HasComment("系统版本");

        builder.HasOne(host => host.MetadataGroup)
            .WithOne()
            .HasForeignKey<MonitorHost>(entity => entity.MetadataGroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    #endregion
}