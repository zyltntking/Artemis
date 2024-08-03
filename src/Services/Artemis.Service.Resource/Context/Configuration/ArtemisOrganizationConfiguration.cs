using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.Resource.Context.Configuration;

/// <summary>
///     组织机构配置
/// </summary>
internal sealed class ArtemisOrganizationConfiguration : ConcurrencyModelEntityConfiguration<ArtemisOrganization>
{
    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "组织机构数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisOrganization).TableName();

    /// <summary>
    ///     实体字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityFieldConfigure(EntityTypeBuilder<ArtemisOrganization> builder)
    {
        builder.Property(entity => entity.EstablishTime)
            .HasColumnType(DataTypeSet.DateTime);
    }

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<ArtemisOrganization> builder)
    {
        // index
        builder.HasIndex(organization => organization.DesignCode)
            .IsUnique()
            .HasDatabaseName(IndexName(nameof(ArtemisOrganization.DesignCode)));

        builder.HasIndex(organization => organization.Code)
            .HasDatabaseName(IndexName(nameof(ArtemisOrganization.Code)));

        builder.HasIndex(organization => organization.Type)
            .HasDatabaseName(IndexName(nameof(ArtemisOrganization.Type)));

        builder.HasIndex(organization => organization.Name)
            .HasDatabaseName(IndexName(nameof(ArtemisOrganization.Name)));

        builder.HasIndex(organization => organization.Status)
            .HasDatabaseName(IndexName(nameof(ArtemisOrganization.Status)));

        // Each Organization can have many Children Organization
        builder.HasMany(organization => organization.Children)
            .WithOne(child => child.Parent)
            .HasForeignKey(child => child.ParentId)
            .HasConstraintName(ForeignKeyName(
                nameof(ArtemisOrganization).TableName(),
                nameof(ArtemisOrganization).TableName()))
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
    }
}