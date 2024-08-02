using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;

namespace Artemis.Service.Business.VisionScreen.Context.Configuration;

/// <summary>
///     视力表数据集配置
/// </summary>
internal sealed class ArtemisVisualChartConfiguration : ConcurrencyPartitionEntityConfiguration<ArtemisVisualChart>
{
    #region Overrides of ConcurrencyPartitionEntityConfiguration<ArtemisStudent>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "视力表数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisVisualChart).TableName();

    #endregion
}