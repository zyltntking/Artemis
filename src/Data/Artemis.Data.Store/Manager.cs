using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace Artemis.Data.Store;

#region Interface

/// <summary>
///     提供用于管理具键存储模型TEntity的存储器的API接口
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IManager<TEntity> : IManager<TEntity, Guid> where TEntity : class, IKeySlot;

/// <summary>
///     提供用于管理具键存储模型TEntity的存储器的API接口
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
public interface IManager<TEntity, in TKey> : IManager<TEntity, TKey, Guid>
    where TEntity : class, IKeySlot<TKey>
    where TKey : IEquatable<TKey>;

/// <summary>
///     提供用于管理具键存储模型TEntity的存储器的API接口
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
public interface IManager<TEntity, in TKey, THandler> : IKeyLessManager<TEntity, THandler>
    where TEntity : class, IKeySlot<TKey>
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>;

/// <summary>
///     提供用于管理无键存储模型TEntity的存储器的API接口
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IKeyLessManager<TEntity> : IKeyLessManager<TEntity, Guid> where TEntity : class
{
}

/// <summary>
///     提供用于管理无键存储模型TEntity的存储器的API接口
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="THandler"></typeparam>
public interface IKeyLessManager<TEntity, THandler> : IDisposable
    where TEntity : class
    where THandler : IEquatable<THandler>
{
}

#endregion

/// <summary>
///     提供用于管理具键存储模型TEntity的存储器的API
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class Manager<TEntity> : Manager<TEntity, Guid>, IManager<TEntity>
    where TEntity : class, IKeySlot
{
    /// <summary>
    ///     创建新的管理器实例
    /// </summary>
    /// <param name="store">存储访问器依赖</param>
    /// <param name="options">配置依赖</param>
    /// <param name="logger">日志依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    protected Manager(
        IStore<TEntity> store,
        IManagerOptions? options = null,
        ILogger? logger = null) : base(store, options, logger)
    {
    }
}

/// <summary>
///     提供用于管理具键存储模型TEntity的存储器的API
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
public abstract class Manager<TEntity, TKey> :
    Manager<TEntity, TKey, Guid>,
    IManager<TEntity, TKey>
    where TEntity : class, IKeySlot<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     创建新的管理器实例
    /// </summary>
    /// <param name="store">存储访问器依赖</param>
    /// <param name="options">配置依赖</param>
    /// <param name="logger">日志依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    protected Manager(
        IStore<TEntity, TKey> store,
        IManagerOptions? options = null,
        ILogger? logger = null) : base(store, options, logger)
    {
    }
}

/// <summary>
///     提供用于管理具键存储模型TEntity的存储器的API
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
public abstract class Manager<TEntity, TKey, THandler> :
    KeyLessManager<TEntity, THandler>,
    IManager<TEntity, TKey, THandler>
    where TEntity : class, IKeySlot<TKey>
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>
{
    /// <summary>
    ///     创建新的管理器实例
    /// </summary>
    /// <param name="store">存储访问器依赖</param>
    /// <param name="options">配置依赖</param>
    /// <param name="logger">日志依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    protected Manager(
        IStore<TEntity, TKey, THandler> store,
        IManagerOptions? options = null,
        ILogger? logger = null) : base(store, options, logger)
    {
    }
}

/// <summary>
///     提供用于管理无键存储模型TEntity的存储器的API
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class KeyLessManager<TEntity> : KeyLessManager<TEntity, Guid>, IKeyLessManager<TEntity>
    where TEntity : class
{
    /// <summary>
    ///     创建新的管理器实例
    /// </summary>
    /// <param name="store">存储访问器依赖</param>
    /// <param name="options">配置依赖</param>
    /// <param name="logger">日志依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    protected KeyLessManager(
        IKeyLessStore<TEntity> store,
        IManagerOptions? options = null,
        ILogger? logger = null) : base(store, options, logger)
    {
    }
}

/// <summary>
///     提供用于管理无键存储模型TEntity的存储器的API
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="THandler"></typeparam>
public abstract class KeyLessManager<TEntity, THandler> : IKeyLessManager<TEntity, THandler>
    where TEntity : class
    where
    THandler : IEquatable<THandler>
{
    /// <summary>
    ///     创建新的管理器实例
    /// </summary>
    /// <param name="store">存储访问器依赖</param>
    /// <param name="options">配置依赖</param>
    /// <param name="logger">日志依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    protected KeyLessManager(
        IKeyLessStore<TEntity, THandler> store,
        IManagerOptions? options = null,
        ILogger? logger = null)
    {
        EntityStore = store;
        Options = options ?? new ManagerOptions();
        Logger = logger;
    }

    #region OptionsAccessor

    /// <summary>
    ///     是否启用Debug日志
    /// </summary>
    protected bool EnableLogger => Options.EnableLogger;

    #endregion

    #region Action

    /// <summary>
    ///     异步函数执行前
    /// </summary>
    /// <param name="cancellationToken"></param>
    protected void OnAsyncActionExecuting(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();

        cancellationToken.ThrowIfCancellationRequested();
    }

    #endregion

    #region Properties

    /// <summary>
    ///     具键存储管理器配置接口
    /// </summary>
    private IManagerOptions Options { get; }

    /// <summary>
    ///     日志依赖访问器
    /// </summary>
    protected ILogger? Logger { get; }

    /// <summary>
    ///     实体存储
    /// </summary>
    private IKeyLessStore<TEntity, THandler> EntityStore { get; }

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
        EntityStore.Dispose();
        StoreDispose();
        _disposed = true;
    }

    /// <summary>
    ///     释放托管的Store
    /// </summary>
    protected abstract void StoreDispose();

    /// <summary>
    ///     Throws if this class has been disposed.
    /// </summary>
    protected void ThrowIfDisposed()
    {
        if (_disposed) throw new ManagerDisposedException(GetType().Name);
    }

    #endregion
}