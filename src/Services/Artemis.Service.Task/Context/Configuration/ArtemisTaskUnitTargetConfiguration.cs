using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;

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

    #endregion
}