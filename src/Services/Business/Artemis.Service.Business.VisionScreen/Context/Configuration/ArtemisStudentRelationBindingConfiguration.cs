using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;

namespace Artemis.Service.Business.VisionScreen.Context.Configuration;

/// <summary>
///     验光仪数据集配置
/// </summary>
internal sealed class ArtemisStudentRelationBindingConfiguration : 
    ConcurrencyPartitionEntityConfiguration<ArtemisStudentRelationBinding>
{
    #region Overrides

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "用户学生亲属关系绑定数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisStudentRelationBinding).TableName();


    #endregion
}