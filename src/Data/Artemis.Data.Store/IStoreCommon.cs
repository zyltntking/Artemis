using System.Linq.Expressions;
using Artemis.Data.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Artemis.Data.Store;

/// <summary>
///     通用存储接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public interface IStoreCommon<TEntity> : IStoreCommon<TEntity, Guid> where TEntity : class, IKeySlot;

/// <summary>
///     通用存储接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TKey">键类型</typeparam>
public interface IStoreCommon<TEntity, TKey> : IKeyLessStoreCommon<TEntity>
    where TEntity : class, IKeySlot<TKey>
    where TKey : IEquatable<TKey>
{
    #region Access

    /// <summary>
    ///     键适配查询
    /// </summary>
    IQueryable<TEntity> KeyMatchQuery(TKey key);

    /// <summary>
    ///     键适配查询
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    IQueryable<TEntity> KeyMatchQuery(IEnumerable<TKey> keys);

    #endregion

    #region GetId

    /// <summary>
    ///     获取指定实体Id
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns>Id</returns>
    TKey GetId(TEntity entity);

    /// <summary>
    ///     获取指定实体Id
    /// </summary>
    /// <param name="entity">实体</param>
    /// <param name="cancellationToken">取消异步操作的信号</param>
    /// <returns>Id</returns>
    Task<TKey> GetIdAsync(
        TEntity entity,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     获取指定实体Id字符串
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns>Id字符串</returns>
    string GetIdString(TEntity entity);

    /// <summary>
    ///     获取指定实体Id字符串
    /// </summary>
    /// <param name="entity">实体</param>
    /// <param name="cancellationToken">取消异步操作信号</param>
    /// <returns>Id字符串</returns>
    Task<string> GetIdStringAsync(
        TEntity entity,
        CancellationToken cancellationToken = default);

    #endregion

    #region IsDelete

    /// <summary>
    ///     是否被删除
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns>判断结果</returns>
    bool IsDeleted(TEntity entity);

    /// <summary>
    ///     是否被删除
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    bool IsDeleted(TKey key);

    /// <summary>
    ///     是否被删除
    /// </summary>
    /// <param name="entity">实体</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>判断结果</returns>
    Task<bool> IsDeletedAsync(
        TEntity entity,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     是否被删除
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<bool> IsDeletedAsync(
        TKey key,
        CancellationToken cancellationToken = default);

    #endregion

    #region BatchUpdateEntity & BatchUpdateEntities

    /// <summary>
    ///     更新存储中的实体
    /// </summary>
    /// <param name="id">被更新实体的主键</param>
    /// <param name="setter">更新行为</param>
    /// <returns></returns>
    StoreResult BatchUpdate(
        TKey id,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter);

    /// <summary>
    ///     在存储中更新已存在的实体
    /// </summary>
    /// <param name="ids">被更新实体的主键</param>
    /// <param name="setter">更新行为</param>
    /// <returns></returns>
    StoreResult BatchUpdate(
        IEnumerable<TKey> ids,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter);

    /// <summary>
    ///     在存储中更新已存在的实体
    /// </summary>
    /// <param name="id">被更新实体的主键</param>
    /// <param name="setter">更新行为</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    Task<StoreResult> BatchUpdateAsync(
        TKey id,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     在存储中更新已存在的实体
    /// </summary>
    /// <param name="ids">被更新实体的主键</param>
    /// <param name="setter">更新行为</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    Task<StoreResult> BatchUpdateAsync(
        IEnumerable<TKey> ids,
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
    ///     在存储中删除已存在的实体
    /// </summary>
    /// <param name="ids">被删除实体的主键</param>
    /// <returns></returns>
    StoreResult Delete(IEnumerable<TKey> ids);

    /// <summary>
    ///     在存储中删除已存在的实体
    /// </summary>
    /// <param name="id">被删除实体的主键</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    Task<StoreResult> DeleteAsync(
        TKey id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     在存储中删除已存在的实体
    /// </summary>
    /// <param name="ids">被删除实体的主键</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    Task<StoreResult> DeleteAsync(
        IEnumerable<TKey> ids,
        CancellationToken cancellationToken = default);

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
    ///     在存储中删除已存在的实体
    /// </summary>
    /// <param name="id">被删除实体的主键</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    Task<StoreResult> BatchDeleteAsync(
        TKey id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     在存储中删除已存在的实体
    /// </summary>
    /// <param name="ids">被删除实体的主键</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    Task<StoreResult> BatchDeleteAsync(
        IEnumerable<TKey> ids,
        CancellationToken cancellationToken = default);

    #endregion

    #region FindEntity & FindEntities

    /// <summary>
    ///     根据Id查找实体
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    TEntity? FindEntity(TKey id);

    /// <summary>
    ///     根据Id查找实体
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    IEnumerable<TEntity> FindEntities(IEnumerable<TKey> ids);

    /// <summary>
    ///     根据Id查找实体
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    Task<TEntity?> FindEntityAsync(
        TKey id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     根据Id查找实体
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    Task<IEnumerable<TEntity>> FindEntitiesAsync(
        IEnumerable<TKey> ids,
        CancellationToken cancellationToken = default);

    #endregion

    #region Exists

    /// <summary>
    ///     判断实体是否存在
    /// </summary>
    /// <param name="id">实体键</param>
    /// <returns></returns>
    bool Exists(TKey id);

    /// <summary>
    ///     判断实体是否存在
    /// </summary>
    /// <param name="id">实体键</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<bool> ExistsAsync(
        TKey id,
        CancellationToken cancellationToken = default);

    #endregion
}

/// <summary>
///     无键模型通用存储接口
/// </summary>
public interface IKeyLessStoreCommon<TEntity> : IDisposable
    where TEntity : class
{
    #region Access

    /// <summary>
    ///     Entity集合访问器
    /// </summary>
    DbSet<TEntity> EntitySet { get; }

    /// <summary>
    ///     Entity有追踪访问器
    /// </summary>
    IQueryable<TEntity> TrackingQuery { get; }

    /// <summary>
    ///     Entity无追踪访问器
    /// </summary>
    IQueryable<TEntity> EntityQuery { get; }

    /// <summary>
    ///     存储名称
    /// </summary>
    string StoreName { get; }

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
    Task<StoreResult> CreateAsync(
        TEntity entity,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     在<paramref name="entities" />存储中创建多个新的实体
    /// </summary>
    /// <param name="entities">被创建实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    Task<StoreResult> CreateAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default);

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
    Task<StoreResult> UpdateAsync(
        TEntity entity,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     在<paramref name="entities" />存储中更新多个已存在的实体
    /// </summary>
    /// <param name="entities">被更新实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    Task<StoreResult> UpdateAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default);

    #endregion

    #region BatchUpdateEntity & BatchUpdateEntities

    /// <summary>
    ///     在存储中更新符合条件的实体
    /// </summary>
    /// <param name="setter">更新行为</param>
    /// <param name="predicate">查询表达式</param>
    /// <returns></returns>
    StoreResult BatchUpdate(
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter,
        Expression<Func<TEntity, bool>>? predicate = null);

    /// <summary>
    ///     在存储中更新符合查询描述的实体
    /// </summary>
    /// <param name="query"></param>
    /// <param name="setter">更新行为</param>
    /// <returns></returns>
    StoreResult BatchUpdate(
        IQueryable<TEntity> query,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter);

    /// <summary>
    ///     在存储中更新符合条件的实体
    /// </summary>
    /// <param name="setter">更新行为</param>
    /// <param name="predicate">查询表达式</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<StoreResult> BatchUpdateAsync(
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter,
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     在存储中更新符合查询描述的实体
    /// </summary>
    /// <param name="query"></param>
    /// <param name="setter">更新行为</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> BatchUpdateAsync(
        IQueryable<TEntity> query,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter,
        CancellationToken cancellationToken = default);

    #endregion

    #region DeleteEntity & DeleteEntities

    /// <summary>
    ///     在<paramref name="entity" />存储中删除已存在的实体
    /// </summary>
    /// <param name="entity">被删除实体</param>
    /// <returns></returns>
    StoreResult Delete(TEntity entity);


    /// <summary>
    ///     在<paramref name="entities" />存储中删除已存在的实体
    /// </summary>
    /// <param name="entities">被删除实体</param>
    /// <returns></returns>
    StoreResult Delete(IEnumerable<TEntity> entities);


    /// <summary>
    ///     在<paramref name="entity" />存储中删除已存在的实体
    /// </summary>
    /// <param name="entity">被删除实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    Task<StoreResult> DeleteAsync(
        TEntity entity,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     在<paramref name="entities" />存储中删除已存在的实体
    /// </summary>
    /// <param name="entities">被删除实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    Task<StoreResult> DeleteAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default);

    #endregion

    #region BatchDeleteEntity & BatchDeleteEntities

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
    ///     在存储中删除符合条件的实体
    /// </summary>
    /// <param name="predicate">查询表达式</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<StoreResult> BatchDeleteAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     在存储中删除符合查询描述的实体
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> BatchDeleteAsync(
        IQueryable<TEntity> query,
        CancellationToken cancellationToken = default);

    #endregion
}