using Artemis.Data.Core;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Data.Store;

/// <summary>
///     存储接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public interface IStore<TEntity> :
    IStore<TEntity, Guid>
    where TEntity : class, IModelBase
{
}

/// <summary>
///     存储接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TKey">键类型</typeparam>
public interface IStore<TEntity, TKey> :
    IKeyWithStoreBase<TEntity, TKey>,
    IKeyWithStoreCommon<TEntity, TKey>,
    IStoreMap<TEntity, TKey>
    where TEntity : class, IModelBase<TKey>
    where TKey : IEquatable<TKey>
{
}

/// <summary>
///     无键模型存储
/// </summary>
/// <typeparam name="TEntity">模型类型</typeparam>
public interface IKeyLessStore<TEntity> : IKeyLessStoreBase<TEntity>, IKeyLessStoreCommon<TEntity> where TEntity : class
{
}