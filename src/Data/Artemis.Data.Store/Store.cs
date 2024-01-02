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
    /// <param name="storeOptions"></param>
    /// <param name="cache">缓存依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    protected Store(
        DbContext context,
        IStoreOptions? storeOptions = null,
        IDistributedCache? cache = null,
        ILogger? logger = null) : base(context, storeOptions, cache, logger)
    {
    }

    #region Overrides of StoreBase<TEntity,Guid>

    /// <summary>
    ///     转换字符串到id
    /// </summary>
    /// <param name="id">id字符串</param>
    /// <returns>id</returns>
    protected override Guid ConvertIdFromString(string id)
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
/// <typeparam name="TKey">键类型</typeparam>
public abstract class Store<TEntity, TKey> : KeyWithStoreBase<TEntity, TKey>, IStore<TEntity, TKey>
    where TEntity : class, IModelBase<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     是否被删除
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns>判断结果</returns>
    public override bool IsDeleted(TEntity entity)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     是否被删除
    /// </summary>
    /// <param name="entity">实体</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>判断结果</returns>
    public override Task<bool> IsDeletedAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     创建一个新的基本存储实例
    /// </summary>
    /// <param name="context">数据访问上下文</param>
    /// <param name="logger">日志依赖</param>
    /// <param name="describer">操作异常描述者</param>
    /// <param name="storeOptions">配置依赖</param>
    /// <param name="cache">缓存依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    protected Store(
        DbContext context,
        IStoreOptions? storeOptions = null,
        IDistributedCache? cache = null,
        ILogger? logger = null,
        StoreErrorDescriber? describer = null) : base(context,cache, storeOptions, logger, describer)
    {
        StoreOptions = storeOptions ?? new ArtemisStoreOptions();
    }

    #region DataAccess

    /// <summary>
    ///     Entity有追踪访问器
    /// </summary>
    public override IQueryable<TEntity> TrackingQuery => EntitySet.WhereIf(SoftDelete, entity => entity.DeletedAt != null);

    #endregion

    #region Implementation of IStoreCommon<TEntity,in TKey>

    #region IStoreOptions

    private IStoreOptions StoreOptions { get; }

    #region Setting

    /// <summary>
    ///     设置是否启用元数据托管
    /// </summary>
    protected bool MetaDataHosting => StoreOptions.MetaDataHosting || StoreOptions.SoftDelete;

    /// <summary>
    ///     设置是否启用软删除
    /// </summary>
    protected bool SoftDelete => StoreOptions.SoftDelete;

    #endregion

    #endregion

    #region CreateEntity & CreateEntities

    /// <summary>
    ///     在<paramref name="entity" />存储中创建一个新的实体
    /// </summary>
    /// <param name="entity">被创建实体</param>
    /// <returns></returns>
    public virtual StoreResult Create(TEntity entity)
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
    public virtual StoreResult Create(IEnumerable<TEntity> entities)
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
    public virtual async Task<StoreResult> CreateAsync(
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
    public virtual async Task<StoreResult> CreateAsync(
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
        OnActionExecuting(source, nameof(source));
        config ??= IgnoreIdConfig<TSource>();
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
        config ??= IgnoreIdConfig<TSource>();
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
        config ??= IgnoreIdConfig<TSource>();
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
        config ??= IgnoreIdConfig<TSource>();
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
        config ??= IgnoreMetaConfig<TSource>();
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
        config ??= IgnoreIdConfig<TSource>();
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
        config ??= IgnoreMetaConfig<TSource>();
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
        config ??= IgnoreMetaConfig<TSource>();
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
        config ??= IgnoreIdConfig<TSource>();
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
        config ??= IgnoreMetaConfig<TSource>();
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
        config ??= IgnoreNullConfig<TSource>();
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
        config ??= IgnoreIdAndNullConfig<TSource>();
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
        config ??= IgnoreNullConfig<TSource>();
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
        config ??= IgnoreNullConfig<TSource>();
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
        config ??= IgnoreIdAndNullConfig<TSource>();
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
        config ??= IgnoreNullConfig<TSource>();
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
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Merge {result.EffectRows} {typeof(TEntity).Name} Entities");

            await CacheEntitiesAsync(list, cancellationToken);
        }

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
    protected override void AddEntity(TEntity entity)
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
    protected override void AddEntities(ICollection<TEntity> entities)
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
    protected override void UpdateEntity(TEntity entity)
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
    protected override void UpdateEntities(ICollection<TEntity> entities)
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
    protected override int BatchUpdateEntity(
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
    protected override Task<int> BatchUpdateEntityAsync(
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
    protected override void DeleteEntity(TEntity entity)
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
    protected override void DeleteEntities(ICollection<TEntity> entities)
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
    protected override int BatchDeleteEntity(IQueryable<TEntity> query)
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
    protected override Task<int> BatchDeleteEntityAsync(
        IQueryable<TEntity> query,
        CancellationToken cancellationToken = default)
    {
        if (SoftDelete)
            return query.ExecuteUpdateAsync(setter => setter
                .SetProperty(entity => entity.UpdatedAt, DateTime.Now)
                .SetProperty(entity => entity.DeletedAt, DateTime.Now), cancellationToken);

        return query.ExecuteDeleteAsync(cancellationToken);
    }
    #endregion
}