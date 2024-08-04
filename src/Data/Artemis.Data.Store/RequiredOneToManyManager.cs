using System.Linq.Expressions;
using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;
using Artemis.Data.Store.Extensions;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Data.Store;

#region interface

/// <summary>
///     必须的一对多模型管理器
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TEntityInfo"></typeparam>
/// <typeparam name="TEntityPackage"></typeparam>
/// <typeparam name="TSubEntity"></typeparam>
/// <typeparam name="TSubEntityInfo"></typeparam>
/// <typeparam name="TSubEntityPackage"></typeparam>
public interface IRequiredOneToManyManager<
    TEntity, TEntityInfo, TEntityPackage,
    TSubEntity, TSubEntityInfo, TSubEntityPackage> : IRequiredOneToManyManager<
        TEntity, Guid, TEntityInfo, TEntityPackage,
        TSubEntity, Guid, TSubEntityInfo, TSubEntityPackage>,
    ISeparateManager<TEntityInfo, TEntityPackage>
    where TEntity : class, IKeySlot
    where TEntityInfo : class, IKeySlot
    where TEntityPackage : class
    where TSubEntity : class, IKeySlot
    where TSubEntityInfo : class, IKeySlot
    where TSubEntityPackage : class
{
}

/// <summary>
///     必须的一对多模型管理器
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TEntityInfo"></typeparam>
/// <typeparam name="TEntityPackage"></typeparam>
/// <typeparam name="TSubEntity"></typeparam>
/// <typeparam name="TSubKey"></typeparam>
/// <typeparam name="TSubEntityInfo"></typeparam>
/// <typeparam name="TSubEntityPackage"></typeparam>
public interface IRequiredOneToManyManager<
    TEntity, TKey, TEntityInfo, TEntityPackage,
    TSubEntity, TSubKey, TSubEntityInfo, TSubEntityPackage> :
    ISeparateManager<TKey, TEntityInfo, TEntityPackage>
    where TEntity : class, IKeySlot<TKey>
    where TEntityInfo : class, IKeySlot<TKey>
    where TEntityPackage : class
    where TKey : IEquatable<TKey>
    where TSubEntity : class, IKeySlot<TSubKey>
    where TSubEntityInfo : class, IKeySlot<TSubKey>
    where TSubEntityPackage : class
    where TSubKey : IEquatable<TSubKey>
{
    /// <summary>
    ///     读取子模型信息
    /// </summary>
    /// <param name="key">主模型键</param>
    /// <param name="subKey">子模型键</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<TSubEntityInfo?> ReadSubEntityInfoAsync(TKey key, TSubKey subKey,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     添加子模型
    /// </summary>
    /// <param name="key"></param>
    /// <param name="subEntityPackage"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> CreateSubEntityAsync(TKey key, TSubEntityPackage subEntityPackage,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     批量添加子模型
    /// </summary>
    /// <param name="key"></param>
    /// <param name="subEntityPackages"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> BatchCreateSubEntityAsync(TKey key, IEnumerable<TSubEntityPackage> subEntityPackages,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     更新子模型
    /// </summary>
    /// <param name="key"></param>
    /// <param name="subKey"></param>
    /// <param name="subEntityPackage"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> UpdateSubEntityAsync(TKey key, TSubKey subKey, TSubEntityPackage subEntityPackage,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     批量更新子模型
    /// </summary>
    /// <param name="key"></param>
    /// <param name="dictionary"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> BatchUpdateSubEntityAsync(TKey key, IDictionary<TSubKey, TSubEntityPackage> dictionary,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     删除子模型
    /// </summary>
    /// <param name="key"></param>
    /// <param name="subKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> DeleteSubEntityAsync(TKey key, TSubKey subKey, CancellationToken cancellationToken = default);

    /// <summary>
    ///     批量删除子模型
    /// </summary>
    /// <param name="key"></param>
    /// <param name="subKeys"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> BatchDeleteSubEntityAsync(TKey key, IEnumerable<TSubKey> subKeys,
        CancellationToken cancellationToken = default);
}

#endregion

/// <summary>
///     必须的一对多模型管理器
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TEntityInfo"></typeparam>
/// <typeparam name="TEntityPackage"></typeparam>
/// <typeparam name="TSubEntity"></typeparam>
/// <typeparam name="TSubEntityInfo"></typeparam>
/// <typeparam name="TSubEntityPackage"></typeparam>
public abstract class RequiredOneToManyManager<
    TEntity, TEntityInfo, TEntityPackage,
    TSubEntity, TSubEntityInfo, TSubEntityPackage> : RequiredOneToManyManager<
        TEntity, Guid, TEntityInfo, TEntityPackage,
        TSubEntity, Guid, TSubEntityInfo, TSubEntityPackage>,
    IRequiredOneToManyManager<TEntity, TEntityInfo, TEntityPackage,
        TSubEntity, TSubEntityInfo, TSubEntityPackage>
    where TEntity : class, IKeySlot
    where TEntityInfo : class, IKeySlot
    where TEntityPackage : class
    where TSubEntity : class, IKeySlot
    where TSubEntityInfo : class, IKeySlot
    where TSubEntityPackage : class
{
    /// <summary>
    ///     模型管理器构造
    /// </summary>
    protected RequiredOneToManyManager(
        IStore<TEntity> entityStore,
        IStore<TSubEntity> subEntityStore) : base(entityStore, subEntityStore)
    {
    }
}

/// <summary>
///     必须的一对多模型管理器
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TEntityInfo"></typeparam>
/// <typeparam name="TEntityPackage"></typeparam>
/// <typeparam name="TSubEntity"></typeparam>
/// <typeparam name="TSubKey"></typeparam>
/// <typeparam name="TSubEntityInfo"></typeparam>
/// <typeparam name="TSubEntityPackage"></typeparam>
public abstract class RequiredOneToManyManager<
    TEntity, TKey, TEntityInfo, TEntityPackage,
    TSubEntity, TSubKey, TSubEntityInfo, TSubEntityPackage> :
    SeparateManager<TEntity, TKey, TEntityInfo, TEntityPackage>,
    IRequiredOneToManyManager<TEntity, TKey, TEntityInfo, TEntityPackage,
        TSubEntity, TSubKey, TSubEntityInfo, TSubEntityPackage>
    where TEntity : class, IKeySlot<TKey>
    where TEntityInfo : class, IKeySlot<TKey>
    where TEntityPackage : class
    where TKey : IEquatable<TKey>
    where TSubEntity : class, IKeySlot<TSubKey>
    where TSubEntityInfo : class, IKeySlot<TSubKey>
    where TSubEntityPackage : class
    where TSubKey : IEquatable<TSubKey>
{
    /// <summary>
    ///     模型管理器构造
    /// </summary>
    protected RequiredOneToManyManager(
        IStore<TEntity, TKey> entityStore,
        IStore<TSubEntity, TSubKey> subEntityStore) : base(entityStore)
    {
        SubEntityStore = subEntityStore;
    }

    #region StoreAccess

    /// <summary>
    ///     子模型存储访问器
    /// </summary>
    protected IStore<TSubEntity, TSubKey> SubEntityStore { get; }

    #endregion

    #region Overrides

    /// <summary>
    ///     释放托管的Store
    /// </summary>
    protected override void StoreDispose()
    {
        SubEntityStore.Dispose();

        base.StoreDispose();
    }

    #endregion

    /// <summary>
    ///     子模型使用主模型的键匹配器
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    protected abstract Expression<Func<TSubEntity, bool>> SubEntityKeyMatcher(TKey key);

    /// <summary>
    ///     设置子模型的关联键
    /// </summary>
    /// <param name="subEntity"></param>
    /// <param name="key"></param>
    protected abstract void SetSubEntityRelationalKey(TSubEntity subEntity, TKey key);

    /// <summary>
    ///     映射到新的附属实体
    /// </summary>
    /// <param name="package"></param>
    /// <param name="key"></param>
    /// <param name="loopIndex"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected virtual Task<TSubEntity> MapNewSubEntity(
        TSubEntityPackage package,
        TKey key,
        int loopIndex,
        CancellationToken cancellationToken = default)
    {
        var entity = Instance.CreateInstance<TSubEntity, TSubEntityPackage>(package);

        SetSubEntityRelationalKey(entity, key);

        return Task.FromResult(entity);
    }

    /// <summary>
    ///     覆盖附属实体
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="package"></param>
    /// <param name="loopIndex"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected virtual Task<TSubEntity> MapOverSubEntity(
        TSubEntity entity,
        TSubEntityPackage package,
        int loopIndex,
        CancellationToken cancellationToken = default)
    {
        entity = package.Adapt(entity, IgnoreNullConfig);
        return Task.FromResult(entity);
    }

    #region Implementation

    /// <summary>
    ///     读取子模型信息
    /// </summary>
    /// <param name="key">主模型键</param>
    /// <param name="subKey">子模型键</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<TSubEntityInfo?> ReadSubEntityInfoAsync(
        TKey key,
        TSubKey subKey,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var entityExists = await EntityStore.ExistsAsync(key, cancellationToken);

        if (entityExists)
            return await SubEntityStore
                .KeyMatchQuery(subKey)
                .Where(SubEntityKeyMatcher(key))
                .ProjectToType<TSubEntityInfo>()
                .FirstOrDefaultAsync(cancellationToken);

        throw new EntityNotFoundException(typeof(TEntity).Name, key.IdToString()!);
    }

    /// <summary>
    ///     添加子模型
    /// </summary>
    /// <param name="key"></param>
    /// <param name="subEntityPackage"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> CreateSubEntityAsync(TKey key, TSubEntityPackage subEntityPackage,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var entityExists = await EntityStore.ExistsAsync(key, cancellationToken);

        if (entityExists)
        {
            var subEntity = await MapNewSubEntity(subEntityPackage, key, 1, cancellationToken);

            return await SubEntityStore.CreateAsync(subEntity, cancellationToken);
        }

        throw new EntityNotFoundException(typeof(TEntity).Name, key.IdToString()!);
    }

    /// <summary>
    ///     批量添加子模型
    /// </summary>
    /// <param name="key"></param>
    /// <param name="subEntityPackages"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> BatchCreateSubEntityAsync(TKey key, IEnumerable<TSubEntityPackage> subEntityPackages,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var entityExists = await EntityStore.ExistsAsync(key, cancellationToken);

        if (entityExists)
        {
            var subEntityList = new List<TSubEntity>();

            var loopIndex = 1;

            foreach (var subEntityPackage in subEntityPackages)
            {
                var subEntity = await MapNewSubEntity(subEntityPackage, key, loopIndex, cancellationToken);
                subEntityList.Add(subEntity);
                loopIndex++;
            }

            return await SubEntityStore.CreateAsync(subEntityList, cancellationToken);
        }

        throw new EntityNotFoundException(typeof(TEntity).Name, key.IdToString()!);
    }

    /// <summary>
    ///     更新子模型
    /// </summary>
    /// <param name="key"></param>
    /// <param name="subKey"></param>
    /// <param name="subEntityPackage"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> UpdateSubEntityAsync(TKey key, TSubKey subKey, TSubEntityPackage subEntityPackage,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var entityExists = await EntityStore.ExistsAsync(key, cancellationToken);

        if (entityExists)
        {
            var subEntity = await SubEntityStore
                .KeyMatchQuery(subKey)
                .Where(SubEntityKeyMatcher(key))
                .FirstOrDefaultAsync(cancellationToken);

            if (subEntity is not null)
            {
                subEntity = await MapOverSubEntity(subEntity, subEntityPackage, 1, cancellationToken);

                return await SubEntityStore.UpdateAsync(subEntity, cancellationToken);
            }

            return StoreResult.EntityNotFoundFailed(typeof(TSubEntity).Name, subKey.IdToString()!);
        }

        throw new EntityNotFoundException(typeof(TEntity).Name, key.IdToString()!);
    }

    /// <summary>
    ///     批量更新子模型
    /// </summary>
    /// <param name="key"></param>
    /// <param name="dictionary"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> BatchUpdateSubEntityAsync(TKey key,
        IDictionary<TSubKey, TSubEntityPackage> dictionary, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var entityExists = await EntityStore.ExistsAsync(key, cancellationToken);

        if (entityExists)
        {
            var subKeyList = dictionary.Keys.ToList();

            var subEntities = await SubEntityStore
                .KeyMatchQuery(subKeyList)
                .Where(SubEntityKeyMatcher(key))
                .ToListAsync(cancellationToken);

            if (subEntities.Any())
            {
                var updateSubEntities = new List<TSubEntity>();

                var loopIndex = 1;

                foreach (var subEntity in subEntities)
                {
                    var subPackage = dictionary[subEntity.Id];

                    var updateSubEntity = await MapOverSubEntity(subEntity, subPackage, loopIndex, cancellationToken);

                    updateSubEntities.Add(updateSubEntity);

                    loopIndex++;
                }

                return await SubEntityStore.UpdateAsync(updateSubEntities, cancellationToken);
            }

            var flag = string.Join(",", subKeyList.Select(item => item.IdToString()));

            return StoreResult.EntityNotFoundFailed(typeof(TSubEntity).Name, flag);
        }

        throw new EntityNotFoundException(typeof(TEntity).Name, key.IdToString()!);
    }

    /// <summary>
    ///     删除子模型
    /// </summary>
    /// <param name="key"></param>
    /// <param name="subKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> DeleteSubEntityAsync(TKey key, TSubKey subKey,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var entityExists = await EntityStore.ExistsAsync(key, cancellationToken);

        if (entityExists)
        {
            var subEntity = await SubEntityStore
                .KeyMatchQuery(subKey)
                .Where(SubEntityKeyMatcher(key))
                .FirstOrDefaultAsync(cancellationToken);

            if (subEntity is not null) return await SubEntityStore.DeleteAsync(subEntity, cancellationToken);

            return StoreResult.EntityNotFoundFailed(typeof(TSubEntity).Name, subKey.IdToString()!);
        }

        throw new EntityNotFoundException(typeof(TEntity).Name, key.IdToString()!);
    }

    /// <summary>
    ///     批量删除子模型
    /// </summary>
    /// <param name="key"></param>
    /// <param name="subKeys"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> BatchDeleteSubEntityAsync(TKey key, IEnumerable<TSubKey> subKeys,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var entityExists = await EntityStore.ExistsAsync(key, cancellationToken);

        if (entityExists)
        {
            var subKeyList = subKeys.ToList();

            var subEntities = await SubEntityStore
                .KeyMatchQuery(subKeyList)
                .Where(SubEntityKeyMatcher(key))
                .ToListAsync(cancellationToken);

            if (subEntities.Any()) return await SubEntityStore.DeleteAsync(subEntities, cancellationToken);

            var flag = string.Join(",", subKeyList.Select(item => item.IdToString()));

            return StoreResult.EntityNotFoundFailed(typeof(TSubEntity).Name, flag);
        }

        throw new EntityNotFoundException(typeof(TEntity).Name, key.IdToString()!);
    }

    #endregion
}