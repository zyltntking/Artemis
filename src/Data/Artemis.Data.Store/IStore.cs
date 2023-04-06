using Artemis.Data.Core;

namespace Artemis.Data.Store;

/// <summary>
/// 存储接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public interface IStore<TEntity> : IStore<TEntity, Guid> , IStoreBase<TEntity>, IStoreCommon<TEntity>, IStoreMap<TEntity>
    where TEntity : IModelBase<Guid>
{
}

/// <summary>
/// 存储接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TKey">键类型</typeparam>
public interface IStore<TEntity, TKey> : IStoreBase<TEntity, TKey>, IStoreCommon<TEntity, TKey>, IStoreMap<TEntity, TKey>
    where TEntity : IModelBase<TKey> 
    where TKey : IEquatable<TKey>
{

}