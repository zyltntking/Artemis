namespace Artemis.Data.Store;

#region Interface

/// <summary>
///     存储配置接口
/// </summary>
public interface IStoreOptions : IKeyWithStoreOptions
{
    /// <summary>
    ///     是否启用元数据托管
    /// </summary>
    bool MetaDataHosting { get; set; }

    /// <summary>
    ///     是否启用软删除
    /// </summary>
    bool SoftDelete { get; set; }
}

/// <summary>
/// 具缓存存储配置接口
/// </summary>
public interface IKeyWithStoreOptions : IKeyLessStoreOptions
{
    /// <summary>
    ///     是否启用具缓存策略
    /// </summary>
    bool CachedStore { get; set; }

    /// <summary>
    ///     过期时间(秒)
    /// </summary>
    int Expires { get; set; }
}

/// <summary>
/// 无键存储配置接口
/// </summary>
public interface IKeyLessStoreOptions
{
    /// <summary>
    ///     是否启用自动保存
    /// </summary>
    bool AutoSaveChanges { get; set; }

    /// <summary>
    ///     是否启用Debug日志
    /// </summary>
    bool DebugLogger { get; set; }
}

#endregion

/// <summary>
///     存储配置
/// </summary>
public record ArtemisStoreOptions : KeyWithStoreOptions, IStoreOptions
{
    #region Implementation of IStoreConfig

    /// <summary>
    ///     是否启用元数据托管
    /// </summary>
    public bool MetaDataHosting { get; set; } = true;

    /// <summary>
    ///     是否启用软删除
    /// </summary>
    public bool SoftDelete { get; set; } = false;

    #endregion
}

/// <summary>
/// 缓存存储配置
/// </summary>
public record KeyWithStoreOptions : KeyLessStoreOptions, IKeyWithStoreOptions
{
    #region Implementation of ICacheStoreOptions

    /// <summary>
    ///     是否启用具缓存存储策略
    /// </summary>
    public bool CachedStore { get; set; } = false;

    /// <summary>
    ///     过期时间(秒)
    /// </summary>
    public int Expires { get; set; } = 0;

    #endregion
}

/// <summary>
/// 无键存储配置
/// </summary>
public record KeyLessStoreOptions : IKeyLessStoreOptions
{
    #region Implementation of IStoreConfig

    /// <summary>
    ///     是否启用自动保存
    /// </summary>
    public bool AutoSaveChanges { get; set; } = true;

    /// <summary>
    ///     是否启用Debug日志
    /// </summary>
    public bool DebugLogger { get; set; } = false;

    #endregion
}