namespace Artemis.Data.Store;

/// <summary>
///     存储配置接口
/// </summary>
public interface IStoreOptions
{
    /// <summary>
    ///     是否启用自动保存
    /// </summary>
    bool AutoSaveChanges { get; set; }

    /// <summary>
    ///     是否启用元数据托管
    /// </summary>
    bool MetaDataHosting { get; set; }

    /// <summary>
    ///     是否启用软删除
    /// </summary>
    bool SoftDelete { get; set; }

    /// <summary>
    ///     是否启用具缓存策略
    /// </summary>
    bool CachedStore { get; set; }

    /// <summary>
    ///     过期时间(秒)
    /// </summary>
    int Expires { get; set; }

    /// <summary>
    ///     是否启用Debug日志
    /// </summary>
    bool DebugLogger { get; set; }
}