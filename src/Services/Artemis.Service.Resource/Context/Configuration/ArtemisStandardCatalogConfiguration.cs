using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.Resource.Context.Configuration;

/// <summary>
///     标准目录配置
/// </summary>
public class ArtemisStandardCatalogConfiguration : ConcurrencyModelEntityConfiguration<ArtemisStandardCatalog>
{
    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "标准目录数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisStandardCatalog).TableName();

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<ArtemisStandardCatalog> builder)
    {
        builder.HasIndex(entity => entity.Name)
            .HasDatabaseName(IndexName(nameof(ArtemisStandardCatalog.Name)));

        builder.HasIndex(entity => entity.Code)
            .HasDatabaseName(IndexName(nameof(ArtemisStandardCatalog.Code)));

        builder.HasIndex(entity => entity.Type)
            .HasDatabaseName(IndexName(nameof(ArtemisStandardCatalog.Type)));

        // Each Catalog can have many Items
        builder.HasMany(catalog => catalog.Items)
            .WithOne(item => item.Catalog)
            .HasForeignKey(item => item.StandardCatalogId)
            .HasConstraintName(ForeignKeyName(
                nameof(ArtemisStandardItem).TableName(),
                nameof(ArtemisStandardCatalog).TableName()))
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}