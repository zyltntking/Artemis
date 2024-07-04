using Artemis.Data.Shared;
using Artemis.Data.Store.Configuration;

namespace Artemis.Service.School.Context.Configuration;

/// <summary>
///     班级配置
/// </summary>
internal sealed class ArtemisClassConfiguration : ConcurrencyPartitionEntityConfiguration<ArtemisClass>
{
    #region Overrides of ConcurrencyPartitionEntityConfiguration<ArtemisClass>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "班级数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisClass).TableName();

    #endregion
}