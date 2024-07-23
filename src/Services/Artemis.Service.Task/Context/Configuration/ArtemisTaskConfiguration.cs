using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.Task.Context.Configuration;

/// <summary>
///     任务配置
/// </summary>
internal sealed class ArtemisTaskConfiguration : ConcurrencyPartitionEntityConfiguration<ArtemisTask>
{
    #region Overrides of ConcurrencyPartitionEntityConfiguration<ArtemisTask>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "任务数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisTask).TableName();

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<ArtemisTask> builder)
    {
        // Task Index
        builder.HasIndex(task => task.NormalizedTaskName)
            .HasDatabaseName(IndexName("TaskName"))
            .IsUnique();

        builder.HasIndex(task => task.TaskShip)
            .HasDatabaseName(IndexName("TaskShip"));

        builder.HasIndex(task => task.TaskMode)
            .HasDatabaseName(IndexName("TaskMode"));

        builder.HasIndex(task => task.TaskStatus)
            .HasDatabaseName(IndexName("TaskStatus"));

        builder.HasIndex(task => task.StartTime)
            .HasDatabaseName(IndexName("StartTime"));

        builder.HasIndex(task => task.EndTime)
            .HasDatabaseName(IndexName("EndTime"));

        // Each Task can have many Task Units
        builder.HasMany(task => task.TaskUnits)
            .WithOne(taskUnit => taskUnit.Task)
            .HasForeignKey(taskUnit => taskUnit.TaskId)
            .HasConstraintName(ForeignKeyName(
                nameof(ArtemisTaskUnit).TableName(),
                nameof(ArtemisTask).TableName()))
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Each Task can have many Children Task
        builder.HasMany(task => task.Children)
            .WithOne(child => child.Parent)
            .HasForeignKey(child => child.ParentId)
            .HasConstraintName(ForeignKeyName(
                nameof(ArtemisTask).TableName(),
                nameof(ArtemisTask).TableName()))
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    ///     实体字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityFieldConfigure(EntityTypeBuilder<ArtemisTask> builder)
    {
        builder.Property(task => task.StartTime)
            .HasColumnType(DataTypeSet.DateTime);

        builder.Property(task => task.EndTime)
            .HasColumnType(DataTypeSet.DateTime);
    }

    #endregion
}