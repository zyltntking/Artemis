using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.Task.Context.Configuration;

/// <summary>
///     任务单元配置
/// </summary>
internal sealed class ArtemisTaskUnitConfiguration : ConcurrencyModelEntityConfiguration<ArtemisTaskUnit>
{
    #region Overrides of ConcurrencyPartitionEntityConfiguration<ArtemisTaskUnit>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "任务单元数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisTaskUnit).TableName();

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<ArtemisTaskUnit> builder)
    {
        // Task Index
        builder.HasIndex(task => task.NormalizedUnitName)
            .HasDatabaseName(IndexName("UnitName"));

        builder.HasIndex(task => task.UnitCode)
            .HasDatabaseName(IndexName("UnitCode"));

        builder.HasIndex(task => task.DesignCode)
            .HasDatabaseName(IndexName("DesignCode"));

        builder.HasIndex(task => task.TaskUnitMode)
            .HasDatabaseName(IndexName("TaskUnitMode"));

        builder.HasIndex(task => task.TaskUnitState)
            .HasDatabaseName(IndexName("TaskUnitState"));

        builder.HasIndex(task => task.StartTime)
            .HasDatabaseName(IndexName("StartTime"));

        builder.HasIndex(task => task.EndTime)
            .HasDatabaseName(IndexName("EndTime"));

        // Each Task Unit can have many Task Targets
        builder.HasMany(taskUnit => taskUnit.TaskUnitTargets)
            .WithOne(taskTarget => taskTarget.TaskUnit)
            .HasForeignKey(taskTarget => taskTarget.TaskUnitId)
            .HasConstraintName(ForeignKeyName(nameof(ArtemisTaskUnitTarget), nameof(ArtemisTaskUnit)))
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    ///     实体字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityFieldConfigure(EntityTypeBuilder<ArtemisTaskUnit> builder)
    {
        builder.Property(taskUnit => taskUnit.StartTime)
            .HasColumnType(DataTypeSet.DateTime);

        builder.Property(taskUnit => taskUnit.EndTime)
            .HasColumnType(DataTypeSet.DateTime);
    }

    #endregion
}