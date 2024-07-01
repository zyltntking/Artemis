using Artemis.Data.Store.Configuration;

namespace Artemis.Service.School.Context.Configuration;

/// <summary>
///     教师配置
/// </summary>
internal sealed class ArtemisTeacherConfiguration : ConcurrencyPartitionEntityConfiguration<ArtemisTeacher>
{
    #region Overrides of ConcurrencyPartitionEntityConfiguration<ArtemisTeacher>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "教师数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisTeacher);

    #endregion
}