using Artemis.Data.Core;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Data.Store;

/// <summary>
///     存储接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public interface IStore<TEntity> :
    IStore<TEntity, Guid>,
    IStoreBase<TEntity>,
    IStoreCommon<TEntity>,
    IStoreMap<TEntity>
    where TEntity : class, IModelBase<Guid>
{
}

/// <summary>
///     存储接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TKey">键类型</typeparam>
public interface IStore<TEntity, TKey> :
    IStoreBase<TEntity, TKey>,
    IStoreCommon<TEntity, TKey>,
    IStoreMap<TEntity, TKey>
    where TEntity : class, IModelBase<TKey>
    where TKey : IEquatable<TKey>
{
    #region Access

    /// <summary>
    ///     EntitySet访问器*Main Store Set*
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
}