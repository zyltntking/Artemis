using Artemis.Data.Shared;
using Artemis.Data.Store.Configuration;
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
        // Each Task can have many Task Units
        builder.HasMany(task => task.TaskUnits)
            .WithOne(taskUnit => taskUnit.Task)
            .HasForeignKey(taskUnit => taskUnit.TaskId)
            .HasConstraintName(ForeignKeyName(
                nameof(ArtemisTaskUnit).TableName(), 
                nameof(ArtemisTask).TableName()))
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }

    #endregion
}