using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;

namespace Artemis.Service.Business.VisionScreen.Context.Configuration;

/// <summary>
///     学生眼部照片配置
/// </summary>
internal sealed class ArtemisStudentEyePhotoConfiguration : 
    ConcurrencyPartitionEntityConfiguration<ArtemisStudentEyePhoto>
{
    #region Overrides

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "学生眼部照片数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisStudentEyePhoto).TableName();


    #endregion
}