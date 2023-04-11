using Artemis.Data.Core;
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
    /// <param name="optionsAccessor">配置依赖</param>
    /// <param name="errors">错误依赖</param>
    /// <param name="logger">日志依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    protected Manager(
        IStore<TEntity> store,
        IOptions<IStoreOptions>? optionsAccessor = null,
        IStoreErrorDescriber? errors = null,
        ILogger<IManager<TEntity>>? logger = null) : base(store, optionsAccessor, errors, logger)
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
    /// <exception cref="ArgumentNullException"></exception>
    protected Manager(
        IStore<TEntity, TKey> store,
        IOptions<IStoreOptions>? optionsAccessor = null,
        IStoreErrorDescriber? errors = null,
        ILogger<IManager<TEntity, TKey>>? logger = null)
    {
        Store = store ?? throw new ArgumentNullException(nameof(store));
        StoreOptions = optionsAccessor?.Value ?? new StoreOptions();
        StoreErrorDescriber = errors ?? new StoreErrorDescriber();
        Logger = logger ?? new NullLogger<IManager<TEntity, TKey>>();

        Store.AutoSaveChanges = StoreOptions.AutoSaveChanges;
        Store.MetaDataHosting = StoreOptions.MetaDataHosting;
        Store.SoftDelete = StoreOptions.SoftDelete;
    }

    /// <summary>
    ///     存储访问器
    /// </summary>
    protected IStore<TEntity, TKey> Store { get; set; }

    /// <summary>
    ///     配置访问器
    /// </summary>
    private IStoreOptions StoreOptions { get; }

    /// <summary>
    ///     错误报告生成器
    /// </summary>
    private IStoreErrorDescriber StoreErrorDescriber { get; set; }

    /// <summary>
    ///     日志依赖访问器
    /// </summary>
    protected ILogger Logger { get; }

    /// <summary>
    ///     Throws if this class has been disposed.
    /// </summary>
    protected void ThrowIfDisposed()
    {
        if (_disposed) throw new ObjectDisposedException(GetType().Name);
    }

    #region IDisposable

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
    protected virtual void Dispose(bool disposing)
    {
        if (disposing && !_disposed)
        {
            Store.Dispose();
            _disposed = true;
        }
    }

    #endregion
}