using Artemis.Data.Core;
using Artemis.Data.Store.Extensions;
using Mapster;

namespace Artemis.Data.Store;

#region Interface

/// <summary>
/// 独立模型管理接口
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TEntityInfo"></typeparam>
/// <typeparam name="TEntityPackage"></typeparam>
public interface ISeparateManager<TEntity, TEntityInfo, TEntityPackage> : ISeparateManager<TEntity, Guid, TEntityInfo,
    TEntityPackage>
    where TEntity : class, IKeySlot<Guid>
    where TEntityInfo : class, IKeySlot<Guid>
    where TEntityPackage : class;

/// <summary>
    ///     独立模型接口
    /// </summary>
public interface ISeparateManager<TEntity, TKey, TEntityInfo, TEntityPackage> : IManager
    where TEntity : class, IKeySlot<TKey>
    where TEntityInfo : class, IKeySlot<TKey>
    where TEntityPackage : class
    where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     读取实体信息
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TEntityInfo?> ReadEntityInfoAsync(TKey key, CancellationToken cancellationToken = default);

    /// <summary>
    ///     创建实体
    /// </summary>
    /// <param name="package"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> CreateEntityAsync(TEntityPackage package, CancellationToken cancellationToken = default);

    /// <summary>
    ///     批量创建实体
    /// </summary>
    /// <param name="packages"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> BatchCreateEntityAsync(IEnumerable<TEntityPackage> packages,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     更新实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="package"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> UpdateEntityAsync(TKey key, TEntityPackage package,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     批量更新实体
    /// </summary>
    /// <param name="dictionary"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> BatchUpdateEntityAsync(IDictionary<TKey, TEntityPackage> dictionary,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     删除实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> DeleteEntityAsync(TKey key, CancellationToken cancellationToken = default);

    /// <summary>
    ///     批量删除实体
    /// </summary>
    /// <param name="keys"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> BatchDeleteEntityAsync(IEnumerable<TKey> keys, CancellationToken cancellationToken = default);
}

#endregion

/// <summary>
/// 单独模型管理器
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TEntityInfo"></typeparam>
/// <typeparam name="TEntityPackage"></typeparam>
public abstract class SeparateManager<TEntity, TEntityInfo, TEntityPackage> : 
    SeparateManager<TEntity, Guid, TEntityInfo,
    TEntityPackage>, ISeparateManager<TEntity, TEntityInfo, TEntityPackage>
    where TEntity : class, IKeySlot<Guid>
    where TEntityInfo : class, IKeySlot<Guid>
    where TEntityPackage : class
{
    /// <summary>
    /// 独立模型管理器构造
    /// </summary>
    /// <param name="entityStore"></param>
    protected SeparateManager(IStore<TEntity, Guid> entityStore) : base(entityStore)
    {
    }
}

/// <summary>
///     单独模型
/// </summary>
public abstract class SeparateManager<TEntity, TKey, TEntityInfo, TEntityPackage> : Manager,
    ISeparateManager<TEntity, TKey, TEntityInfo, TEntityPackage>
    where TEntity : class, IKeySlot<TKey>
    where TEntityInfo : class, IKeySlot<TKey>
    where TEntityPackage : class
    where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     独立模型管理器构造
    /// </summary>
    protected SeparateManager(IStore<TEntity, TKey> entityStore)
    {
        EntityStore = entityStore;
    }

    #region StoreAccess

    /// <summary>
    ///     单独实体存储
    /// </summary>
    protected virtual IStore<TEntity, TKey> EntityStore { get; }

    #endregion

    #region Overrides of Manager

    /// <summary>
    ///     释放托管的Store
    /// </summary>
    protected override void StoreDispose()
    {
        EntityStore.Dispose();
    }

    #endregion

    #region Implementation of ISeparateManager<TEntity,TKey,TEntityInfo,TEntityPackage>

    /// <summary>
    ///     读取实体信息
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<TEntityInfo?> ReadEntityInfoAsync(TKey key, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        return EntityStore.FindMapEntityAsync<TEntityInfo>(key, cancellationToken);
    }

    /// <summary>
    ///     创建实体
    /// </summary>
    /// <param name="package"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<StoreResult> CreateEntityAsync(TEntityPackage package, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var entity = MapNewEntity(package);

        return EntityStore.CreateAsync(entity, cancellationToken);
    }

    /// <summary>
    ///     批量创建实体
    /// </summary>
    /// <param name="packages"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<StoreResult> BatchCreateEntityAsync(IEnumerable<TEntityPackage> packages,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var entities = packages.Select(MapNewEntity);

        return EntityStore.CreateAsync(entities, cancellationToken);
    }

    /// <summary>
    ///     更新实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="package"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> UpdateEntityAsync(TKey key, TEntityPackage package,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var entity = await EntityStore.FindEntityAsync(key, cancellationToken);

        if (entity is not null)
            return await EntityStore.UpdateAsync(MapOverEntity(entity, package), cancellationToken);

        return StoreResult.EntityNotFoundFailed(typeof(TEntity).Name, key.IdToString()!);
    }

    /// <summary>
    ///     批量更新实体
    /// </summary>
    /// <param name="dictionary"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> BatchUpdateEntityAsync(IDictionary<TKey, TEntityPackage> dictionary,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var ids = dictionary.Keys;

        var entities = await EntityStore.FindEntitiesAsync(ids, cancellationToken);

        var entityList = entities.ToList();

        if (entityList.Count > 0)
        {
            entities = entityList.Select(entity =>
            {
                var package = dictionary[entity.Id];

                return MapOverEntity(entity, package);
            }).ToList();

            return await EntityStore.UpdateAsync(entities, cancellationToken);
        }

        var flag = string.Join(',', ids.Select(item => item.IdToString()));

        return StoreResult.EntityFoundFailed(typeof(TEntity).Name, flag);
    }

    /// <summary>
    ///     删除实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> DeleteEntityAsync(TKey key, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var entity = await EntityStore.FindEntityAsync(key, cancellationToken);

        if (entity is not null)
            return await EntityStore.DeleteAsync(entity, cancellationToken);

        return StoreResult.EntityNotFoundFailed(typeof(TEntity).Name, key.IdToString()!);
    }

    /// <summary>
    ///     批量删除实体
    /// </summary>
    /// <param name="keys"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> BatchDeleteEntityAsync(IEnumerable<TKey> keys,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var keyList = keys.ToList();

        var entities = await EntityStore.FindEntitiesAsync(keyList, cancellationToken);

        var entityList = entities.ToList();

        if (entityList.Any())
            return await EntityStore.DeleteAsync(entityList, cancellationToken);

        var flag = string.Join(',', keyList.Select(id => id.IdToString()));

        return StoreResult.EntityNotFoundFailed(typeof(TEntity).Name, flag);
    }

    #endregion

    #region EntityMap

    /// <summary>
    ///     映射到新实体
    /// </summary>
    /// <param name="package"></param>
    /// <returns></returns>
    protected virtual TEntity MapNewEntity(TEntityPackage package)
    {
        return Instance.CreateInstance<TEntity, TEntityPackage>(package);
    }

    /// <summary>
    ///     覆盖实体
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="package"></param>
    /// <returns></returns>
    protected virtual TEntity MapOverEntity(TEntity entity, TEntityPackage package)
    {
        return package.Adapt(entity);
    }

    #endregion
}