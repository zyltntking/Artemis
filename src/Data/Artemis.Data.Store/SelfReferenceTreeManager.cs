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
public interface ISelfReferenceTreeManager<TEntity, TEntityInfo, TEntityInfoTree, TEntityPackage> :
    ISelfReferenceTreeManager<TEntity, Guid, Guid?, TEntityInfo, TEntityInfoTree, TEntityPackage>,
    ISeparateManager<TEntityInfo, TEntityPackage>
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
public interface ISelfReferenceTreeManager<TEntity, TKey, TParentKey, TEntityInfo, TEntityInfoTree, TEntityPackage> :
    ISeparateManager<TKey, TEntityInfo, TEntityPackage>
    where TEntity : class, IKeySlot<TKey>, IParentKeySlot<TParentKey?>, ITreeSlot<TEntity, TKey, TParentKey?>
    where TEntityInfo : class, IKeySlot<TKey>, IParentKeySlot<TParentKey?>
    where TEntityInfoTree : class, TEntityInfo, IKeySlot<TKey>, IParentKeySlot<TParentKey?>,
    ITreeInfoSlot<TEntityInfoTree, TKey, TParentKey?>
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
public abstract class SelfReferenceTreeManager<TEntity, TEntityInfo, TEntityInfoTree, TEntityPackage> :
    TreeManager<TEntity, Guid, Guid?, TEntityInfo, TEntityInfoTree, TEntityPackage>,
    ISelfReferenceTreeManager<TEntity, TEntityInfo, TEntityInfoTree, TEntityPackage>
    where TEntity : class, IKeySlot, IParentKeySlot, ITreeSlot<TEntity>
    where TEntityInfo : class, IKeySlot, IParentKeySlot
    where TEntityInfoTree : class, TEntityInfo, IKeySlot, IParentKeySlot, ITreeInfoSlot<TEntityInfoTree>
    where TEntityPackage : class
{
    /// <summary>
    ///     树模型管理器构造
    /// </summary>
    protected SelfReferenceTreeManager(IStore<TEntity> entityStore) : base(entityStore)
    {
    }

    #region Overrides of SelfReferenceTreeManager<TEntity,Guid,Guid?,TEntityInfo,TEntityInfoTree,TEntityPackage>

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
    ISelfReferenceTreeManager<TEntity, TKey, TParentKey?, TEntityInfo, TEntityInfoTree, TEntityPackage>
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
    ///     获取非根节点的树节点列表
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected abstract Task<List<TEntityInfo>> FetchNonRootTreeNodeList(TKey key, CancellationToken cancellationToken);

    /// <summary>
    ///     获取生成树节点列表
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
            return await EntityStore.EntityQuery.ProjectToType<TEntityInfo>().ToListAsync(cancellationToken);

        return await FetchNonRootTreeNodeList(key, cancellationToken);
    }

    /// <summary>
    ///     递归生成树
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
    ///     在添加子节点之前
    /// </summary>
    /// <param name="parent">父节点</param>
    /// <param name="child">子节点</param>
    /// <param name="loopIndex"></param>
    /// <param name="cancellationToken"></param>
    protected virtual Task BeforeAddChildNode(TEntity parent, TEntity child, int loopIndex,
        CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    ///     在添加子节点之后
    /// </summary>
    /// <param name="parent">父节点</param>
    /// <param name="loopIndex"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="child"></param>
    protected virtual Task AfterAddChildNode(TEntity parent, TEntity? child, int loopIndex,
        CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    ///     在移除子节点之前
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="child">子节点</param>
    /// <param name="loopIndex"></param>
    /// <param name="cancellationToken"></param>
    protected virtual Task BeforeRemoveChildNode(TEntity parent, TEntity child, int loopIndex,
        CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    ///     在移除子节点之后
    /// </summary>
    /// <param name="parent">父节点</param>
    /// <param name="loopIndex"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="child"></param>
    protected virtual Task AfterRemoveChildNode(TEntity parent, TEntity? child, int loopIndex,
        CancellationToken cancellationToken = default)
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
            var child = await MapNewEntity(package, 1, cancellationToken);

            child.ParentId = MapToParentKey(key);

            await BeforeAddChildNode(entity, child, 1, cancellationToken);

            var result = await EntityStore.CreateAsync(child, cancellationToken);

            if (result.Succeeded)
                await AfterAddChildNode(entity, child, 1, cancellationToken);

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
            var index = 1;

            var children = new List<TEntity>();

            foreach (var package in packages)
            {
                var child = await MapNewEntity(package, index, cancellationToken);

                child.ParentId = MapToParentKey(key);

                await BeforeAddChildNode(entity, child, index, cancellationToken);

                children.Add(child);

                index++;
            }

            var result = await EntityStore.CreateAsync(children, cancellationToken);

            if (result.Succeeded)
                await AfterAddChildNode(entity, null, 1, cancellationToken);

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

                await BeforeAddChildNode(entity, child, 1, cancellationToken);

                var result = await EntityStore.UpdateAsync(child, cancellationToken);

                if (result.Succeeded)
                    await AfterAddChildNode(entity, child, 1, cancellationToken);

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
                var index = 1;

                foreach (var child in childrenList)
                {
                    child.ParentId = MapToParentKey(key);
                    await BeforeAddChildNode(entity, child, index, cancellationToken);
                    index++;
                }

                var result = await EntityStore.UpdateAsync(childrenList, cancellationToken);

                if (result.Succeeded)
                    await AfterAddChildNode(entity, null, 1, cancellationToken);

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

                if (result.Succeeded)
                    await AfterRemoveChildNode(entity, child, 1, cancellationToken);

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

                if (result.Succeeded)
                    await AfterRemoveChildNode(entity, null, 1, cancellationToken);

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

        var entity = await EntityStore.FindEntityAsync(key, cancellationToken);

        if (entity is not null)
        {
            var child = await EntityStore.FindEntityAsync(childKey, cancellationToken);

            if (child is not null && child.ParentId is not null)
            {
                child.ParentId = default;

                await BeforeRemoveChildNode(entity, child, 1, cancellationToken);

                var result = await EntityStore.UpdateAsync(child, cancellationToken);

                if (result.Succeeded) await AfterRemoveChildNode(entity, child, 1, cancellationToken);

                return result;
            }

            return StoreResult.EntityNotFoundFailed(typeof(TEntity).Name, childKey.IdToString()!);
        }

        return StoreResult.EntityNotFoundFailed(typeof(TEntity).Name, key.IdToString()!);
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

        var entity = await EntityStore.FindEntityAsync(key, cancellationToken);

        if (entity is not null)
        {
            var keyList = childrenKeys.ToList();

            var children = await EntityStore.FindEntitiesAsync(keyList, cancellationToken);

            var childrenList = children.ToList();

            var updateList = new List<TEntity>();

            if (childrenList.Any())
            {
                var index = 1;

                foreach (var child in childrenList)
                    if (child.ParentId is not null && child.ParentId.Equals(key))
                    {
                        child.ParentId = default;
                        await BeforeRemoveChildNode(entity, child, index, cancellationToken);

                        updateList.Add(child);

                        index++;
                    }

                var result = await EntityStore.UpdateAsync(updateList, cancellationToken);

                if (result.Succeeded) await AfterRemoveChildNode(entity, null, 1, cancellationToken);

                return result;
            }

            return StoreResult.EntityNotFoundFailed(typeof(TEntity).Name,
                string.Join(",", keyList.Select(item => item.IdToString())));
        }

        return StoreResult.EntityNotFoundFailed(typeof(TEntity).Name, key.IdToString()!);
    }

    #endregion
}