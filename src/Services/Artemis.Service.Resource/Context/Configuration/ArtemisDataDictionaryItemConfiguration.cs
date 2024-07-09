using Artemis.Data.Shared;
using Artemis.Data.Store.Configuration;

namespace Artemis.Service.Resource.Context.Configuration;

/// <summary>
///     数据字典项目配置
/// </summary>
internal sealed class
    ArtemisDataDictionaryItemConfiguration : ConcurrencyModelEntityConfiguration<ArtemisDataDictionaryItem>
{
    #region Overrides of BaseEntityConfiguration<ArtemisDataDictionary,Guid,Guid,string,int>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "数据字典项目数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisDataDictionary).TableName();

    #endregion
}