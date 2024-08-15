using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.Task.Context.Configuration;

/// <summary>
///     任务目标配置
/// </summary>
internal sealed class ArtemisTaskUnitTargetConfiguration : ConcurrencyModelEntityConfiguration<ArtemisTaskUnitTarget>
{
    #region Overrides 

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "任务目标数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisTaskUnitTarget).TableName();

    /// <summary>
    ///     实体字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityFieldConfigure(EntityTypeBuilder<ArtemisTaskUnitTarget> builder)
    {
        builder.Property(taskUnit => taskUnit.ExecuteTime)
            .HasColumnType(DataTypeSet.DateTime);
    }

    #endregion
}