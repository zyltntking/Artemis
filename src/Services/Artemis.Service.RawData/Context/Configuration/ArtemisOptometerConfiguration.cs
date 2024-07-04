using Artemis.Data.Shared;
using Artemis.Data.Store.Configuration;

namespace Artemis.Service.RawData.Context.Configuration;

/// <summary>
///     验光仪数据集配置
/// </summary>
internal sealed class ArtemisOptometerConfiguration : ConcurrencyPartitionEntityConfiguration<ArtemisOptometer>
{
    #region Overrides of ConcurrencyPartitionEntityConfiguration<ArtemisOptometer>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "验光仪数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisOptometer).TableName();

    #endregion
}