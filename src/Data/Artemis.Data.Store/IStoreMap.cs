using Artemis.Data.Core;

namespace Artemis.Data.Store;

/// <summary>
/// 可映射存储接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public interface IStoreMap<out TEntity> : IStoreMap<TEntity, Guid> where TEntity : IModelBase<Guid>
{

}

/// <summary>
/// 可映射存储接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TKey">键类型</typeparam>
public interface IStoreMap<out TEntity, TKey> where TEntity : IModelBase<TKey> where TKey : IEquatable<TKey>
{
    #region CreateNewEntity & CreateNewEntities

    /// <summary>
    /// 通过类型映射创建一个新实例
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <returns>创建结果</returns>
    StoreResult CreateNew<TSource>(TSource source);

    /// <summary>
    /// 通过类型映射创建一个新实例
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <returns>创建结果</returns>
    StoreResult CreateNew<TSource>(IEnumerable<TSource> sources);

    /// <summary>
    /// 通过类型映射创建一个新实例
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns>创建结果</returns>
    Task<StoreResult> CreateNewAsync<TSource>(TSource source, CancellationToken cancellationToken = default);

    /// <summary>
    /// 通过类型映射创建一个新实例
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns>创建结果</returns>
    Task<StoreResult> CreateNewAsync<TSource>(IEnumerable<TSource> sources, CancellationToken cancellationToken = default);

    #endregion

}