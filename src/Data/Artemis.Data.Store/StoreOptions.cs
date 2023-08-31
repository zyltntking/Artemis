namespace Artemis.Data.Store;

/// <summary>
///     存储配置
/// </summary>
public class StoreOptions : IStoreOptions
{
    #region Implementation of IStoreConfig

    /// <summary>
    ///     是否启用自动保存
    /// </summary>
    public bool AutoSaveChanges { get; set; } = true;

    /// <summary>
    ///     是否启用元数据托管
    /// </summary>
    public bool MetaDataHosting { get; set; } = true;

    /// <summary>
    ///     是否启用软删除
    /// </summary>
    public bool SoftDelete { get; set; } = false;

    /// <summary>
    ///     是否启用具缓存策略
    /// </summary>
    public bool CachedStore { get; set; } = false;

    /// <summary>
    ///     过期时间(秒)
    /// </summary>
    public int Expires { get; set; } = 0;

    /// <summary>
    ///     是否启用Debug日志
    /// </summary>
    public bool DebugLogger { get; set; } = false;

    #endregion
}