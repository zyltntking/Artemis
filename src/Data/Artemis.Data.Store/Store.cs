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
///     存储实现
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class Store<TEntity> : Store<TEntity, Guid>, IStore<TEntity>
    where TEntity : class, IKeySlot
{
    /// <summary>
    ///     基本存储实例构造
    /// </summary>
    /// <param name="context"></param>
    /// <param name="storeOptions"></param>
    /// <param name="cache"></param>
    /// <param name="logger"></param>
    /// <param name="describer"></param>
    protected Store(DbContext context, IStoreOptions? storeOptions = null, IDistributedCache? cache = null,
        ILogger? logger = null, StoreErrorDescriber? describer = null) : base(context, storeOptions, cache, logger,
        describer)
    {
    }

    #region Access

    /// <summary>
    ///     实体键生成委托
    /// </summary>
    protected sealed override Func<TEntity, string>? EntityKey { get; init; } = entity => entity.Id.GuidToString();

    #endregion

    #region Convert

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
///     存储实现
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
public abstract class Store<TEntity, TKey> : Store<TEntity, TKey, Guid>, IStore<TEntity, TKey>
    where TEntity : class, IKeySlot<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     基本存储实例构造
    /// </summary>
    /// <param name="context"></param>
    /// <param name="storeOptions"></param>
    /// <param name="cache"></param>
    /// <param name="logger"></param>
    /// <param name="describer"></param>
    protected Store(DbContext context, IStoreOptions? storeOptions = null, IDistributedCache? cache = null,
        ILogger? logger = null, StoreErrorDescriber? describer = null) : base(context, storeOptions, cache, logger,
        describer)
    {
    }
}

/// <summary>
///     存储实现
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
public abstract class Store<TEntity, TKey, THandler> : KeyLessStore<TEntity, THandler>, IStore<TEntity, TKey, THandler>
    where TEntity : class, IKeySlot<TKey>
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>
{
    /// <summary>
    ///     基本存储实例构造
    /// </summary>
    /// <param name="context"></param>
    /// <param name="storeOptions"></param>
    /// <param name="cache"></param>
    /// <param name="logger"></param>
    /// <param name="describer"></param>
    /// <exception cref="StoreParameterNullException"></exception>
    protected Store(DbContext context, IStoreOptions? storeOptions = null, IDistributedCache? cache = null,
        ILogger? logger = null, StoreErrorDescriber? describer = null) : base(context, storeOptions, cache, logger,
        describer)
    {
    }

    #region Access

    /// <summary>
    ///     实体键生成委托
    /// </summary>
    protected override Func<TEntity, string>? EntityKey { get; init; } = entity => entity.Id.IdToString()!;

    /// <summary>
    ///     生成键
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    private string ConvertToEntityKey(TKey key)
    {
        return ConvertIdToString(key)!;
    }

    #endregion

    #region Implementation of IStoreCommon<TEntity,TKey>

    #region QueryAccess

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
        var keyList = keys.ToList();

        var query = EntityQuery;

        foreach (var key in keyList)
        {
            var segment = EntityQuery.Where(entity => entity.Id.Equals(key));

            query = query.Union(segment);
        }

        return query;
    }

    #endregion

    #region GetId

    #region Convert

    /// <summary>
    ///     转换字符串到id
    /// </summary>
    /// <param name="id">id字符串</param>
    /// <returns>id</returns>
    protected virtual TKey? ConvertIdFromString(string id)
    {
        return id.IdFromString<TKey>();
    }

    /// <summary>
    ///     转换Id为字符串
    /// </summary>
    /// <param name="id">id</param>
    /// <returns>字符串</returns>
    protected virtual string? ConvertIdToString(TKey id)
    {
        return id.IdToString();
    }

    #endregion

    /// <summary>
    ///     获取指定实体Id
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns>Id</returns>
    public TKey GetId(TEntity entity)
    {
        OnActionExecuting(entity, nameof(entity));
        return entity.Id;
    }

    /// <summary>
    ///     获取指定实体Id
    /// </summary>
    /// <param name="entity">实体</param>
    /// <param name="cancellationToken">取消异步操作的信号</param>
    /// <returns>Id</returns>
    public Task<TKey> GetIdAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(entity, nameof(entity), cancellationToken);
        return Task.FromResult(entity.Id);
    }

    /// <summary>
    ///     获取指定实体Id字符串
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns>Id字符串</returns>
    public string GetIdString(TEntity entity)
    {
        OnActionExecuting(entity, nameof(entity));
        return ConvertIdToString(entity.Id)!;
    }

    /// <summary>
    ///     获取指定实体Id字符串
    /// </summary>
    /// <param name="entity">实体</param>
    /// <param name="cancellationToken">取消异步操作信号</param>
    /// <returns>Id字符串</returns>
    public Task<string> GetIdStringAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(entity, nameof(entity), cancellationToken);
        return Task.FromResult(ConvertIdToString(entity.Id)!);
    }

    #endregion

    #region IsDeleted

    /// <summary>
    ///     是否被删除
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns>判断结果</returns>
    public bool IsDeleted(TEntity entity)
    {
        return IsDeleted(entity.Id);
    }

    /// <summary>
    ///     是否被删除
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    public bool IsDeleted(TKey key)
    {
        return !KeyMatchQuery(key).Any();
    }

    /// <summary>
    ///     是否被删除
    /// </summary>
    /// <param name="entity">实体</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>判断结果</returns>
    public Task<bool> IsDeletedAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return IsDeletedAsync(entity.Id, cancellationToken);
    }

    /// <summary>
    ///     是否被删除
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<bool> IsDeletedAsync(TKey key, CancellationToken cancellationToken = default)
    {
        var exists = await KeyMatchQuery(key).AnyAsync(cancellationToken);

        return !exists;
    }

    #endregion

    #region BatchUpdate

    /// <summary>
    ///     更新存储中的实体
    /// </summary>
    /// <param name="id">被更新实体的主键</param>
    /// <param name="setter">更新行为</param>
    /// <returns></returns>
    public StoreResult BatchUpdate(TKey id,
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
    public StoreResult BatchUpdate(IEnumerable<TKey> ids,
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
    public async Task<StoreResult> BatchUpdateAsync(IEnumerable<TKey> ids,
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

    #endregion

    #region Delete

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
    ///     在存储中删除已存在的实体
    /// </summary>
    /// <param name="id">被删除实体的主键</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> DeleteAsync(TKey id, CancellationToken cancellationToken = default)
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
    ///     在存储中删除已存在的实体
    /// </summary>
    /// <param name="ids">被删除实体的主键</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> DeleteAsync(IEnumerable<TKey> ids, CancellationToken cancellationToken = default)
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

    #region BatchDelete

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

            if (changes > 0) RemoveCachedEntity(ConvertToEntityKey(id));

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

            if (changes > 0) RemoveCachedEntities(idList.Select(ConvertToEntityKey));

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
    public async Task<StoreResult> BatchDeleteAsync(TKey id, CancellationToken cancellationToken = default)
    {
        if (CachedStore)
            return StoreResult.Failed(Describer.EnableCache());

        OnAsyncActionExecuting(id, nameof(id), cancellationToken);
        try
        {
            var changes = await BatchDeleteEntityAsync(KeyMatchQuery(id), cancellationToken);

            if (changes > 0) await RemoveCachedEntityAsync(ConvertToEntityKey(id), cancellationToken);

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
    public async Task<StoreResult> BatchDeleteAsync(IEnumerable<TKey> ids,
        CancellationToken cancellationToken = default)
    {
        if (CachedStore)
            return StoreResult.Failed(Describer.EnableCache());

        var idList = ids as List<TKey> ?? ids.ToList();
        OnAsyncActionExecuting(idList, nameof(ids), cancellationToken);
        try
        {
            var changes = await BatchDeleteEntityAsync(KeyMatchQuery(idList), cancellationToken);

            if (changes > 0) await RemoveCachedEntitiesAsync(idList.Select(ConvertToEntityKey), cancellationToken);

            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(Describer.ConcurrencyFailure());
        }
    }

    #endregion

    #endregion

    #region FindEntity

    /// <summary>
    ///     根据Id查找实体
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public TEntity? FindEntity(TKey id)
    {
        OnActionExecuting(id, nameof(id));

        var cached = GetEntity(ConvertToEntityKey(id));

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

        var cached = GetEntities(idArray.Select(ConvertToEntityKey));

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
    public async Task<TEntity?> FindEntityAsync(TKey id, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(id, nameof(id), cancellationToken);

        var cached = await GetEntityAsync(ConvertToEntityKey(id), cancellationToken);

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
    public async Task<IEnumerable<TEntity>> FindEntitiesAsync(IEnumerable<TKey> ids,
        CancellationToken cancellationToken = default)
    {
        var idArray = ids as TKey[] ?? ids.ToArray();
        OnAsyncActionExecuting(idArray, nameof(ids), cancellationToken);

        var cached = await GetEntitiesAsync(idArray.Select(ConvertToEntityKey), cancellationToken);

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
    public Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default)
    {
        OnActionExecuting(id, nameof(id));
        return KeyMatchQuery(id).AnyAsync(cancellationToken);
    }

    #endregion

    #endregion

    #region Implementation of IStoreMap<TEntity,TKey>

    #region FindMapEntity

    /// <summary>
    ///     根据id查找映射实体
    /// </summary>
    /// <typeparam name="TMapEntity">映射类型</typeparam>
    /// <param name="id">id</param>
    /// <returns></returns>
    public TMapEntity? FindMapEntity<TMapEntity>(TKey id)
    {
        OnActionExecuting(id, nameof(id));

        var cached = GetEntity(ConvertToEntityKey(id));

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

        var cached = GetEntities(idArray.Select(ConvertToEntityKey));

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
    public async Task<TMapEntity?> FindMapEntityAsync<TMapEntity>(TKey id,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(id, nameof(id), cancellationToken);

        var cached = await GetEntityAsync(ConvertToEntityKey(id), cancellationToken);

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
    public async Task<IEnumerable<TMapEntity>> FindMapEntitiesAsync<TMapEntity>(IEnumerable<TKey> ids,
        CancellationToken cancellationToken = default)
    {
        var idArray = ids as TKey[] ?? ids.ToArray();
        OnAsyncActionExecuting(idArray, nameof(ids), cancellationToken);

        var cached = await GetEntitiesAsync(idArray.Select(ConvertToEntityKey), cancellationToken);

        if (cached is not null)
        {
            var list = cached.ToList();

            if (list.Any()) return list.Adapt<List<TMapEntity>>();
        }

        return await FindByIdsAsync<TMapEntity>(idArray, cancellationToken);
    }

    #endregion

    #region Over

    /// <summary>
    ///     通过类型映射覆盖对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="config">映射配置</param>
    /// <returns></returns>
    public StoreResult Over<TSource>(TSource source, TypeAdapterConfig? config = null) where TSource : IKeySlot<TKey>
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
    ///     通过类型映射覆盖对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="config">映射配置</param>
    /// <returns></returns>
    public StoreResult Over<TSource>(IEnumerable<TSource> sources, TypeAdapterConfig? config = null)
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
    ///     通过类型映射覆盖对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> OverAsync<TSource>(TSource source, TypeAdapterConfig? config = null,
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
    ///     通过类型映射覆盖对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> OverAsync<TSource>(IEnumerable<TSource> sources, TypeAdapterConfig? config = null,
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

    #endregion

    #region Merge

    /// <summary>
    ///     通过类型映射合并对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="config">映射配置</param>
    /// <returns></returns>
    public StoreResult Merge<TSource>(TSource source, TypeAdapterConfig? config = null) where TSource : IKeySlot<TKey>
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
    ///     通过类型映射合并对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="config">映射配置</param>
    /// <returns></returns>
    public StoreResult Merge<TSource>(IEnumerable<TSource> sources, TypeAdapterConfig? config = null)
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

    #endregion

    #endregion

    #region Actions

    #region ActionExecution

    #region FindById

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

    #endregion

    #endregion
}

/// <summary>
///     无键模型存储接
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class KeyLessStore<TEntity> : KeyLessStore<TEntity, Guid>, IKeyLessStore<TEntity> where TEntity : class
{
    /// <summary>
    ///     无键模型基本存储实例构造
    /// </summary>
    /// <param name="context"></param>
    /// <param name="storeOptions"></param>
    /// <param name="cache"></param>
    /// <param name="logger"></param>
    /// <param name="describer"></param>
    /// <exception cref="StoreParameterNullException"></exception>
    protected KeyLessStore(
        DbContext context,
        IStoreOptions? storeOptions = null,
        IDistributedCache? cache = null,
        ILogger? logger = null,
        StoreErrorDescriber? describer = null) : base(context, storeOptions, cache, logger, describer)
    {
    }
}

/// <summary>
///     无键数据模型存储
/// </summary>
/// <typeparam name="TEntity">模型类型</typeparam>
/// <typeparam name="THandler">操作者类型</typeparam>
public abstract class KeyLessStore<TEntity, THandler> : IKeyLessStore<TEntity, THandler>
    where TEntity : class
    where THandler : IEquatable<THandler>
{
    /// <summary>
    ///     无键模型基本存储实例构造
    /// </summary>
    /// <param name="context"></param>
    /// <param name="storeOptions"></param>
    /// <param name="cache"></param>
    /// <param name="logger"></param>
    /// <param name="describer"></param>
    /// <exception cref="StoreParameterNullException"></exception>
    protected KeyLessStore(
        DbContext context,
        IStoreOptions? storeOptions = null,
        IDistributedCache? cache = null,
        ILogger? logger = null,
        StoreErrorDescriber? describer = null)
    {
        Context = context ?? throw new StoreParameterNullException(nameof(context));
        StoreOptions = storeOptions ?? new StoreOptions();
        Cache = cache;
        Logger = logger;
        Describer = describer ?? new StoreErrorDescriber();
    }

    #region Implementation of IDisposable

    /// <summary>
    ///     已释放标识
    /// </summary>
    private bool _disposed;

    /// <summary>
    ///     Throws if this class has been disposed.
    /// </summary>
    /// <exception cref="StoreDisposedException"></exception>
    private void ThrowIfDisposed()
    {
        if (_disposed) throw new StoreDisposedException(GetType());
    }

    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        _disposed = true;
        GC.SuppressFinalize(this);
    }

    #endregion

    #region Access

    /// <summary>
    ///     注册操作员委托
    /// </summary>
    public Func<THandler>? HandlerRegister { get; set; }

    /// <summary>
    ///     生成键
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public string GenerateKey(TEntity entity)
    {
        ThrowIfDisposed();

        return EntityKey?.Invoke(entity) ?? throw new InstanceNotImplementException(nameof(EntityKey));
    }

    /// <summary>
    ///     缓存依赖
    /// </summary>
    protected IDistributedCache? Cache { get; }

    /// <summary>
    ///     数据访问上下文
    /// </summary>
    private DbContext Context { get; }

    /// <summary>
    ///     日志依赖
    /// </summary>
    protected ILogger? Logger { get; }

    /// <summary>
    ///     存储配置
    /// </summary>
    private IStoreOptions StoreOptions { get; }

    /// <summary>
    ///     设置当前发生错误的错误描述者
    /// </summary>
    protected StoreErrorDescriber Describer { get; }

    /// <summary>
    ///     Entity集合访问器
    /// </summary>
    public DbSet<TEntity> EntitySet => Context.Set<TEntity>();

    /// <summary>
    ///     Entity有追踪访问器
    /// </summary>
    public IQueryable<TEntity> TrackingQuery => EntitySet;

    /// <summary>
    ///     Entity无追踪访问器
    /// </summary>
    public IQueryable<TEntity> EntityQuery => TrackingQuery.AsNoTracking();

    /// <summary>
    ///     存储名称
    /// </summary>
    public virtual string StoreName => $"{typeof(TEntity).Name}";

    /// <summary>
    ///     实体键生成委托
    /// </summary>
    protected virtual Func<TEntity, string>? EntityKey { get; init; } = null;

    #endregion

    #region SettingAccessor

    /// <summary>
    ///     是否启用Debug日志
    /// </summary>
    protected bool DebugLogger => StoreOptions.DebugLogger && Logger != null;

    /// <summary>
    ///     设置是否自动保存更改
    /// </summary>
    private bool AutoSaveChanges => StoreOptions.AutoSaveChanges;

    /// <summary>
    ///     元数据托管标识
    /// </summary>
    private bool MetaDataHosting => StoreOptions.MetaDataHosting;

    /// <summary>
    ///     软删除标识
    /// </summary>
    private bool SoftDelete => StoreOptions is { MetaDataHosting: true, SoftDelete: true };

    /// <summary>
    ///     操作员托管标识
    /// </summary>
    private bool HandlerHosting => StoreOptions is { MetaDataHosting: true, HandlerHosting: true };

    /// <summary>
    ///     是否启用缓存
    /// </summary>
    protected bool CachedStore => StoreOptions is { CachedStore: true, Expires: >= 0 } && Cache is not null &&
                                  EntityKey is not null;

    /// <summary>
    ///     缓存过期时间
    /// </summary>
    private int Expires => StoreOptions.Expires;

    #endregion

    #region Implementation of IKeyLessStoreCommon<TEntity>

    #region Create

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
    public async Task<StoreResult> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
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
    public async Task<StoreResult> CreateAsync(IEnumerable<TEntity> entities,
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

    #region Update

    /// <summary>
    ///     在<paramref name="entity" />存储中更新已存在的实体
    /// </summary>
    /// <param name="entity">被更新实体</param>
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
    /// <param name="entities">被更新实体</param>
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
    /// <param name="entity">被更新实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
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
    /// <param name="entities">被更新实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> UpdateAsync(IEnumerable<TEntity> entities,
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

    #region BatchUpdate

    /// <summary>
    ///     在存储中更新符合条件的实体
    /// </summary>
    /// <param name="setter">更新行为</param>
    /// <param name="predicate">查询表达式</param>
    /// <returns></returns>
    public StoreResult BatchUpdate(Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter,
        Expression<Func<TEntity, bool>>? predicate = null)
    {
        ThrowIfDisposed();

        if (CachedStore) return StoreResult.Failed(Describer.EnableCache());

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
    public StoreResult BatchUpdate(IQueryable<TEntity> query,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter)
    {
        ThrowIfDisposed();

        if (CachedStore) return StoreResult.Failed(Describer.EnableCache());

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
        ThrowIfDisposed();
        cancellationToken.ThrowIfCancellationRequested();

        if (CachedStore) return StoreResult.Failed(Describer.EnableCache());

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
    public async Task<StoreResult> BatchUpdateAsync(IQueryable<TEntity> query,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        cancellationToken.ThrowIfCancellationRequested();

        if (CachedStore) return StoreResult.Failed(Describer.EnableCache());

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

    #endregion

    #region Delete

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
    ///     在<paramref name="entity" />存储中删除已存在的实体
    /// </summary>
    /// <param name="entity">被删除实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
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
    ///     在<paramref name="entities" />存储中删除已存在的实体
    /// </summary>
    /// <param name="entities">被删除实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> DeleteAsync(IEnumerable<TEntity> entities,
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

    #region BatchDelete

    /// <summary>
    ///     在存储中删除符合条件的实体
    /// </summary>
    /// <param name="predicate">查询表达式</param>
    /// <returns></returns>
    public StoreResult BatchDelete(Expression<Func<TEntity, bool>>? predicate = null)
    {
        ThrowIfDisposed();

        if (CachedStore) return StoreResult.Failed(Describer.EnableCache());

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
        ThrowIfDisposed();

        if (CachedStore) return StoreResult.Failed(Describer.EnableCache());

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
    ///     在存储中删除符合条件的实体
    /// </summary>
    /// <param name="predicate">查询表达式</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> BatchDeleteAsync(Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        cancellationToken.ThrowIfCancellationRequested();

        if (CachedStore) return StoreResult.Failed(Describer.EnableCache());

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
    public async Task<StoreResult> BatchDeleteAsync(IQueryable<TEntity> query,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        cancellationToken.ThrowIfCancellationRequested();

        if (CachedStore) return StoreResult.Failed(Describer.EnableCache());

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

    #endregion

    #endregion

    #region Implementation of IKeyLessStoreMap<TEntity>

    #region MapConfig

    /// <summary>
    ///     创建新对象的映射配置缓存
    /// </summary>
    private TypeAdapterConfig? _createNewConfig;

    /// <summary>
    ///     创建新对象的映射配置
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    private TypeAdapterConfig CreateNewConfig<TSource>()
    {
        if (_createNewConfig is not null) return _createNewConfig;

        _createNewConfig = new TypeAdapterConfig();

        _createNewConfig.NewConfig<TSource, TEntity>();

        if (Generator.IsInherit<TEntity>(typeof(IKeySlot<>))) _createNewConfig.ForType<TSource, TEntity>().Ignore("Id");

        if (MetaDataHosting)
        {
            if (Generator.IsInherit<TEntity>(typeof(IMateSlot)))
                _createNewConfig.ForType<TSource, TEntity>()
                    .Ignore(dest => ((IMateSlot)dest).CreatedAt)
                    .Ignore(dest => ((IMateSlot)dest).UpdatedAt)
                    .Ignore(dest => ((IMateSlot)dest).DeletedAt!);

            if (HandlerHosting && HandlerRegister != null &&
                Generator.IsInherit<TEntity>(typeof(IHandlerSlot<THandler>)))
                _createNewConfig.ForType<TSource, TEntity>()
                    .Ignore(dest => ((IHandlerSlot<THandler>)dest).CreateBy)
                    .Ignore(dest => ((IHandlerSlot<THandler>)dest).ModifyBy)
                    .Ignore(dest => ((IHandlerSlot<THandler>)dest).RemoveBy!);
        }

        return _createNewConfig;
    }

    /// <summary>
    ///     覆盖对象的映射配置缓存(对象明确)
    /// </summary>
    private TypeAdapterConfig? _overCertaintyConfig;

    /// <summary>
    ///     覆盖对象的映射配置缓存(对象不明确)
    /// </summary>
    private TypeAdapterConfig? _overDeCertaintyConfig;

    /// <summary>
    ///     覆盖对象的映射配置
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="certainty">对象是否明确</param>
    /// <returns></returns>
    protected TypeAdapterConfig OverConfig<TSource>(bool certainty)
    {
        if (certainty)
        {
            if (_overCertaintyConfig is not null) return _overCertaintyConfig;

            _overCertaintyConfig = CreateNewConfig<TSource>();

            return _overCertaintyConfig;
        }

        if (_overDeCertaintyConfig is not null) return _overDeCertaintyConfig;

        _overDeCertaintyConfig = new TypeAdapterConfig();

        _overDeCertaintyConfig.NewConfig<TSource, TEntity>();

        if (MetaDataHosting)
        {
            if (Generator.IsInherit<TEntity>(typeof(IMateSlot)))
                _overDeCertaintyConfig.ForType<TSource, TEntity>()
                    .Ignore(dest => ((IMateSlot)dest).CreatedAt)
                    .Ignore(dest => ((IMateSlot)dest).UpdatedAt)
                    .Ignore(dest => ((IMateSlot)dest).DeletedAt!);

            if (HandlerHosting && HandlerRegister != null &&
                Generator.IsInherit<TEntity>(typeof(IHandlerSlot<THandler>)))
                _overDeCertaintyConfig.ForType<TSource, TEntity>()
                    .Ignore(dest => ((IHandlerSlot<THandler>)dest).CreateBy)
                    .Ignore(dest => ((IHandlerSlot<THandler>)dest).ModifyBy)
                    .Ignore(dest => ((IHandlerSlot<THandler>)dest).RemoveBy!);
        }

        return _overDeCertaintyConfig;
    }

    /// <summary>
    ///     合并对象的映射配置缓存(对象明确)
    /// </summary>
    private TypeAdapterConfig? _mergeCertaintyConfig;

    /// <summary>
    ///     合并对象的映射配置缓存(对象不明确)
    /// </summary>
    private TypeAdapterConfig? _mergeDeCertaintyConfig;

    /// <summary>
    ///     合并对象的映射配置
    /// </summary>
    /// <param name="certainty"></param>
    /// <returns></returns>
    protected TypeAdapterConfig MergeConfig<TSource
    >(bool certainty)
    {
        if (certainty)
        {
            if (_mergeCertaintyConfig is not null) return _mergeCertaintyConfig;

            _mergeCertaintyConfig = new TypeAdapterConfig();

            _mergeCertaintyConfig.NewConfig<TSource, TEntity>().IgnoreNullValues(true);

            if (Generator.IsInherit<TEntity>(typeof(IKeySlot<>)))
                _mergeCertaintyConfig.ForType<TSource, TEntity>().Ignore("Id");

            if (MetaDataHosting)
            {
                if (Generator.IsInherit<TEntity>(typeof(IMateSlot)))
                    _mergeCertaintyConfig.ForType<TSource, TEntity>()
                        .Ignore(dest => ((IMateSlot)dest).CreatedAt)
                        .Ignore(dest => ((IMateSlot)dest).UpdatedAt)
                        .Ignore(dest => ((IMateSlot)dest).DeletedAt!);

                if (HandlerHosting && HandlerRegister != null &&
                    Generator.IsInherit<TEntity>(typeof(IHandlerSlot<THandler>)))
                    _mergeCertaintyConfig.ForType<TSource, TEntity>()
                        .Ignore(dest => ((IHandlerSlot<THandler>)dest).CreateBy)
                        .Ignore(dest => ((IHandlerSlot<THandler>)dest).ModifyBy)
                        .Ignore(dest => ((IHandlerSlot<THandler>)dest).RemoveBy!);
            }

            return _mergeCertaintyConfig;
        }

        if (_mergeDeCertaintyConfig is not null) return _mergeDeCertaintyConfig;

        _mergeDeCertaintyConfig = new TypeAdapterConfig();

        _mergeDeCertaintyConfig.NewConfig<TSource, TEntity>().IgnoreNullValues(true);

        if (MetaDataHosting)
        {
            if (Generator.IsInherit<TEntity>(typeof(IMateSlot)))
                _mergeDeCertaintyConfig.ForType<TSource, TEntity>()
                    .Ignore(dest => ((IMateSlot)dest).CreatedAt)
                    .Ignore(dest => ((IMateSlot)dest).UpdatedAt)
                    .Ignore(dest => ((IMateSlot)dest).DeletedAt!);

            if (HandlerHosting && HandlerRegister != null &&
                Generator.IsInherit<TEntity>(typeof(IHandlerSlot<THandler>)))
                _mergeDeCertaintyConfig.ForType<TSource, TEntity>()
                    .Ignore(dest => ((IHandlerSlot<THandler>)dest).CreateBy)
                    .Ignore(dest => ((IHandlerSlot<THandler>)dest).ModifyBy)
                    .Ignore(dest => ((IHandlerSlot<THandler>)dest).RemoveBy!);
        }

        return _mergeDeCertaintyConfig;
    }

    #endregion

    #region CreateNew

    /// <summary>
    ///     通过类型映射创建一个新实例
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="config">映射配置</param>
    /// <returns>创建结果</returns>
    public StoreResult CreateNew<TSource>(TSource source, TypeAdapterConfig? config = null)
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
    public StoreResult CreateNew<TSource>(IEnumerable<TSource> sources, TypeAdapterConfig? config = null)
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
    public async Task<StoreResult> CreateNewAsync<TSource>(TSource source, TypeAdapterConfig? config = null,
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
    public async Task<StoreResult> CreateNewAsync<TSource>(IEnumerable<TSource> sources,
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

    #region Over

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
    ///     通过类型映射覆盖对应实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TJKey">连接键类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="destinations">目标数据</param>
    /// <param name="destinationKeySelector">目标键选择器</param>
    /// <param name="config">映射配置</param>
    /// <param name="sourceKeySelector">源键选择器</param>
    /// <returns></returns>
    public StoreResult Over<TSource, TJKey>(IEnumerable<TSource> sources, IEnumerable<TEntity> destinations,
        Func<TSource, TJKey> sourceKeySelector,
        Func<TEntity, TJKey> destinationKeySelector, TypeAdapterConfig? config = null)
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
    ///     通过类型映射覆盖对应实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="destination">目标数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> OverAsync<TSource>(TSource source, TEntity destination,
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
    public async Task<StoreResult> OverAsync<TSource, TJKey>(IEnumerable<TSource> sources,
        IEnumerable<TEntity> destinations,
        Func<TSource, TJKey> sourceKeySelector,
        Func<TEntity, TJKey> destinationKeySelector, TypeAdapterConfig? config = null,
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

    #region Merge

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
    ///     通过类型映射合并对应实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="destination">目标数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> MergeAsync<TSource>(TSource source, TEntity destination,
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
    public async Task<StoreResult> MergeAsync<TSource, TJKey>(IEnumerable<TSource> sources,
        IEnumerable<TEntity> destinations,
        Func<TSource, TJKey> sourceKeySelector,
        Func<TEntity, TJKey> destinationKeySelector, TypeAdapterConfig? config = null,
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

    #region Actions

    #region CacheAction

    /// <summary>
    ///     生成键
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    private string CacheKey(TEntity entity)
    {
        var key = GenerateKey(entity);
        return $"{StoreName}:{key}";
    }

    /// <summary>
    ///     生成键
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    private string CacheKey(string key)
    {
        return $"{StoreName}:{key}";
    }

    /// <summary>
    ///     缓存实体
    /// </summary>
    /// <param name="entity"></param>
    protected void CacheEntity(TEntity entity)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            Cache?.Set(CacheKey(entity), entity, Expires);

            if (DebugLogger)
                Logger?.LogDebug($"{typeof(TEntity).Name} Cached");
        }
    }

    /// <summary>
    ///     缓存实体
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected Task CacheEntityAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            Cache?.SetAsync(CacheKey(entity), entity, Expires, cancellationToken);

            if (DebugLogger) Logger?.LogDebug($"{typeof(TEntity).Name} Cached");
        }

        return Task.CompletedTask;
    }

    /// <summary>
    ///     缓存实体
    /// </summary>
    /// <param name="entities"></param>
    protected void CacheEntities(IEnumerable<TEntity> entities)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            var count = 0;

            foreach (var entity in entities)
            {
                Cache?.SetAsync(CacheKey(entity), entity, Expires);
                count++;
            }

            if (DebugLogger) Logger?.LogDebug($"Cached {count} {typeof(TEntity).Name} Entities");
        }
    }

    /// <summary>
    ///     缓存实体
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="cancellationToken"></param>
    protected Task CacheEntitiesAsync(
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
                Cache?.SetAsync(CacheKey(entity), entity, Expires, cancellationToken);
                count++;
            }

            if (DebugLogger) Logger?.LogDebug($"Cached {count} {typeof(TEntity).Name} Entities");
        }

        return Task.CompletedTask;
    }

    /// <summary>
    ///     获取实体
    /// </summary>
    /// <param name="key">缓存键</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    protected TEntity? GetEntity(string key)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            var entity = Cache?.Get<TEntity>(CacheKey(key));

            if (DebugLogger) Logger?.LogDebug($"Get {typeof(TEntity).Name} Entity From Cache");

            return entity;
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
    protected async Task<TEntity?> GetEntityAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            var entity = await Cache?.GetAsync<TEntity>(CacheKey(key), cancellationToken)!;

            if (DebugLogger) Logger?.LogDebug($"Get {typeof(TEntity).Name} Entity From Cache");

            return entity;
        }

        return default;
    }

    /// <summary>
    ///     获取实体
    /// </summary>
    /// <param name="keys">实体键列表</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    protected IEnumerable<TEntity>? GetEntities(IEnumerable<string> keys)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            var list = new List<TEntity>();

            foreach (var key in keys)
            {
                var entity = Cache?.Get<TEntity>(CacheKey(key));

                if (entity is not null)
                    list.Add(entity);
            }

            if (DebugLogger) Logger?.LogDebug($"Get {list.Count} {typeof(TEntity).Name} Entities From Cache");

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
    protected async Task<IEnumerable<TEntity>?> GetEntitiesAsync(
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
                var entity = await Cache?.GetAsync<TEntity>(CacheKey(key), cancellationToken)!;

                if (entity is not null)
                    list.Add(entity);
            }

            if (DebugLogger) Logger?.LogDebug($"Get {list.Count} {typeof(TEntity).Name} Entities From Cache");

            return list;
        }

        return default;
    }

    /// <summary>
    ///     移除被缓存的实体
    /// </summary>
    /// <param name="key"></param>
    /// <exception cref="InstanceNotImplementException"></exception>
    protected void RemoveCachedEntity(string key)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            Cache?.Remove(CacheKey(key));

            if (DebugLogger) Logger?.LogDebug($"{typeof(TEntity).Name} Removed From Cache");
        }
    }

    /// <summary>
    ///     移除被缓存的实体
    /// </summary>
    /// <param name="entity"></param>
    protected void RemoveCachedEntity(TEntity entity)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            Cache?.Remove(CacheKey(entity));

            if (DebugLogger) Logger?.LogDebug($"{typeof(TEntity).Name} Removed From Cache");
        }
    }

    /// <summary>
    ///     移除被缓存的实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    protected async Task RemoveCachedEntityAsync(string key, CancellationToken cancellationToken = default)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            await Cache?.RemoveAsync(CacheKey(key), cancellationToken)!;

            if (DebugLogger) Logger?.LogDebug($"{typeof(TEntity).Name} Removed From Cache");
        }
    }

    /// <summary>
    ///     移除被缓存的实体
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    protected async Task RemoveCachedEntityAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            await Cache?.RemoveAsync(CacheKey(entity), cancellationToken)!;

            if (DebugLogger) Logger?.LogDebug($"{typeof(TEntity).Name} Removed From Cache");
        }
    }

    /// <summary>
    ///     移除被缓存的实体
    /// </summary>
    /// <param name="keys"></param>
    protected void RemoveCachedEntities(IEnumerable<string> keys)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            var count = 0;
            foreach (var key in keys)
            {
                Cache?.Remove(CacheKey(key));
                count++;
            }

            if (DebugLogger) Logger?.LogDebug($"{count} {typeof(TEntity).Name} Entities Removed From Cache");
        }
    }

    /// <summary>
    ///     移除被缓存的实体
    /// </summary>
    /// <param name="entities"></param>
    protected void RemoveCachedEntities(IEnumerable<TEntity> entities)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            var count = 0;
            foreach (var entity in entities)
            {
                Cache?.Remove(CacheKey(entity));
                count++;
            }

            if (DebugLogger) Logger?.LogDebug($"{count} {typeof(TEntity).Name} Entities Removed From Cache");
        }
    }

    /// <summary>
    ///     移除被缓存的实体
    /// </summary>
    /// <param name="keys"></param>
    /// <param name="cancellationToken"></param>
    protected async Task RemoveCachedEntitiesAsync(
        IEnumerable<string> keys,
        CancellationToken cancellationToken = default)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            var count = 0;
            foreach (var key in keys)
            {
                await Cache?.RemoveAsync(CacheKey(key), cancellationToken)!;
                count++;
            }

            if (DebugLogger) Logger?.LogDebug($"{count} {typeof(TEntity).Name} Entities Removed From Cache");
        }
    }

    /// <summary>
    ///     移除被缓存的实体
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="cancellationToken"></param>
    protected async Task RemoveCachedEntitiesAsync(
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
                await Cache?.RemoveAsync(CacheKey(entity), cancellationToken)!;
                count++;
            }

            if (DebugLogger) Logger?.LogDebug($"{count} {typeof(TEntity).Name} Entities Removed From Cache");
        }
    }

    #endregion

    #region ActionExecution

    /// <summary>
    ///     添加单个实体
    /// </summary>
    /// <param name="entity">实体</param>
    private void AddEntity(TEntity entity)
    {
        if (entity is IConcurrencyStamp concurrency)
            concurrency.ConcurrencyStamp = Guid.NewGuid().IdToString();

        if (MetaDataHosting)
        {
            if (entity is IMateSlot metaSlot)
            {
                var now = DateTime.Now;
                metaSlot.CreatedAt = now;
                metaSlot.UpdatedAt = now;
            }

            if (HandlerHosting && HandlerRegister != null && entity is IHandlerSlot<THandler> handlerSlot)
            {
                var handler = HandlerRegister();
                handlerSlot.CreateBy = handler;
                handlerSlot.ModifyBy = handler;
            }
        }

        Context.Add(entity);

        if (MetaDataHosting)
        {
            if (entity is IMateSlot metaSlot)
                Context.Entry(metaSlot).Property(item => item.DeletedAt).IsModified = false;

            if (HandlerHosting && HandlerRegister != null && entity is IHandlerSlot<THandler> handlerSlot)
                Context.Entry(handlerSlot).Property(item => item.RemoveBy).IsModified = false;
        }
    }

    /// <summary>
    ///     添加多个实体
    /// </summary>
    /// <param name="entities">实体</param>
    private void AddEntities(ICollection<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            if (entity is IConcurrencyStamp concurrency) concurrency.ConcurrencyStamp = Guid.NewGuid().IdToString();

            if (MetaDataHosting)
            {
                if (entity is IMateSlot metaSlot)
                {
                    var now = DateTime.Now;
                    metaSlot.CreatedAt = now;
                    metaSlot.UpdatedAt = now;
                }

                if (HandlerHosting && HandlerRegister != null && entity is IHandlerSlot<THandler> handlerSlot)
                {
                    handlerSlot.CreateBy = HandlerRegister();
                    handlerSlot.ModifyBy = HandlerRegister();
                }
            }
        }

        Context.AddRange(entities);

        foreach (var entity in entities)
            if (MetaDataHosting)
            {
                if (entity is IMateSlot metaSlot)
                    Context.Entry(metaSlot).Property(item => item.DeletedAt).IsModified = false;

                if (HandlerHosting && HandlerRegister != null && entity is IHandlerSlot<THandler> handlerSlot)
                    Context.Entry(handlerSlot).Property(item => item.RemoveBy).IsModified = false;
            }
    }

    /// <summary>
    ///     追踪一个实体更新
    /// </summary>
    /// <param name="entity">实体</param>
    protected void UpdateEntity(TEntity entity)
    {
        Context.Attach(entity);

        if (entity is IConcurrencyStamp concurrency) concurrency.ConcurrencyStamp = Guid.NewGuid().IdToString();

        if (MetaDataHosting)
        {
            if (entity is IMateSlot metaSlot) metaSlot.UpdatedAt = DateTime.Now;

            if (HandlerHosting && HandlerRegister != null && entity is IHandlerSlot<THandler> handlerSlot)
                handlerSlot.ModifyBy = HandlerRegister();
        }

        Context.Update(entity);

        if (MetaDataHosting)
        {
            if (entity is IMateSlot metaSlot)
            {
                Context.Entry(metaSlot).Property(item => item.CreatedAt).IsModified = false;
                Context.Entry(metaSlot).Property(item => item.DeletedAt).IsModified = false;
            }

            if (HandlerHosting && HandlerRegister != null && entity is IHandlerSlot<THandler> handlerSlot)
            {
                Context.Entry(handlerSlot).Property(item => item.CreateBy).IsModified = false;
                Context.Entry(handlerSlot).Property(item => item.RemoveBy).IsModified = false;
            }
        }
    }

    /// <summary>
    ///     追踪多个实体更新
    /// </summary>
    /// <param name="entities">实体</param>
    protected void UpdateEntities(ICollection<TEntity> entities)
    {
        Context.AttachRange(entities);

        foreach (var entity in entities)
        {
            if (entity is IConcurrencyStamp concurrency) concurrency.ConcurrencyStamp = Guid.NewGuid().IdToString();

            if (MetaDataHosting)
            {
                if (entity is IMateSlot metaSlot)
                {
                    var now = DateTime.Now;
                    metaSlot.UpdatedAt = now;
                }

                if (HandlerHosting && HandlerRegister != null && entity is IHandlerSlot<THandler> handlerSlot)
                {
                    var handler = HandlerRegister();
                    handlerSlot.ModifyBy = handler;
                }
            }
        }

        Context.UpdateRange(entities);

        foreach (var entity in entities)
            if (MetaDataHosting)
            {
                if (entity is IMateSlot metaSlot)
                {
                    Context.Entry(metaSlot).Property(item => item.CreatedAt).IsModified = false;
                    Context.Entry(metaSlot).Property(item => item.DeletedAt).IsModified = false;
                }

                if (HandlerHosting && HandlerRegister != null && entity is IHandlerSlot<THandler> handlerSlot)
                {
                    Context.Entry(handlerSlot).Property(item => item.CreateBy).IsModified = false;
                    Context.Entry(handlerSlot).Property(item => item.RemoveBy).IsModified = false;
                }
            }
    }

    /// <summary>
    ///     批量更新实体
    /// </summary>
    /// <param name="query">查询</param>
    /// <param name="setter">更新委托</param>
    /// <returns></returns>
    protected int BatchUpdateEntity(
        IQueryable<TEntity> query,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter)
    {
        if (Generator.IsInherit<TEntity>(typeof(IConcurrencyStamp)))
            setter = setter.AppendSetProperty(stampSetter => stampSetter
                .SetProperty(
                    entity => ((IConcurrencyStamp)entity).ConcurrencyStamp,
                    Guid.NewGuid().IdToString()));

        if (MetaDataHosting)
        {
            if (Generator.IsInherit<TEntity>(typeof(IMateSlot)))
                setter = setter.AppendSetProperty(mateSetter => mateSetter
                    .SetProperty(
                        entity => ((IMateSlot)entity).UpdatedAt,
                        DateTime.Now)
                );

            if (HandlerHosting && HandlerRegister != null &&
                Generator.IsInherit<TEntity>(typeof(IHandlerSlot<THandler>)))
                setter = setter.AppendSetProperty(mateSetter => mateSetter
                    .SetProperty(
                        entity => ((IHandlerSlot<THandler>)entity).ModifyBy,
                        HandlerRegister())
                );
        }

        return query.ExecuteUpdate(setter);
    }

    /// <summary>
    ///     批量删除实体
    /// </summary>
    /// <param name="query">查询</param>
    /// <param name="setter">更新委托</param>
    /// <param name="cancellationToken">异步操作取消信号</param>
    /// <returns></returns>
    protected Task<int> BatchUpdateEntityAsync(
        IQueryable<TEntity> query,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter,
        CancellationToken cancellationToken = default)
    {
        if (Generator.IsInherit<TEntity>(typeof(IConcurrencyStamp)))
            setter = setter.AppendSetProperty(stampSetter => stampSetter
                .SetProperty(
                    entity => ((IConcurrencyStamp)entity).ConcurrencyStamp,
                    Guid.NewGuid().IdToString()));

        if (MetaDataHosting)
        {
            if (Generator.IsInherit<TEntity>(typeof(IMateSlot)))
                setter = setter.AppendSetProperty(mateSetter => mateSetter
                    .SetProperty(
                        entity => ((IMateSlot)entity).UpdatedAt,
                        DateTime.Now)
                );

            if (HandlerHosting && HandlerRegister != null &&
                Generator.IsInherit<TEntity>(typeof(IHandlerSlot<THandler>)))
                setter = setter.AppendSetProperty(mateSetter => mateSetter
                    .SetProperty(
                        entity => ((IHandlerSlot<THandler>)entity).ModifyBy,
                        HandlerRegister())
                );
        }

        return query.ExecuteUpdateAsync(setter, cancellationToken);
    }

    /// <summary>
    ///     追踪一个实体删除
    /// </summary>
    /// <param name="entity">实体</param>
    protected void DeleteEntity(TEntity entity)
    {
        if (SoftDelete)
        {
            Context.Attach(entity);

            if (entity is IConcurrencyStamp concurrency) concurrency.ConcurrencyStamp = Guid.NewGuid().IdToString();

            if (entity is IMateSlot metaSlotChange) metaSlotChange.DeletedAt = DateTime.Now;

            if (HandlerHosting && HandlerRegister != null && entity is IHandlerSlot<THandler> handlerSlotChange)
                handlerSlotChange.RemoveBy = HandlerRegister();

            Context.Update(entity);

            if (entity is IMateSlot metaSlotKeep)
            {
                Context.Entry(metaSlotKeep).Property(item => item.CreatedAt).IsModified = false;
                Context.Entry(metaSlotKeep).Property(item => item.UpdatedAt).IsModified = false;
            }

            if (HandlerHosting && HandlerRegister != null && entity is IHandlerSlot<THandler> handlerSlotKeep)
            {
                Context.Entry(handlerSlotKeep).Property(item => item.CreateBy).IsModified = false;
                Context.Entry(handlerSlotKeep).Property(item => item.ModifyBy).IsModified = false;
            }
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
    protected void DeleteEntities(ICollection<TEntity> entities)
    {
        if (SoftDelete)
        {
            Context.AttachRange(entities);

            foreach (var entity in entities)
            {
                if (entity is IConcurrencyStamp concurrency) concurrency.ConcurrencyStamp = Guid.NewGuid().IdToString();

                if (entity is IMateSlot metaSlot) metaSlot.DeletedAt = DateTime.Now;

                if (HandlerHosting && HandlerRegister != null && entity is IHandlerSlot<THandler> handlerSlot)
                    handlerSlot.RemoveBy = HandlerRegister();
            }

            Context.AttachRange(entities);

            foreach (var entity in entities)
            {
                if (entity is IMateSlot metaSlot)
                {
                    Context.Entry(metaSlot).Property(item => item.CreatedAt).IsModified = false;
                    Context.Entry(metaSlot).Property(item => item.UpdatedAt).IsModified = false;
                }

                if (HandlerHosting && HandlerRegister != null && entity is IHandlerSlot<THandler> handlerSlot)
                {
                    Context.Entry(handlerSlot).Property(item => item.CreateBy).IsModified = false;
                    Context.Entry(handlerSlot).Property(item => item.ModifyBy).IsModified = false;
                }
            }
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
    protected int BatchDeleteEntity(IQueryable<TEntity> query)
    {
        if (SoftDelete)
        {
            Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter =
                calls => calls;

            if (Generator.IsInherit<TEntity>(typeof(IConcurrencyStamp)))
                setter = setter.AppendSetProperty(stampSetter => stampSetter
                    .SetProperty(
                        entity => ((IConcurrencyStamp)entity).ConcurrencyStamp,
                        Guid.NewGuid().IdToString()));

            if (Generator.IsInherit<TEntity>(typeof(IMateSlot)))
                setter = setter.AppendSetProperty(mateSetter => mateSetter
                    .SetProperty(
                        entity => ((IMateSlot)entity).DeletedAt,
                        DateTime.Now)
                );

            if (HandlerHosting && HandlerRegister != null &&
                Generator.IsInherit<TEntity>(typeof(IHandlerSlot<THandler>)))
                setter = setter.AppendSetProperty(mateSetter => mateSetter
                    .SetProperty(
                        entity => ((IHandlerSlot<THandler>)entity).RemoveBy,
                        HandlerRegister())
                );

            return query.ExecuteUpdate(setter);
        }

        return query.ExecuteDelete();
    }

    /// <summary>
    ///     异步批量删除实体
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected Task<int> BatchDeleteEntityAsync(
        IQueryable<TEntity> query,
        CancellationToken cancellationToken = default)
    {
        if (SoftDelete)
        {
            Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter =
                calls => calls;

            if (Generator.IsInherit<TEntity>(typeof(IConcurrencyStamp)))
                setter = setter.AppendSetProperty(stampSetter => stampSetter
                    .SetProperty(
                        entity => ((IConcurrencyStamp)entity).ConcurrencyStamp,
                        Guid.NewGuid().IdToString()));

            if (Generator.IsInherit<TEntity>(typeof(IMateSlot)))
                setter = setter.AppendSetProperty(mateSetter => mateSetter
                    .SetProperty(
                        entity => ((IMateSlot)entity).DeletedAt,
                        DateTime.Now)
                );

            if (HandlerHosting && HandlerRegister != null &&
                Generator.IsInherit<TEntity>(typeof(IHandlerSlot<THandler>)))
                setter = setter.AppendSetProperty(mateSetter => mateSetter
                    .SetProperty(
                        entity => ((IHandlerSlot<THandler>)entity).RemoveBy,
                        HandlerRegister())
                );

            return query.ExecuteUpdateAsync(setter, cancellationToken);
        }

        return query.ExecuteDeleteAsync(cancellationToken);
    }

    #endregion

    #region OnActionExecution

    /// <summary>
    ///     方法执行前
    /// </summary>
    /// <param name="value">实体</param>
    /// <param name="name">参数名</param>
    /// <exception cref="StoreParameterNullException">空参数异常</exception>
    protected void OnActionExecuting<TValue>(TValue value, string name)
    {
        ThrowIfDisposed();
        if (value is null)
            throw new StoreParameterNullException(name);
    }

    /// <summary>
    ///     异步方法执行前
    /// </summary>
    /// <param name="value">实体</param>
    /// <param name="name">参数名</param>
    /// <param name="cancellationToken">取消信号</param>
    protected void OnAsyncActionExecuting<TValue>(TValue value, string name,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        OnActionExecuting(value, name);
    }

    #endregion

    #endregion

    #region SaveChanges

    #region AttacheChange

    /// <summary>
    ///     保存追踪
    /// </summary>
    /// <returns></returns>
    protected StoreResult AttacheChange()
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
    protected async Task<StoreResult> AttacheChangeAsync(CancellationToken cancellationToken = default)
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

    /// <summary>
    ///     保存当前存储
    /// </summary>
    /// <returns></returns>
    private int SaveChanges()
    {
        if (DebugLogger) Logger?.LogDebug(nameof(SaveChanges));

        return AutoSaveChanges ? Context.SaveChanges() : 0;
    }

    /// <summary>
    ///     保存当前存储
    /// </summary>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>异步取消结果</returns>
    private Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (DebugLogger) Logger?.LogDebug(nameof(SaveChangesAsync));

        return AutoSaveChanges ? Context.SaveChangesAsync(cancellationToken) : Task.FromResult(0);
    }

    #endregion
}