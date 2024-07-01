using Artemis.Data.Store.Configuration;

namespace Artemis.Service.School.Context.Configuration;

/// <summary>
///     学生配置
/// </summary>
internal sealed class ArtemisStudentConfiguration : ConcurrencyPartitionEntityConfiguration<ArtemisStudent>
{
    #region Overrides of ConcurrencyPartitionEntityConfiguration<ArtemisStudent>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "学生数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => "Student";

    #endregion
}