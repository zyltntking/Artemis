using Artemis.Data.Core;
using Artemis.Data.Store.Extensions;
using Mapster;

namespace Artemis.Data.Store;

#region Interface

/// <summary>
///     独立模型管理接口
/// </summary>
/// <typeparam name="TEntityInfo"></typeparam>
/// <typeparam name="TEntityPackage"></typeparam>
public interface ISeparateManager<TEntityInfo, TEntityPackage> : ISeparateManager<Guid, TEntityInfo,
    TEntityPackage>
    where TEntityInfo : class, IKeySlot
    where TEntityPackage : class;

/// <summary>
///     独立模型接口
/// </summary>
public interface ISeparateManager<TKey, TEntityInfo, TEntityPackage> : IManager
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
///     单独模型管理器
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TEntityInfo"></typeparam>
/// <typeparam name="TEntityPackage"></typeparam>
public abstract class SeparateManager<TEntity, TEntityInfo, TEntityPackage> :
    SeparateManager<TEntity, Guid, TEntityInfo,
        TEntityPackage>, ISeparateManager<TEntityInfo, TEntityPackage>
    where TEntity : class, IKeySlot
    where TEntityInfo : class, IKeySlot
    where TEntityPackage : class
{
    /// <summary>
    ///     独立模型管理器构造
    /// </summary>
    /// <param name="entityStore"></param>
    protected SeparateManager(IStore<TEntity> entityStore) : base(entityStore)
    {
    }
}

/// <summary>
///     单独模型
/// </summary>
public abstract class SeparateManager<TEntity, TKey, TEntityInfo, TEntityPackage> : Manager,
    ISeparateManager<TKey, TEntityInfo, TEntityPackage>
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
    protected IStore<TEntity, TKey> EntityStore { get; }

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
    public async Task<StoreResult> CreateEntityAsync(TEntityPackage package,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var entity = await MapNewEntity(package, 1, cancellationToken);

        return await EntityStore.CreateAsync(entity, cancellationToken);
    }

    /// <summary>
    ///     批量创建实体
    /// </summary>
    /// <param name="packages"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> BatchCreateEntityAsync(IEnumerable<TEntityPackage> packages,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var entities = new List<TEntity>();

        var index = 1;

        foreach (var package in packages)
        {
            var entity = await MapNewEntity(package, index, cancellationToken);
            entities.Add(entity);
            index++;
        }

        return await EntityStore.CreateAsync(entities, cancellationToken);
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
        {
            entity = await MapOverEntity(entity, package, 1, cancellationToken);

            return await EntityStore.UpdateAsync(entity, cancellationToken);
        }

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

        var targetEntityList = new List<TEntity>();

        if (entityList.Count > 0)
        {
            var index = 1;

            foreach (var entity in entityList)
            {
                var package = dictionary[entity.Id];
                var targetEntity = await MapOverEntity(entity, package, index, cancellationToken);
                targetEntityList.Add(targetEntity);
                index++;
            }

            return await EntityStore.UpdateAsync(targetEntityList, cancellationToken);
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
    ///     忽略空值配置
    /// </summary>
    private TypeAdapterConfig? _ignoreNullConfig;

    /// <summary>
    ///     忽略空值配置
    /// </summary>
    /// <returns></returns>
    private TypeAdapterConfig IgnoreNullConfig
    {
        get
        {
            if (_ignoreNullConfig != null)
                return _ignoreNullConfig;
            _ignoreNullConfig = new TypeAdapterConfig();

            _ignoreNullConfig
                .NewConfig<TEntity, TEntityPackage>()
                .IgnoreNullValues(true);

            return _ignoreNullConfig;
        }
    }

    /// <summary>
    ///     映射到新实体
    /// </summary>
    /// <param name="package"></param>
    /// <param name="loopIndex"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected virtual Task<TEntity> MapNewEntity(
        TEntityPackage package,
        int loopIndex,
        CancellationToken cancellationToken = default)
    {
        var entity = Instance.CreateInstance<TEntity, TEntityPackage>(package);
        return Task.FromResult(entity);
    }

    /// <summary>
    ///     覆盖实体
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="package"></param>
    /// <param name="loopIndex"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected virtual Task<TEntity> MapOverEntity(
        TEntity entity,
        TEntityPackage package,
        int loopIndex,
        CancellationToken cancellationToken = default)
    {
        entity = package.Adapt(entity, IgnoreNullConfig);
        return Task.FromResult(entity);
    }

    #endregion
}