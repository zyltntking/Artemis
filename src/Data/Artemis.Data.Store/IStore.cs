using Artemis.Data.Core;

namespace Artemis.Data.Store;

/// <summary>
///     具键存储接口
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IStore<TEntity> : IStore<TEntity, Guid>,
    IStoreBase<TEntity>,
    IStoreCommon<TEntity>,
    IStoreMap<TEntity>
    where TEntity : class, IKeySlot
{
}

/// <summary>
///     具键存储接口
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
public interface IStore<TEntity, TKey> :
    IKeyLessStore<TEntity>,
    IStoreBase<TEntity, TKey>,
    IStoreCommon<TEntity, TKey>,
    IStoreMap<TEntity, TKey>
    where TEntity : class, IKeySlot<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     注册操作员
    /// </summary>
    Func<TKey>? HandlerRegister { get; set; }
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