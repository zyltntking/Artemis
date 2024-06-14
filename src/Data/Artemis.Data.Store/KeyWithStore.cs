using System.Linq.Expressions;
using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Artemis.Data.Store;

/// <summary>
///     具键模型存储基类
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class KeyWithStore<TEntity> :
    KeyWithStore<TEntity, Guid>,
    IStore<TEntity>
    where TEntity : class, IKeySlot
{
    /// <summary>
    ///     具键模型存储构造
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cache"></param>
    /// <param name="storeOptions"></param>
    /// <param name="logger"></param>
    /// <param name="describer"></param>
    protected KeyWithStore(
        DbContext context,
        IDistributedCache? cache = null,
        IKeyWithStoreOptions? storeOptions = null,
        ILogger? logger = null,
        StoreErrorDescriber? describer = null) : base(context, cache, storeOptions, logger, describer)
    {
    }

    #region Overrides of StoreBase<TEntity,Guid>

    /// <summary>
    ///     转换字符串到id
    /// </summary>
    /// <param name="id">id字符串</param>
    /// <returns>id</returns>
    protected sealed override Guid ConvertIdFromString(string id)
    {
        return id.GuidFromString();
    }

    /// <summary>
    ///     转换Id为字符串
    /// </summary>
    /// <param name="id">id</param>
    /// <returns>字符串</returns>
    protected sealed override string ConvertIdToString(Guid id)
    {
        return id.GuidToString();
    }

    #endregion
}

/// <summary>
///     具键模型存储基类
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
public abstract class KeyWithStore<TEntity, TKey> :
    StoreBase<TEntity, TKey>,
    IStore<TEntity, TKey>
    where TEntity : class, IKeySlot<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     具键模型存储构造
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cache"></param>
    /// <param name="storeOptions"></param>
    /// <param name="logger"></param>
    /// <param name="describer"></param>
    protected KeyWithStore(
        DbContext context,
        IDistributedCache? cache = null,
        IKeyWithStoreOptions? storeOptions = null,
        ILogger? logger = null,
        StoreErrorDescriber? describer = null) : base(context, cache, storeOptions, logger, describer)
    {
    }

    #region Implementation of IStore<TEntity,TKey>

    /// <summary>
    ///     注册操作员
    /// </summary>
    public abstract Func<TKey>? HandlerRegister { get; set; }

    #endregion

    #region Implementation of IKeyWithStoreCommon<TEntity,in TKey>

    #region CreateEntity & CreateEntities

    /// <summary>
    ///     在<paramref name="entity" />存储中创建一个新的实体
    /// </summary>
    /// <param name="entity">被创建实体</param>
    /// <returns></returns>
    public StoreResult Create(TEntity entity)
    {
        OnActionExecuting(entity, nameof(entity));

        AddEntity(entity);

        var result = AttacheChange();

        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Create {typeof(TEntity).Name}");

            CacheEntity(entity);
        }

        return result;
    }

    /// <summary>
    ///     在<paramref name="entities" />存储中创建多个新的实体
    /// </summary>
    /// <param name="entities">被创建实体</param>
    /// <returns></returns>
    public StoreResult Create(IEnumerable<TEntity> entities)
    {
        var list = entities.ToList();

        OnActionExecuting(list, nameof(entities));

        AddEntities(list);

        var result = AttacheChange();

        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Create {result.EffectRows} {typeof(TEntity).Name} Entities");

            CacheEntities(list);
        }

        return result;
    }

    /// <summary>
    ///     在<paramref name="entity" />存储中创建一个新的实体
    /// </summary>
    /// <param name="entity">被创建实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> CreateAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(entity, nameof(entity), cancellationToken);

        AddEntity(entity);

        var result = await AttacheChangeAsync(cancellationToken);

        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Create {typeof(TEntity).Name}");

            await CacheEntityAsync(entity, cancellationToken);
        }

        return result;
    }

    /// <summary>
    ///     在<paramref name="entities" />存储中创建多个新的实体
    /// </summary>
    /// <param name="entities">被创建实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> CreateAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        var list = entities.ToList();

        OnAsyncActionExecuting(list, nameof(entities), cancellationToken);

        AddEntities(list);

        var result = await AttacheChangeAsync(cancellationToken);

        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Create {result.EffectRows} {typeof(TEntity).Name} Entities");

            await CacheEntitiesAsync(list, cancellationToken);
        }

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
        OnActionExecuting(entity, nameof(entity));

        UpdateEntity(entity);

        var result = AttacheChange();

        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Update {typeof(TEntity).Name}");

            CacheEntity(entity);
        }

        return result;
    }

    /// <summary>
    ///     在<paramref name="entities" />存储中更新多个已存在的实体
    /// </summary>
    /// <param name="entities">被创建实体</param>
    /// <returns></returns>
    public StoreResult Update(IEnumerable<TEntity> entities)
    {
        var list = entities.ToList();

        OnActionExecuting(list, nameof(entities));

        UpdateEntities(list);

        var result = AttacheChange();

        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Update {result.EffectRows} {typeof(TEntity).Name} Entities");

            CacheEntities(list);
        }

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
        OnAsyncActionExecuting(entity, nameof(entity), cancellationToken);

        UpdateEntity(entity);

        var result = await AttacheChangeAsync(cancellationToken);

        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Update {typeof(TEntity).Name}");

            await CacheEntityAsync(entity, cancellationToken);
        }

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
        var list = entities.ToList();

        OnAsyncActionExecuting(list, nameof(entities), cancellationToken);

        UpdateEntities(list);

        var result = await AttacheChangeAsync(cancellationToken);

        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Update {list.Count} {typeof(TEntity).Name} Entities");

            await CacheEntitiesAsync(list, cancellationToken);
        }

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
        OnActionExecuting(id, nameof(id));

        var entity = FindEntity(id);

        if (entity is null)
            return StoreResult.Failed(Describer.NotFoundId(ConvertIdToString(id)));

        DeleteEntity(entity);

        var result = AttacheChange();

        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Delete {typeof(TEntity).Name}");

            RemoveCachedEntity(entity);
        }

        return result;
    }

    /// <summary>
    ///     在<paramref name="entity" />存储中删除已存在的实体
    /// </summary>
    /// <param name="entity">被删除实体</param>
    /// <returns></returns>
    public StoreResult Delete(TEntity entity)
    {
        OnActionExecuting(entity, nameof(entity));

        DeleteEntity(entity);

        var result = AttacheChange();

        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Delete {typeof(TEntity).Name}");

            RemoveCachedEntity(entity);
        }

        return result;
    }

    /// <summary>
    ///     在存储中删除已存在的实体
    /// </summary>
    /// <param name="ids">被删除实体的主键</param>
    /// <returns></returns>
    public StoreResult Delete(IEnumerable<TKey> ids)
    {
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

        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Delete {result.EffectRows} {typeof(TEntity).Name} Entities");

            RemoveCachedEntities(list);
        }

        return result;
    }

    /// <summary>
    ///     在<paramref name="entities" />存储中删除已存在的实体
    /// </summary>
    /// <param name="entities">被删除实体</param>
    /// <returns></returns>
    public StoreResult Delete(IEnumerable<TEntity> entities)
    {
        var list = entities.ToList();

        OnActionExecuting(list, nameof(entities));

        DeleteEntities(list);

        var result = AttacheChange();

        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Delete {result.EffectRows} {typeof(TEntity).Name} Entities");

            RemoveCachedEntities(list);
        }

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
        OnAsyncActionExecuting(id, nameof(id), cancellationToken);

        var entity = await FindEntityAsync(id, cancellationToken);

        if (entity is null)
            return StoreResult.Failed(Describer.NotFoundId(ConvertIdToString(id)));

        DeleteEntity(entity);

        var result = await AttacheChangeAsync(cancellationToken);

        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Delete {typeof(TEntity).Name}");

            await RemoveCachedEntityAsync(entity, cancellationToken);
        }

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
        OnAsyncActionExecuting(entity, nameof(entity), cancellationToken);

        DeleteEntity(entity);

        var result = await AttacheChangeAsync(cancellationToken);

        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Delete {typeof(TEntity).Name}");

            await RemoveCachedEntityAsync(entity, cancellationToken);
        }

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

        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Delete {result.EffectRows} {typeof(TEntity).Name} Entities");

            await RemoveCachedEntitiesAsync(list, cancellationToken);
        }

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
        var list = entities.ToList();

        OnAsyncActionExecuting(list, nameof(entities), cancellationToken);

        DeleteEntities(list);

        var result = await AttacheChangeAsync(cancellationToken);

        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Delete {result.EffectRows} {typeof(TEntity).Name} Entities");

            await RemoveCachedEntitiesAsync(list, cancellationToken);
        }

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

            if (changes > 0) RemoveCachedEntity(id);

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

            if (changes > 0) RemoveCachedEntities(idList);

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

            if (changes > 0) await RemoveCachedEntityAsync(id, cancellationToken);

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

            if (changes > 0) await RemoveCachedEntitiesAsync(idList, cancellationToken);

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

        var cached = GetEntity(id);

        if (cached is not null) return cached;

        var entity = FindById(id);

        if (entity is not null) CacheEntity(entity);

        return entity;
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

        var cached = GetEntities(idArray);

        if (cached is not null)
        {
            var list = cached.ToList();

            if (list.Any()) return list;
        }

        var entities = FindByIds(idArray).ToList();

        if (entities.Any()) CacheEntities(entities);

        return entities;
    }

    /// <summary>
    ///     根据Id查找实体
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<TEntity?> FindEntityAsync(
        TKey id,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(id, nameof(id), cancellationToken);

        var cached = await GetEntityAsync(id, cancellationToken);

        if (cached is not null) return cached;

        var entity = await FindByIdAsync(id, cancellationToken);

        if (entity is not null) await CacheEntityAsync(entity, cancellationToken);

        return entity;
    }

    /// <summary>
    ///     根据Id查找实体
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<IEnumerable<TEntity>> FindEntitiesAsync(
        IEnumerable<TKey> ids,
        CancellationToken cancellationToken = default)
    {
        var idArray = ids as TKey[] ?? ids.ToArray();
        OnAsyncActionExecuting(idArray, nameof(ids), cancellationToken);

        var cached = await GetEntitiesAsync(idArray, cancellationToken);

        if (cached is not null)
        {
            var list = cached.ToList();

            if (list.Any()) return list;
        }

        var entities = await FindByIdsAsync(idArray, cancellationToken);

        if (entities.Any()) await CacheEntitiesAsync(entities, cancellationToken);

        return entities;
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
        OnActionExecuting(id, nameof(id));
        return KeyMatchQuery(id).AnyAsync(cancellationToken);
    }

    #endregion

    #endregion

    #region Implementation of IKeyLessStoreMap<TEntity>

    #region FindMapEntity & FindMapEntities

    /// <summary>
    ///     根据id查找映射实体
    /// </summary>
    /// <typeparam name="TMapEntity">映射类型</typeparam>
    /// <param name="id">id</param>
    /// <returns></returns>
    public TMapEntity? FindMapEntity<TMapEntity>(TKey id)
    {
        OnActionExecuting(id, nameof(id));

        var cached = GetEntity(id);

        if (cached is not null)
            return cached.Adapt<TMapEntity>();

        return FindById<TMapEntity>(id);
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

        var cached = GetEntities(idArray);

        if (cached is not null)
        {
            var list = cached.ToList();

            if (list.Any()) return list.Adapt<IEnumerable<TMapEntity>>();
        }

        return FindByIds<TMapEntity>(idArray);
    }

    /// <summary>
    ///     根据Id查找映射实体
    /// </summary>
    /// <typeparam name="TMapEntity">映射类型</typeparam>
    /// <param name="id"></param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<TMapEntity?> FindMapEntityAsync<TMapEntity>(
        TKey id,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(id, nameof(id), cancellationToken);

        var cached = await GetEntityAsync(id, cancellationToken);

        if (cached is not null)
            return cached.Adapt<TMapEntity>();

        return await FindByIdAsync<TMapEntity>(id, cancellationToken);
    }

    /// <summary>
    ///     根据Id查找映射实体
    /// </summary>
    /// <typeparam name="TMapEntity">映射类型</typeparam>
    /// <param name="ids"></param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<IEnumerable<TMapEntity>> FindMapEntitiesAsync<TMapEntity>(
        IEnumerable<TKey> ids,
        CancellationToken cancellationToken = default)
    {
        var idArray = ids as TKey[] ?? ids.ToArray();
        OnAsyncActionExecuting(idArray, nameof(ids), cancellationToken);

        var cached = await GetEntitiesAsync(idArray, cancellationToken);

        if (cached is not null)
        {
            var list = cached.ToList();

            if (list.Any()) return list.Adapt<List<TMapEntity>>();
        }

        return await FindByIdsAsync<TMapEntity>(idArray, cancellationToken);
    }

    #endregion

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
        OnActionExecuting(source, nameof(source));
        config ??= CreateNewConfig<TSource>();
        var entity = source!.Adapt<TSource, TEntity>(config);
        if (entity is null)
            throw new MapTargetNullException(nameof(entity));
        AddEntity(entity);
        var result = AttacheChange();
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"CreateNew {typeof(TEntity).Name}");

            CacheEntity(entity);
        }

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
        OnActionExecuting(sources, nameof(sources));
        config ??= CreateNewConfig<TSource>();
        var entities = sources.Adapt<IEnumerable<TEntity>>(config);
        if (entities is null)
            throw new MapTargetNullException(nameof(entities));
        var list = entities.ToList();
        AddEntities(list);
        var result = AttacheChange();
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"CreateNew {result.EffectRows} {typeof(TEntity).Name} Entities");

            CacheEntities(list);
        }

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
        OnAsyncActionExecuting(source, nameof(source), cancellationToken);
        config ??= CreateNewConfig<TSource>();
        var entity = source!.Adapt<TSource, TEntity>(config);
        if (entity is null)
            throw new MapTargetNullException(nameof(entity));
        AddEntity(entity);
        var result = await AttacheChangeAsync(cancellationToken);
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"CreateNew {typeof(TEntity).Name}");

            await CacheEntityAsync(entity, cancellationToken);
        }

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
        OnAsyncActionExecuting(sources, nameof(sources), cancellationToken);
        config ??= CreateNewConfig<TSource>();
        var entities = sources.Adapt<IEnumerable<TEntity>>(config);
        if (entities is null)
            throw new MapTargetNullException(nameof(entities));
        var list = entities.ToList();
        AddEntities(list);
        var result = await AttacheChangeAsync(cancellationToken);
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"CreateNew {result.EffectRows} {typeof(TEntity).Name} Entities");

            await CacheEntitiesAsync(list, cancellationToken);
        }

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
        OnActionExecuting(source, nameof(source));
        config ??= OverConfig<TSource>(false);
        var entity = source.Adapt<TSource, TEntity>(config);
        if (entity is null)
            throw new MapTargetNullException(nameof(entity));
        UpdateEntity(entity);
        var result = AttacheChange();
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Over {typeof(TEntity).Name}");

            CacheEntity(entity);
        }

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
        OnActionExecuting(source, nameof(source));
        config ??= OverConfig<TSource>(true);
        source.Adapt(destination, config);
        if (destination is null)
            throw new MapTargetNullException(nameof(destination));
        UpdateEntity(destination);
        var result = AttacheChange();
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Over {typeof(TEntity).Name}");

            CacheEntity(destination);
        }

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
        OnActionExecuting(sources, nameof(sources));
        config ??= OverConfig<TSource>(false);
        var entities = sources.Adapt<IEnumerable<TEntity>>(config);
        if (entities is null)
            throw new MapTargetNullException(nameof(entities));
        var list = entities.ToList();
        UpdateEntities(list);
        var result = AttacheChange();
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Over {result.EffectRows} {typeof(TEntity).Name} Entities");

            CacheEntities(list);
        }

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
        var sourceList = sources.ToList();
        OnActionExecuting(sourceList, nameof(sources));
        config ??= OverConfig<TSource>(true);
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
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Over {result.EffectRows} {typeof(TEntity).Name} Entities");

            CacheEntities(list);
        }

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
        OnAsyncActionExecuting(source, nameof(source), cancellationToken);
        config ??= OverConfig<TSource>(false);
        var entity = source.Adapt<TSource, TEntity>(config);
        if (entity is null)
            throw new MapTargetNullException(nameof(entity));
        UpdateEntity(entity);
        var result = await AttacheChangeAsync(cancellationToken);
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Over {typeof(TEntity).Name}");

            await CacheEntityAsync(entity, cancellationToken);
        }

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
        OnAsyncActionExecuting(source, nameof(source), cancellationToken);
        config ??= OverConfig<TSource>(true);
        source.Adapt(destination, config);
        if (destination is null)
            throw new MapTargetNullException(nameof(destination));
        UpdateEntity(destination);
        var result = await AttacheChangeAsync(cancellationToken);
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Over {typeof(TEntity).Name}");

            await CacheEntityAsync(destination, cancellationToken);
        }

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
        OnAsyncActionExecuting(sources, nameof(sources), cancellationToken);
        config ??= OverConfig<TSource>(false);
        var entities = sources.Adapt<IEnumerable<TEntity>>(config);
        if (entities is null)
            throw new MapTargetNullException(nameof(entities));
        var list = entities.ToList();
        UpdateEntities(list);
        var result = await AttacheChangeAsync(cancellationToken);
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Over {result.EffectRows} {typeof(TEntity).Name} Entities");

            await CacheEntitiesAsync(list, cancellationToken);
        }

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
        var sourceList = sources.ToList();
        OnAsyncActionExecuting(sourceList, nameof(sources), cancellationToken);
        config ??= OverConfig<TSource>(true);
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
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Over {result.EffectRows} {typeof(TEntity).Name} Entities");

            await CacheEntitiesAsync(list, cancellationToken);
        }

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
        OnActionExecuting(source, nameof(source));
        var entity = FindById(source.Id);
        if (entity is null)
            throw new MapTargetNullException(nameof(entity));
        config ??= MergeConfig<TSource>(false);
        source.Adapt(entity, config);
        UpdateEntity(entity);
        var result = AttacheChange();
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Merge {typeof(TEntity).Name}");

            CacheEntity(entity);
        }

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
        OnActionExecuting(source, nameof(source));
        config ??= MergeConfig<TSource>(true);
        source.Adapt(destination, config);
        if (destination is null)
            throw new MapTargetNullException(nameof(destination));
        UpdateEntity(destination);
        var result = AttacheChange();
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Merge {typeof(TEntity).Name}");

            CacheEntity(destination);
        }

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
        var sourceList = sources.ToList();
        OnActionExecuting(sourceList, nameof(sources));
        var destinations = FindByIds(sourceList.Select(source => source.Id));
        if (destinations is null)
            throw new MapTargetNullException(nameof(destinations));
        config ??= MergeConfig<TSource>(false);
        var list = sourceList.Join(
            destinations, source => source.Id,
            destination => destination.Id,
            (source, destination) => source.Adapt(destination, config)).ToList();
        UpdateEntities(list);
        var result = AttacheChange();
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Merge {result.EffectRows} {typeof(TEntity).Name} Entities");

            CacheEntities(list);
        }

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
        var sourceList = sources.ToList();
        OnActionExecuting(sourceList, nameof(sources));
        config ??= MergeConfig<TSource>(true);
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
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Merge {result.EffectRows} {typeof(TEntity).Name} Entities");

            CacheEntities(list);
        }

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
        OnAsyncActionExecuting(source, nameof(source), cancellationToken);
        var entity = await FindByIdAsync(source.Id, cancellationToken);
        if (entity is null)
            throw new MapTargetNullException(nameof(entity));
        config ??= MergeConfig<TSource>(false);
        source.Adapt(entity, config);
        UpdateEntity(entity);
        var result = await AttacheChangeAsync(cancellationToken);
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Merge {typeof(TEntity).Name}");

            await CacheEntityAsync(entity, cancellationToken);
        }

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
        OnAsyncActionExecuting(source, nameof(source), cancellationToken);
        config ??= MergeConfig<TSource>(true);
        source.Adapt(destination, config);
        if (destination is null)
            throw new MapTargetNullException(nameof(destination));
        UpdateEntity(destination);
        var result = await AttacheChangeAsync(cancellationToken);
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Merge {typeof(TEntity).Name}");

            await CacheEntityAsync(destination, cancellationToken);
        }

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
        var sourceList = sources.ToList();
        OnAsyncActionExecuting(sourceList, nameof(sources), cancellationToken);
        var destinations = await FindByIdsAsync(sourceList.Select(source => source.Id), cancellationToken);
        if (destinations is null)
            throw new MapTargetNullException(nameof(destinations));
        config ??= MergeConfig<TSource>(false);
        var list = sourceList.Join(
            destinations, source => source.Id,
            destination => destination.Id,
            (source, destination) => source.Adapt(destination, config)).ToList();
        UpdateEntities(list);
        var result = await AttacheChangeAsync(cancellationToken);
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Merge {result.EffectRows} {typeof(TEntity).Name} Entities");

            await CacheEntitiesAsync(list, cancellationToken);
        }

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
        var sourceList = sources.ToList();
        OnAsyncActionExecuting(sourceList, nameof(sources), cancellationToken);
        config ??= MergeConfig<TSource>(true);
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
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Merge {result.EffectRows} {typeof(TEntity).Name} Entities");

            await CacheEntitiesAsync(list, cancellationToken);
        }

        return result;
    }

    #endregion

    #endregion
}