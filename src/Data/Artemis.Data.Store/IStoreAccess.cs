using Artemis.Data.Core;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Data.Store;

/// <summary>
///     通用存储接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public interface IStoreAccess<TEntity> : IStoreAccess<TEntity, Guid> where TEntity : class, IModelBase<Guid>
{
}

/// <summary>
///     存储数据访问器
/// </summary>
public interface IStoreAccess<TEntity, TKey> where TEntity : class, IModelBase<TKey> where TKey : IEquatable<TKey>
{
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
    /// 键适配查询
    /// </summary>
    IQueryable<TEntity> KeyMatchQuery(TKey key);

    /// <summary>
    /// 键适配查询
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    IQueryable<TEntity> KeyMatchQuery(IEnumerable<TKey> keys);
}