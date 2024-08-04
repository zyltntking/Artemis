using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.Resource.Context.Configuration;

/// <summary>
///     标准项目配置
/// </summary>
public class ArtemisStandardItemConfiguration : ConcurrencyModelEntityConfiguration<ArtemisStandardItem>
{
    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "标准项目数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisStandardItem).TableName();

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<ArtemisStandardItem> builder)
    {
        builder.HasIndex(entity => entity.Name)
            .HasDatabaseName(IndexName(nameof(ArtemisStandardItem.Name)));

        builder.HasIndex(entity => entity.Code)
            .HasDatabaseName(IndexName(nameof(ArtemisStandardItem.Code)));
    }
}