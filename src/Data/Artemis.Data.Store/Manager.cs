using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;
using Artemis.Data.Store.Extensions;
using Mapster;
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
    where THandler : IEquatable<THandler>
{
    #region BaseResourceManager

    /// <summary>
    ///     获取实体信息
    /// </summary>
    /// <typeparam name="TEntityInfo">实体信息类型</typeparam>
    /// <param name="id">实体标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<TEntityInfo?> GetEntityAsync<TEntityInfo>(TKey id, CancellationToken cancellationToken);

    /// <summary>
    ///     获取实体信息列表
    /// </summary>
    /// <typeparam name="TEntityInfo">实体信息类型</typeparam>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>实体信息列表</returns>
    Task<List<TEntityInfo>> GetEntitiesAsync<TEntityInfo>(int? page, int? size,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     创建实体
    /// </summary>
    /// <typeparam name="TEntityInfo">实体信息类型</typeparam>
    /// <typeparam name="TEntityPack">实体包类型</typeparam>
    /// <param name="package">实体包</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>创建结果</returns>
    Task<(StoreResult result, TEntityInfo? info)> CreateEntityAsync<TEntityInfo, TEntityPack>(
        TEntityPack package,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     更新实体
    /// </summary>
    /// <typeparam name="TEntityInfo">实体信息类型</typeparam>
    /// <typeparam name="TEntityPack">实体包类型</typeparam>
    /// <param name="id">实体键</param>
    /// <param name="pack">实体包</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>更新结果</returns>
    Task<(StoreResult result, TEntityInfo? info)> UpdateEntityAsync<TEntityInfo, TEntityPack>(
        TKey id,
        TEntityPack pack,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     更新或创建实体
    /// </summary>
    /// <typeparam name="TEntityInfo">实体信息类型</typeparam>
    /// <typeparam name="TEntityPack">实体包类型</typeparam>
    /// <param name="id">实体键</param>
    /// <param name="package">实体包</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>更新或创建结果</returns>
    Task<(StoreResult? result, TEntityInfo? info)> UpdateOrCreateEntityAsync<TEntityInfo, TEntityPack>(
        TKey id,
        TEntityPack package,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     删除实体
    /// </summary>
    /// <param name="id">标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    Task<StoreResult> DeleteEntityAsync(TKey id, CancellationToken cancellationToken = default);

    #endregion
}

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
    /// <summary>
    ///     注册操作员
    /// </summary>
    Func<THandler>? HandlerRegister { get; set; }
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
        EntityStore = store;
        EntityStore.HandlerRegister = HandlerRegister;
    }

    #region Implementation of IManager<TEntity>

    /// <summary>
    ///     实体存储
    /// </summary>
    protected new IStore<TEntity> EntityStore { get; }

    #endregion
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
        EntityStore = store;
        EntityStore.HandlerRegister = HandlerRegister;
    }

    #region Implementation of IManager<TEntity,THandler>

    /// <summary>
    ///     实体存储
    /// </summary>
    protected new IStore<TEntity, TKey> EntityStore { get; }

    #endregion
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
        EntityStore = store;
        EntityStore.HandlerRegister = HandlerRegister;
    }

    #region Properties

    /// <summary>
    ///     实体存储
    /// </summary>
    protected new IStore<TEntity, TKey, THandler> EntityStore { get; }

    #endregion

    #region Implementation of IKeyWithManager<TEntity,TKey>

    #region BaseResourceManager

    /// <summary>
    ///     获取实体信息
    /// </summary>
    /// <typeparam name="TEntityInfo">实体信息类型</typeparam>
    /// <param name="id">实体标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public Task<TEntityInfo?> GetEntityAsync<TEntityInfo>(TKey id, CancellationToken cancellationToken)
    {
        OnAsyncActionExecuting(cancellationToken);

        if (EnableLogger) Logger?.LogDebug($"GetEntityAsync<{typeof(TEntity).Name}>: {id}");

        return EntityStore.FindMapEntityAsync<TEntityInfo>(id, cancellationToken);
    }

    /// <summary>
    ///     获取实体信息列表
    /// </summary>
    /// <typeparam name="TEntityInfo">实体信息类型</typeparam>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>实体信息列表</returns>
    public Task<List<TEntityInfo>> GetEntitiesAsync<TEntityInfo>(int? page, int? size,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        if (EnableLogger) Logger?.LogDebug($"GetEntitiesAsync<{typeof(TEntity).Name}>: {page} {size}");

        return EntityStore.EntityQuery
            .MapPageAsync<TEntity, TKey, TEntityInfo>(
                page,
                size,
                cancellationToken);
    }

    /// <summary>
    ///     创建实体
    /// </summary>
    /// <typeparam name="TEntityInfo">实体信息类型</typeparam>
    /// <typeparam name="TEntityPack">实体包类型</typeparam>
    /// <param name="package">实体包</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>创建结果</returns>
    public async Task<(StoreResult result, TEntityInfo? info)> CreateEntityAsync<TEntityInfo, TEntityPack>(
        TEntityPack package,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        if (EnableLogger) Logger?.LogDebug($"CreateEntityAsync<{typeof(TEntity).Name}>: {package}");

        var entity = Instance.CreateInstance<TEntity, TEntityPack>(package);

        var result = await EntityStore.CreateAsync(entity, cancellationToken);

        return (result, entity.Adapt<TEntityInfo>());
    }

    /// <summary>
    ///     更新实体
    /// </summary>
    /// <typeparam name="TEntityInfo">实体信息类型</typeparam>
    /// <typeparam name="TEntityPack">实体包类型</typeparam>
    /// <param name="id">实体键</param>
    /// <param name="pack">实体包</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>更新结果</returns>
    public async Task<(StoreResult result, TEntityInfo? info)> UpdateEntityAsync<TEntityInfo, TEntityPack>(
        TKey id,
        TEntityPack pack,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        if (EnableLogger) Logger?.LogDebug($"UpdateEntityAsync<{typeof(TEntity).Name}>: {id} {pack}");

        var entity = await EntityStore.FindEntityAsync(id, cancellationToken);

        if (entity is null)
            return (StoreResult.EntityFoundFailed(typeof(TEntity).Name, id.ToString()!), default);

        pack.Adapt(entity);

        var result = await EntityStore.UpdateAsync(entity, cancellationToken);

        return (result, entity.Adapt<TEntityInfo>());
    }

    /// <summary>
    ///     更新或创建实体
    /// </summary>
    /// <typeparam name="TEntityInfo">实体信息类型</typeparam>
    /// <typeparam name="TEntityPack">实体包类型</typeparam>
    /// <param name="id">实体键</param>
    /// <param name="package">实体包</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>更新或创建结果</returns>
    public async Task<(StoreResult? result, TEntityInfo? info)> UpdateOrCreateEntityAsync<TEntityInfo, TEntityPack>(
        TKey id,
        TEntityPack package,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        if (EnableLogger) Logger?.LogDebug($"UpdateOrCreateEntityAsync<{typeof(TEntity).Name}>: {id} {package}");

        var entity = await EntityStore.FindEntityAsync(id, cancellationToken);

        if (entity is null)
        {
            entity = Instance.CreateInstance<TEntity, TEntityPack>(package);

            entity.Id = id;

            var createResult = await EntityStore.CreateAsync(entity, cancellationToken);

            return (createResult, entity.Adapt<TEntityInfo>());
        }

        package.Adapt(entity);

        var updateResult = await EntityStore.UpdateAsync(entity, cancellationToken);

        return (updateResult, entity.Adapt<TEntityInfo>());
    }

    /// <summary>
    ///     删除实体
    /// </summary>
    /// <param name="id">标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    public async Task<StoreResult> DeleteEntityAsync(TKey id, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        if (EnableLogger) Logger?.LogDebug($"DeleteEntityAsync<{typeof(TEntity).Name}>: {id}");

        var entity = await EntityStore.FindEntityAsync(id, cancellationToken);

        if (entity is null)
            return StoreResult.EntityFoundFailed(typeof(TEntity).Name, id.ToString()!);

        return await EntityStore.DeleteAsync(entity, cancellationToken);
    }

    #endregion

    #endregion
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
        EntityStore = store;
        EntityStore.HandlerRegister = HandlerRegister;
    }

    #region Properties

    /// <summary>
    ///     实体存储
    /// </summary>
    protected new IKeyLessStore<TEntity> EntityStore { get; }

    #endregion
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
        EntityStore.HandlerRegister = HandlerRegister;

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
    protected IKeyLessStore<TEntity, THandler> EntityStore { get; }

    /// <summary>
    ///     注册操作员
    /// </summary>
    public Func<THandler>? HandlerRegister { get; set; }

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