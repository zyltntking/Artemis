using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Artemis.Data.Store;

#region Interface

/// <summary>
///     提供用于管理TEntity的存储器的API接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public interface IManager<TEntity> : IManager<TEntity, Guid> where TEntity : IModelBase
{
}

/// <summary>
///     提供用于管理TEntity的存储器的API接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TKey">键类型</typeparam>
public interface IManager<TEntity, TKey> where TEntity : IModelBase<TKey> where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     规范化键
    /// </summary>
    /// <param name="key">键</param>
    /// <returns>规范化后的键</returns>
    public string NormalizeKey(string key);

    /// <summary>
    ///     缓存键
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    public void SetKey(string key, TKey value);

    /// <summary>
    ///     缓存键
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public Task? SetKeyAsync(string key, TKey value, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取键
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    public TKey? GetKey(string key);

    /// <summary>
    /// 获取键
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public Task<TKey?> GetKeyAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// 移除键
    /// </summary>
    /// <param name="key">键</param>
    public void RemoveKey(string key);

    /// <summary>
    /// 移除键
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public Task? RemoveKeyAsync(string key, CancellationToken cancellationToken = default);
}

#endregion

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
    /// <param name="logger">日志依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    protected Manager(
        IStore<TEntity> store,
        IDistributedCache? cache = null,
        IOptions<StoreOptions>? optionsAccessor = null,
        ILogger? logger = null) : base(store, cache, optionsAccessor, null, logger)
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
        StoreErrorDescriber? errors = null,
        ILogger? logger = null)
    {
        Store = store;
        StoreOptions = optionsAccessor?.Value ?? new StoreOptions();
        Describer = errors ?? new StoreErrorDescriber();
        Logger = logger;
        Cache = cache;

        Store.SetOptions(StoreOptions);
    }

    #region Properties

    /// <summary>
    ///     存储访问器
    /// </summary>
    protected IStore<TEntity, TKey> Store { get; }

    /// <summary>
    ///     缓存访问器
    /// </summary>
    private IDistributedCache? Cache { get; }

    /// <summary>
    ///     缓存是否可用
    /// </summary>
    protected bool CacheAvailable => StoreOptions.CachedManager;

    /// <summary>
    ///     配置访问器
    /// </summary>
    private StoreOptions StoreOptions { get; }

    /// <summary>
    ///     错误报告生成器
    /// </summary>
    protected StoreErrorDescriber Describer { get; }

    /// <summary>
    ///     日志依赖访问器
    /// </summary>
    private ILogger? Logger { get; }

    #endregion

    #region DebugLogger

    /// <summary>
    ///     设置Debug日志
    /// </summary>
    /// <param name="message">日志消息</param>
    protected void SetDebugLog(string message)
    {
        if (StoreOptions.DebugLogger)
            Logger?.LogDebug(message);
    }

    #endregion

    /// <summary>
    ///     Throws if this class has been disposed.
    /// </summary>
    protected void ThrowIfDisposed()
    {
        if (_disposed) throw new ManagerDisposedException(GetType().Name);
    }

    /// <summary>
    /// 键前缀
    /// </summary>
    protected string KeyPrefix => "Manager";

    /// <summary>
    ///     生成Key
    /// </summary>
    /// <param name="args">生成参数</param>
    /// <returns></returns>
    protected string GenerateKey(params string[] args)
    {
        return string.Join(":", args);
    }

    #region Implementation of IManager<TEntity,in TKey>

    /// <summary>
    ///     规范化键
    /// </summary>
    /// <param name="key">键</param>
    /// <returns>规范化后的键</returns>
    public string NormalizeKey(string key) => Store.NormalizeKey(key);

    #region CacheAccess

    /// <summary>
    ///     缓存键
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    public void SetKey(string key, TKey value) => Cache?.SetString(key, Store.ConvertIdToString(value)!);

    /// <summary>
    ///     缓存键
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public Task? SetKeyAsync(string key, TKey value, CancellationToken cancellationToken = default) => Cache?.SetStringAsync(key, Store.ConvertIdToString(value)!, cancellationToken);

    /// <summary>
    /// 获取键
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    public TKey? GetKey(string key)
    {
        var keyString = Cache?.GetString(key);
        return keyString is null ? default : Store.ConvertIdFromString(keyString)!;
    }

    /// <summary>
    /// 获取键
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<TKey?> GetKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        var keyString = await Cache?.GetStringAsync(key, cancellationToken)!;
        return keyString is null ? default : Store.ConvertIdFromString(keyString)!;
    }

    /// <summary>
    /// 移除键
    /// </summary>
    /// <param name="key">键</param>
    public void RemoveKey(string key) => Cache?.Remove(key);

    /// <summary>
    /// 移除键
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public Task? RemoveKeyAsync(string key, CancellationToken cancellationToken = default) => Cache?.RemoveAsync(key, cancellationToken);

    #endregion

    #endregion

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
        if (!disposing || _disposed)
            return;
        StoreDispose();
        _disposed = true;
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