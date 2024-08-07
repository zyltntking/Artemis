using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.Business.VisionScreen.Context.Configuration;

/// <summary>
///     验光仪数据集配置
/// </summary>
internal sealed class ArtemisOptometerConfiguration : 
    ConcurrencyPartitionEntityConfiguration<ArtemisOptometer>
{
    #region Overrides

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "验光仪数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisOptometer).TableName();

    /// <summary>
    ///     实体字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityFieldConfigure(EntityTypeBuilder<ArtemisOptometer> builder)
    {
        builder.Property(record => record.OptometerOperationTime)
            .HasColumnType(DataTypeSet.DateTime);
    }

    #endregion
}