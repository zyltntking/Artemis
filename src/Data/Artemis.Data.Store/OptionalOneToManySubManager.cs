using Artemis.Data.Core;

namespace Artemis.Data.Store;

#region Interface

/// <summary>
///     可选的一对多模型管理器(子模型)
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TEntityInfo"></typeparam>
/// <typeparam name="TEntityPackage"></typeparam>
/// <typeparam name="TMainEntity"></typeparam>
public interface IOptionalOneToManySubManager<TMainEntity, TEntity, TEntityInfo, TEntityPackage> :
    IOptionalOneToManySubManager<TMainEntity, Guid, TEntity, Guid, TEntityInfo, TEntityPackage>,
    ISeparateManager<TEntityInfo, TEntityPackage>
    where TMainEntity : class, IKeySlot
    where TEntity : class, IKeySlot
    where TEntityInfo : class, IKeySlot
    where TEntityPackage : class;

/// <summary>
///     可选的一对多模型管理器(子模型)
/// </summary>
/// <typeparam name="TMainKey"></typeparam>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TEntityInfo"></typeparam>
/// <typeparam name="TEntityPackage"></typeparam>
/// <typeparam name="TMainEntity"></typeparam>
public interface IOptionalOneToManySubManager<TMainEntity, TMainKey, TEntity, TKey, TEntityInfo, TEntityPackage> :
    ISeparateManager<TKey, TEntityInfo, TEntityPackage>
    where TMainEntity : class, IKeySlot<TMainKey>
    where TMainKey : IEquatable<TMainKey>
    where TEntity : class, IKeySlot<TKey>
    where TEntityInfo : class, IKeySlot<TKey>
    where TEntityPackage : class
    where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     绑定关系
    /// </summary>
    /// <param name="mainKey"></param>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> BindRelationshipAsync(TMainKey mainKey, TEntity entity,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     绑定关系
    /// </summary>
    /// <param name="mainKey"></param>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> BindRelationshipAsync(TMainKey mainKey, TKey key, CancellationToken cancellationToken = default);

    /// <summary>
    ///     批量绑定关系
    /// </summary>
    /// <param name="mainKey"></param>
    /// <param name="entities"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> BindRelationshipAsync(TMainKey mainKey, IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     批量绑定关系
    /// </summary>
    /// <param name="mainKey"></param>
    /// <param name="keys"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> BindRelationshipAsync(TMainKey mainKey, IEnumerable<TKey> keys,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     解绑关系
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> UnbindRelationshipAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    ///     解绑关系
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> UnbindRelationshipAsync(TKey key, CancellationToken cancellationToken = default);

    /// <summary>
    ///     批量解绑关系
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> UnbindRelationshipAsync(IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     批量解绑关系
    /// </summary>
    /// <param name="keys"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> UnbindRelationshipAsync(IEnumerable<TKey> keys, CancellationToken cancellationToken = default);
}

#endregion

/// <summary>
///     可选的一对多模型管理器(子模型)
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TEntityInfo"></typeparam>
/// <typeparam name="TEntityPackage"></typeparam>
/// <typeparam name="TMainEntity"></typeparam>
public abstract class OptionalOneToManySubManager<TMainEntity, TEntity, TEntityInfo, TEntityPackage> :
    OptionalOneToManySubManager<TMainEntity, Guid, TEntity, Guid, TEntityInfo, TEntityPackage>,
    IOptionalOneToManySubManager<TMainEntity, TEntity, TEntityInfo, TEntityPackage>
    where TMainEntity : class, IKeySlot
    where TEntity : class, IKeySlot
    where TEntityInfo : class, IKeySlot
    where TEntityPackage : class
{
    /// <summary>
    ///     可选的一对多模型管理器构造
    /// </summary>
    protected OptionalOneToManySubManager(
        IStore<TMainEntity> mainEntityStore,
        IStore<TEntity> entityStore) : base(mainEntityStore, entityStore)
    {
    }
}

/// <summary>
///     可选的一对多模型管理器(子模型)
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TEntityInfo"></typeparam>
/// <typeparam name="TEntityPackage"></typeparam>
/// <typeparam name="TMainKey"></typeparam>
/// <typeparam name="TMainEntity"></typeparam>
public abstract class OptionalOneToManySubManager<TMainEntity, TMainKey, TEntity, TKey, TEntityInfo, TEntityPackage> :
    SeparateManager<TEntity, TKey, TEntityInfo, TEntityPackage>,
    IOptionalOneToManySubManager<TMainEntity, TMainKey, TEntity, TKey, TEntityInfo, TEntityPackage>
    where TMainEntity : class, IKeySlot<TMainKey>
    where TMainKey : IEquatable<TMainKey>
    where TEntity : class, IKeySlot<TKey>
    where TEntityInfo : class, IKeySlot<TKey>
    where TEntityPackage : class
    where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     可选的一对多模型管理器构造
    /// </summary>
    protected OptionalOneToManySubManager(
        IStore<TMainEntity, TMainKey> mainEntityStore,
        IStore<TEntity, TKey> entityStore) : base(entityStore)
    {
        MainEntityStore = mainEntityStore;
    }

    #region StoreAccess

    /// <summary>
    ///     主模型存储访问器
    /// </summary>
    protected IStore<TMainEntity, TMainKey> MainEntityStore { get; }

    #endregion

    #region Overrides

    /// <summary>
    ///     释放托管的Store
    /// </summary>
    protected override void StoreDispose()
    {
        MainEntityStore.Dispose();

        base.StoreDispose();
    }

    #endregion

    /// <summary>
    ///     设置模型的关联键
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="mainKey"></param>
    protected abstract void SetSubEntityRelationalKey(TEntity entity, TMainKey mainKey);

    /// <summary>
    ///     移除模型的关联键
    /// </summary>
    /// <param name="entity"></param>
    protected abstract void RemoveEntityRelationalKey(TEntity entity);

    #region Implementation of IOptionalOneToManyManager<TMainKey,TEntity,TKey,TEntityInfo,TEntityPackage>

    /// <summary>
    ///     绑定关系
    /// </summary>
    /// <param name="mainKey"></param>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> BindRelationshipAsync(TMainKey mainKey, TEntity entity,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var exists = await MainEntityStore.ExistsAsync(mainKey, cancellationToken);

        if (!exists) return StoreResult.EntityNotFoundFailed(typeof(TMainEntity).Name, mainKey.IdToString()!);

        SetSubEntityRelationalKey(entity, mainKey);

        return await EntityStore.UpdateAsync(entity, cancellationToken);
    }

    /// <summary>
    ///     绑定关系
    /// </summary>
    /// <param name="mainKey"></param>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> BindRelationshipAsync(TMainKey mainKey, TKey key,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var exists = await MainEntityStore.ExistsAsync(mainKey, cancellationToken);

        if (!exists) return StoreResult.EntityNotFoundFailed(typeof(TMainEntity).Name, mainKey.IdToString()!);

        var entity = await EntityStore.FindEntityAsync(key, cancellationToken);

        if (entity is not null)
        {
            SetSubEntityRelationalKey(entity, mainKey);

            return await EntityStore.UpdateAsync(entity, cancellationToken);
        }

        return StoreResult.EntityNotFoundFailed(typeof(TEntity).Name, key.IdToString()!);
    }

    /// <summary>
    ///     批量绑定关系
    /// </summary>
    /// <param name="mainKey"></param>
    /// <param name="entities"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> BindRelationshipAsync(TMainKey mainKey, IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var exists = await MainEntityStore.ExistsAsync(mainKey, cancellationToken);

        if (!exists) return StoreResult.EntityNotFoundFailed(typeof(TMainEntity).Name, mainKey.IdToString()!);

        var entityList = entities.ToList();

        foreach (var entity in entityList) SetSubEntityRelationalKey(entity, mainKey);

        return await EntityStore.UpdateAsync(entityList, cancellationToken);
    }

    /// <summary>
    ///     批量绑定关系
    /// </summary>
    /// <param name="mainKey"></param>
    /// <param name="keys"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> BindRelationshipAsync(TMainKey mainKey, IEnumerable<TKey> keys,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var exists = await MainEntityStore.ExistsAsync(mainKey, cancellationToken);

        if (!exists) return StoreResult.EntityNotFoundFailed(typeof(TMainEntity).Name, mainKey.IdToString()!);

        var entities = await EntityStore.FindEntitiesAsync(keys, cancellationToken);

        var entityList = entities.ToList();

        foreach (var entity in entityList) SetSubEntityRelationalKey(entity, mainKey);

        return await EntityStore.UpdateAsync(entityList, cancellationToken);
    }

    /// <summary>
    ///     解绑关系
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<StoreResult> UnbindRelationshipAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        RemoveEntityRelationalKey(entity);

        return EntityStore.UpdateAsync(entity, cancellationToken);
    }

    /// <summary>
    ///     解绑关系
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> UnbindRelationshipAsync(TKey key, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var entity = await EntityStore.FindEntityAsync(key, cancellationToken);

        if (entity is not null)
        {
            RemoveEntityRelationalKey(entity);

            return await EntityStore.UpdateAsync(entity, cancellationToken);
        }

        return StoreResult.EntityNotFoundFailed(typeof(TEntity).Name, key.IdToString()!);
    }

    /// <summary>
    ///     批量解绑关系
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<StoreResult> UnbindRelationshipAsync(IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var entityList = entities.ToList();

        foreach (var entity in entityList) RemoveEntityRelationalKey(entity);

        return EntityStore.UpdateAsync(entityList, cancellationToken);
    }

    /// <summary>
    ///     批量解绑关系
    /// </summary>
    /// <param name="keys"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> UnbindRelationshipAsync(IEnumerable<TKey> keys,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var entities = await EntityStore.FindEntitiesAsync(keys, cancellationToken);

        var entityList = entities.ToList();

        foreach (var entity in entityList) RemoveEntityRelationalKey(entity);

        return await EntityStore.UpdateAsync(entityList, cancellationToken);
    }

    #endregion
}