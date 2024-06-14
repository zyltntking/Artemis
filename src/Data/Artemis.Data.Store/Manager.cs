using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;
using Artemis.Data.Store.Extensions;
using Mapster;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Artemis.Data.Store;

#region Interface

/// <summary>
///     提供用于管理具键存储模型TEntity的存储器的API接口
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IManager<TEntity> : IManager<TEntity, Guid>
    where TEntity : class, IKeySlot
{
}

/// <summary>
///     提供用于管理具键存储模型TEntity的存储器的API接口
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
public interface IManager<TEntity, TKey> : IKeyLessManager<TEntity>
    where TEntity : class, IKeySlot<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     实体存储
    /// </summary>
    new IStore<TEntity, TKey> EntityStore { get; }

    /// <summary>
    ///     注册操作员
    /// </summary>
    Func<TKey>? HandlerRegister { get; set; }

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
public interface IKeyLessManager<TEntity> : IDisposable
    where TEntity : class
{
    /// <summary>
    ///     实体存储
    /// </summary>
    IKeyLessStore<TEntity> EntityStore { get; }

    /// <summary>
    ///     规范化键
    /// </summary>
    /// <param name="key">键</param>
    /// <returns>规范化后的键</returns>
    public string NormalizeKey(string key);
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
    /// <param name="errors">错误依赖</param>
    /// <param name="logger">日志依赖</param>
    /// <param name="cache">缓存依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    protected Manager(
        IStore<TEntity, Guid> store,
        IDistributedCache? cache = null,
        IStoreManagerOptions? options = null,
        StoreErrorDescriber? errors = null,
        ILogger? logger = null) : base(store, cache, options, errors, logger)
    {
    }
}

/// <summary>
///     提供用于管理具键存储模型TEntity的存储器的API
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
public abstract class Manager<TEntity, TKey> : KeyLessManager<TEntity>, IManager<TEntity, TKey>
    where TEntity : class, IKeySlot<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     创建新的管理器实例
    /// </summary>
    /// <param name="store">存储访问器依赖</param>
    /// <param name="options">配置依赖</param>
    /// <param name="errors">错误依赖</param>
    /// <param name="logger">日志依赖</param>
    /// <param name="cache">缓存依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    protected Manager(
        IStore<TEntity, TKey> store,
        IDistributedCache? cache = null,
        IStoreManagerOptions? options = null,
        StoreErrorDescriber? errors = null,
        ILogger? logger = null) : base(store, options, errors, logger)
    {
        Store = store;
        RegisterHandler();
        Options = options ?? new StoreManagerOptions();
        Cache = cache;
    }

    #region Properties

    /// <summary>
    ///     存储访问器
    /// </summary>
    protected new IStore<TEntity, TKey> Store { get; }

    /// <summary>
    ///     缓存访问器
    /// </summary>
    protected IDistributedCache? Cache { get; }

    /// <summary>
    ///     具键存储管理器配置接口
    /// </summary>
    private IStoreManagerOptions Options { get; }

    #endregion

    #region OptionsAccessor

    /// <summary>
    ///     是否启用具缓存策略
    /// </summary>
    public bool CachedManager => Options is { CachedManager: true, Expires: >= 0 } && Cache is not null;

    /// <summary>
    ///     过期时间(秒)
    /// </summary>
    public int Expires => Options.Expires;

    #endregion

    #region Implementation of IKeyWithManager<TEntity,TKey>

    /// <summary>
    ///     实体存储
    /// </summary>
    public new IStore<TEntity, TKey> EntityStore => Store;

    /// <summary>
    ///     注册操作员
    /// </summary>
    public abstract Func<TKey>? HandlerRegister { get; set; }

    /// <summary>
    ///     注册操作员
    /// </summary>
    private void RegisterHandler()
    {
        RegisterStoreHandler();
    }

    /// <summary>
    /// 注册存储操作员
    /// </summary>
    protected virtual void RegisterStoreHandler()
    {
        Store.HandlerRegister = HandlerRegister;
    }

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

        return Store.FindMapEntityAsync<TEntityInfo>(id, cancellationToken);
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

        return Store.EntityQuery
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

        var entity = Instance.CreateInstance<TEntity, TEntityPack>(package);

        var result = await Store.CreateAsync(entity, cancellationToken);

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

        var entity = await Store.FindEntityAsync(id, cancellationToken);

        if (entity is null)
            return (StoreResult.EntityFoundFailed(typeof(TEntity).Name, id.ToString()!), default);

        pack.Adapt(entity);

        var result = await Store.UpdateAsync(entity, cancellationToken);

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

        var entity = await Store.FindEntityAsync(id, cancellationToken);

        if (entity is null)
        {
            entity = Instance.CreateInstance<TEntity, TEntityPack>(package);

            entity.Id = id;

            var createResult = await Store.CreateAsync(entity, cancellationToken);

            return (createResult, entity.Adapt<TEntityInfo>());
        }

        package.Adapt(entity);

        var updateResult = await Store.UpdateAsync(entity, cancellationToken);

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

        var entity = await Store.FindEntityAsync(id, cancellationToken);

        if (entity is null)
            return StoreResult.EntityFoundFailed(typeof(TEntity).Name, id.ToString()!);

        return await Store.DeleteAsync(entity, cancellationToken);
    }

    #endregion

    #endregion
}

/// <summary>
///     提供用于管理无键存储模型TEntity的存储器的API
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class KeyLessManager<TEntity> : IKeyLessManager<TEntity> where TEntity : class
{
    /// <summary>
    ///     创建新的管理器实例
    /// </summary>
    /// <param name="store">存储访问器依赖</param>
    /// <param name="options">配置依赖</param>
    /// <param name="errors">错误依赖</param>
    /// <param name="logger">日志依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    protected KeyLessManager(
        IKeyLessStore<TEntity> store,
        IKeyLessStoreManagerOptions? options = null,
        StoreErrorDescriber? errors = null,
        ILogger? logger = null)
    {
        Store = store;
        Describer = errors ?? new StoreErrorDescriber();
        Options = options ?? new KeyLessStoreManagerOptions();
        Logger = logger;
    }

    /// <summary>
    ///     键前缀
    /// </summary>
    protected string KeyPrefix => "Manager";

    #region OptionsAccessor

    /// <summary>
    ///     是否启用Debug日志
    /// </summary>
    public bool DebugLogger => Options.DebugLogger;

    #endregion

    /// <summary>
    ///     异步函数执行前
    /// </summary>
    /// <param name="cancellationToken"></param>
    protected void OnAsyncActionExecuting(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();

        cancellationToken.ThrowIfCancellationRequested();
    }

    /// <summary>
    ///     生成Key
    /// </summary>
    /// <param name="args">生成参数</param>
    /// <returns></returns>
    protected string GenerateCacheKey(params string[] args)
    {
        var parameters = args.Prepend(KeyPrefix);

        return string.Join(":", parameters);
    }

    #region Properties

    /// <summary>
    ///     存储访问器
    /// </summary>
    protected IKeyLessStore<TEntity> Store { get; }

    /// <summary>
    ///     具键存储管理器配置接口
    /// </summary>
    private IKeyLessStoreManagerOptions Options { get; }

    /// <summary>
    ///     错误报告生成器
    /// </summary>
    protected StoreErrorDescriber Describer { get; }

    /// <summary>
    ///     日志依赖访问器
    /// </summary>
    protected ILogger? Logger { get; }

    #endregion

    #region Implementation of IKeyLessManager<TEntity>

    /// <summary>
    ///     实体存储
    /// </summary>
    public IKeyLessStore<TEntity> EntityStore => Store;

    /// <summary>
    ///     规范化键
    /// </summary>
    /// <param name="key">键</param>
    /// <returns>规范化后的键</returns>
    public string NormalizeKey(string key)
    {
        return key.StringNormalize();
    }

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