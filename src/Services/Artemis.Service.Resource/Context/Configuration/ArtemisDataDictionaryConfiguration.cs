using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.Resource.Context.Configuration;

/// <summary>
///     数据字典配置
/// </summary>
internal sealed class ArtemisDataDictionaryConfiguration : ConcurrencyModelEntityConfiguration<ArtemisDataDictionary>
{
    #region Overrides of BaseEntityConfiguration<ArtemisDataDictionary,Guid,Guid,string,int>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "数据字典数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisDataDictionary).TableName();

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<ArtemisDataDictionary> builder)
    {
        // Each DataDictionary can have many DataDictionaryItems
        builder.HasMany(dataDictionary => dataDictionary.Items)
            .WithOne(item => item.DataDictionary)
            .HasForeignKey(item => item.DataDictionaryId)
            .HasConstraintName(ForeignKeyName(
                nameof(ArtemisDataDictionaryItem).TableName(),
                nameof(ArtemisDataDictionary).TableName()))
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }

    #endregion
}