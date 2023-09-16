using System.Linq.Expressions;
using Artemis.Data.Core;
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
public class Store<TEntity> : Store<TEntity, Guid>, IStore<TEntity>
    where TEntity : class, IModelBase, IModelBase<Guid>
{
    /// <summary>
    ///     创建一个新的基本存储实例
    /// </summary>
    /// <param name="context">数据访问上下文</param>
    /// <param name="logger">日志依赖</param>
    /// <param name="describer">操作异常描述者</param>
    /// <param name="cache">缓存依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    public Store(
        DbContext context,
        IDistributedCache? cache = null,
        ILogger? logger = null,
        IStoreErrorDescriber? describer = null) : base(context, cache, logger, describer)
    {
    }

    #region Overrides of StoreBase<TEntity,Guid>

    /// <summary>
    ///     转换字符串到id
    /// </summary>
    /// <param name="id">id字符串</param>
    /// <returns>id</returns>
    protected override Guid ConvertIdFromString(string? id)
    {
        return id.GuidFromString();
    }

    /// <summary>
    ///     转换Id为字符串
    /// </summary>
    /// <param name="id">id</param>
    /// <returns>字符串</returns>
    protected override string ConvertIdToString(Guid id)
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
    /// <param name="describer">操作异常描述者</param>
    /// <param name="cache">缓存依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    protected Store(
        DbContext context,
        IDistributedCache? cache = null,
        ILogger? logger = null,
        IStoreErrorDescriber? describer = null) : base(context, cache, logger, describer)
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
        IStoreErrorDescriber? describer = null) : base(describer ?? new StoreErrorDescriber())
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
        if (DebugLogger) Logger?.LogDebug(message);
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
    public IQueryable<TEntity> TrackingQuery => EntitySet.Where(item => !SoftDelete || item.DeletedAt != null);

    /// <summary>
    ///     Entity无追踪访问器
    /// </summary>
    public IQueryable<TEntity> EntityQuery =>
        EntitySet.Where(item => !SoftDelete || item.DeletedAt != null).AsNoTracking();

    #endregion

    #region Cache

    /// <summary>
    ///     缓存选项
    /// </summary>
    private DistributedCacheEntryOptions? CacheOption
    {
        get
        {
            if (Expires <= 0) return null;

            return new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(Expires)
            };
        }
    }

    /// <summary>
    ///     缓存实体
    /// </summary>
    /// <param name="entity"></param>
    private void CacheEntity(TEntity entity)
    {
        if (CachedStore)
        {
            if (CacheOption == null)
                Cache?.SetString(entity.GenerateKey, entity.Serialize());
            else
                Cache?.SetString(entity.GenerateKey, entity.Serialize(), CacheOption);
            SetDebugLog("Entity Cached");
        }
    }

    /// <summary>
    ///     缓存实体
    /// </summary>
    /// <param name="entities"></param>
    private void CacheEntities(IEnumerable<TEntity> entities)
    {
        if (CachedStore)
        {
            var count = 0;
            foreach (var entity in entities)
            {
                Cache?.SetString(entity.GenerateKey, entity.Serialize());
                count++;
            }

            SetDebugLog($"Cached {count} Entities");
        }
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
            var json = Cache?.GetString(key);

            return json?.Deserialize<TEntity>();
        }

        throw new NotImplementedException(nameof(Cache));
    }

    /// <summary>
    ///     获取实体
    /// </summary>
    /// <param name="keys">实体键列表</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private IEnumerable<TEntity> GetEntities(IEnumerable<string> keys)
    {
        if (CachedStore)
        {
            var list = new List<TEntity>();

            foreach (var key in keys)
            {
                var json = Cache?.GetString(key);

                var entity = json?.Deserialize<TEntity>();

                if (entity != null) list.Add(entity);
            }

            return list;
        }

        throw new NotImplementedException(nameof(Cache));
    }

    /// <summary>
    ///     移除被缓存的实体
    /// </summary>
    /// <param name="entity"></param>
    private void RemoveCachedEntity(TEntity entity)
    {
        if (CachedStore) Cache?.Remove(entity.GenerateKey);
        SetDebugLog("Entity Remove");
    }

    /// <summary>
    ///     移除被缓存的实体
    /// </summary>
    /// <param name="entities"></param>
    private void RemoveCachedEntities(IEnumerable<TEntity> entities)
    {
        if (CachedStore)
        {
            var count = 0;
            foreach (var entity in entities)
            {
                Cache?.Remove(entity.GenerateKey);
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
    public bool CachedStore { get; set; }

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
    public bool DebugLogger { get; set; }

    #endregion

    /// <summary>
    ///     设置配置
    /// </summary>
    /// <param name="storeOptions"></param>
    public void SetOptions(IStoreOptions storeOptions)
    {
        AutoSaveChanges = storeOptions.AutoSaveChanges;
        MetaDataHosting = storeOptions.MetaDataHosting;
        SoftDelete = storeOptions.SoftDelete;
        CachedStore = storeOptions.CachedStore;
        Expires = storeOptions.Expires;
        DebugLogger = storeOptions.DebugLogger;
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
        var changes = SaveChanges();
        CacheEntity(entity);
        return StoreResult.Success(changes);
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
        var changes = SaveChanges();
        CacheEntities(list);
        return StoreResult.Success(changes);
    }

    /// <summary>
    ///     在<paramref name="entity" />存储中创建一个新的实体
    /// </summary>
    /// <param name="entity">被创建实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public virtual async Task<StoreResult> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        SetDebugLog(nameof(CreateAsync));
        OnAsyncActionExecuting(entity, nameof(entity), cancellationToken);
        AddEntity(entity);
        var changes = await SaveChangesAsync(cancellationToken);
        CacheEntity(entity);
        return StoreResult.Success(changes);
    }

    /// <summary>
    ///     在<paramref name="entities" />存储中创建多个新的实体
    /// </summary>
    /// <param name="entities">被创建实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public virtual async Task<StoreResult> CreateAsync(IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog(nameof(CreateAsync));
        var list = entities.ToList();
        OnAsyncActionExecuting(list, nameof(entities), cancellationToken);
        AddEntities(list);
        var changes = await SaveChangesAsync(cancellationToken);
        CacheEntities(list);
        return StoreResult.Success(changes);
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
        CacheEntity(entity);
        return AttacheChange();
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
        CacheEntities(list);
        return AttacheChange();
    }

    /// <summary>
    ///     在<paramref name="entity" />存储中更新已存在的实体
    /// </summary>
    /// <param name="entity">被创建实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public Task<StoreResult> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        SetDebugLog(nameof(UpdateAsync));
        OnAsyncActionExecuting(entity, nameof(entity), cancellationToken);
        UpdateEntity(entity);
        CacheEntity(entity);
        return AttacheChangeAsync(cancellationToken);
    }

    /// <summary>
    ///     在<paramref name="entities" />存储中更新多个已存在的实体
    /// </summary>
    /// <param name="entities">被创建实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public Task<StoreResult> UpdateAsync(IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog(nameof(UpdateAsync));
        var list = entities.ToList();
        OnAsyncActionExecuting(list, nameof(entities), cancellationToken);
        UpdateEntities(list);
        CacheEntities(list);
        return AttacheChangeAsync(cancellationToken);
    }

    #endregion

    #region BatchUpdateEntity & BatchUpdateEntities

    /// <summary>
    ///     更新存储中的实体
    /// </summary>
    /// <param name="id">被更新实体的主键</param>
    /// <param name="setter">更新行为</param>
    /// <returns></returns>
    public StoreResult BatchUpdate(TKey id,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter)
    {
        if (CachedStore) return StoreResult.Failed(ErrorDescriber.EnableCache());
        OnActionExecuting(id, nameof(id));
        try
        {
            var query = Context.Set<TEntity>()
                .Where(item => item.Id.Equals(id));

            var changes = BatchUpdateEntity(query, setter);

            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(ErrorDescriber.ConcurrencyFailure());
        }
    }

    /// <summary>
    ///     在存储中更新已存在的实体
    /// </summary>
    /// <param name="ids">被更新实体的主键</param>
    /// <param name="setter">更新行为</param>
    /// <returns></returns>
    public StoreResult BatchUpdate(IEnumerable<TKey> ids,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter)
    {
        if (CachedStore) return StoreResult.Failed(ErrorDescriber.EnableCache());
        var idList = ids as List<TKey> ?? ids.ToList();
        OnActionExecuting(idList, nameof(ids));
        try
        {
            var query = Context.Set<TEntity>()
                .Where(item => idList.Contains(item.Id));

            var changes = BatchUpdateEntity(query, setter);

            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(ErrorDescriber.ConcurrencyFailure());
        }
    }

    /// <summary>
    ///     在存储中更新符合条件的实体
    /// </summary>
    /// <param name="setter">更新行为</param>
    /// <param name="predicate">查询表达式</param>
    /// <returns></returns>
    public StoreResult BatchUpdate(Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter,
        Expression<Func<TEntity, bool>>? predicate = null)
    {
        if (CachedStore) return StoreResult.Failed(ErrorDescriber.EnableCache());
        ThrowIfDisposed();
        try
        {
            var query = Context.Set<TEntity>().AsNoTracking();

            if (predicate != null)
                query = query.Where(predicate);

            var changes = BatchUpdateEntity(query, setter);

            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(ErrorDescriber.ConcurrencyFailure());
        }
    }

    /// <summary>
    ///     在存储中更新符合查询描述的实体
    /// </summary>
    /// <param name="query"></param>
    /// <param name="setter">更新行为</param>
    /// <returns></returns>
    public StoreResult BatchUpdate(IQueryable<TEntity> query,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter)
    {
        if (CachedStore) return StoreResult.Failed(ErrorDescriber.EnableCache());
        ThrowIfDisposed();
        try
        {
            var changes = BatchUpdateEntity(query, setter);

            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(ErrorDescriber.ConcurrencyFailure());
        }
    }

    /// <summary>
    ///     在存储中更新已存在的实体
    /// </summary>
    /// <param name="id">被更新实体的主键</param>
    /// <param name="setter">更新行为</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> BatchUpdateAsync(TKey id,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter,
        CancellationToken cancellationToken = default)
    {
        if (CachedStore) return StoreResult.Failed(ErrorDescriber.EnableCache());
        OnAsyncActionExecuting(id, nameof(id), cancellationToken);
        try
        {
            var query = Context.Set<TEntity>()
                .Where(item => item.Id.Equals(id));

            var changes = await BatchUpdateEntityAsync(query, setter, cancellationToken);

            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(ErrorDescriber.ConcurrencyFailure());
        }
    }

    /// <summary>
    ///     在存储中更新已存在的实体
    /// </summary>
    /// <param name="ids">被更新实体的主键</param>
    /// <param name="setter">更新行为</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> BatchUpdateAsync(IEnumerable<TKey> ids,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter,
        CancellationToken cancellationToken = default)
    {
        if (CachedStore) return StoreResult.Failed(ErrorDescriber.EnableCache());
        var idList = ids as List<TKey> ?? ids.ToList();
        OnAsyncActionExecuting(idList, nameof(ids), cancellationToken);
        try
        {
            var query = Context.Set<TEntity>()
                .Where(item => idList.Contains(item.Id));

            var changes = await BatchUpdateEntityAsync(query, setter, cancellationToken);

            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(ErrorDescriber.ConcurrencyFailure());
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
        Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        if (CachedStore) return StoreResult.Failed(ErrorDescriber.EnableCache());
        ThrowIfDisposed();
        cancellationToken.ThrowIfCancellationRequested();
        try
        {
            var query = Context.Set<TEntity>().AsNoTracking();

            if (predicate != null)
                query =
                    query.Where(predicate);

            var changes = await BatchUpdateEntityAsync(query, setter, cancellationToken);

            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(ErrorDescriber.ConcurrencyFailure());
        }
    }

    /// <summary>
    ///     在存储中更新符合查询描述的实体
    /// </summary>
    /// <param name="query"></param>
    /// <param name="setter">更新行为</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> BatchUpdateAsync(IQueryable<TEntity> query,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter,
        CancellationToken cancellationToken = default)
    {
        if (CachedStore) return StoreResult.Failed(ErrorDescriber.EnableCache());
        ThrowIfDisposed();
        cancellationToken.ThrowIfCancellationRequested();
        try
        {
            var changes = await BatchUpdateEntityAsync(query, setter, cancellationToken);

            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(ErrorDescriber.ConcurrencyFailure());
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
        if (entity == null) return StoreResult.Failed(ErrorDescriber.NotFoundId(ConvertIdToString(id)));
        DeleteEntity(entity);
        RemoveCachedEntity(entity);
        return AttacheChange();
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
        RemoveCachedEntity(entity);
        return AttacheChange();
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
        var entityList = FindEntities(idList).ToList();
        if (!entityList.Any())
        {
            var idsString = string.Join(",", idList.Select(ConvertIdToString));
            return StoreResult.Failed(ErrorDescriber.NotFoundId(idsString));
        }

        DeleteEntities(entityList);
        RemoveCachedEntities(entityList);
        return AttacheChange();
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
        RemoveCachedEntities(list);
        return AttacheChange();
    }

    /// <summary>
    ///     在存储中删除已存在的实体
    /// </summary>
    /// <param name="id">被删除实体的主键</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> DeleteAsync(TKey id, CancellationToken cancellationToken = default)
    {
        SetDebugLog(nameof(DeleteAsync));
        OnAsyncActionExecuting(id, nameof(id), cancellationToken);
        var entity = await FindEntityAsync(id, cancellationToken);
        if (entity == null) return StoreResult.Failed(ErrorDescriber.NotFoundId(ConvertIdToString(id)));
        DeleteEntity(entity);
        RemoveCachedEntity(entity);
        return await AttacheChangeAsync(cancellationToken);
    }

    /// <summary>
    ///     在<paramref name="entity" />存储中删除已存在的实体
    /// </summary>
    /// <param name="entity">被删除实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public Task<StoreResult> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        SetDebugLog(nameof(DeleteAsync));
        OnAsyncActionExecuting(entity, nameof(entity), cancellationToken);
        DeleteEntity(entity);
        RemoveCachedEntity(entity);
        return AttacheChangeAsync(cancellationToken);
    }

    /// <summary>
    ///     在存储中删除已存在的实体
    /// </summary>
    /// <param name="ids">被删除实体的主键</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> DeleteAsync(IEnumerable<TKey> ids,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog(nameof(DeleteAsync));
        var idList = ids as List<TKey> ?? ids.ToList();
        OnAsyncActionExecuting(idList, nameof(ids), cancellationToken);
        var entityList = (await FindEntitiesAsync(idList, cancellationToken)).ToList();
        if (!entityList.Any())
        {
            var idsString = string.Join(",", idList.Select(ConvertIdToString));
            return StoreResult.Failed(ErrorDescriber.NotFoundId(idsString));
        }

        DeleteEntities(entityList);
        RemoveCachedEntities(entityList);
        return await AttacheChangeAsync(cancellationToken);
    }

    /// <summary>
    ///     在<paramref name="entities" />存储中删除已存在的实体
    /// </summary>
    /// <param name="entities">被删除实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public Task<StoreResult> DeleteAsync(IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog(nameof(DeleteAsync));
        var list = entities.ToList();
        OnAsyncActionExecuting(list, nameof(entities), cancellationToken);
        DeleteEntities(list);
        RemoveCachedEntities(list);
        return AttacheChangeAsync(cancellationToken);
    }

    #endregion

    #region BatchDeleteEntity & BatchDeleteEntities

    /// <summary>
    ///     在存储中删除已存在的实体
    /// </summary>
    /// <param name="id">被删除实体的主键</param>
    public StoreResult BatchDelete(TKey id)
    {
        if (CachedStore) return StoreResult.Failed(ErrorDescriber.EnableCache());
        OnActionExecuting(id, nameof(id));
        try
        {
            var query = Context.Set<TEntity>()
                .Where(item => item.Id.Equals(id));

            var changes = BatchDeleteEntity(query);

            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(ErrorDescriber.ConcurrencyFailure());
        }
    }

    /// <summary>
    ///     在存储中删除已存在的实体
    /// </summary>
    /// <param name="ids">被删除实体的主键</param>
    /// <returns></returns>
    public StoreResult BatchDelete(IEnumerable<TKey> ids)
    {
        if (CachedStore) return StoreResult.Failed(ErrorDescriber.EnableCache());
        var idList = ids as List<TKey> ?? ids.ToList();
        OnActionExecuting(idList, nameof(ids));
        try
        {
            var query = Context.Set<TEntity>()
                .Where(item => idList.Contains(item.Id));

            var changes = BatchDeleteEntity(query);

            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(ErrorDescriber.ConcurrencyFailure());
        }
    }

    /// <summary>
    ///     在存储中删除符合条件的实体
    /// </summary>
    /// <param name="predicate">查询表达式</param>
    /// <returns></returns>
    public StoreResult BatchDelete(Expression<Func<TEntity, bool>>? predicate)
    {
        if (CachedStore) return StoreResult.Failed(ErrorDescriber.EnableCache());
        ThrowIfDisposed();
        try
        {
            var query = Context.Set<TEntity>().AsNoTracking();

            if (predicate != null)
                query = query.Where(predicate);

            var changes = BatchDeleteEntity(query);

            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(ErrorDescriber.ConcurrencyFailure());
        }
    }

    /// <summary>
    ///     在存储中删除符合查询描述的实体
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public StoreResult BatchDelete(IQueryable<TEntity> query)
    {
        if (CachedStore) return StoreResult.Failed(ErrorDescriber.EnableCache());
        ThrowIfDisposed();
        try
        {
            var changes = BatchDeleteEntity(query);

            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(ErrorDescriber.ConcurrencyFailure());
        }
    }

    /// <summary>
    ///     在存储中删除已存在的实体
    /// </summary>
    /// <param name="id">被删除实体的主键</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> BatchDeleteAsync(TKey id, CancellationToken cancellationToken = default)
    {
        if (CachedStore) return StoreResult.Failed(ErrorDescriber.EnableCache());
        OnAsyncActionExecuting(id, nameof(id), cancellationToken);
        try
        {
            var query = Context.Set<TEntity>()
                .Where(item => item.Id.Equals(id));

            var changes = await BatchDeleteEntityAsync(query, cancellationToken);

            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(ErrorDescriber.ConcurrencyFailure());
        }
    }

    /// <summary>
    ///     在存储中删除已存在的实体
    /// </summary>
    /// <param name="ids">被删除实体的主键</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> BatchDeleteAsync(IEnumerable<TKey> ids,
        CancellationToken cancellationToken = default)
    {
        if (CachedStore) return StoreResult.Failed(ErrorDescriber.EnableCache());
        var idList = ids as List<TKey> ?? ids.ToList();
        OnAsyncActionExecuting(idList, nameof(ids), cancellationToken);
        try
        {
            var query = Context.Set<TEntity>()
                .Where(item => idList.Contains(item.Id));

            var changes = await BatchDeleteEntityAsync(query, cancellationToken);

            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(ErrorDescriber.ConcurrencyFailure());
        }
    }

    /// <summary>
    ///     在存储中删除符合条件的实体
    /// </summary>
    /// <param name="predicate">查询表达式</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> BatchDeleteAsync(Expression<Func<TEntity, bool>>? predicate,
        CancellationToken cancellationToken = default)
    {
        if (CachedStore) return StoreResult.Failed(ErrorDescriber.EnableCache());
        ThrowIfDisposed();
        cancellationToken.ThrowIfCancellationRequested();
        try
        {
            var query = Context.Set<TEntity>().AsNoTracking();

            if (predicate != null)
                query =
                    query.Where(predicate);

            var changes = await BatchDeleteEntityAsync(query, cancellationToken);

            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(ErrorDescriber.ConcurrencyFailure());
        }
    }

    /// <summary>
    ///     在存储中删除符合查询描述的实体
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> BatchDeleteAsync(IQueryable<TEntity> query,
        CancellationToken cancellationToken = default)
    {
        if (CachedStore) return StoreResult.Failed(ErrorDescriber.EnableCache());
        ThrowIfDisposed();
        cancellationToken.ThrowIfCancellationRequested();
        try
        {
            var changes = await BatchDeleteEntityAsync(query, cancellationToken);

            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(ErrorDescriber.ConcurrencyFailure());
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
    ///     根据缓存键查找实体
    /// </summary>
    /// <param name="keys">缓存键</param>
    /// <returns></returns>
    public IEnumerable<TEntity> FindEntitiesViaKeys(IEnumerable<string> keys)
    {
        var keyList = keys.ToList();
        OnActionExecuting(keyList, nameof(keys));
        return GetEntities(keyList);
    }

    /// <summary>
    ///     根据Id查找实体
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public Task<TEntity?> FindEntityAsync(TKey id, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(id, nameof(id), cancellationToken);
        return FindByIdAsync(id, cancellationToken);
    }

    /// <summary>
    ///     根据Id查找实体
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public Task<IEnumerable<TEntity>> FindEntitiesAsync(IEnumerable<TKey> ids,
        CancellationToken cancellationToken = default)
    {
        var idArray = ids as TKey[] ?? ids.ToArray();
        OnAsyncActionExecuting(idArray, nameof(ids), cancellationToken);
        return FindByIdsAsync(idArray, cancellationToken);
    }

    #endregion

    #region Exists

    /// <summary>
    /// 判断实体是否存在
    /// </summary>
    /// <param name="id">实体键</param>
    /// <returns></returns>
    public bool Exists(TKey id)
    {
        OnActionExecuting(id, nameof(id));
        return Context.Set<TEntity>()
            .Any(entity => entity.Id.Equals(id));
    }

    /// <summary>
    /// 判断实体是否存在
    /// </summary>
    /// <param name="id">实体键</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default)
    {
        OnActionExecuting(id, nameof(id));
        return Context.Set<TEntity>()
            .AnyAsync(entity => entity.Id.Equals(id), cancellationToken: cancellationToken);
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
    public StoreResult CreateNew<TSource>(TSource source, TypeAdapterConfig? config = null)
    {
        SetDebugLog(nameof(CreateNew));
        OnActionExecuting(source, nameof(source));
        config ??= IgnoreIdConfig<TSource>();
        var entity = source!.Adapt<TSource, TEntity>(config);
        if (entity == null) throw new MapTargetNullException(nameof(entity));
        AddEntity(entity);
        var changes = SaveChanges();
        CacheEntity(entity);
        return StoreResult.Success(changes);
    }

    /// <summary>
    ///     通过类型映射创建一组新实例
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="config">映射配置</param>
    /// <returns>创建结果</returns>
    public StoreResult CreateNew<TSource>(IEnumerable<TSource> sources, TypeAdapterConfig? config = null)
    {
        SetDebugLog(nameof(CreateNew));
        OnActionExecuting(sources, nameof(sources));
        config ??= IgnoreIdConfig<TSource>();
        var entities = sources.Adapt<IEnumerable<TEntity>>(config);
        if (entities == null) throw new MapTargetNullException(nameof(entities));
        var list = entities.ToList();
        AddEntities(list);
        var changes = SaveChanges();
        CacheEntities(list);
        return StoreResult.Success(changes);
    }

    /// <summary>
    ///     通过类型映射创建一个新实例
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns>创建结果</returns>
    public async Task<StoreResult> CreateNewAsync<TSource>(TSource source, TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog(nameof(CreateNewAsync));
        OnAsyncActionExecuting(source, nameof(source), cancellationToken);
        config ??= IgnoreIdConfig<TSource>();
        var entity = source!.Adapt<TSource, TEntity>(config);
        if (entity == null) throw new MapTargetNullException(nameof(entity));
        AddEntity(entity);
        var changes = await SaveChangesAsync(cancellationToken);
        CacheEntity(entity);
        return StoreResult.Success(changes);
    }

    /// <summary>
    ///     通过类型映射创建一个新实例
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns>创建结果</returns>
    public async Task<StoreResult> CreateNewAsync<TSource>(IEnumerable<TSource> sources,
        TypeAdapterConfig? config = null, CancellationToken cancellationToken = default)
    {
        SetDebugLog(nameof(CreateNewAsync));
        OnAsyncActionExecuting(sources, nameof(sources), cancellationToken);
        config ??= IgnoreIdConfig<TSource>();
        var entities = sources.Adapt<IEnumerable<TEntity>>(config);
        if (entities == null) throw new MapTargetNullException(nameof(entities));
        var list = entities.ToList();
        AddEntities(list);
        var changes = await SaveChangesAsync(cancellationToken);
        CacheEntities(list);
        return StoreResult.Success(changes);
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
    public StoreResult Over<TSource>(TSource source, TypeAdapterConfig? config = null) where TSource : IKeySlot<TKey>
    {
        SetDebugLog(nameof(Over));
        OnActionExecuting(source, nameof(source));
        config ??= IgnoreMetaConfig<TSource>();
        var entity = source.Adapt<TSource, TEntity>(config);
        if (entity == null) throw new MapTargetNullException(nameof(entity));
        UpdateEntity(entity);
        CacheEntity(entity);
        return AttacheChange();
    }

    /// <summary>
    ///     通过类型映射覆盖对应实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="destination">目标数据</param>
    /// <param name="config">映射配置</param>
    /// <returns></returns>
    public StoreResult Over<TSource>(TSource source, TEntity destination, TypeAdapterConfig? config = null)
    {
        SetDebugLog(nameof(Over));
        OnActionExecuting(source, nameof(source));
        config ??= IgnoreIdConfig<TSource>();
        source.Adapt(destination, config);
        if (destination == null) throw new MapTargetNullException(nameof(destination));
        UpdateEntity(destination);
        CacheEntity(destination);
        return AttacheChange();
    }

    /// <summary>
    ///     通过类型映射覆盖对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="config">映射配置</param>
    /// <returns></returns>
    public StoreResult Over<TSource>(IEnumerable<TSource> sources, TypeAdapterConfig? config = null)
        where TSource : IKeySlot<TKey>
    {
        SetDebugLog(nameof(Over));
        OnActionExecuting(sources, nameof(sources));
        config ??= IgnoreMetaConfig<TSource>();
        var entities = sources.Adapt<IEnumerable<TEntity>>(config);
        if (entities == null) throw new MapTargetNullException(nameof(entities));
        var list = entities.ToList();
        UpdateEntities(list);
        CacheEntities(list);
        return AttacheChange();
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
        var entities = sourceList.Join(destinations, sourceKeySelector, destinationKeySelector,
            (source, _) => source.Adapt<TSource, TEntity>(config));
        if (entities == null) throw new MapTargetNullException(nameof(entities));
        var list = entities.ToList();
        UpdateEntities(list);
        CacheEntities(list);
        return AttacheChange();
    }

    /// <summary>
    ///     通过类型映射覆盖对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public Task<StoreResult> OverAsync<TSource>(TSource source, TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default) where TSource : IKeySlot<TKey>
    {
        SetDebugLog(nameof(OverAsync));
        OnAsyncActionExecuting(source, nameof(source), cancellationToken);
        config ??= IgnoreMetaConfig<TSource>();
        var entity = source.Adapt<TSource, TEntity>(config);
        if (entity == null) throw new MapTargetNullException(nameof(entity));
        UpdateEntity(entity);
        CacheEntity(entity);
        return AttacheChangeAsync(cancellationToken);
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
    public Task<StoreResult> OverAsync<TSource>(TSource source, TEntity destination, TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog(nameof(OverAsync));
        OnAsyncActionExecuting(source, nameof(source), cancellationToken);
        config ??= IgnoreIdConfig<TSource>();
        source.Adapt(destination, config);
        if (destination == null) throw new MapTargetNullException(nameof(destination));
        UpdateEntity(destination);
        CacheEntity(destination);
        return AttacheChangeAsync(cancellationToken);
    }

    /// <summary>
    ///     通过类型映射覆盖对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public Task<StoreResult> OverAsync<TSource>(IEnumerable<TSource> sources, TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default) where TSource : IKeySlot<TKey>
    {
        SetDebugLog(nameof(OverAsync));
        OnAsyncActionExecuting(sources, nameof(sources), cancellationToken);
        config ??= IgnoreMetaConfig<TSource>();
        var entities = sources.Adapt<IEnumerable<TEntity>>(config);
        if (entities == null) throw new MapTargetNullException(nameof(entities));
        var list = entities.ToList();
        UpdateEntities(list);
        CacheEntities(list);
        return AttacheChangeAsync(cancellationToken);
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
    public Task<StoreResult> OverAsync<TSource, TJKey>(
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
        var entities = sourceList.Join(destinations, sourceKeySelector, destinationKeySelector,
            (source, _) => source.Adapt<TSource, TEntity>(config));
        if (entities == null) throw new MapTargetNullException(nameof(entities));
        var list = entities.ToList();
        UpdateEntities(list);
        CacheEntities(list);
        return AttacheChangeAsync(cancellationToken);
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
    public StoreResult Merge<TSource>(TSource source, TypeAdapterConfig? config = null) where TSource : IKeySlot<TKey>
    {
        SetDebugLog(nameof(Merge));
        OnActionExecuting(source, nameof(source));
        var entity = FindById(source.Id);
        if (entity == null) throw new MapTargetNullException(nameof(entity));
        config ??= IgnoreNullConfig<TSource>();
        source.Adapt(entity, config);
        UpdateEntity(entity);
        CacheEntity(entity);
        return AttacheChange();
    }

    /// <summary>
    ///     通过类型映射合并对应实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="destination">目标数据</param>
    /// <param name="config">映射配置</param>
    /// <returns></returns>
    public StoreResult Merge<TSource>(TSource source, TEntity destination, TypeAdapterConfig? config = null)
    {
        SetDebugLog(nameof(Merge));
        OnActionExecuting(source, nameof(source));
        config ??= IgnoreIdAndNullConfig<TSource>();
        source.Adapt(destination, config);
        if (destination == null) throw new MapTargetNullException(nameof(destination));
        UpdateEntity(destination);
        CacheEntity(destination);
        return AttacheChange();
    }

    /// <summary>
    ///     通过类型映射合并对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="config">映射配置</param>
    /// <returns></returns>
    public StoreResult Merge<TSource>(IEnumerable<TSource> sources, TypeAdapterConfig? config = null)
        where TSource : IKeySlot<TKey>
    {
        SetDebugLog(nameof(Merge));
        var sourceList = sources.ToList();
        OnActionExecuting(sourceList, nameof(sources));
        var destinations = FindByIds(sourceList.Select(source => source.Id));
        if (destinations == null) throw new MapTargetNullException(nameof(destinations));
        config ??= IgnoreNullConfig<TSource>();
        var entities = sourceList.Join(destinations, source => source.Id, destination => destination.Id,
            (source, destination) => source.Adapt(destination, config)).ToList();
        UpdateEntities(entities);
        CacheEntities(entities);
        return AttacheChange();
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
    public StoreResult Merge<TSource, TJKey>(IEnumerable<TSource> sources, IEnumerable<TEntity> destinations,
        Func<TSource, TJKey> sourceKeySelector,
        Func<TEntity, TJKey> destinationKeySelector, TypeAdapterConfig? config = null)
    {
        SetDebugLog(nameof(Merge));
        var sourceList = sources.ToList();
        OnActionExecuting(sourceList, nameof(sources));
        config ??= IgnoreIdAndNullConfig<TSource>();
        var entities = sourceList.Join(destinations, sourceKeySelector, destinationKeySelector,
            (source, _) => source.Adapt<TSource, TEntity>(config));
        if (entities == null) throw new MapTargetNullException(nameof(entities));
        var list = entities.ToList();
        UpdateEntities(list);
        CacheEntities(list);
        return AttacheChange();
    }

    /// <summary>
    ///     通过类型映射合并对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> MergeAsync<TSource>(TSource source, TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default) where TSource : IKeySlot<TKey>
    {
        SetDebugLog(nameof(MergeAsync));
        OnAsyncActionExecuting(source, nameof(source), cancellationToken);
        var entity = await FindByIdAsync(source.Id, cancellationToken);
        if (entity == null) throw new MapTargetNullException(nameof(entity));
        config ??= IgnoreNullConfig<TSource>();
        source.Adapt(entity, config);
        UpdateEntity(entity);
        CacheEntity(entity);
        return await AttacheChangeAsync(cancellationToken);
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
    public Task<StoreResult> MergeAsync<TSource>(TSource source, TEntity destination, TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog(nameof(MergeAsync));
        OnAsyncActionExecuting(source, nameof(source), cancellationToken);
        config ??= IgnoreIdAndNullConfig<TSource>();
        source.Adapt(destination, config);
        if (destination == null) throw new MapTargetNullException(nameof(destination));
        UpdateEntity(destination);
        CacheEntity(destination);
        return AttacheChangeAsync(cancellationToken);
    }

    /// <summary>
    ///     通过类型映射合并对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> MergeAsync<TSource>(IEnumerable<TSource> sources, TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default) where TSource : IKeySlot<TKey>
    {
        SetDebugLog(nameof(MergeAsync));
        var sourceList = sources.ToList();
        OnAsyncActionExecuting(sourceList, nameof(sources), cancellationToken);
        var destinations = await FindByIdsAsync(sourceList.Select(source => source.Id), cancellationToken);
        if (destinations == null) throw new MapTargetNullException(nameof(destinations));
        config ??= IgnoreNullConfig<TSource>();
        var entities = sourceList.Join(destinations, source => source.Id, destination => destination.Id,
            (source, destination) => source.Adapt(destination, config)).ToList();
        UpdateEntities(entities);
        CacheEntities(entities);
        return await AttacheChangeAsync(cancellationToken);
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
    public Task<StoreResult> MergeAsync<TSource, TJKey>(IEnumerable<TSource> sources, IEnumerable<TEntity> destinations,
        Func<TSource, TJKey> sourceKeySelector,
        Func<TEntity, TJKey> destinationKeySelector, TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog(nameof(MergeAsync));
        var sourceList = sources.ToList();
        OnAsyncActionExecuting(sourceList, nameof(sources), cancellationToken);
        config ??= IgnoreIdAndNullConfig<TSource>();
        var entities = sourceList.Join(destinations, sourceKeySelector, destinationKeySelector,
            (source, _) => source.Adapt<TSource, TEntity>(config));
        if (entities == null) throw new MapTargetNullException(nameof(entities));
        var list = entities.ToList();
        UpdateEntities(list);
        CacheEntities(list);
        return AttacheChangeAsync(cancellationToken);
    }

    #endregion

    #region MapConfig

    /// <summary>
    ///     id和空值忽略缓存
    /// </summary>
    private TypeAdapterConfig? _ignoreIdAndNullConfig;

    /// <summary>
    ///     空值或空字符值忽略缓存
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public TypeAdapterConfig IgnoreIdAndNullConfig<TSource>()
    {
        if (_ignoreIdAndNullConfig != null) return _ignoreIdAndNullConfig;
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
    ///     空值或空字符值忽略缓存
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public TypeAdapterConfig IgnoreNullConfig<TSource>()
    {
        if (_ignoreNullConfig != null) return _ignoreNullConfig;
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
    ///     Id忽略访问器
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public TypeAdapterConfig IgnoreIdConfig<TSource>()
    {
        if (_ignoreIdConfig != null) return _ignoreIdConfig;
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
    ///     元数据忽略访问器
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public TypeAdapterConfig IgnoreMetaConfig<TSource>()
    {
        if (_ignoreMetaConfig != null) return _ignoreMetaConfig;
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
        if (MetaDataHosting)
        {
            var now = DateTime.Now;
            entity.UpdatedAt = now;
        }

        Context.Update(entity);
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
    }

    /// <summary>
    ///     批量删除实体
    /// </summary>
    /// <param name="query">查询</param>
    /// <param name="setter">更新委托</param>
    /// <returns></returns>
    private int BatchUpdateEntity(IQueryable<TEntity> query,
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
    private Task<int> BatchUpdateEntityAsync(IQueryable<TEntity> query,
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
    private Task<int> BatchDeleteEntityAsync(IQueryable<TEntity> query, CancellationToken cancellationToken = default)
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
        return EntityQuery.FirstOrDefault(entity => entity.Id.Equals(id));
    }

    /// <summary>
    ///     根据Id查询实体
    /// </summary>
    /// <param name="ids">id</param>
    /// <returns></returns>
    private IEnumerable<TEntity> FindByIds(IEnumerable<TKey> ids)
    {
        return EntityQuery.Where(entity => ids.Contains(entity.Id)).ToList();
    }

    /// <summary>
    ///     根据Id查找实体
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    private Task<TEntity?> FindByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return EntityQuery.FirstOrDefaultAsync(entity => entity.Id.Equals(id), cancellationToken);
    }

    /// <summary>
    ///     根据Id查找实体
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    private async Task<IEnumerable<TEntity>> FindByIdsAsync(IEnumerable<TKey> ids,
        CancellationToken cancellationToken = default)
    {
        return await EntityQuery.Where(entity => ids.Contains(entity.Id)).ToListAsync(cancellationToken);
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
            return StoreResult.Failed(ErrorDescriber.ConcurrencyFailure());
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
            return StoreResult.Failed(ErrorDescriber.ConcurrencyFailure());
        }
    }

    #endregion
}