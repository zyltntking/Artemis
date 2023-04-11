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
/// 存储数据访问器
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
    IQueryable<TEntity> TrackingQuery { get;}

    /// <summary>
    ///     Entity无追踪访问器
    /// </summary>
    IQueryable<TEntity> EntityQuery { get; }
}