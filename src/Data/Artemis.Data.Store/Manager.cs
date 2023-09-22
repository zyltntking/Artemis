﻿using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Artemis.Data.Store;

/// <summary>
///     提供用于管理TEntity的存储器的API
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public abstract class Manager<TEntity> : Manager<TEntity, Guid>, IManager<TEntity> where TEntity : class, IModelBase
{
    /// <summary>
    ///     创建新的管理器实例
    /// </summary>
    /// <param name="store">存储访问器依赖</param>
    /// <param name="cache">缓存管理器</param>
    /// <param name="optionsAccessor">配置依赖</param>
    /// <param name="errors">错误依赖</param>
    /// <param name="logger">日志依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    protected Manager(
        IStore<TEntity> store,
        IDistributedCache? cache = null,
        IOptions<StoreOptions>? optionsAccessor = null,
        IStoreErrorDescriber? errors = null,
        ILogger<IManager<TEntity>>? logger = null) : base(store, cache, optionsAccessor, errors, logger)
    {
    }
}

/// <summary>
///     提供用于管理TEntity的存储器的API
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TKey">键类型</typeparam>
public abstract class Manager<TEntity, TKey> : IManager<TEntity, TKey>, IDisposable
    where TEntity : class, IModelBase<TKey> where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     创建新的管理器实例
    /// </summary>
    /// <param name="store">存储访问器依赖</param>
    /// <param name="optionsAccessor">配置依赖</param>
    /// <param name="errors">错误依赖</param>
    /// <param name="logger">日志依赖</param>
    /// <param name="cache">缓存依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    protected Manager(
        IStore<TEntity, TKey> store,
        IDistributedCache? cache = null,
        IOptions<StoreOptions>? optionsAccessor = null,
        IStoreErrorDescriber? errors = null,
        ILogger<IManager<TEntity, TKey>>? logger = null)
    {
        Store = store;
        StoreOptions = optionsAccessor?.Value ?? new StoreOptions();
        StoreErrorDescriber = errors ?? new StoreErrorDescriber();
        Logger = logger ?? new NullLogger<IManager<TEntity, TKey>>();
        Cache = cache;

        Store.SetOptions(StoreOptions);
    }

    /// <summary>
    ///     存储访问器
    /// </summary>
    protected IStore<TEntity, TKey> Store { get; }

    /// <summary>
    /// 缓存访问器
    /// </summary>
    private IDistributedCache? Cache { get; }

    /// <summary>
    /// 缓存是否可用
    /// </summary>
    protected bool CacheAvailable => StoreOptions.CachedManager;

    /// <summary>
    ///     配置访问器
    /// </summary>
    private StoreOptions StoreOptions { get; }

    /// <summary>
    ///     错误报告生成器
    /// </summary>
    protected IStoreErrorDescriber StoreErrorDescriber { get; }

    /// <summary>
    ///     日志依赖访问器
    /// </summary>
    private ILogger Logger { get; }

    #region DebugLogger

    /// <summary>
    ///     设置Debug日志
    /// </summary>
    /// <param name="message">日志消息</param>
    protected void SetDebugLog(string message)
    {
        if (StoreOptions.DebugLogger)
            Logger.LogDebug(message);
    }

    #endregion

    #region Implementation of IManager<TEntity,in TKey>

    /// <summary>
    /// 规范化键
    /// </summary>
    /// <param name="key">键</param>
    /// <returns>规范化后的键</returns>
    public string NormalizeKey(string key)
    {
        return Store.NormalizeKey(key);
    }

    /// <summary>
    /// 缓存键
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    public void CacheKey(string key, TKey value)
    {
        Cache?.SetString(key, Store.ConvertIdToString(value)!);
    }

    /// <summary>
    /// 缓存键
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public Task? CacheKeyAsync(string key, TKey value, CancellationToken cancellationToken = default)
    {
        return Cache?.SetStringAsync(key, Store.ConvertIdToString(value)!, cancellationToken);
    }

    #endregion

    /// <summary>
    ///     Throws if this class has been disposed.
    /// </summary>
    protected void ThrowIfDisposed()
    {
        if (_disposed) throw new ManagerDisposedException(GetType().Name);
    }

    #region IDisposable

    /// <summary>
    ///     标记资源是否已被或是否需要释放
    /// </summary>
    private bool _disposed;

    /// <summary>
    ///     Releases all resources used by the user manager.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Releases the unmanaged resources used by the role manager and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">
    ///     true to release both managed and unmanaged resources; false to release only unmanaged
    ///     resources.
    /// </param>
    private void Dispose(bool disposing)
    {
        if (disposing && !_disposed)
        {
            StoreDispose();
            _disposed = true;
        }
    }

    /// <summary>
    ///     释放托管的Store
    /// </summary>
    protected virtual void StoreDispose()
    {
        Store.Dispose();
    }

    #endregion
}