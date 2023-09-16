using System.Linq.Expressions;
using Artemis.Data.Core;
using Microsoft.EntityFrameworkCore.Query;

namespace Artemis.Data.Store;

/// <summary>
///     通用存储接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public interface IStoreCommon<TEntity> : IStoreCommon<TEntity, Guid> where TEntity : IModelBase<Guid>
{
}

/// <summary>
///     通用存储接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TKey">键类型</typeparam>
public interface IStoreCommon<TEntity, in TKey> : IStoreOptions
    where TEntity : IModelBase<TKey> where TKey : IEquatable<TKey>
{
    #region IStoreOptions

    /// <summary>
    ///     设置配置
    /// </summary>
    /// <param name="storeOptions"></param>
    void SetOptions(IStoreOptions storeOptions);

    #endregion

    #region CreateEntity & CreateEntities

    /// <summary>
    ///     在<paramref name="entity" />存储中创建一个新的实体
    /// </summary>
    /// <param name="entity">被创建实体</param>
    /// <returns></returns>
    StoreResult Create(TEntity entity);

    /// <summary>
    ///     在<paramref name="entities" />存储中创建多个新的实体
    /// </summary>
    /// <param name="entities">被创建实体</param>
    /// <returns></returns>
    StoreResult Create(IEnumerable<TEntity> entities);

    /// <summary>
    ///     在<paramref name="entity" />存储中创建一个新的实体
    /// </summary>
    /// <param name="entity">被创建实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    Task<StoreResult> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    ///     在<paramref name="entities" />存储中创建多个新的实体
    /// </summary>
    /// <param name="entities">被创建实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    Task<StoreResult> CreateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    #endregion

    #region UpdateEntity & UpdateEntities

    /// <summary>
    ///     在<paramref name="entity" />存储中更新已存在的实体
    /// </summary>
    /// <param name="entity">被更新实体</param>
    /// <returns></returns>
    StoreResult Update(TEntity entity);

    /// <summary>
    ///     在<paramref name="entities" />存储中更新多个已存在的实体
    /// </summary>
    /// <param name="entities">被更新实体</param>
    /// <returns></returns>
    StoreResult Update(IEnumerable<TEntity> entities);

    /// <summary>
    ///     在<paramref name="entity" />存储中更新已存在的实体
    /// </summary>
    /// <param name="entity">被更新实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    Task<StoreResult> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    ///     在<paramref name="entities" />存储中更新多个已存在的实体
    /// </summary>
    /// <param name="entities">被更新实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    Task<StoreResult> UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    #endregion

    #region BatchUpdateEntity & BatchUpdateEntities

    /// <summary>
    ///     更新存储中的实体
    /// </summary>
    /// <param name="id">被更新实体的主键</param>
    /// <param name="setter">更新行为</param>
    /// <returns></returns>
    StoreResult BatchUpdate(TKey id, Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter);

    /// <summary>
    ///     在存储中更新已存在的实体
    /// </summary>
    /// <param name="ids">被更新实体的主键</param>
    /// <param name="setter">更新行为</param>
    /// <returns></returns>
    StoreResult BatchUpdate(IEnumerable<TKey> ids,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter);

    /// <summary>
    ///     在存储中更新符合条件的实体
    /// </summary>
    /// <param name="setter">更新行为</param>
    /// <param name="predicate">查询表达式</param>
    /// <returns></returns>
    StoreResult BatchUpdate(Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter,
        Expression<Func<TEntity, bool>>? predicate = null);

    /// <summary>
    ///     在存储中更新符合查询描述的实体
    /// </summary>
    /// <param name="query"></param>
    /// <param name="setter">更新行为</param>
    /// <returns></returns>
    StoreResult BatchUpdate(IQueryable<TEntity> query,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter);

    /// <summary>
    ///     在存储中更新已存在的实体
    /// </summary>
    /// <param name="id">被更新实体的主键</param>
    /// <param name="setter">更新行为</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    Task<StoreResult> BatchUpdateAsync(TKey id,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     在存储中更新已存在的实体
    /// </summary>
    /// <param name="ids">被更新实体的主键</param>
    /// <param name="setter">更新行为</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    Task<StoreResult> BatchUpdateAsync(IEnumerable<TKey> ids,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     在存储中更新符合条件的实体
    /// </summary>
    /// <param name="setter">更新行为</param>
    /// <param name="predicate">查询表达式</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<StoreResult> BatchUpdateAsync(Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter,
        Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     在存储中更新符合查询描述的实体
    /// </summary>
    /// <param name="query"></param>
    /// <param name="setter">更新行为</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> BatchUpdateAsync(IQueryable<TEntity> query,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter,
        CancellationToken cancellationToken = default);

    #endregion

    #region DeleteEntity & DeleteEntities

    /// <summary>
    ///     在存储中删除已存在的实体
    /// </summary>
    /// <param name="id">被删除实体的主键</param>
    StoreResult Delete(TKey id);

    /// <summary>
    ///     在<paramref name="entity" />存储中删除已存在的实体
    /// </summary>
    /// <param name="entity">被删除实体</param>
    /// <returns></returns>
    StoreResult Delete(TEntity entity);

    /// <summary>
    ///     在存储中删除已存在的实体
    /// </summary>
    /// <param name="ids">被删除实体的主键</param>
    /// <returns></returns>
    StoreResult Delete(IEnumerable<TKey> ids);

    /// <summary>
    ///     在<paramref name="entities" />存储中删除已存在的实体
    /// </summary>
    /// <param name="entities">被删除实体</param>
    /// <returns></returns>
    StoreResult Delete(IEnumerable<TEntity> entities);

    /// <summary>
    ///     在存储中删除已存在的实体
    /// </summary>
    /// <param name="id">被删除实体的主键</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    Task<StoreResult> DeleteAsync(TKey id, CancellationToken cancellationToken = default);

    /// <summary>
    ///     在<paramref name="entity" />存储中删除已存在的实体
    /// </summary>
    /// <param name="entity">被删除实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    Task<StoreResult> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    ///     在存储中删除已存在的实体
    /// </summary>
    /// <param name="ids">被删除实体的主键</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    Task<StoreResult> DeleteAsync(IEnumerable<TKey> ids, CancellationToken cancellationToken = default);

    /// <summary>
    ///     在<paramref name="entities" />存储中删除已存在的实体
    /// </summary>
    /// <param name="entities">被删除实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    Task<StoreResult> DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    #endregion

    #region BatchDeleteEntity & BatchDeleteEntities

    /// <summary>
    ///     在存储中删除已存在的实体
    /// </summary>
    /// <param name="id">被删除实体的主键</param>
    StoreResult BatchDelete(TKey id);

    /// <summary>
    ///     在存储中删除已存在的实体
    /// </summary>
    /// <param name="ids">被删除实体的主键</param>
    /// <returns></returns>
    StoreResult BatchDelete(IEnumerable<TKey> ids);

    /// <summary>
    ///     在存储中删除符合条件的实体
    /// </summary>
    /// <param name="predicate">查询表达式</param>
    /// <returns></returns>
    StoreResult BatchDelete(Expression<Func<TEntity, bool>>? predicate = null);

    /// <summary>
    ///     在存储中删除符合查询描述的实体
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    StoreResult BatchDelete(IQueryable<TEntity> query);

    /// <summary>
    ///     在存储中删除已存在的实体
    /// </summary>
    /// <param name="id">被删除实体的主键</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    Task<StoreResult> BatchDeleteAsync(TKey id, CancellationToken cancellationToken = default);

    /// <summary>
    ///     在存储中删除已存在的实体
    /// </summary>
    /// <param name="ids">被删除实体的主键</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    Task<StoreResult> BatchDeleteAsync(IEnumerable<TKey> ids, CancellationToken cancellationToken = default);

    /// <summary>
    ///     在存储中删除符合条件的实体
    /// </summary>
    /// <param name="predicate">查询表达式</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<StoreResult> BatchDeleteAsync(Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     在存储中删除符合查询描述的实体
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> BatchDeleteAsync(IQueryable<TEntity> query, CancellationToken cancellationToken = default);

    #endregion

    #region FindEntity & FindEntities

    /// <summary>
    ///     根据Id查找实体
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    TEntity? FindEntity(TKey id);

    /// <summary>
    ///     根据缓存键查找实体
    /// </summary>
    /// <param name="key">缓存键</param>
    /// <returns></returns>
    TEntity? FindEntityViaKey(string key);

    /// <summary>
    ///     根据Id查找实体
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    IEnumerable<TEntity> FindEntities(IEnumerable<TKey> ids);

    /// <summary>
    ///     根据缓存键查找实体
    /// </summary>
    /// <param name="keys">缓存键</param>
    /// <returns></returns>
    IEnumerable<TEntity> FindEntitiesViaKeys(IEnumerable<string> keys);

    /// <summary>
    ///     根据Id查找实体
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    Task<TEntity?> FindEntityAsync(TKey id, CancellationToken cancellationToken = default);

    /// <summary>
    ///     根据Id查找实体
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    Task<IEnumerable<TEntity>> FindEntitiesAsync(IEnumerable<TKey> ids, CancellationToken cancellationToken = default);

    #endregion

    #region Exists

    /// <summary>
    /// 判断实体是否存在
    /// </summary>
    /// <param name="id">实体键</param>
    /// <returns></returns>
    bool Exists(TKey id);

    /// <summary>
    /// 判断实体是否存在
    /// </summary>
    /// <param name="id">实体键</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default);

    #endregion
}