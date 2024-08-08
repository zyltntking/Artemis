using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;

namespace Artemis.Service.Business.VisionScreen.Context.Configuration;

/// <summary>
///     通知消息配置
/// </summary>
internal sealed class ArtemisNotificationMessageConfiguration : 
    ConcurrencyPartitionEntityConfiguration<ArtemisNotificationMessage>
{
    #region Overrides

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "通知消息数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisNotificationMessage).TableName();


    #endregion
}