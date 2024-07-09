using Artemis.Data.Shared;
using Artemis.Data.Store.Configuration;

namespace Artemis.Service.Resource.Context.Configuration;

/// <summary>
///     设备实体配置
/// </summary>
internal sealed class ArtemisDeviceConfiguration : ConcurrencyModelEntityConfiguration<ArtemisDevice>
{
    #region Overrides of BaseEntityConfiguration<ArtemisDevice,Guid,Guid,string,int>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "设备数据集";

    /// <summary>
    ///     表明
    /// </summary>
    protected override string TableName => nameof(ArtemisDevice).TableName();

    #endregion
}