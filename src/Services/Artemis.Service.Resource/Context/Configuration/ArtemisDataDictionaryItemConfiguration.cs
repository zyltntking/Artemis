using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.Resource.Context.Configuration;

/// <summary>
///     数据字典项目配置
/// </summary>
internal sealed class
    ArtemisDataDictionaryItemConfiguration : ConcurrencyModelEntityConfiguration<ArtemisDataDictionaryItem>
{
    #region Overrides of BaseEntityConfiguration<ArtemisDataDictionary,Guid,Guid,string,int>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "数据字典项目数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisDataDictionaryItem).TableName();

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<ArtemisDataDictionaryItem> builder)
    {
        builder.HasIndex(entity => entity.Key)
            .HasDatabaseName(IndexName("Key"));

        builder.HasIndex(entity => new { entity.DataDictionaryId, entity.Key })
            .HasDatabaseName(IndexName("DataDictionaryId", "Key"))
            .IsUnique();
    }

    #endregion
}