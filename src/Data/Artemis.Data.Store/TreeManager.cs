using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Data.Store;

#region Interface

/// <summary>
/// 树管理器
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TEntityInfo"></typeparam>
/// <typeparam name="TEntityInfoTree"></typeparam>
/// <typeparam name="TEntityPackage"></typeparam>
/// <typeparam name="TParentKey"></typeparam>
public interface ITreeManager<TEntity, TKey, TParentKey, TEntityInfo, TEntityInfoTree, TEntityPackage> : 
    ISeparateManager<TEntity, TKey, TEntityInfo, TEntityPackage>
    where TEntity : class, IKeySlot<TKey>, IParentKeySlot<TParentKey>, ITreeSlot<TEntity, TKey, TParentKey>
    where TEntityInfo : class, IKeySlot<TKey>, IParentKeySlot<TParentKey>
    where TEntityInfoTree : class, IKeySlot<TKey>, IParentKeySlot<TParentKey>, ITreeInfoSlot<TEntityInfoTree, TKey, TParentKey>
    where TEntityPackage : class
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// 获取实体信息树
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TEntityInfoTree?> GetEntityInfoTreeAsync(TKey key, CancellationToken cancellationToken = default);

    /// <summary>
    /// 创建父实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="package"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> CreateParentEntityAsync(TKey key, TEntityPackage package, CancellationToken cancellationToken = default);

    /// <summary>
    /// 创建子实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="package"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> CreateChildEntityAsync(TKey key, TEntityPackage package, CancellationToken cancellationToken = default);

    /// <summary>
    /// 批量创建子实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="packages"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> BatchCreateChildEntityAsync(TKey key, IEnumerable<TEntityPackage> packages, CancellationToken cancellationToken = default);

    /// <summary>
    /// 添加父实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="parentKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> AddParentEntityAsync(TKey key, TKey parentKey, CancellationToken cancellationToken = default);

    /// <summary>
    /// 添加子实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="childKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> AddChildEntityAsync(TKey key, TKey childKey, CancellationToken cancellationToken = default);

    /// <summary>
    /// 批量添加子实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="childrenKeys"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> BatchAddChildEntityAsync(TKey key, IEnumerable<TKey> childrenKeys, CancellationToken cancellationToken = default);

    /// <summary>
    /// 删除子实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="childKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> DeleteChildEntityAsync(TKey key, TKey childKey, CancellationToken cancellationToken = default);

    /// <summary>
    /// 批量删除子实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="childrenKeys"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> BatchDeleteChildEntityAsync(TKey key, IEnumerable<TKey> childrenKeys, CancellationToken cancellationToken = default);

    /// <summary>
    /// 移除子实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="childKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> RemoveChildEntityAsync(TKey key, TKey childKey, CancellationToken cancellationToken = default);

    /// <summary>
    /// 批量移除子实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="childrenKeys"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> BatchRemoveChildEntityAsync(TKey key, IEnumerable<TKey> childrenKeys, CancellationToken cancellationToken = default);
}

#endregion


/// <summary>
/// 树管理器
/// </summary>
public abstract class TreeManager<TEntity, TKey, TParentKey, TEntityInfo, TEntityInfoTree, TEntityPackage> : 
    SeparateManager<TEntity, TKey, TEntityInfo, TEntityPackage>, 
    ITreeManager<TEntity, TKey, TParentKey, TEntityInfo, TEntityInfoTree, TEntityPackage>
    where TEntity : class, IKeySlot<TKey>, IParentKeySlot<TParentKey>, ITreeSlot<TEntity, TKey, TParentKey>
    where TEntityInfo : class, IKeySlot<TKey>, IParentKeySlot<TParentKey>
    where TEntityInfoTree : class, IKeySlot<TKey>, IParentKeySlot<TParentKey>, ITreeInfoSlot<TEntityInfoTree, TKey, TParentKey>
    where TEntityPackage : class
    where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     树模型管理器构造
    /// </summary>
    protected TreeManager(
        IStore<TEntity, TKey> entityStore) : base(entityStore)
    {
    }

    #region Implementation of ITreeManager<TEntity,TKey,TEntityInfo,TEntityInfoTree,TEntityPackage>

    /// <summary>
    /// 获取实体信息树
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TEntityInfoTree?> GetEntityInfoTreeAsync(TKey key, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var tree = await EntityStore.KeyMatchQuery(key)
            .Include(entity => entity.Children)
            .ProjectToType<TEntityInfoTree>()
            .SingleOrDefaultAsync(cancellationToken);

        return tree ?? throw new EntityNotFoundException(typeof(TEntity).Name, key.IdToString()!);
    }

    /// <summary>
    /// 创建父实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="package"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> CreateParentEntityAsync(TKey key, TEntityPackage package, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var entity = await EntityStore.FindEntityAsync(key, cancellationToken);

        if (entity is not null)
        {
            if (entity.ParentId is null)
            {
                var parent = MapNewEntity(package);

                await EntityStore.CreateAsync(parent, cancellationToken);

                entity.ParentId = MapToParentKey(parent.Id);

                return await EntityStore.UpdateAsync(entity, cancellationToken);
            }

            return StoreResult.EntityFoundFailed(typeof(TEntity).Name, MapToKey(entity.ParentId).IdToString()!);

        }

        return StoreResult.EntityNotFoundFailed(typeof(TEntity).Name, key.IdToString()!);
    }

    /// <summary>
    /// 创建子实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="package"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> CreateChildEntityAsync(TKey key, TEntityPackage package, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var entity = await EntityStore.FindEntityAsync(key, cancellationToken);

        if (entity is not null)
        {
            var child = MapNewEntity(package);

            child.ParentId = MapToParentKey(key);

            return await EntityStore.CreateAsync(child, cancellationToken);
        }

        return StoreResult.EntityNotFoundFailed(typeof(TEntity).Name, key.IdToString()!);
    }

    /// <summary>
    /// 批量创建子实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="packages"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> BatchCreateChildEntityAsync(TKey key, IEnumerable<TEntityPackage> packages, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var entity = await EntityStore.FindEntityAsync(key, cancellationToken);

        if (entity is not null)
        {
            var children = packages.Select(package =>
            {
                var child = MapNewEntity(package);

                child.ParentId = MapToParentKey(key);

                return child;
            });

            return await EntityStore.CreateAsync(children, cancellationToken);
        }

        return StoreResult.EntityNotFoundFailed(typeof(TEntity).Name, key.IdToString()!);
    }

    /// <summary>
    /// 添加父实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="parentKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> AddParentEntityAsync(TKey key, TKey parentKey, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var entity = await EntityStore.FindEntityAsync(key, cancellationToken);

        if (entity is not null)
        {
            if (entity.ParentId is null)
            {
                var parentExists = await EntityStore.ExistsAsync(parentKey, cancellationToken);

                if (parentExists)
                {
                    entity.ParentId = MapToParentKey(parentKey);

                    return await EntityStore.UpdateAsync(entity, cancellationToken);
                }

                return StoreResult.EntityNotFoundFailed(typeof(TEntity).Name, parentKey.IdToString()!);
            }

            return StoreResult.EntityFoundFailed(typeof(TEntity).Name, MapToKey(entity.ParentId).IdToString()!);

        }

        return StoreResult.EntityNotFoundFailed(typeof(TEntity).Name, key.IdToString()!);
    }

    /// <summary>
    /// 添加子实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="childKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> AddChildEntityAsync(TKey key, TKey childKey, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var entity = await EntityStore.FindEntityAsync(key, cancellationToken);

        if (entity is not null)
        {
            var child = await EntityStore.FindEntityAsync(childKey, cancellationToken);

            if (child is not null)
            {
                child.ParentId = MapToParentKey(key);

                return await EntityStore.UpdateAsync(child, cancellationToken);
            }

            return StoreResult.EntityNotFoundFailed(typeof(TEntity).Name, key.IdToString()!);
        }

        return StoreResult.EntityNotFoundFailed(typeof(TEntity).Name, key.IdToString()!);
    }

    /// <summary>
    /// 批量添加子实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="childrenKeys"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> BatchAddChildEntityAsync(TKey key, IEnumerable<TKey> childrenKeys, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var entity = await EntityStore.FindEntityAsync(key, cancellationToken);

        if (entity is not null)
        {
            var keyList = childrenKeys.ToList();

            var children = await EntityStore.FindEntitiesAsync(keyList, cancellationToken);

            var childrenList = children.ToList();

            if (childrenList.Any())
            {
                foreach (var child in childrenList)
                {
                    child.ParentId = MapToParentKey(key);
                }

                return await EntityStore.UpdateAsync(childrenList, cancellationToken);
            }

            return StoreResult.EntityNotFoundFailed(typeof(TEntity).Name, string.Join(",", keyList.Select(item => item.IdToString())));
        }

        return StoreResult.EntityNotFoundFailed(typeof(TEntity).Name, key.IdToString()!);
    }

    /// <summary>
    /// 删除子实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="childKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> DeleteChildEntityAsync(TKey key, TKey childKey, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var entity = await EntityStore.FindEntityAsync(key, cancellationToken);

        if (entity is not null)
        {
            var child = await EntityStore.FindEntityAsync(childKey, cancellationToken);

            if (child is not null)
            {
                return await EntityStore.DeleteAsync(childKey, cancellationToken);
            }

            return StoreResult.EntityNotFoundFailed(typeof(TEntity).Name, key.IdToString()!);
        }

        return StoreResult.EntityNotFoundFailed(typeof(TEntity).Name, key.IdToString()!);
    }

    /// <summary>
    /// 批量删除子实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="childrenKeys"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> BatchDeleteChildEntityAsync(TKey key, IEnumerable<TKey> childrenKeys, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var entity = await EntityStore.FindEntityAsync(key, cancellationToken);

        if (entity is not null)
        {
            var keyList = childrenKeys.ToList();

            var children = await EntityStore.FindEntitiesAsync(keyList, cancellationToken);

            var childrenList = children.ToList();

            if (childrenList.Any())
            {
                return await EntityStore.DeleteAsync(keyList, cancellationToken);
            }

            return StoreResult.EntityNotFoundFailed(typeof(TEntity).Name, string.Join(",", keyList.Select(item => item.IdToString())));
        }

        return StoreResult.EntityNotFoundFailed(typeof(TEntity).Name, key.IdToString()!);
    }

    /// <summary>
    /// 移除子实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="childKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> RemoveChildEntityAsync(TKey key, TKey childKey, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var childEntity = await EntityStore.FindEntityAsync(childKey, cancellationToken);

        if (childEntity is not null && childEntity.ParentId is not null)
        {
            childEntity.ParentId = default!;

            return await EntityStore.UpdateAsync(childEntity, cancellationToken);
        }

        return StoreResult.EntityNotFoundFailed(typeof(TEntity).Name, childKey.IdToString()!);
    }

    /// <summary>
    /// 批量移除子实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="childrenKeys"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> BatchRemoveChildEntityAsync(TKey key, IEnumerable<TKey> childrenKeys, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var keyList = childrenKeys.ToList();

        var children = await EntityStore.FindEntitiesAsync(keyList, cancellationToken);

        var childrenList = children.ToList();

        if (childrenList.Any())
        {
            var updateList = childrenList
                .Where(item => item.ParentId is not null && item.ParentId.Equals(key))
                .Select(item =>
                {
                    item.ParentId = default!;
                    return item;
                })
                .ToList();

            return await EntityStore.UpdateAsync(updateList, cancellationToken);
        }

        return StoreResult.EntityNotFoundFailed(typeof(TEntity).Name, string.Join(",", keyList.Select(item => item.IdToString())));
    }

    #endregion

    /// <summary>
    /// 映射到键
    /// </summary>
    /// <param name="parentKey">父级键</param>
    /// <returns></returns>
    protected abstract TKey MapToKey(TParentKey parentKey);

    /// <summary>
    /// 映射到父级键
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    protected abstract TParentKey MapToParentKey(TKey key);
}