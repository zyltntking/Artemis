﻿namespace Artemis.Data.Store;

#region Interface

/// <summary>
///     具键存储管理器配置接口
/// </summary>
public interface IStoreManagerOptions : IKeyLessStoreManagerOptions
{
    /// <summary>
    ///     是否启用具缓存策略
    /// </summary>
    bool CachedManager { get; set; }

    /// <summary>
    ///     过期时间(秒)
    /// </summary>
    int Expires { get; set; }
}

/// <summary>
///     无键存储管理器配置接口
/// </summary>
public interface IKeyLessStoreManagerOptions
{
    /// <summary>
    ///     是否启用Debug日志
    /// </summary>
    bool DebugLogger { get; set; }
}

#endregion

/// <summary>
///     具键存储管理器配置实例
/// </summary>
public class StoreManagerOptions : KeyLessStoreManagerOptions, IStoreManagerOptions
{
    #region Implementation of IStoreManagerOptions

    /// <summary>
    ///     是否启用具缓存策略
    /// </summary>
    public bool CachedManager { get; set; } = false;

    /// <summary>
    ///     过期时间(秒)
    /// </summary>
    public int Expires { get; set; } = 0;

    #endregion
}

/// <summary>
///     无键存储管理器配置实例
/// </summary>
public class KeyLessStoreManagerOptions : IKeyLessStoreManagerOptions
{
    #region Implementation of IKeyLessStoreManagerOptions

    /// <summary>
    ///     是否启用Debug日志
    /// </summary>
    public bool DebugLogger { get; set; } = false;

    #endregion
}