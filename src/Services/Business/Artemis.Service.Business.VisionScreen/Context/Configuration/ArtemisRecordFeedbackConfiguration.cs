using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.Business.VisionScreen.Context.Configuration;

/// <summary>
///     记录反馈数据集配置
/// </summary>
internal sealed class ArtemisRecordFeedbackConfiguration : ConcurrencyPartitionEntityConfiguration<ArtemisRecordFeedback>
{
    #region Overrides of ConcurrencyPartitionEntityConfiguration<ArtemisStudent>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "记录反馈数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisRecordFeedback).TableName();

    /// <summary>
    ///     实体字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityFieldConfigure(EntityTypeBuilder<ArtemisRecordFeedback> builder)
    {
        builder.Property(feedBack => feedBack.FeedBackTime)
            .HasColumnType(DataTypeSet.DateTime);

        builder.Property(feedBack => feedBack.CheckDate)
            .HasColumnType(DataTypeSet.DateTime);
    }

    #endregion
}