using Artemis.Data.Core;

namespace Artemis.Data.Store;

/// <summary>
///     存储接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public interface IStore<TEntity> : 
    IStore<TEntity, Guid>, 
    IStoreBase<TEntity>, 
    IStoreCommon<TEntity>,
    IStoreAccess<TEntity>,
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
    IStoreAccess<TEntity, TKey>,
    IStoreMap<TEntity, TKey>
    where TEntity : class, IModelBase<TKey>
    where TKey : IEquatable<TKey>
{
}