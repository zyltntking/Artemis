using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;

namespace Artemis.Service.Task.Context.Configuration;

/// <summary>
///     任务单元代理配置
/// </summary>
internal sealed class ArtemisTaskUnitAgentConfiguration : BaseEntityConfiguration<ArtemisTaskUnitAgent>
{
    #region Overrides of BaseConfiguration<ArtemisTaskAgent,Guid,Guid,string,int>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "任务单元代理配置";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisTaskUnitAgent).TableName();

    #endregion
}