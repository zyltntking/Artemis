using Artemis.Data.Core;

namespace Artemis.Data.Store;

/// <summary>
///     存储接口
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IStore<TEntity> : IStore<TEntity, Guid>,
    IStoreCommon<TEntity>,
    IStoreMap<TEntity>
    where TEntity : class, IKeySlot;

/// <summary>
///     存储接口
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="THandler"></typeparam>
public interface IStore<TEntity, THandler> : IStore<TEntity, Guid, THandler>
    where TEntity : class, IKeySlot<Guid>
    where THandler : IEquatable<THandler>;

/// <summary>
///     存储接口
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
public interface IStore<TEntity, TKey, THandler> :
    IKeyLessStore<TEntity, THandler>,
    IStoreCommon<TEntity, TKey>,
    IStoreMap<TEntity, TKey>
    where TEntity : class, IKeySlot<TKey>
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>;

/// <summary>
///     无键存储接口
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IKeyLessStore<TEntity> : IKeyLessStore<TEntity, Guid> where TEntity : class;

/// <summary>
///     无键存储接口
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="THandler"></typeparam>
public interface IKeyLessStore<TEntity, THandler> :
    IKeyLessStoreCommon<TEntity>,
    IKeyLessStoreMap<TEntity>
    where TEntity : class
    where THandler : IEquatable<THandler>
{
    /// <summary>
    ///     注册操作员
    /// </summary>
    Func<THandler>? HandlerRegister { get; set; }

    /// <summary>
    ///     生成键
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    string GenerateKey(TEntity entity);
}