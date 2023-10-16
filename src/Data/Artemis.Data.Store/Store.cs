using System.Linq.Expressions;
using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;
using Artemis.Data.Store.Extensions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Artemis.Data.Store;

/// <summary>
///     抽象存储实现
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public abstract class Store<TEntity> : Store<TEntity, Guid>, IStore<TEntity>
    where TEntity : class, IModelBase, IModelBase<Guid>
{
    /// <summary>
    ///     创建一个新的基本存储实例
    /// </summary>
    /// <param name="context">数据访问上下文</param>
    /// <param name="logger">日志依赖</param>
    /// <param name="cache">缓存依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    protected Store(
        DbContext context,
        IDistributedCache? cache = null,
        ILogger? logger = null) : base(context, cache, logger)
    {
    }

    #region Overrides of StoreBase<TEntity,Guid>

    /// <summary>
    ///     转换字符串到id
    /// </summary>
    /// <param name="id">id字符串</param>
    /// <returns>id</returns>
    public override Guid ConvertIdFromString(string id)
    {
        return id.GuidFromString();
    }

    /// <summary>
    ///     转换Id为字符串
    /// </summary>
    /// <param name="id">id</param>
    /// <returns>字符串</returns>
    public override string ConvertIdToString(Guid id)
    {
        return id.GuidToString();
    }

    #endregion
}

/// <summary>
///     抽象存储实现
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TKey">数据上下文类型</typeparam>
public abstract class Store<TEntity, TKey> : Store<TEntity, DbContext, TKey>
    where TEntity : class, IModelBase<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     创建一个新的基本存储实例
    /// </summary>
    /// <param name="context">数据访问上下文</param>
    /// <param name="logger">日志依赖</param>
    /// <param name="cache">缓存依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    protected Store(
        DbContext context,
        IDistributedCache? cache = null,
        ILogger? logger = null) : base(context, cache, logger)
    {
    }
}

/// <summary>
///     抽象存储实现
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TContext">数据上下文类型</typeparam>
/// <typeparam name="TKey">键类型</typeparam>
public abstract class Store<TEntity, TContext, TKey> : StoreBase<TEntity, TKey>, IStore<TEntity, TKey>
    where TEntity : class, IModelBase<TKey>
    where TContext : DbContext
    where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     创建一个新的基本存储实例
    /// </summary>
    /// <param name="context">数据访问上下文</param>
    /// <param name="logger">日志依赖</param>
    /// <param name="describer">操作异常描述者</param>
    /// <param name="cache">缓存依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    protected Store(
        TContext context,
        IDistributedCache? cache = null,
        ILogger? logger = null,
        StoreErrorDescriber? describer = null) : base(describer)
    {
        Context = context;
        Cache = cache;
        Logger = logger;
    }

    #region DebugLogger

    /// <summary>
    ///     设置Debug日志
    /// </summary>
    /// <param name="message">日志消息</param>
    private void SetDebugLog(string message)
    {
        if (DebugLogger)
            Logger?.LogDebug(message);
    }

    #endregion

    #region DataAccess

    /// <summary>
    ///     数据访问上下文
    /// </summary>
    private TContext Context { get; }

    /// <summary>
    ///     缓存依赖
    /// </summary>
    private IDistributedCache? Cache { get; }

    /// <summary>
    ///     日志依赖
    /// </summary>
    private ILogger? Logger { get; }

    /// <summary>
    ///     EntitySet访问器*Main Store Set*
    /// </summary>
    public DbSet<TEntity> EntitySet => Context.Set<TEntity>();

    /// <summary>
    ///     Entity有追踪访问器
    /// </summary>
    public IQueryable<TEntity> TrackingQuery => EntitySet.WhereIf(SoftDelete, entity => entity.DeletedAt != null);

    /// <summary>
    ///     Entity无追踪访问器
    /// </summary>
    public IQueryable<TEntity> EntityQuery => TrackingQuery.AsNoTracking();

    /// <summary>
    ///     键适配查询
    /// </summary>
    public IQueryable<TEntity> KeyMatchQuery(TKey key)
    {
        return EntityQuery.Where(entity => entity.Id.Equals(key));
    }

    /// <summary>
    ///     键适配查询
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    public IQueryable<TEntity> KeyMatchQuery(IEnumerable<TKey> keys)
    {
        return EntityQuery.Where(item => keys.Contains(item.Id));
    }

    #endregion

    #region Cache

    /// <summary>
    ///     缓存前缀
    /// </summary>
    protected virtual string Prefix => "Store";

    /// <summary>
    ///     缓存实体
    /// </summary>
    /// <param name="entity"></param>
    private void CacheEntity(TEntity entity)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            Cache?.Set(entity.GenerateKey(Prefix), entity, Expires);

            SetDebugLog("Entity Cached");
        }
    }

    /// <summary>
    ///     缓存实体
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private Task CacheEntityAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            Cache?.SetAsync(entity.GenerateKey(Prefix), entity, Expires, cancellationToken);

            SetDebugLog("Entity Cached");
        }

        return Task.CompletedTask;
    }

    /// <summary>
    ///     缓存实体
    /// </summary>
    /// <param name="entities"></param>
    private void CacheEntities(IEnumerable<TEntity> entities)
    {
        if (CachedStore)
        {
            if (Cache is null) throw new InstanceNotImplementException(nameof(Cache));

            var count = 0;

            foreach (var entity in entities)
            {
                Cache?.SetAsync(entity.GenerateKey(Prefix), entity, Expires);
                count++;
            }

            SetDebugLog($"Cached {count} Entities");
        }
    }

    /// <summary>
    ///     缓存实体
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="cancellationToken"></param>
    private Task CacheEntitiesAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        if (CachedStore)
        {
            if (Cache is null) throw new InstanceNotImplementException(nameof(Cache));

            var count = 0;

            foreach (var entity in entities)
            {
                Cache?.SetAsync(entity.GenerateKey(Prefix), entity, Expires, cancellationToken);
                count++;
            }

            SetDebugLog($"Cached {count} Entities");
        }

        return Task.CompletedTask;
    }

    /// <summary>
    ///     获取实体
    /// </summary>
    /// <param name="key">缓存键</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private TEntity? GetEntity(string key)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            return Cache?.Get<TEntity>(key);
        }

        return default;
    }

    /// <summary>
    ///     获取实体
    /// </summary>
    /// <param name="key">缓存键</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private async Task<TEntity?> GetEntityAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            return await Cache?.GetAsync<TEntity>(key, cancellationToken)!;
        }

        return default;
    }

    /// <summary>
    ///     获取实体
    /// </summary>
    /// <param name="keys">实体键列表</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private IEnumerable<TEntity>? GetEntities(IEnumerable<string> keys)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            var list = new List<TEntity>();

            foreach (var key in keys)
            {
                var entity = Cache?.Get<TEntity>(key);

                if (entity is not null)
                    list.Add(entity);
            }

            return list;
        }

        return default;
    }

    /// <summary>
    ///     获取实体
    /// </summary>
    /// <param name="keys">实体键列表</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private async Task<IEnumerable<TEntity>?> GetEntitiesAsync(
        IEnumerable<string> keys,
        CancellationToken cancellationToken = default)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            var list = new List<TEntity>();

            foreach (var key in keys)
            {
                var entity = await Cache?.GetAsync<TEntity>(key, cancellationToken)!;

                if (entity is not null)
                    list.Add(entity);
            }

            return list;
        }

        return default;
    }

    /// <summary>
    ///     移除被缓存的实体
    /// </summary>
    /// <param name="entity"></param>
    private void RemoveCachedEntity(TEntity entity)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            Cache?.Remove(entity.GenerateKey(Prefix));
            SetDebugLog("Entity Remove");
        }
    }

    /// <summary>
    ///     移除被缓存的实体
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    private async Task RemoveCachedEntityAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            await Cache?.RemoveAsync(entity.GenerateKey(Prefix), cancellationToken)!;
            SetDebugLog("Entity Remove");
        }
    }

    /// <summary>
    ///     移除被缓存的实体
    /// </summary>
    /// <param name="entities"></param>
    private void RemoveCachedEntities(IEnumerable<TEntity> entities)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            var count = 0;
            foreach (var entity in entities)
            {
                Cache?.Remove(entity.GenerateKey(Prefix));
                count++;
            }

            SetDebugLog($"Removed {count} Entities From Cache");
        }
    }

    /// <summary>
    ///     移除被缓存的实体
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="cancellationToken"></param>
    private async Task RemoveCachedEntitiesAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            var count = 0;
            foreach (var entity in entities)
            {
                await Cache?.RemoveAsync(entity.GenerateKey(Prefix), cancellationToken)!;
                count++;
            }

            SetDebugLog($"Removed {count} Entities From Cache");
        }
    }

    #endregion

    #region SaveChanges

    /// <summary>
    ///     保存当前存储
    /// </summary>
    /// <returns></returns>
    private int SaveChanges()
    {
        SetDebugLog(nameof(SaveChanges));
        return AutoSaveChanges ? Context.SaveChanges() : 0;
    }

    /// <summary>
    ///     保存当前存储
    /// </summary>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>异步取消结果</returns>
    private Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetDebugLog(nameof(SaveChangesAsync));
        return AutoSaveChanges ? Context.SaveChangesAsync(cancellationToken) : Task.FromResult(0);
    }

    #endregion

    #region Implementation of IStoreCommon<TEntity,in TKey>

    #region IStoreOptions

    #region Setting

    /// <summary>
    ///     设置是否自动保存更改
    /// </summary>
    public bool AutoSaveChanges { get; set; } = true;

    /// <summary>
    ///     MetaDataHosting标记
    /// </summary>
    private bool _metaDataHosting = true;

    /// <summary>
    ///     设置是否启用元数据托管
    /// </summary>
    public bool MetaDataHosting
    {
        get => _metaDataHosting;
        set => _metaDataHosting = SoftDelete || value;
    }

    /// <summary>
    ///     SoftDelete标记
    /// </summary>
    private bool _softDelete;

    /// <summary>
    ///     设置是否启用软删除
    /// </summary>
    public bool SoftDelete
    {
        get => _softDelete;
        set
        {
            _softDelete = value;
            if (_softDelete) MetaDataHosting = _softDelete;
        }
    }

    /// <summary>
    ///     是否启用具缓存策略
    /// </summary>
    private bool _cachedStore;

    /// <summary>
    ///     是否启用具缓存策略
    /// </summary>
    public bool CachedStore
    {
        get => _cachedStore;
        set => _cachedStore = value && Cache != null;
    }

    /// <summary>
    ///     过期时间(秒)
    /// </summary>
    private int _expires;

    /// <summary>
    ///     过期时间(秒)
    /// </summary>
    public int Expires
    {
        get => _expires;
        set
        {
            _expires = value;
            CachedStore = _expires switch
            {
                > 0 => true,
                < 0 => false,
                _ => CachedStore
            };
        }
    }

    /// <summary>
    ///     是否启用Debug日志
    /// </summary>
    private bool _debugLogger;

    /// <summary>
    ///     是否启用Debug日志
    /// </summary>
    public bool DebugLogger
    {
        get => _debugLogger;
        set => _debugLogger = value && Logger != null;
    }

    #endregion

    /// <summary>
    ///     设置配置
    /// </summary>
    /// <param name="artemisStoreOptions"></param>
    public void SetOptions(ArtemisStoreOptions artemisStoreOptions)
    {
        AutoSaveChanges = artemisStoreOptions.AutoSaveChanges;
        MetaDataHosting = artemisStoreOptions.MetaDataHosting;
        SoftDelete = artemisStoreOptions.SoftDelete;
        CachedStore = artemisStoreOptions.CachedStore;
        Expires = artemisStoreOptions.Expires;
        DebugLogger = artemisStoreOptions.DebugLogger;
    }

    #endregion

    #region CreateEntity & CreateEntities

    /// <summary>
    ///     在<paramref name="entity" />存储中创建一个新的实体
    /// </summary>
    /// <param name="entity">被创建实体</param>
    /// <returns></returns>
    public virtual StoreResult Create(TEntity entity)
    {
        SetDebugLog(nameof(Create));

        OnActionExecuting(entity, nameof(entity));

        AddEntity(entity);

        var result = AttacheChange();

        CacheEntity(entity);

        return result;
    }

    /// <summary>
    ///     在<paramref name="entities" />存储中创建多个新的实体
    /// </summary>
    /// <param name="entities">被创建实体</param>
    /// <returns></returns>
    public virtual StoreResult Create(IEnumerable<TEntity> entities)
    {
        SetDebugLog(nameof(Create));

        var list = entities.ToList();

        OnActionExecuting(list, nameof(entities));

        AddEntities(list);

        var result = AttacheChange();

        CacheEntities(list);

        return result;
    }

    /// <summary>
    ///     在<paramref name="entity" />存储中创建一个新的实体
    /// </summary>
    /// <param name="entity">被创建实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public virtual async Task<StoreResult> CreateAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog(nameof(CreateAsync));

        OnAsyncActionExecuting(entity, nameof(entity), cancellationToken);

        AddEntity(entity);

        var result = await AttacheChangeAsync(cancellationToken);

        await CacheEntityAsync(entity, cancellationToken);

        return result;
    }

    /// <summary>
    ///     在<paramref name="entities" />存储中创建多个新的实体
    /// </summary>
    /// <param name="entities">被创建实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public virtual async Task<StoreResult> CreateAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog(nameof(CreateAsync));

        var list = entities.ToList();

        OnAsyncActionExecuting(list, nameof(entities), cancellationToken);

        AddEntities(list);

        var result = await AttacheChangeAsync(cancellationToken);

        await CacheEntitiesAsync(list, cancellationToken);

        return result;
    }

    #endregion

    #region UpdateEntity & UpdateEntities

    /// <summary>
    ///     在<paramref name="entity" />存储中更新已存在的实体
    /// </summary>
    /// <param name="entity">被创建实体</param>
    /// <returns></returns>
    public StoreResult Update(TEntity entity)
    {
        SetDebugLog(nameof(Update));

        OnActionExecuting(entity, nameof(entity));

        UpdateEntity(entity);

        var result = AttacheChange();

        CacheEntity(entity);

        return result;
    }

    /// <summary>
    ///     在<paramref name="entities" />存储中更新多个已存在的实体
    /// </summary>
    /// <param name="entities">被创建实体</param>
    /// <returns></returns>
    public StoreResult Update(IEnumerable<TEntity> entities)
    {
        SetDebugLog(nameof(Update));

        var list = entities.ToList();

        OnActionExecuting(list, nameof(entities));

        UpdateEntities(list);

        var result = AttacheChange();

        CacheEntities(list);

        return result;
    }

    /// <summary>
    ///     在<paramref name="entity" />存储中更新已存在的实体
    /// </summary>
    /// <param name="entity">被创建实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> UpdateAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog(nameof(UpdateAsync));

        OnAsyncActionExecuting(entity, nameof(entity), cancellationToken);

        UpdateEntity(entity);

        var result = await AttacheChangeAsync(cancellationToken);

        await CacheEntityAsync(entity, cancellationToken);

        return result;
    }

    /// <summary>
    ///     在<paramref name="entities" />存储中更新多个已存在的实体
    /// </summary>
    /// <param name="entities">被创建实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> UpdateAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog(nameof(UpdateAsync));

        var list = entities.ToList();

        OnAsyncActionExecuting(list, nameof(entities), cancellationToken);

        UpdateEntities(list);

        var result = await AttacheChangeAsync(cancellationToken);

        await CacheEntitiesAsync(list, cancellationToken);

        return result;
    }

    #endregion

    #region BatchUpdateEntity & BatchUpdateEntities

    /// <summary>
    ///     更新存储中的实体
    /// </summary>
    /// <param name="id">被更新实体的主键</param>
    /// <param name="setter">更新行为</param>
    /// <returns></returns>
    public StoreResult BatchUpdate(
        TKey id,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter)
    {
        if (CachedStore) return StoreResult.Failed(Describer.EnableCache());
        OnActionExecuting(id, nameof(id));
        try
        {
            var changes = BatchUpdateEntity(KeyMatchQuery(id), setter);

            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(Describer.ConcurrencyFailure());
        }
    }

    /// <summary>
    ///     在存储中更新已存在的实体
    /// </summary>
    /// <param name="ids">被更新实体的主键</param>
    /// <param name="setter">更新行为</param>
    /// <returns></returns>
    public StoreResult BatchUpdate(
        IEnumerable<TKey> ids,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter)
    {
        if (CachedStore) return StoreResult.Failed(Describer.EnableCache());
        var idList = ids as List<TKey> ?? ids.ToList();
        OnActionExecuting(idList, nameof(ids));
        try
        {
            var changes = BatchUpdateEntity(KeyMatchQuery(idList), setter);

            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(Describer.ConcurrencyFailure());
        }
    }

    /// <summary>
    ///     在存储中更新符合条件的实体
    /// </summary>
    /// <param name="setter">更新行为</param>
    /// <param name="predicate">查询表达式</param>
    /// <returns></returns>
    public StoreResult BatchUpdate(
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter,
        Expression<Func<TEntity, bool>>? predicate = null)
    {
        if (CachedStore) return StoreResult.Failed(Describer.EnableCache());
        ThrowIfDisposed();
        try
        {
            var query = predicate is not null ? EntityQuery.Where(predicate) : EntityQuery;

            var changes = BatchUpdateEntity(query, setter);

            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(Describer.ConcurrencyFailure());
        }
    }

    /// <summary>
    ///     在存储中更新符合查询描述的实体
    /// </summary>
    /// <param name="query"></param>
    /// <param name="setter">更新行为</param>
    /// <returns></returns>
    public StoreResult BatchUpdate(
        IQueryable<TEntity> query,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter)
    {
        if (CachedStore) return StoreResult.Failed(Describer.EnableCache());
        ThrowIfDisposed();
        try
        {
            var changes = BatchUpdateEntity(query, setter);

            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(Describer.ConcurrencyFailure());
        }
    }

    /// <summary>
    ///     在存储中更新已存在的实体
    /// </summary>
    /// <param name="id">被更新实体的主键</param>
    /// <param name="setter">更新行为</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> BatchUpdateAsync(
        TKey id,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter,
        CancellationToken cancellationToken = default)
    {
        if (CachedStore) return StoreResult.Failed(Describer.EnableCache());
        OnAsyncActionExecuting(id, nameof(id), cancellationToken);
        try
        {
            var changes = await BatchUpdateEntityAsync(KeyMatchQuery(id), setter, cancellationToken);

            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(Describer.ConcurrencyFailure());
        }
    }

    /// <summary>
    ///     在存储中更新已存在的实体
    /// </summary>
    /// <param name="ids">被更新实体的主键</param>
    /// <param name="setter">更新行为</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> BatchUpdateAsync(
        IEnumerable<TKey> ids,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter,
        CancellationToken cancellationToken = default)
    {
        if (CachedStore) return StoreResult.Failed(Describer.EnableCache());
        var idList = ids as List<TKey> ?? ids.ToList();
        OnAsyncActionExecuting(idList, nameof(ids), cancellationToken);
        try
        {
            var changes = await BatchUpdateEntityAsync(KeyMatchQuery(idList), setter, cancellationToken);

            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(Describer.ConcurrencyFailure());
        }
    }

    /// <summary>
    ///     在存储中更新符合条件的实体
    /// </summary>
    /// <param name="setter">更新行为</param>
    /// <param name="predicate">查询表达式</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> BatchUpdateAsync(
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter,
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        if (CachedStore) return StoreResult.Failed(Describer.EnableCache());
        ThrowIfDisposed();
        cancellationToken.ThrowIfCancellationRequested();
        try
        {
            var query = predicate is not null ? EntityQuery.Where(predicate) : EntityQuery;

            var changes = await BatchUpdateEntityAsync(query, setter, cancellationToken);

            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(Describer.ConcurrencyFailure());
        }
    }

    /// <summary>
    ///     在存储中更新符合查询描述的实体
    /// </summary>
    /// <param name="query"></param>
    /// <param name="setter">更新行为</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> BatchUpdateAsync(
        IQueryable<TEntity> query,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter,
        CancellationToken cancellationToken = default)
    {
        if (CachedStore) return StoreResult.Failed(Describer.EnableCache());
        ThrowIfDisposed();
        cancellationToken.ThrowIfCancellationRequested();
        try
        {
            var changes = await BatchUpdateEntityAsync(query, setter, cancellationToken);

            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(Describer.ConcurrencyFailure());
        }
    }

    #endregion

    #region DeleteEntity & DeleteEntities

    /// <summary>
    ///     在存储中删除已存在的实体
    /// </summary>
    /// <param name="id">被删除实体的主键</param>
    public StoreResult Delete(TKey id)
    {
        SetDebugLog(nameof(Delete));

        OnActionExecuting(id, nameof(id));

        var entity = FindEntity(id);

        if (entity is null)
            return StoreResult.Failed(Describer.NotFoundId(ConvertIdToString(id)));

        DeleteEntity(entity);

        var result = AttacheChange();

        RemoveCachedEntity(entity);

        return result;
    }

    /// <summary>
    ///     在<paramref name="entity" />存储中删除已存在的实体
    /// </summary>
    /// <param name="entity">被删除实体</param>
    /// <returns></returns>
    public StoreResult Delete(TEntity entity)
    {
        SetDebugLog(nameof(Delete));

        OnActionExecuting(entity, nameof(entity));

        DeleteEntity(entity);

        var result = AttacheChange();

        RemoveCachedEntity(entity);

        return result;
    }

    /// <summary>
    ///     在存储中删除已存在的实体
    /// </summary>
    /// <param name="ids">被删除实体的主键</param>
    /// <returns></returns>
    public StoreResult Delete(IEnumerable<TKey> ids)
    {
        SetDebugLog(nameof(Delete));

        var idList = ids as List<TKey> ?? ids.ToList();

        OnActionExecuting(idList, nameof(ids));

        var list = FindEntities(idList).ToList();

        if (!list.Any())
        {
            var idsString = string.Join(",", idList.Select(ConvertIdToString));
            return StoreResult.Failed(Describer.NotFoundId(idsString));
        }

        DeleteEntities(list);

        var result = AttacheChange();

        RemoveCachedEntities(list);

        return result;
    }

    /// <summary>
    ///     在<paramref name="entities" />存储中删除已存在的实体
    /// </summary>
    /// <param name="entities">被删除实体</param>
    /// <returns></returns>
    public StoreResult Delete(IEnumerable<TEntity> entities)
    {
        SetDebugLog(nameof(Delete));

        var list = entities.ToList();

        OnActionExecuting(list, nameof(entities));

        DeleteEntities(list);

        var result = AttacheChange();

        RemoveCachedEntities(list);

        return result;
    }

    /// <summary>
    ///     在存储中删除已存在的实体
    /// </summary>
    /// <param name="id">被删除实体的主键</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> DeleteAsync(
        TKey id,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog(nameof(DeleteAsync));

        OnAsyncActionExecuting(id, nameof(id), cancellationToken);

        var entity = await FindEntityAsync(id, cancellationToken);

        if (entity is null)
            return StoreResult.Failed(Describer.NotFoundId(ConvertIdToString(id)));

        DeleteEntity(entity);

        var result = await AttacheChangeAsync(cancellationToken);

        await RemoveCachedEntityAsync(entity, cancellationToken);

        return result;
    }

    /// <summary>
    ///     在<paramref name="entity" />存储中删除已存在的实体
    /// </summary>
    /// <param name="entity">被删除实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> DeleteAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog(nameof(DeleteAsync));

        OnAsyncActionExecuting(entity, nameof(entity), cancellationToken);

        DeleteEntity(entity);

        var result = await AttacheChangeAsync(cancellationToken);

        await RemoveCachedEntityAsync(entity, cancellationToken);

        return result;
    }

    /// <summary>
    ///     在存储中删除已存在的实体
    /// </summary>
    /// <param name="ids">被删除实体的主键</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> DeleteAsync(
        IEnumerable<TKey> ids,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog(nameof(DeleteAsync));

        var idList = ids as List<TKey> ?? ids.ToList();

        OnAsyncActionExecuting(idList, nameof(ids), cancellationToken);

        var list = (await FindEntitiesAsync(idList, cancellationToken)).ToList();

        if (!list.Any())
        {
            var idsString = string.Join(",", idList.Select(ConvertIdToString));
            return StoreResult.Failed(Describer.NotFoundId(idsString));
        }

        DeleteEntities(list);

        var result = await AttacheChangeAsync(cancellationToken);

        await RemoveCachedEntitiesAsync(list, cancellationToken);

        return result;
    }

    /// <summary>
    ///     在<paramref name="entities" />存储中删除已存在的实体
    /// </summary>
    /// <param name="entities">被删除实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> DeleteAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog(nameof(DeleteAsync));

        var list = entities.ToList();

        OnAsyncActionExecuting(list, nameof(entities), cancellationToken);

        DeleteEntities(list);

        var result = await AttacheChangeAsync(cancellationToken);

        await RemoveCachedEntitiesAsync(list, cancellationToken);

        return result;
    }

    #endregion

    #region BatchDeleteEntity & BatchDeleteEntities

    /// <summary>
    ///     在存储中删除已存在的实体
    /// </summary>
    /// <param name="id">被删除实体的主键</param>
    public StoreResult BatchDelete(TKey id)
    {
        if (CachedStore)
            return StoreResult.Failed(Describer.EnableCache());

        OnActionExecuting(id, nameof(id));
        try
        {
            var changes = BatchDeleteEntity(KeyMatchQuery(id));

            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(Describer.ConcurrencyFailure());
        }
    }

    /// <summary>
    ///     在存储中删除已存在的实体
    /// </summary>
    /// <param name="ids">被删除实体的主键</param>
    /// <returns></returns>
    public StoreResult BatchDelete(IEnumerable<TKey> ids)
    {
        if (CachedStore)
            return StoreResult.Failed(Describer.EnableCache());

        var idList = ids as List<TKey> ?? ids.ToList();

        OnActionExecuting(idList, nameof(ids));
        try
        {
            var changes = BatchDeleteEntity(KeyMatchQuery(idList));

            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(Describer.ConcurrencyFailure());
        }
    }

    /// <summary>
    ///     在存储中删除符合条件的实体
    /// </summary>
    /// <param name="predicate">查询表达式</param>
    /// <returns></returns>
    public StoreResult BatchDelete(Expression<Func<TEntity, bool>>? predicate)
    {
        if (CachedStore)
            return StoreResult.Failed(Describer.EnableCache());

        ThrowIfDisposed();
        try
        {
            var query = predicate is not null ? EntityQuery.Where(predicate) : EntityQuery;

            var changes = BatchDeleteEntity(query);

            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(Describer.ConcurrencyFailure());
        }
    }

    /// <summary>
    ///     在存储中删除符合查询描述的实体
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public StoreResult BatchDelete(IQueryable<TEntity> query)
    {
        if (CachedStore)
            return StoreResult.Failed(Describer.EnableCache());

        ThrowIfDisposed();
        try
        {
            var changes = BatchDeleteEntity(query);

            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(Describer.ConcurrencyFailure());
        }
    }

    /// <summary>
    ///     在存储中删除已存在的实体
    /// </summary>
    /// <param name="id">被删除实体的主键</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> BatchDeleteAsync(
        TKey id,
        CancellationToken cancellationToken = default)
    {
        if (CachedStore)
            return StoreResult.Failed(Describer.EnableCache());

        OnAsyncActionExecuting(id, nameof(id), cancellationToken);
        try
        {
            var changes = await BatchDeleteEntityAsync(KeyMatchQuery(id), cancellationToken);

            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(Describer.ConcurrencyFailure());
        }
    }

    /// <summary>
    ///     在存储中删除已存在的实体
    /// </summary>
    /// <param name="ids">被删除实体的主键</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> BatchDeleteAsync(
        IEnumerable<TKey> ids,
        CancellationToken cancellationToken = default)
    {
        if (CachedStore)
            return StoreResult.Failed(Describer.EnableCache());

        var idList = ids as List<TKey> ?? ids.ToList();
        OnAsyncActionExecuting(idList, nameof(ids), cancellationToken);
        try
        {
            var changes = await BatchDeleteEntityAsync(KeyMatchQuery(idList), cancellationToken);

            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(Describer.ConcurrencyFailure());
        }
    }

    /// <summary>
    ///     在存储中删除符合条件的实体
    /// </summary>
    /// <param name="predicate">查询表达式</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> BatchDeleteAsync(
        Expression<Func<TEntity, bool>>? predicate,
        CancellationToken cancellationToken = default)
    {
        if (CachedStore)
            return StoreResult.Failed(Describer.EnableCache());

        ThrowIfDisposed();
        cancellationToken.ThrowIfCancellationRequested();
        try
        {
            var query = predicate is not null ? EntityQuery.Where(predicate) : EntityQuery;

            var changes = await BatchDeleteEntityAsync(query, cancellationToken);

            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(Describer.ConcurrencyFailure());
        }
    }

    /// <summary>
    ///     在存储中删除符合查询描述的实体
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> BatchDeleteAsync(
        IQueryable<TEntity> query,
        CancellationToken cancellationToken = default)
    {
        if (CachedStore) return StoreResult.Failed(Describer.EnableCache());
        ThrowIfDisposed();
        cancellationToken.ThrowIfCancellationRequested();
        try
        {
            var changes = await BatchDeleteEntityAsync(query, cancellationToken);

            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(Describer.ConcurrencyFailure());
        }
    }

    #endregion

    #region FindEntity & FindEntities

    /// <summary>
    ///     根据Id查找实体
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public TEntity? FindEntity(TKey id)
    {
        OnActionExecuting(id, nameof(id));
        return FindById(id);
    }

    /// <summary>
    ///     根据id查找映射实体
    /// </summary>
    /// <typeparam name="TMapEntity">映射类型</typeparam>
    /// <param name="id">id</param>
    /// <returns></returns>
    public TMapEntity? FindMapEntity<TMapEntity>(TKey id)
    {
        OnActionExecuting(id, nameof(id));
        return FindById<TMapEntity>(id);
    }

    /// <summary>
    ///     根据缓存键查找实体
    /// </summary>
    /// <param name="key">缓存键</param>
    /// <returns></returns>
    public TEntity? FindEntityViaKey(string key)
    {
        OnActionExecuting(key, nameof(key));
        return GetEntity(key);
    }

    /// <summary>
    ///     根据缓存键查找实体查找映射实体
    /// </summary>
    /// <typeparam name="TMapEntity">映射类型</typeparam>
    /// <param name="key">id</param>
    /// <returns></returns>
    public TMapEntity? FindMapEntityViaKey<TMapEntity>(string key)
    {
        OnActionExecuting(key, nameof(key));

        var entity = GetEntity(key);

        return entity is not null ? entity.Adapt<TMapEntity>() : default;
    }

    /// <summary>
    ///     根据Id查找实体
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public IEnumerable<TEntity> FindEntities(IEnumerable<TKey> ids)
    {
        var idArray = ids as TKey[] ?? ids.ToArray();
        OnActionExecuting(idArray, nameof(ids));
        return FindByIds(idArray);
    }

    /// <summary>
    ///     根据id查找映射实体
    /// </summary>
    /// <typeparam name="TMapEntity">映射类型</typeparam>
    /// <param name="ids">ids</param>
    /// <returns></returns>
    public IEnumerable<TMapEntity> FindMapEntities<TMapEntity>(IEnumerable<TKey> ids)
    {
        var idArray = ids as TKey[] ?? ids.ToArray();
        OnActionExecuting(idArray, nameof(ids));
        return FindByIds<TMapEntity>(idArray);
    }

    /// <summary>
    ///     根据缓存键查找实体
    /// </summary>
    /// <param name="keys">缓存键</param>
    /// <returns></returns>
    public IEnumerable<TEntity>? FindEntitiesViaKeys(IEnumerable<string> keys)
    {
        var keyList = keys.ToList();
        OnActionExecuting(keyList, nameof(keys));
        return GetEntities(keyList);
    }

    /// <summary>
    ///     根据缓存键查找实体查找映射实体
    /// </summary>
    /// <typeparam name="TMapEntity">映射类型</typeparam>
    /// <param name="keys">keys</param>
    /// <returns></returns>
    public IEnumerable<TMapEntity>? FindMapEntitiesViaKeys<TMapEntity>(IEnumerable<string> keys)
    {
        var keyList = keys.ToList();
        OnActionExecuting(keyList, nameof(keys));
        var entities = GetEntities(keyList);
        return entities?.Select(entity => entity.Adapt<TMapEntity>());
    }

    /// <summary>
    ///     根据Id查找实体
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public Task<TEntity?> FindEntityAsync(
        TKey id,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(id, nameof(id), cancellationToken);
        return FindByIdAsync(id, cancellationToken);
    }

    /// <summary>
    ///     根据Id查找映射实体
    /// </summary>
    /// <typeparam name="TMapEntity">映射类型</typeparam>
    /// <param name="id"></param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public Task<TMapEntity?> FindMapEntityAsync<TMapEntity>(
        TKey id,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(id, nameof(id), cancellationToken);
        return FindByIdAsync<TMapEntity>(id, cancellationToken);
    }

    /// <summary>
    ///     根据缓存键查找实体
    /// </summary>
    /// <param name="key">缓存键</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<TEntity?> FindEntityViaKeyAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        OnActionExecuting(key, nameof(key));
        return GetEntityAsync(key, cancellationToken);
    }

    /// <summary>
    ///     根据缓存键查找实体查找映射实体
    /// </summary>
    /// <typeparam name="TMapEntity">映射类型</typeparam>
    /// <param name="key">id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TMapEntity?> FindMapEntityViaKeyAsync<TMapEntity>(
        string key, CancellationToken
            cancellationToken = default)
    {
        OnActionExecuting(key, nameof(key));

        var entity = await GetEntityAsync(key, cancellationToken);

        return entity is not null ? entity.Adapt<TMapEntity>() : default;
    }

    /// <summary>
    ///     根据Id查找实体
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public Task<List<TEntity>> FindEntitiesAsync(
        IEnumerable<TKey> ids,
        CancellationToken cancellationToken = default)
    {
        var idArray = ids as TKey[] ?? ids.ToArray();
        OnAsyncActionExecuting(idArray, nameof(ids), cancellationToken);
        return FindByIdsAsync(idArray, cancellationToken);
    }

    /// <summary>
    ///     根据Id查找映射实体
    /// </summary>
    /// <typeparam name="TMapEntity">映射类型</typeparam>
    /// <param name="ids"></param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public Task<List<TMapEntity>> FindMapEntitiesAsync<TMapEntity>(
        IEnumerable<TKey> ids,
        CancellationToken cancellationToken = default)
    {
        var idArray = ids as TKey[] ?? ids.ToArray();
        OnAsyncActionExecuting(idArray, nameof(ids), cancellationToken);
        return FindByIdsAsync<TMapEntity>(idArray, cancellationToken);
    }

    /// <summary>
    ///     根据缓存键查找实体
    /// </summary>
    /// <param name="keys">缓存键</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<IEnumerable<TEntity>?> FindEntitiesViaKeysAsync(
        IEnumerable<string> keys,
        CancellationToken cancellationToken = default)
    {
        var keyList = keys.ToList();
        OnActionExecuting(keyList, nameof(keys));
        return GetEntitiesAsync(keyList, cancellationToken);
    }

    /// <summary>
    ///     根据缓存键查找实体查找映射实体
    /// </summary>
    /// <typeparam name="TMapEntity">映射类型</typeparam>
    /// <param name="keys">keys</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IEnumerable<TMapEntity>?> FindMapEntitiesViaKeysAsync<TMapEntity>(
        IEnumerable<string> keys,
        CancellationToken cancellationToken = default)
    {
        var keyList = keys.ToList();
        OnActionExecuting(keyList, nameof(keys));
        var entities = await GetEntitiesAsync(keyList, cancellationToken);
        return entities?.Select(entity => entity.Adapt<TMapEntity>());
    }

    #endregion

    #region Exists

    /// <summary>
    ///     判断实体是否存在
    /// </summary>
    /// <param name="id">实体键</param>
    /// <returns></returns>
    public bool Exists(TKey id)
    {
        SetDebugLog(nameof(Exists));
        OnActionExecuting(id, nameof(id));
        return KeyMatchQuery(id).Any();
    }

    /// <summary>
    ///     判断实体是否存在
    /// </summary>
    /// <param name="id">实体键</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public Task<bool> ExistsAsync(
        TKey id,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog(nameof(ExistsAsync));
        OnActionExecuting(id, nameof(id));
        return KeyMatchQuery(id).AnyAsync(cancellationToken);
    }

    #endregion

    #endregion

    #region Implementation of IStoreMap<TEntity,TKey>

    #region CreateEntity & CreateEntities

    /// <summary>
    ///     通过类型映射创建一个新实例
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="config">映射配置</param>
    /// <returns>映射后实体</returns>
    public StoreResult CreateNew<TSource>(
        TSource source,
        TypeAdapterConfig? config = null)
    {
        SetDebugLog(nameof(CreateNew));
        OnActionExecuting(source, nameof(source));
        config ??= IgnoreIdConfig<TSource>();
        var entity = source!.Adapt<TSource, TEntity>(config);
        if (entity is null)
            throw new MapTargetNullException(nameof(entity));
        AddEntity(entity);
        var result = AttacheChange();
        CacheEntity(entity);
        return result;
    }

    /// <summary>
    ///     通过类型映射创建一组新实例
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="config">映射配置</param>
    /// <returns>创建结果</returns>
    public StoreResult CreateNew<TSource>(
        IEnumerable<TSource> sources,
        TypeAdapterConfig? config = null)
    {
        SetDebugLog(nameof(CreateNew));
        OnActionExecuting(sources, nameof(sources));
        config ??= IgnoreIdConfig<TSource>();
        var entities = sources.Adapt<IEnumerable<TEntity>>(config);
        if (entities is null)
            throw new MapTargetNullException(nameof(entities));
        var list = entities.ToList();
        AddEntities(list);
        var result = AttacheChange();
        CacheEntities(list);
        return result;
    }

    /// <summary>
    ///     通过类型映射创建一个新实例
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns>创建结果</returns>
    public async Task<StoreResult> CreateNewAsync<TSource>(
        TSource source,
        TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog(nameof(CreateNewAsync));
        OnAsyncActionExecuting(source, nameof(source), cancellationToken);
        config ??= IgnoreIdConfig<TSource>();
        var entity = source!.Adapt<TSource, TEntity>(config);
        if (entity is null)
            throw new MapTargetNullException(nameof(entity));
        AddEntity(entity);
        var result = await AttacheChangeAsync(cancellationToken);
        await CacheEntityAsync(entity, cancellationToken);
        return result;
    }

    /// <summary>
    ///     通过类型映射创建一个新实例
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns>创建结果</returns>
    public async Task<StoreResult> CreateNewAsync<TSource>(
        IEnumerable<TSource> sources,
        TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog(nameof(CreateNewAsync));
        OnAsyncActionExecuting(sources, nameof(sources), cancellationToken);
        config ??= IgnoreIdConfig<TSource>();
        var entities = sources.Adapt<IEnumerable<TEntity>>(config);
        if (entities is null)
            throw new MapTargetNullException(nameof(entities));
        var list = entities.ToList();
        AddEntities(list);
        var result = await AttacheChangeAsync(cancellationToken);
        await CacheEntitiesAsync(list, cancellationToken);
        return result;
    }

    #endregion

    #region OverEntity & OverEntities

    /// <summary>
    ///     通过类型映射覆盖对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="config">映射配置</param>
    /// <returns></returns>
    public StoreResult Over<TSource>(
        TSource source,
        TypeAdapterConfig? config = null) where TSource : IKeySlot<TKey>
    {
        SetDebugLog(nameof(Over));
        OnActionExecuting(source, nameof(source));
        config ??= IgnoreMetaConfig<TSource>();
        var entity = source.Adapt<TSource, TEntity>(config);
        if (entity is null)
            throw new MapTargetNullException(nameof(entity));
        UpdateEntity(entity);
        var result = AttacheChange();
        CacheEntity(entity);
        return result;
    }

    /// <summary>
    ///     通过类型映射覆盖对应实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="destination">目标数据</param>
    /// <param name="config">映射配置</param>
    /// <returns></returns>
    public StoreResult Over<TSource>(
        TSource source,
        TEntity destination,
        TypeAdapterConfig? config = null)
    {
        SetDebugLog(nameof(Over));
        OnActionExecuting(source, nameof(source));
        config ??= IgnoreIdConfig<TSource>();
        source.Adapt(destination, config);
        if (destination is null)
            throw new MapTargetNullException(nameof(destination));
        UpdateEntity(destination);
        var result = AttacheChange();
        CacheEntity(destination);
        return result;
    }

    /// <summary>
    ///     通过类型映射覆盖对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="config">映射配置</param>
    /// <returns></returns>
    public StoreResult Over<TSource>(
        IEnumerable<TSource> sources,
        TypeAdapterConfig? config = null)
        where TSource : IKeySlot<TKey>
    {
        SetDebugLog(nameof(Over));
        OnActionExecuting(sources, nameof(sources));
        config ??= IgnoreMetaConfig<TSource>();
        var entities = sources.Adapt<IEnumerable<TEntity>>(config);
        if (entities is null)
            throw new MapTargetNullException(nameof(entities));
        var list = entities.ToList();
        UpdateEntities(list);
        var result = AttacheChange();
        CacheEntities(list);
        return result;
    }

    /// <summary>
    ///     通过类型映射覆盖对应实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TJKey">连接键类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="destinations">目标数据</param>
    /// <param name="destinationKeySelector">目标连接键选择器</param>
    /// <param name="config">映射配置</param>
    /// <param name="sourceKeySelector">源连接键选择器</param>
    /// <returns></returns>
    public StoreResult Over<TSource, TJKey>(
        IEnumerable<TSource> sources,
        IEnumerable<TEntity> destinations,
        Func<TSource, TJKey> sourceKeySelector,
        Func<TEntity, TJKey> destinationKeySelector,
        TypeAdapterConfig? config = null)
    {
        SetDebugLog(nameof(Over));
        var sourceList = sources.ToList();
        OnActionExecuting(sourceList, nameof(sources));
        config ??= IgnoreIdConfig<TSource>();
        var entities = sourceList.Join(
            destinations,
            sourceKeySelector,
            destinationKeySelector,
            (source, _) => source.Adapt<TSource, TEntity>(config));
        if (entities is null)
            throw new MapTargetNullException(nameof(entities));
        var list = entities.ToList();
        UpdateEntities(list);
        var result = AttacheChange();
        CacheEntities(list);
        return result;
    }

    /// <summary>
    ///     通过类型映射覆盖对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> OverAsync<TSource>(
        TSource source,
        TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default) where TSource : IKeySlot<TKey>
    {
        SetDebugLog(nameof(OverAsync));
        OnAsyncActionExecuting(source, nameof(source), cancellationToken);
        config ??= IgnoreMetaConfig<TSource>();
        var entity = source.Adapt<TSource, TEntity>(config);
        if (entity is null)
            throw new MapTargetNullException(nameof(entity));
        UpdateEntity(entity);
        var result = await AttacheChangeAsync(cancellationToken);
        await CacheEntityAsync(entity, cancellationToken);
        return result;
    }

    /// <summary>
    ///     通过类型映射覆盖对应实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="destination">目标数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> OverAsync<TSource>(
        TSource source,
        TEntity destination,
        TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog(nameof(OverAsync));
        OnAsyncActionExecuting(source, nameof(source), cancellationToken);
        config ??= IgnoreIdConfig<TSource>();
        source.Adapt(destination, config);
        if (destination is null)
            throw new MapTargetNullException(nameof(destination));
        UpdateEntity(destination);
        var result = await AttacheChangeAsync(cancellationToken);
        await CacheEntityAsync(destination, cancellationToken);
        return result;
    }

    /// <summary>
    ///     通过类型映射覆盖对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> OverAsync<TSource>(
        IEnumerable<TSource> sources,
        TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default) where TSource : IKeySlot<TKey>
    {
        SetDebugLog(nameof(OverAsync));
        OnAsyncActionExecuting(sources, nameof(sources), cancellationToken);
        config ??= IgnoreMetaConfig<TSource>();
        var entities = sources.Adapt<IEnumerable<TEntity>>(config);
        if (entities is null)
            throw new MapTargetNullException(nameof(entities));
        var list = entities.ToList();
        UpdateEntities(list);
        var result = await AttacheChangeAsync(cancellationToken);
        await CacheEntitiesAsync(list, cancellationToken);
        return result;
    }

    /// <summary>
    ///     通过类型映射覆盖对应实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TJKey">连接键类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="destinations">目标数据</param>
    /// <param name="destinationKeySelector">目标键选择器</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <param name="sourceKeySelector">源键选择器</param>
    /// <returns></returns>
    public async Task<StoreResult> OverAsync<TSource, TJKey>(
        IEnumerable<TSource> sources,
        IEnumerable<TEntity> destinations,
        Func<TSource, TJKey> sourceKeySelector,
        Func<TEntity, TJKey> destinationKeySelector,
        TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog(nameof(OverAsync));
        var sourceList = sources.ToList();
        OnAsyncActionExecuting(sourceList, nameof(sources), cancellationToken);
        config ??= IgnoreIdConfig<TSource>();
        var entities = sourceList.Join(
            destinations,
            sourceKeySelector,
            destinationKeySelector,
            (source, _) => source.Adapt<TSource, TEntity>(config));
        if (entities is null)
            throw new MapTargetNullException(nameof(entities));
        var list = entities.ToList();
        UpdateEntities(list);
        var result = await AttacheChangeAsync(cancellationToken);
        await CacheEntitiesAsync(list, cancellationToken);
        return result;
    }

    #endregion

    #region MergeEntity & MergeEntities

    /// <summary>
    ///     通过类型映射合并对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="config">映射配置</param>
    /// <returns></returns>
    public StoreResult Merge<TSource>(
        TSource source,
        TypeAdapterConfig? config = null) where TSource : IKeySlot<TKey>
    {
        SetDebugLog(nameof(Merge));
        OnActionExecuting(source, nameof(source));
        var entity = FindById(source.Id);
        if (entity is null)
            throw new MapTargetNullException(nameof(entity));
        config ??= IgnoreNullConfig<TSource>();
        source.Adapt(entity, config);
        UpdateEntity(entity);
        var result = AttacheChange();
        CacheEntity(entity);
        return result;
    }

    /// <summary>
    ///     通过类型映射合并对应实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="destination">目标数据</param>
    /// <param name="config">映射配置</param>
    /// <returns></returns>
    public StoreResult Merge<TSource>(
        TSource source,
        TEntity destination,
        TypeAdapterConfig? config = null)
    {
        SetDebugLog(nameof(Merge));
        OnActionExecuting(source, nameof(source));
        config ??= IgnoreIdAndNullConfig<TSource>();
        source.Adapt(destination, config);
        if (destination is null)
            throw new MapTargetNullException(nameof(destination));
        UpdateEntity(destination);
        var result = AttacheChange();
        CacheEntity(destination);
        return result;
    }

    /// <summary>
    ///     通过类型映射合并对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="config">映射配置</param>
    /// <returns></returns>
    public StoreResult Merge<TSource>(
        IEnumerable<TSource> sources,
        TypeAdapterConfig? config = null)
        where TSource : IKeySlot<TKey>
    {
        SetDebugLog(nameof(Merge));
        var sourceList = sources.ToList();
        OnActionExecuting(sourceList, nameof(sources));
        var destinations = FindByIds(sourceList.Select(source => source.Id));
        if (destinations is null)
            throw new MapTargetNullException(nameof(destinations));
        config ??= IgnoreNullConfig<TSource>();
        var list = sourceList.Join(
            destinations, source => source.Id,
            destination => destination.Id,
            (source, destination) => source.Adapt(destination, config)).ToList();
        UpdateEntities(list);
        var result = AttacheChange();
        CacheEntities(list);
        return result;
    }

    /// <summary>
    ///     通过类型映射合并对应实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TJKey">连接键类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="destinations">目标数据</param>
    /// <param name="destinationKeySelector">目标键选择器</param>
    /// <param name="config">映射配置</param>
    /// <param name="sourceKeySelector">源键选择器</param>
    /// <returns></returns>
    public StoreResult Merge<TSource, TJKey>(
        IEnumerable<TSource> sources,
        IEnumerable<TEntity> destinations,
        Func<TSource, TJKey> sourceKeySelector,
        Func<TEntity, TJKey> destinationKeySelector,
        TypeAdapterConfig? config = null)
    {
        SetDebugLog(nameof(Merge));
        var sourceList = sources.ToList();
        OnActionExecuting(sourceList, nameof(sources));
        config ??= IgnoreIdAndNullConfig<TSource>();
        var entities = sourceList.Join(
            destinations,
            sourceKeySelector,
            destinationKeySelector,
            (source, _) => source.Adapt<TSource, TEntity>(config));
        if (entities is null)
            throw new MapTargetNullException(nameof(entities));
        var list = entities.ToList();
        UpdateEntities(list);
        var result = AttacheChange();
        CacheEntities(list);
        return result;
    }

    /// <summary>
    ///     通过类型映射合并对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> MergeAsync<TSource>(
        TSource source,
        TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default) where TSource : IKeySlot<TKey>
    {
        SetDebugLog(nameof(MergeAsync));
        OnAsyncActionExecuting(source, nameof(source), cancellationToken);
        var entity = await FindByIdAsync(source.Id, cancellationToken);
        if (entity is null)
            throw new MapTargetNullException(nameof(entity));
        config ??= IgnoreNullConfig<TSource>();
        source.Adapt(entity, config);
        UpdateEntity(entity);
        var result = await AttacheChangeAsync(cancellationToken);
        await CacheEntityAsync(entity, cancellationToken);
        return result;
    }

    /// <summary>
    ///     通过类型映射合并对应实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="destination">目标数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> MergeAsync<TSource>(
        TSource source,
        TEntity destination,
        TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog(nameof(MergeAsync));
        OnAsyncActionExecuting(source, nameof(source), cancellationToken);
        config ??= IgnoreIdAndNullConfig<TSource>();
        source.Adapt(destination, config);
        if (destination is null)
            throw new MapTargetNullException(nameof(destination));
        UpdateEntity(destination);
        var result = await AttacheChangeAsync(cancellationToken);
        await CacheEntityAsync(destination, cancellationToken);
        return result;
    }

    /// <summary>
    ///     通过类型映射合并对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> MergeAsync<TSource>(
        IEnumerable<TSource> sources,
        TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default) where TSource : IKeySlot<TKey>
    {
        SetDebugLog(nameof(MergeAsync));
        var sourceList = sources.ToList();
        OnAsyncActionExecuting(sourceList, nameof(sources), cancellationToken);
        var destinations = await FindByIdsAsync(sourceList.Select(source => source.Id), cancellationToken);
        if (destinations is null)
            throw new MapTargetNullException(nameof(destinations));
        config ??= IgnoreNullConfig<TSource>();
        var list = sourceList.Join(
            destinations, source => source.Id,
            destination => destination.Id,
            (source, destination) => source.Adapt(destination, config)).ToList();
        UpdateEntities(list);
        var result = await AttacheChangeAsync(cancellationToken);
        await CacheEntitiesAsync(list, cancellationToken);
        return result;
    }

    /// <summary>
    ///     通过类型映射合并对应实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TJKey">连接键类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="destinations">目标数据</param>
    /// <param name="destinationKeySelector">目标键选择器</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <param name="sourceKeySelector">源键选择器</param>
    /// <returns></returns>
    public async Task<StoreResult> MergeAsync<TSource, TJKey>(
        IEnumerable<TSource> sources,
        IEnumerable<TEntity> destinations,
        Func<TSource, TJKey> sourceKeySelector,
        Func<TEntity, TJKey> destinationKeySelector,
        TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog(nameof(MergeAsync));
        var sourceList = sources.ToList();
        OnAsyncActionExecuting(sourceList, nameof(sources), cancellationToken);
        config ??= IgnoreIdAndNullConfig<TSource>();
        var entities = sourceList.Join(
            destinations,
            sourceKeySelector,
            destinationKeySelector,
            (source, _) => source.Adapt<TSource, TEntity>(config));
        if (entities is null)
            throw new MapTargetNullException(nameof(entities));
        var list = entities.ToList();
        UpdateEntities(list);
        var result = await AttacheChangeAsync(cancellationToken);
        await CacheEntitiesAsync(list, cancellationToken);
        return result;
    }

    #endregion

    #region MapConfig

    /// <summary>
    ///     id和空值忽略缓存
    /// </summary>
    private TypeAdapterConfig? _ignoreIdAndNullConfig;

    /// <summary>
    ///     在忽略目标实体元数据属性的基础上忽略源实体的Id属性和空值属性
    /// </summary>
    /// <typeparam name="TSource">源数据类型</typeparam>
    /// <returns>映射配置</returns>
    public TypeAdapterConfig IgnoreIdAndNullConfig<TSource>()
    {
        if (_ignoreIdAndNullConfig is not null)
            return _ignoreIdAndNullConfig;
        _ignoreIdAndNullConfig = IgnoreNullConfig<TSource>().Clone();
        _ignoreIdAndNullConfig.NewConfig<TSource, TEntity>()
            .IgnoreNullValues(true);
        return _ignoreIdAndNullConfig;
    }

    /// <summary>
    ///     空值或空字符值忽略缓存
    /// </summary>
    private TypeAdapterConfig? _ignoreNullConfig;

    /// <summary>
    ///     在忽略目标实体元数据属性的基础上忽略源实体的空值属性
    /// </summary>
    /// <typeparam name="TSource">源数据类型</typeparam>
    /// <returns>映射配置</returns>
    public TypeAdapterConfig IgnoreNullConfig<TSource>()
    {
        if (_ignoreNullConfig is not null)
            return _ignoreNullConfig;
        _ignoreNullConfig = IgnoreMetaConfig<TSource>().Clone();
        _ignoreNullConfig.NewConfig<TSource, TEntity>()
            .IgnoreNullValues(true);
        return _ignoreNullConfig;
    }

    /// <summary>
    ///     Id忽略缓存
    /// </summary>
    private TypeAdapterConfig? _ignoreIdConfig;

    /// <summary>
    ///     在忽略目标实体元数据属性的基础上忽略目标实体的Id属性
    /// </summary>
    /// <typeparam name="TSource">源数据类型</typeparam>
    /// <returns>映射配置</returns>
    public TypeAdapterConfig IgnoreIdConfig<TSource>()
    {
        if (_ignoreIdConfig is not null)
            return _ignoreIdConfig;
        _ignoreIdConfig = IgnoreMetaConfig<TSource>().Clone();
        _ignoreIdConfig.NewConfig<TSource, TEntity>()
            .Ignore(item => item.Id);
        return _ignoreIdConfig;
    }

    /// <summary>
    ///     元数据忽略缓存
    /// </summary>
    private TypeAdapterConfig? _ignoreMetaConfig;

    /// <summary>
    ///     忽略目标实体的元数据属性
    /// </summary>
    /// <typeparam name="TSource">源数据类型</typeparam>
    /// <returns>映射配置</returns>
    public TypeAdapterConfig IgnoreMetaConfig<TSource>()
    {
        if (_ignoreMetaConfig is not null)
            return _ignoreMetaConfig;
        _ignoreMetaConfig = new TypeAdapterConfig();
        _ignoreMetaConfig.NewConfig<TSource, TEntity>()
            .Ignore(item => item.CreatedAt)
            .Ignore(item => item.UpdatedAt)
            .Ignore(item => item.DeletedAt!);
        return _ignoreMetaConfig;
    }

    #endregion

    #endregion

    #region ActionExecution

    /// <summary>
    ///     添加单个实体
    /// </summary>
    /// <param name="entity">实体</param>
    private void AddEntity(TEntity entity)
    {
        if (MetaDataHosting)
        {
            var now = DateTime.Now;
            entity.CreatedAt = now;
            entity.UpdatedAt = now;
        }

        Context.Add(entity);
    }

    /// <summary>
    ///     添加多个实体
    /// </summary>
    /// <param name="entities">实体</param>
    private void AddEntities(ICollection<TEntity> entities)
    {
        if (MetaDataHosting)
        {
            var now = DateTime.Now;
            foreach (var entity in entities)
            {
                entity.CreatedAt = now;
                entity.UpdatedAt = now;
            }
        }

        Context.AddRange(entities);
    }

    /// <summary>
    ///     追踪一个实体更新
    /// </summary>
    /// <param name="entity">实体</param>
    private void UpdateEntity(TEntity entity)
    {
        Context.Attach(entity);

        if (entity is IConcurrencyStamp concurrency)
            concurrency.ConcurrencyStamp = Guid.NewGuid().ToString();

        if (MetaDataHosting)
        {
            var now = DateTime.Now;
            entity.UpdatedAt = now;
        }

        Context.Update(entity);

        if (MetaDataHosting)
            Context.Entry(entity).Property(item => item.CreatedAt).IsModified = false;
    }

    /// <summary>
    ///     追踪多个实体更新
    /// </summary>
    /// <param name="entities">实体</param>
    private void UpdateEntities(ICollection<TEntity> entities)
    {
        Context.AttachRange(entities);
        if (MetaDataHosting)
        {
            var now = DateTime.Now;
            foreach (var entity in entities) entity.UpdatedAt = now;
        }

        Context.UpdateRange(entities);

        if (MetaDataHosting)
            foreach (var entity in entities)
                Context.Entry(entity).Property(item => item.CreatedAt).IsModified = false;
    }

    /// <summary>
    ///     批量删除实体
    /// </summary>
    /// <param name="query">查询</param>
    /// <param name="setter">更新委托</param>
    /// <returns></returns>
    private int BatchUpdateEntity(
        IQueryable<TEntity> query,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter)
    {
        if (MetaDataHosting)
            query.ExecuteUpdate(metaSetter => metaSetter.SetProperty(entity => entity.UpdatedAt, DateTime.Now));

        return query.ExecuteUpdate(setter);
    }

    /// <summary>
    ///     批量删除实体
    /// </summary>
    /// <param name="query">查询</param>
    /// <param name="setter">更新委托</param>
    /// <param name="cancellationToken">异步操作取消信号</param>
    /// <returns></returns>
    private Task<int> BatchUpdateEntityAsync(
        IQueryable<TEntity> query,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter,
        CancellationToken cancellationToken = default)
    {
        if (MetaDataHosting)
            query.ExecuteUpdate(metaSetter => metaSetter.SetProperty(entity => entity.UpdatedAt, DateTime.Now));

        return query.ExecuteUpdateAsync(setter, cancellationToken);
    }

    /// <summary>
    ///     追踪一个实体删除
    /// </summary>
    /// <param name="entity">实体</param>
    private void DeleteEntity(TEntity entity)
    {
        if (SoftDelete)
        {
            Context.Attach(entity);
            var now = DateTime.Now;
            entity.UpdatedAt = now;
            entity.DeletedAt = now;
            Context.Update(entity);

            Context.Entry(entity).Property(item => item.CreatedAt).IsModified = false;
        }
        else
        {
            Context.Remove(entity);
        }
    }

    /// <summary>
    ///     追踪多个实体删除
    /// </summary>
    /// <param name="entities">实体</param>
    private void DeleteEntities(ICollection<TEntity> entities)
    {
        if (SoftDelete)
        {
            Context.AttachRange(entities);
            var now = DateTime.Now;
            foreach (var entity in entities)
            {
                entity.UpdatedAt = now;
                entity.DeletedAt = now;
            }

            Context.UpdateRange(entities);

            foreach (var entity in entities)
                Context.Entry(entity).Property(item => item.CreatedAt).IsModified = false;
        }
        else
        {
            Context.RemoveRange(entities);
        }
    }

    /// <summary>
    ///     批量删除实体
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    private int BatchDeleteEntity(IQueryable<TEntity> query)
    {
        if (SoftDelete)
            return query.ExecuteUpdate(setter => setter
                .SetProperty(entity => entity.UpdatedAt, DateTime.Now)
                .SetProperty(entity => entity.DeletedAt, DateTime.Now));

        return query.ExecuteDelete();
    }

    /// <summary>
    ///     异步批量删除实体
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private Task<int> BatchDeleteEntityAsync(
        IQueryable<TEntity> query,
        CancellationToken cancellationToken = default)
    {
        if (SoftDelete)
            return query.ExecuteUpdateAsync(setter => setter
                .SetProperty(entity => entity.UpdatedAt, DateTime.Now)
                .SetProperty(entity => entity.DeletedAt, DateTime.Now), cancellationToken);

        return query.ExecuteDeleteAsync(cancellationToken);
    }

    /// <summary>
    ///     根据Id查询实体
    /// </summary>
    /// <param name="id">id</param>
    /// <returns></returns>
    private TEntity? FindById(TKey id)
    {
        return KeyMatchQuery(id).FirstOrDefault();
    }

    /// <summary>
    ///     根据Id查询实体并映射到指定类型
    /// </summary>
    /// <typeparam name="TMapEntity">映射类型</typeparam>
    /// <param name="id">id</param>
    /// <returns></returns>
    private TMapEntity? FindById<TMapEntity>(TKey id)
    {
        return KeyMatchQuery(id)
            .ProjectToType<TMapEntity>()
            .FirstOrDefault();
    }

    /// <summary>
    ///     根据Id查询实体
    /// </summary>
    /// <param name="ids">id表</param>
    /// <returns></returns>
    private IEnumerable<TEntity> FindByIds(IEnumerable<TKey> ids)
    {
        return KeyMatchQuery(ids).ToList();
    }

    /// <summary>
    ///     根据Id查询实体并映射到指定类型
    /// </summary>
    /// <typeparam name="TMapEntity">映射类型</typeparam>
    /// <param name="ids">id表</param>
    /// <returns></returns>
    private IEnumerable<TMapEntity> FindByIds<TMapEntity>(IEnumerable<TKey> ids)
    {
        return KeyMatchQuery(ids)
            .ProjectToType<TMapEntity>()
            .ToList();
    }

    /// <summary>
    ///     根据Id查找实体
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    private Task<TEntity?> FindByIdAsync(
        TKey id,
        CancellationToken cancellationToken = default)
    {
        return KeyMatchQuery(id).FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    ///     根据Id查询实体并映射到指定类型
    /// </summary>
    /// <typeparam name="TMapEntity">映射类型</typeparam>
    /// <param name="id"></param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    private Task<TMapEntity?> FindByIdAsync<TMapEntity>(
        TKey id,
        CancellationToken cancellationToken = default)
    {
        return KeyMatchQuery(id)
            .ProjectToType<TMapEntity>()
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    ///     根据Id查找实体
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    private Task<List<TEntity>> FindByIdsAsync(
        IEnumerable<TKey> ids,
        CancellationToken cancellationToken = default)
    {
        return KeyMatchQuery(ids)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    ///     根据Id查询实体并映射到指定类型
    /// </summary>
    /// <typeparam name="TMapEntity">映射类型</typeparam>
    /// <param name="ids"></param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    private Task<List<TMapEntity>> FindByIdsAsync<TMapEntity>(
        IEnumerable<TKey> ids,
        CancellationToken cancellationToken = default)
    {
        return KeyMatchQuery(ids)
            .ProjectToType<TMapEntity>()
            .ToListAsync(cancellationToken);
    }

    #endregion

    #region OnActionExecution

    /// <summary>
    ///     保存追踪
    /// </summary>
    /// <returns></returns>
    private StoreResult AttacheChange()
    {
        try
        {
            var changes = SaveChanges();
            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(Describer.ConcurrencyFailure());
        }
    }

    /// <summary>
    ///     保存异步追踪
    /// </summary>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    private async Task<StoreResult> AttacheChangeAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var changes = await SaveChangesAsync(cancellationToken);
            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(Describer.ConcurrencyFailure());
        }
    }

    #endregion
}