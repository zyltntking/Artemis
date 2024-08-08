using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.Business.VisionScreen.Context.Configuration;

/// <summary>
///     视力档案数据集配置
/// </summary>
internal sealed class ArtemisVisionScreenRecordConfiguration : ConcurrencyPartitionEntityConfiguration<ArtemisVisionScreenRecord>
{
    #region Overrides of ConcurrencyPartitionEntityConfiguration<ArtemisStudent>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "视力档案数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisVisionScreenRecord).TableName();

    /// <summary>
    ///     实体字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityFieldConfigure(EntityTypeBuilder<ArtemisVisionScreenRecord> builder)
    {
        builder.Property(record => record.PrescribedTime)
            .HasColumnType(DataTypeSet.DateTime);

        builder.Property(record => record.Birthday)
            .HasColumnType(DataTypeSet.DateTime);

        builder.Property(record => record.ChartOperationTime)
            .HasColumnType(DataTypeSet.DateTime);

        builder.Property(record => record.OptometerOperationTime)
            .HasColumnType(DataTypeSet.DateTime);

        builder.Property(record => record.CheckTime)
            .HasColumnType(DataTypeSet.DateTime);

        builder.Property(record => record.ReportSendTime)
            .HasColumnType(DataTypeSet.DateTime);

        builder.Property(record => record.ReportReceiveTime)
            .HasColumnType(DataTypeSet.DateTime);
    }

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<ArtemisVisionScreenRecord> builder)
    {
        // each VisionScreenRecord has many Optometer
        builder.HasMany(record => record.Optometers)
            .WithOne(optometer => optometer.VisionScreenRecord)
            .HasForeignKey(optometer => optometer.RecordId)
            .HasConstraintName(ForeignKeyName(
                nameof(ArtemisOptometer).TableName(),
                nameof(ArtemisVisionScreenRecord).TableName()))
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // each VisionScreenRecord has many VisualChart
        builder.HasMany(record => record.VisualCharts)
            .WithOne(chart => chart.VisionScreenRecord)
            .HasForeignKey(chart => chart.RecordId)
            .HasConstraintName(ForeignKeyName(
                nameof(ArtemisVisualChart).TableName(),
                nameof(ArtemisVisionScreenRecord).TableName()))
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // each VisionScreenRecord has many RecordFeedback
        builder.HasMany(record => record.RecordFeedbacks)
            .WithOne(feedback => feedback.VisionScreenRecord)
            .HasForeignKey(feedback => feedback.RecordId)
            .HasConstraintName(ForeignKeyName(
                nameof(ArtemisRecordFeedback).TableName(),
                nameof(ArtemisVisionScreenRecord).TableName()))
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
     
    }

    #endregion
}