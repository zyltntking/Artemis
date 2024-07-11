﻿using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.Task.Context.Configuration;

/// <summary>
///     任务单元配置
/// </summary>
internal sealed class ArtemisTaskUnitConfiguration : ConcurrencyPartitionEntityConfiguration<ArtemisTaskUnit>
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
        // Each Task Unit can have many Task Targets
        builder.HasMany(taskUnit => taskUnit.TaskTargets)
            .WithOne(taskTarget => taskTarget.TaskUnit)
            .HasForeignKey(taskTarget => taskTarget.TaskUnitId)
            .HasConstraintName(ForeignKeyName(nameof(ArtemisTaskTarget), nameof(ArtemisTaskUnit)))
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }

    #endregion
}