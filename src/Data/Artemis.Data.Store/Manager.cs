using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;
using Artemis.Data.Store.Extensions;
using Mapster;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Artemis.Data.Store;

#region Interface

/// <summary>
///     提供用于管理TEntity的存储器的API接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public interface IManager<TEntity> : IManager<TEntity, Guid> 
    where TEntity : class, IModelBase
{
}

/// <summary>
///     提供用于管理TEntity的存储器的API接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TKey">键类型</typeparam>
public interface IManager<TEntity, TKey> 
    where TEntity : class, IModelBase<TKey> 
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// 实体存储
    /// </summary>
    public IStore<TEntity, TKey> EntityStore { get; }

    /// <summary>
    ///     规范化键
    /// </summary>
    /// <param name="key">键</param>
    /// <returns>规范化后的键</returns>
    public string NormalizeKey(string key);

    #region BaseResourceManager

    /// <summary>
    /// 获取实体信息列表
    /// </summary>
    /// <typeparam name="TEntityInfo">实体信息类型</typeparam>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>实体信息列表</returns>
    Task<List<TEntityInfo>> GetEntitiesAsync<TEntityInfo>(int page = 20, int size = 1,
        CancellationToken cancellationToken = default);

    #endregion
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
        IOptions<ArtemisStoreOptions>? optionsAccessor = null,
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
    where TEntity : class, IModelBase<TKey>
    where TKey : IEquatable<TKey>
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
        IOptions<ArtemisStoreOptions>? optionsAccessor = null,
        StoreErrorDescriber? errors = null,
        ILogger? logger = null)
    {
        Store = store;
        ArtemisStoreOptions = optionsAccessor?.Value ?? new ArtemisStoreOptions();
        Describer = errors ?? new StoreErrorDescriber();
        Logger = logger;
        Cache = cache;

        Store.SetOptions(ArtemisStoreOptions);
    }

    /// <summary>
    ///     键前缀
    /// </summary>
    protected virtual string KeyPrefix => "Manager";

    #region DebugLogger

    /// <summary>
    ///     设置Debug日志
    /// </summary>
    /// <param name="message">日志消息</param>
    protected void SetDebugLog(string message)
    {
        if (ArtemisStoreOptions.DebugLogger)
            Logger?.LogDebug(message);
    }

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
    ///     Throws if this class has been disposed.
    /// </summary>
    private void ThrowIfDisposed()
    {
        if (_disposed) throw new ManagerDisposedException(GetType().Name);
    }

    /// <summary>
    ///     生成Key
    /// </summary>
    /// <param name="args">生成参数</param>
    /// <returns></returns>
    protected virtual string GenerateCacheKey(params string[] args)
    {
        var parameters = args.Prepend(KeyPrefix);

        return string.Join(":", parameters);
    }

    #region Properties

    /// <summary>
    ///     存储访问器
    /// </summary>
    protected IStore<TEntity, TKey> Store { get; }

    /// <summary>
    ///     缓存访问器
    /// </summary>
    protected IDistributedCache? Cache { get; }

    /// <summary>
    ///     缓存是否可用
    /// </summary>
    protected bool CacheAvailable => ArtemisStoreOptions.CachedManager;

    /// <summary>
    ///     配置访问器
    /// </summary>
    private ArtemisStoreOptions ArtemisStoreOptions { get; }

    /// <summary>
    ///     错误报告生成器
    /// </summary>
    protected StoreErrorDescriber Describer { get; }

    /// <summary>
    ///     日志依赖访问器
    /// </summary>
    private ILogger? Logger { get; }

    #endregion

    #region Implementation of IManager<TEntity,in TKey>

    /// <summary>
    /// 实体存储
    /// </summary>
    public IStore<TEntity, TKey> EntityStore => Store;

    /// <summary>
    ///     规范化键
    /// </summary>
    /// <param name="key">键</param>
    /// <returns>规范化后的键</returns>
    public string NormalizeKey(string key)
    {
        return Store.NormalizeKey(key);
    }

    #region BaseResourceManager

    /// <summary>
    /// 获取实体信息列表
    /// </summary>
    /// <typeparam name="TEntityInfo">实体信息类型</typeparam>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>实体信息列表</returns>
    public Task<List<TEntityInfo>> GetEntitiesAsync<TEntityInfo>(int page = 1, int size = 20, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        return Store.EntityQuery
            .MapPageAsync<TEntity, TKey, TEntityInfo>(
                page, 
                size, 
                cancellationToken);
    }

    /// <summary>
    /// 获取实体信息
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
    /// 创建实体
    /// </summary>
    /// <typeparam name="TEntityInfo">实体信息类型</typeparam>
    /// <typeparam name="TEntityPack">实体包类型</typeparam>
    /// <param name="pack">实体包</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>创建结果</returns>
    public async Task<(StoreResult result, TEntityInfo? info)> CreateEntityAsync<TEntityInfo, TEntityPack>(
        TEntityPack pack, 
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var entity = Instance.CreateInstance<TEntity>();

        pack.Adapt(entity);

        var result = await Store.CreateAsync(entity, cancellationToken);

        return (result, entity.Adapt<TEntityInfo>());
    }

    /// <summary>
    /// 更新实体
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
    /// 删除实体
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

    #endregion
}