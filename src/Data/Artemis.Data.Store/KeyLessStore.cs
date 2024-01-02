using System.Linq.Expressions;
using Artemis.Data.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;

namespace Artemis.Data.Store;

/// <summary>
///     无键存储模型基类
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class KeyLessStore<TEntity> :
    KeyLessStoreBase<TEntity>,
    IKeyLessStoreCommon<TEntity>
    where TEntity : class
{
    /// <summary>
    ///     无键模型存储实例构造
    /// </summary>
    /// <param name="storeOptions"></param>
    /// <param name="logger"></param>
    /// <param name="describer"></param>
    /// <param name="context"></param>
    /// <exception cref="StoreParameterNullException"></exception>
    protected KeyLessStore(
        DbContext context,
        IKeyLessStoreOptions? storeOptions = null,
        ILogger? logger = null,
        StoreErrorDescriber? describer = null) : base(context, storeOptions, logger, describer)
    {
    }

    #region Implementation of IKeyLessStoreCommon<TEntity>

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

        if (!result.Succeeded) return result;
        if (DebugLogger) Logger?.LogDebug($"Create {typeof(TEntity).Name}");

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

        if (!result.Succeeded) return result;
        if (DebugLogger) Logger?.LogDebug($"Create {result.EffectRows} {typeof(TEntity).Name} Entities");

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

        if (!result.Succeeded) return result;
        if (DebugLogger) Logger?.LogDebug($"Create {typeof(TEntity).Name}");

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

        if (!result.Succeeded) return result;
        if (DebugLogger) Logger?.LogDebug($"Create {result.EffectRows} {typeof(TEntity).Name} Entities");

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

        if (!result.Succeeded) return result;
        if (DebugLogger) Logger?.LogDebug($"Update {typeof(TEntity).Name}");

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

        if (!result.Succeeded) return result;
        if (DebugLogger) Logger?.LogDebug($"Update {result.EffectRows} {typeof(TEntity).Name} Entities");

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

        if (!result.Succeeded) return result;
        if (DebugLogger) Logger?.LogDebug($"Update {typeof(TEntity).Name}");

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
            if (DebugLogger)
                Logger?.LogDebug($"Update {list.Count} {typeof(TEntity).Name} Entities");

        return result;
    }

    #endregion

    #region BatchUpdateEntity & BatchUpdateEntities

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
    ///     在<paramref name="entity" />存储中删除已存在的实体
    /// </summary>
    /// <param name="entity">被删除实体</param>
    /// <returns></returns>
    public StoreResult Delete(TEntity entity)
    {
        OnActionExecuting(entity, nameof(entity));

        DeleteEntity(entity);

        var result = AttacheChange();

        if (!result.Succeeded) return result;
        if (DebugLogger) Logger?.LogDebug($"Delete {typeof(TEntity).Name}");

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

        if (!result.Succeeded) return result;
        if (DebugLogger) Logger?.LogDebug($"Delete {result.EffectRows} {typeof(TEntity).Name} Entities");

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

        if (!result.Succeeded) return result;
        if (DebugLogger) Logger?.LogDebug($"Delete {typeof(TEntity).Name}");

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
            if (DebugLogger)
                Logger?.LogDebug($"Delete {result.EffectRows} {typeof(TEntity).Name} Entities");

        return result;
    }

    #endregion

    #region BatchDeleteEntity & BatchDeleteEntities

    /// <summary>
    ///     在存储中删除符合条件的实体
    /// </summary>
    /// <param name="predicate">查询表达式</param>
    /// <returns></returns>
    public StoreResult BatchDelete(Expression<Func<TEntity, bool>>? predicate)
    {
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
    ///     在存储中删除符合条件的实体
    /// </summary>
    /// <param name="predicate">查询表达式</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> BatchDeleteAsync(
        Expression<Func<TEntity, bool>>? predicate,
        CancellationToken cancellationToken = default)
    {
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

    #endregion
}