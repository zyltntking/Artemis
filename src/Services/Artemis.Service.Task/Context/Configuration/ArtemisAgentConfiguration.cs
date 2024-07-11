using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;

namespace Artemis.Service.Task.Context.Configuration;

/// <summary>
///     代理数据集配置
/// </summary>
internal sealed class ArtemisAgentConfiguration : ConcurrencyPartitionEntityConfiguration<ArtemisAgent>
{
    #region Overrides of ConcurrencyPartitionEntityConfiguration<ArtemisAgent>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "代理数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisAgent).TableName();

    #endregion
}