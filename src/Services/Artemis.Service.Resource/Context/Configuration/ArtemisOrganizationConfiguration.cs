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
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<ArtemisOrganization> builder)
    {
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