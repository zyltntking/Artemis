using Artemis.Data.Core;

namespace Artemis.Data.Store;

/// <summary>
///     存储接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public interface IStore<TEntity> :
    IStore<TEntity, Guid>,
    IKeyWithStore<TEntity>
    where TEntity : class, IModelBase
{
}

/// <summary>
///     存储接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TKey">键类型</typeparam>
public interface IStore<TEntity, TKey> : IKeyWithStore<TEntity, TKey>
    where TEntity : class, IModelBase<TKey>
    where TKey : IEquatable<TKey>
{
}

/// <summary>
///     具键存储接口
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IKeyWithStore<TEntity> : IKeyWithStore<TEntity, Guid>,
    IKeyWithStoreBase<TEntity>,
    IKeyWithStoreCommon<TEntity>,
    IKeyWithStoreMap<TEntity>
    where TEntity : class, IKeySlot
{
}

/// <summary>
///     具键存储接口
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
public interface IKeyWithStore<TEntity, TKey> :
    IKeyLessStore<TEntity>,
    IKeyWithStoreBase<TEntity, TKey>,
    IKeyWithStoreCommon<TEntity, TKey>,
    IKeyWithStoreMap<TEntity, TKey>
    where TEntity : class, IKeySlot<TKey>
    where TKey : IEquatable<TKey>
{
}

/// <summary>
///     无键存储接口
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IKeyLessStore<TEntity> :
    IKeyLessStoreBase<TEntity>,
    IKeyLessStoreCommon<TEntity>,
    IKeyLessStoreMap<TEntity>
    where TEntity : class
{
}