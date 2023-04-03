namespace Artemis.Data.Store;

/// <summary>
/// 存储配置接口
/// </summary>
public interface IStoreOptions
{
    /// <summary>
    /// 是否启用自动保存
    /// </summary>
    bool AutoSaveChanges { get; set; }

    /// <summary>
    /// 是否启用元数据托管
    /// </summary>
    bool MetaDataHosting { get; set; }

    /// <summary>
    /// 是否启用软删除
    /// </summary>
    bool SoftDelete { get; set; }
}