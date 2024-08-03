using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Data.Store;

#region Interface

/// <summary>
///     树管理器
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TEntityInfo"></typeparam>
/// <typeparam name="TEntityInfoTree"></typeparam>
/// <typeparam name="TEntityPackage"></typeparam>
public interface ITreeManager<TEntity, TEntityInfo, TEntityInfoTree, TEntityPackage> :
    ITreeManager<TEntity, Guid, Guid?, TEntityInfo, TEntityInfoTree, TEntityPackage>,
    ISeparateManager<TEntity, TEntityInfo, TEntityPackage>
    where TEntity : class, IKeySlot, IParentKeySlot, ITreeSlot<TEntity>
    where TEntityInfo : class, IKeySlot, IParentKeySlot
    where TEntityInfoTree : class, TEntityInfo, IKeySlot, IParentKeySlot, ITreeInfoSlot<TEntityInfoTree>
    where TEntityPackage : class;

/// <summary>
///     树管理器
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TEntityInfo"></typeparam>
/// <typeparam name="TEntityInfoTree"></typeparam>
/// <typeparam name="TEntityPackage"></typeparam>
/// <typeparam name="TParentKey"></typeparam>
public interface ITreeManager<TEntity, TKey, TParentKey, TEntityInfo, TEntityInfoTree, TEntityPackage> :
    ISeparateManager<TEntity, TKey, TEntityInfo, TEntityPackage>
    where TEntity : class, IKeySlot<TKey>, IParentKeySlot<TParentKey?>, ITreeSlot<TEntity, TKey, TParentKey?>
    where TEntityInfo : class, IKeySlot<TKey>, IParentKeySlot<TParentKey?>
    where TEntityInfoTree : class, TEntityInfo, IKeySlot<TKey>, IParentKeySlot<TParentKey?>, ITreeInfoSlot<TEntityInfoTree, TKey, TParentKey?>
    where TEntityPackage : class
    where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     获取实体信息树
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TEntityInfoTree?> GetEntityInfoTreeAsync(TKey key, CancellationToken cancellationToken = default);

    /// <summary>
    ///     创建子实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="package"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> CreateChildEntityAsync(TKey key, TEntityPackage package,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     批量创建子实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="packages"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> BatchCreateChildEntityAsync(TKey key, IEnumerable<TEntityPackage> packages,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     添加子实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="childKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> AddChildEntityAsync(TKey key, TKey childKey, CancellationToken cancellationToken = default);

    /// <summary>
    ///     批量添加子实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="childrenKeys"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> BatchAddChildEntityAsync(TKey key, IEnumerable<TKey> childrenKeys,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     删除子实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="childKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> DeleteChildEntityAsync(TKey key, TKey childKey, CancellationToken cancellationToken = default);

    /// <summary>
    ///     批量删除子实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="childrenKeys"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> BatchDeleteChildEntityAsync(TKey key, IEnumerable<TKey> childrenKeys,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     移除子实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="childKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> RemoveChildEntityAsync(TKey key, TKey childKey, CancellationToken cancellationToken = default);

    /// <summary>
    ///     批量移除子实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="childrenKeys"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> BatchRemoveChildEntityAsync(TKey key, IEnumerable<TKey> childrenKeys,
        CancellationToken cancellationToken = default);
}

#endregion

/// <summary>
///     树管理器
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TEntityInfo"></typeparam>
/// <typeparam name="TEntityInfoTree"></typeparam>
/// <typeparam name="TEntityPackage"></typeparam>
public abstract class TreeManager<TEntity, TEntityInfo, TEntityInfoTree, TEntityPackage> :
    TreeManager<TEntity, Guid, Guid?, TEntityInfo, TEntityInfoTree, TEntityPackage>,
    ITreeManager<TEntity, TEntityInfo, TEntityInfoTree, TEntityPackage>
    where TEntity : class, IKeySlot, IParentKeySlot, ITreeSlot<TEntity>
    where TEntityInfo : class, IKeySlot, IParentKeySlot
    where TEntityInfoTree : class, TEntityInfo, IKeySlot, IParentKeySlot, ITreeInfoSlot<TEntityInfoTree>
    where TEntityPackage : class
{
    /// <summary>
    ///     树模型管理器构造
    /// </summary>
    protected TreeManager(IStore<TEntity> entityStore) : base(entityStore)
    {
    }

    #region Overrides of TreeManager<TEntity,Guid,Guid?,TEntityInfo,TEntityInfoTree,TEntityPackage>

    /// <summary>
    ///     映射到父级键
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    protected sealed override Guid? MapToParentKey(Guid key)
    {
        return key;
    }

    #endregion
}

/// <summary>
///     树管理器
/// </summary>
public abstract class TreeManager<TEntity, TKey, TParentKey, TEntityInfo, TEntityInfoTree, TEntityPackage> :
    SeparateManager<TEntity, TKey, TEntityInfo, TEntityPackage>,
    ITreeManager<TEntity, TKey, TParentKey?, TEntityInfo, TEntityInfoTree, TEntityPackage>
    where TEntity : class, IKeySlot<TKey>, IParentKeySlot<TParentKey?>, ITreeSlot<TEntity, TKey, TParentKey?>
    where TEntityInfo : class, IKeySlot<TKey>, IParentKeySlot<TParentKey?>
    where TEntityInfoTree : class, TEntityInfo, IKeySlot<TKey>, IParentKeySlot<TParentKey?>,
    ITreeInfoSlot<TEntityInfoTree, TKey, TParentKey?>
    where TEntityPackage : class
    where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     树模型管理器构造
    /// </summary>
    protected TreeManager(IStore<TEntity, TKey> entityStore) : base(entityStore)
    {
    }

    /// <summary>
    ///     映射到父级键
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    protected abstract TParentKey MapToParentKey(TKey key);

    /// <summary>
    /// 获取非根节点的树节点列表
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected abstract Task<List<TEntityInfo>> FetchNonRootTreeNodeList(TKey key, CancellationToken cancellationToken);

    /// <summary>
    /// 获取生成树节点列表
    /// </summary>
    /// <param name="key">标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    private async Task<List<TEntityInfo>> FetchTreeNodeListAsync(TKey key, CancellationToken cancellationToken)
    {
        var parentKey = await EntityStore
            .KeyMatchQuery(key)
            .Select(entity => entity.ParentId)
            .FirstOrDefaultAsync(cancellationToken);

        if (parentKey == null)
        {
            return await EntityStore.EntityQuery.ProjectToType<TEntityInfo>().ToListAsync(cancellationToken);
        }

        return await FetchNonRootTreeNodeList(key, cancellationToken);
    }

    /// <summary>
    /// 递归生成树
    /// </summary>
    /// <param name="rootKey">根标识</param>
    /// <param name="nodeList">节点列表</param>
    /// <returns></returns>
    private TEntityInfoTree GenerateTree(TKey rootKey, List<TEntityInfo> nodeList)
    {
        var root = nodeList.FirstOrDefault(item => item.Id.Equals(rootKey));

        if (root is null) 
            return default!;

        var tree = root.Adapt<TEntityInfoTree>();

        var children = nodeList
            .Where(item => item.ParentId is not null && item.ParentId.Equals(tree.Id))
            .Select(item => GenerateTree(item.Id, nodeList))
            .ToList();

        tree.Children = children;

        return tree;
    }

    /// <summary>
    /// 在添加子节点之前
    /// </summary>
    /// <param name="parent">父节点</param>
    /// <param name="child">子节点</param>
    protected virtual void BeforeAddChildNode(TEntity parent, TEntity child)
    {
    }

    /// <summary>
    ///     在添加子节点之后
    /// </summary>
    /// <param name="parent">父节点</param>
    /// <param name="cancellationToken"></param>
    protected virtual Task AfterAddChildNode(TEntity parent, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    ///     在移除子节点之前
    /// </summary>
    /// <param name="child">子节点</param>
    protected virtual void BeforeRemoveChildNode(TEntity child)
    {
    }

    /// <summary>
    ///     在移除子节点之后
    /// </summary>
    /// <param name="parent">父节点</param>
    /// <param name="cancellationToken"></param>
    protected virtual Task AfterRemoveChildNode(TEntity parent, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    #region Implementation of ITreeManager<TEntity,TKey,TEntityInfo,TEntityInfoTree,TEntityPackage>

    /// <summary>
    ///     获取实体信息树
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TEntityInfoTree?> GetEntityInfoTreeAsync(TKey key, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var infoList = await FetchTreeNodeListAsync(key, cancellationToken);

        var tree = GenerateTree(key, infoList);

        return tree ?? throw new EntityNotFoundException(typeof(TEntity).Name, key.IdToString()!);
    }

    /// <summary>
    ///     创建子实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="package"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> CreateChildEntityAsync(TKey key, TEntityPackage package,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var entity = await EntityStore.FindEntityAsync(key, cancellationToken);

        if (entity is not null)
        {
            var child = MapNewEntity(package);

            child.ParentId = MapToParentKey(key);

            BeforeAddChildNode(entity, child);

            var result = await EntityStore.CreateAsync(child, cancellationToken);

            if (result.Succeeded) await AfterAddChildNode(entity, cancellationToken);

            return result;
        }

        return StoreResult.EntityNotFoundFailed(typeof(TEntity).Name, key.IdToString()!);
    }

    /// <summary>
    ///     批量创建子实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="packages"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> BatchCreateChildEntityAsync(TKey key, IEnumerable<TEntityPackage> packages,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var entity = await EntityStore.FindEntityAsync(key, cancellationToken);

        if (entity is not null)
        {
            var children = packages.Select(package =>
            {
                var child = MapNewEntity(package);

                child.ParentId = MapToParentKey(key);

                BeforeAddChildNode(entity, child);

                return child;
            });

            var result = await EntityStore.CreateAsync(children, cancellationToken);

            if (result.Succeeded) await AfterAddChildNode(entity, cancellationToken);

            return result;
        }

        return StoreResult.EntityNotFoundFailed(typeof(TEntity).Name, key.IdToString()!);
    }

    /// <summary>
    ///     添加子实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="childKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> AddChildEntityAsync(TKey key, TKey childKey,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var entity = await EntityStore.FindEntityAsync(key, cancellationToken);

        if (entity is not null)
        {
            var child = await EntityStore.FindEntityAsync(childKey, cancellationToken);

            if (child is not null)
            {
                child.ParentId = MapToParentKey(key);

                BeforeAddChildNode(entity, child);

                var result = await EntityStore.UpdateAsync(child, cancellationToken);

                if (result.Succeeded) 
                    await AfterAddChildNode(entity, cancellationToken);

                return result;
            }

            return StoreResult.EntityNotFoundFailed(typeof(TEntity).Name, key.IdToString()!);
        }

        return StoreResult.EntityNotFoundFailed(typeof(TEntity).Name, key.IdToString()!);
    }

    /// <summary>
    ///     批量添加子实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="childrenKeys"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> BatchAddChildEntityAsync(TKey key, IEnumerable<TKey> childrenKeys,
        CancellationToken cancellationToken = default)
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
                    BeforeAddChildNode(entity, child);
                }

                var result = await EntityStore.UpdateAsync(childrenList, cancellationToken);

                if (result.Succeeded) await AfterAddChildNode(entity, cancellationToken);

                return result;
            }

            return StoreResult.EntityNotFoundFailed(typeof(TEntity).Name,
                string.Join(",", keyList.Select(item => item.IdToString())));
        }

        return StoreResult.EntityNotFoundFailed(typeof(TEntity).Name, key.IdToString()!);
    }

    /// <summary>
    ///     删除子实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="childKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> DeleteChildEntityAsync(TKey key, TKey childKey,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var entity = await EntityStore.FindEntityAsync(key, cancellationToken);

        if (entity is not null)
        {
            var child = await EntityStore.FindEntityAsync(childKey, cancellationToken);

            if (child is not null)
            {
                var result = await EntityStore.DeleteAsync(childKey, cancellationToken);

                if (result.Succeeded) await AfterRemoveChildNode(entity, cancellationToken);

                return result;
            }

            return StoreResult.EntityNotFoundFailed(typeof(TEntity).Name, key.IdToString()!);
        }

        return StoreResult.EntityNotFoundFailed(typeof(TEntity).Name, key.IdToString()!);
    }

    /// <summary>
    ///     批量删除子实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="childrenKeys"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> BatchDeleteChildEntityAsync(TKey key, IEnumerable<TKey> childrenKeys,
        CancellationToken cancellationToken = default)
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
                var result = await EntityStore.DeleteAsync(keyList, cancellationToken);

                if (result.Succeeded) await AfterRemoveChildNode(entity, cancellationToken);

                return result;
            }

            return StoreResult.EntityNotFoundFailed(typeof(TEntity).Name,
                string.Join(",", keyList.Select(item => item.IdToString())));
        }

        return StoreResult.EntityNotFoundFailed(typeof(TEntity).Name, key.IdToString()!);
    }

    /// <summary>
    ///     移除子实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="childKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> RemoveChildEntityAsync(TKey key, TKey childKey,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var childEntity = await EntityStore.FindEntityAsync(childKey, cancellationToken);

        if (childEntity is not null && childEntity.ParentId is not null)
        {
            childEntity.ParentId = default;
            BeforeRemoveChildNode(childEntity);

            var result = await EntityStore.UpdateAsync(childEntity, cancellationToken);

            if (result.Succeeded)
            {
                var entity = await EntityStore.FindEntityAsync(key, cancellationToken);

                await AfterRemoveChildNode(entity!, cancellationToken);
            }

            return result;
        }

        return StoreResult.EntityNotFoundFailed(typeof(TEntity).Name, childKey.IdToString()!);
    }

    /// <summary>
    ///     批量移除子实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="childrenKeys"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> BatchRemoveChildEntityAsync(TKey key, IEnumerable<TKey> childrenKeys,
        CancellationToken cancellationToken = default)
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
                    item.ParentId = default;
                    BeforeRemoveChildNode(item);
                    return item;
                })
                .ToList();

            var result = await EntityStore.UpdateAsync(updateList, cancellationToken);

            if (result.Succeeded)
            {
                var entity = await EntityStore.FindEntityAsync(key, cancellationToken);

                await AfterRemoveChildNode(entity!, cancellationToken);
            }

            return result;
        }

        return StoreResult.EntityNotFoundFailed(typeof(TEntity).Name,
            string.Join(",", keyList.Select(item => item.IdToString())));
    }

    #endregion
}