using Artemis.Data.Core.Fundamental.Types;
using Artemis.Data.Store.Configuration;
using Artemis.Data.Store.ValueConverter;
using Artemis.Service.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.Resource.Context.Configuration;

/// <summary>
/// 系统模块配置
/// </summary>
internal sealed class ArtemisSystemModuleConfiguration : ConcurrencyModelEntityConfiguration<ArtemisSystemModule>
{
    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "系统模块数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisSystemModule).TableName();

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<ArtemisSystemModule> builder)
    {
        // Index
        builder.HasIndex(module => module.Name)
            .HasDatabaseName(IndexName(nameof(ArtemisSystemModule.Name)));

        // Each Module can have many Children Module
        builder.HasMany(module => module.Children)
            .WithOne(child => child.Parent)
            .HasForeignKey(child => child.ParentId)
            .HasConstraintName(ForeignKeyName(
                nameof(ArtemisSystemModule).TableName(),
                nameof(ArtemisSystemModule).TableName()))
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
    }
}