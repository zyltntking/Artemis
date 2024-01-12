using Artemis.Data.Core;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Data.Store;

/// <summary>
///     无元数据模型基本存储接口
/// </summary>
/// <typeparam name="TEntity">模型类型</typeparam>
public interface IKeyWithStoreBase<TEntity> : IKeyWithStoreBase<TEntity, Guid> where TEntity : class, IKeySlot
{
}

/// <summary>
///     无元数据模型基本存储接口
/// </summary>
/// <typeparam name="TEntity">模型类型</typeparam>
/// <typeparam name="TKey">键类型</typeparam>
public interface IKeyWithStoreBase<TEntity, TKey> : IKeyLessStoreBase<TEntity>
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
}

/// <summary>
///     无键模型基本存储接口
/// </summary>
/// <typeparam name="TEntity">模型类型</typeparam>
public interface IKeyLessStoreBase<TEntity> : IDisposable
    where TEntity : class
{
    /// <summary>
    ///     规范化键
    /// </summary>
    /// <param name="value">被规范化的值</param>
    /// <returns></returns>
    string NormalizeKey(string value);

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

    #endregion
}