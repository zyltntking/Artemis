﻿using Artemis.Data.Core;
using Mapster;

namespace Artemis.Data.Store;

/// <summary>
///     可映射存储接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public interface IStoreMap<TEntity> : IStoreMap<TEntity, Guid> where TEntity : IModelBase<Guid>
{
}

/// <summary>
///     可映射存储接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TKey">键类型</typeparam>
public interface IStoreMap<TEntity, TKey> where TEntity : IModelBase<TKey> where TKey : IEquatable<TKey>
{
    #region MapConfig

    /// <summary>
    ///     在忽略目标实体元数据属性的基础上忽略源实体的Id属性和空值属性
    /// </summary>
    /// <typeparam name="TSource">源数据类型</typeparam>
    /// <returns>映射配置</returns>
    TypeAdapterConfig IgnoreIdAndNullConfig<TSource>();

    /// <summary>
    ///     在忽略目标实体元数据属性的基础上忽略源实体的空值属性
    /// </summary>
    /// <typeparam name="TSource">源数据类型</typeparam>
    /// <returns>映射配置</returns>
    TypeAdapterConfig IgnoreNullConfig<TSource>();

    /// <summary>
    ///     在忽略目标实体元数据属性的基础上忽略目标实体的Id属性
    /// </summary>
    /// <typeparam name="TSource">源数据类型</typeparam>
    /// <returns>映射配置</returns>
    TypeAdapterConfig IgnoreIdConfig<TSource>();

    /// <summary>
    ///     忽略目标实体的元数据属性
    /// </summary>
    /// <typeparam name="TSource">源数据类型</typeparam>
    /// <returns>映射配置</returns>
    TypeAdapterConfig IgnoreMetaConfig<TSource>();

    #endregion

    #region FindMapEntity & FindMapEntities

    /// <summary>
    ///     根据id查找映射实体
    /// </summary>
    /// <typeparam name="TMapEntity">映射类型</typeparam>
    /// <param name="id">id</param>
    /// <returns></returns>
    TMapEntity? FindMapEntity<TMapEntity>(TKey id);

    /// <summary>
    ///     根据id查找映射实体
    /// </summary>
    /// <typeparam name="TMapEntity">映射类型</typeparam>
    /// <param name="ids">ids</param>
    /// <returns></returns>
    IEnumerable<TMapEntity> FindMapEntities<TMapEntity>(IEnumerable<TKey> ids);

    /// <summary>
    ///     根据Id查找映射实体
    /// </summary>
    /// <typeparam name="TMapEntity">映射类型</typeparam>
    /// <param name="id"></param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    Task<TMapEntity?> FindMapEntityAsync<TMapEntity>(
        TKey id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     根据Id查找映射实体
    /// </summary>
    /// <typeparam name="TMapEntity">映射类型</typeparam>
    /// <param name="ids"></param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    Task<List<TMapEntity>> FindMapEntitiesAsync<TMapEntity>(
        IEnumerable<TKey> ids,
        CancellationToken cancellationToken = default);

    #endregion

    #region CreateNewEntity & CreateNewEntities

    /// <summary>
    ///     通过类型映射创建一个新实例
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="config">映射配置</param>
    /// <returns>创建结果</returns>
    StoreResult CreateNew<TSource>(
        TSource source,
        TypeAdapterConfig? config = null);

    /// <summary>
    ///     通过类型映射创建一组新实例
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="config">映射配置</param>
    /// <returns>创建结果</returns>
    StoreResult CreateNew<TSource>(
        IEnumerable<TSource> sources,
        TypeAdapterConfig? config = null);

    /// <summary>
    ///     通过类型映射创建一个新实例
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns>创建结果</returns>
    Task<StoreResult> CreateNewAsync<TSource>(
        TSource source,
        TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     通过类型映射创建一个新实例
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns>创建结果</returns>
    Task<StoreResult> CreateNewAsync<TSource>(
        IEnumerable<TSource> sources,
        TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default);

    #endregion

    #region OverEntity & OverEntities

    /// <summary>
    ///     通过类型映射覆盖对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="config">映射配置</param>
    /// <returns></returns>
    StoreResult Over<TSource>(
        TSource source,
        TypeAdapterConfig? config = null) where TSource : IKeySlot<TKey>;

    /// <summary>
    ///     通过类型映射覆盖对应实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="destination">目标数据</param>
    /// <param name="config">映射配置</param>
    /// <returns></returns>
    StoreResult Over<TSource>(
        TSource source,
        TEntity destination,
        TypeAdapterConfig? config = null);

    /// <summary>
    ///     通过类型映射覆盖对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="config">映射配置</param>
    /// <returns></returns>
    StoreResult Over<TSource>(
        IEnumerable<TSource> sources,
        TypeAdapterConfig? config = null) where TSource : IKeySlot<TKey>;

    /// <summary>
    ///     通过类型映射覆盖对应实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TJKey">连接键类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="destinations">目标数据</param>
    /// <param name="destinationKeySelector">目标键选择器</param>
    /// <param name="config">映射配置</param>
    /// <param name="sourceKeySelector">源键选择器</param>
    /// <returns></returns>
    StoreResult Over<TSource, TJKey>(
        IEnumerable<TSource> sources,
        IEnumerable<TEntity> destinations,
        Func<TSource, TJKey> sourceKeySelector,
        Func<TEntity, TJKey> destinationKeySelector,
        TypeAdapterConfig? config = null);

    /// <summary>
    ///     通过类型映射覆盖对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    Task<StoreResult> OverAsync<TSource>(
        TSource source,
        TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default) where TSource : IKeySlot<TKey>;

    /// <summary>
    ///     通过类型映射覆盖对应实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="destination">目标数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    Task<StoreResult> OverAsync<TSource>(
        TSource source,
        TEntity destination,
        TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     通过类型映射覆盖对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    Task<StoreResult> OverAsync<TSource>(
        IEnumerable<TSource> sources,
        TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default) where TSource : IKeySlot<TKey>;

    /// <summary>
    ///     通过类型映射覆盖对应实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TJKey">连接键类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="destinations">目标数据</param>
    /// <param name="destinationKeySelector">目标键选择器</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <param name="sourceKeySelector">源键选择器</param>
    /// <returns></returns>
    public Task<StoreResult> OverAsync<TSource, TJKey>(
        IEnumerable<TSource> sources,
        IEnumerable<TEntity> destinations,
        Func<TSource, TJKey> sourceKeySelector,
        Func<TEntity, TJKey> destinationKeySelector,
        TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default);

    #endregion

    #region MergeEntity & MergeEntities

    /// <summary>
    ///     通过类型映射合并对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="config">映射配置</param>
    /// <returns></returns>
    StoreResult Merge<TSource>(
        TSource source,
        TypeAdapterConfig? config = null) where TSource : IKeySlot<TKey>;

    /// <summary>
    ///     通过类型映射合并对应实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="destination">目标数据</param>
    /// <param name="config">映射配置</param>
    /// <returns></returns>
    StoreResult Merge<TSource>(
        TSource source,
        TEntity destination,
        TypeAdapterConfig? config = null);

    /// <summary>
    ///     通过类型映射合并对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="config">映射配置</param>
    /// <returns></returns>
    StoreResult Merge<TSource>(
        IEnumerable<TSource> sources,
        TypeAdapterConfig? config = null) where TSource : IKeySlot<TKey>;

    /// <summary>
    ///     通过类型映射合并对应实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TJKey">连接键类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="destinations">目标数据</param>
    /// <param name="destinationKeySelector">目标键选择器</param>
    /// <param name="config">映射配置</param>
    /// <param name="sourceKeySelector">源键选择器</param>
    /// <returns></returns>
    StoreResult Merge<TSource, TJKey>(
        IEnumerable<TSource> sources,
        IEnumerable<TEntity> destinations,
        Func<TSource, TJKey> sourceKeySelector,
        Func<TEntity, TJKey> destinationKeySelector,
        TypeAdapterConfig? config = null);

    /// <summary>
    ///     通过类型映射合并对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    Task<StoreResult> MergeAsync<TSource>(
        TSource source,
        TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default) where TSource : IKeySlot<TKey>;

    /// <summary>
    ///     通过类型映射合并对应实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="destination">目标数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    Task<StoreResult> MergeAsync<TSource>(
        TSource source,
        TEntity destination,
        TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     通过类型映射合并对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    Task<StoreResult> MergeAsync<TSource>(
        IEnumerable<TSource> sources,
        TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default) where TSource : IKeySlot<TKey>;

    /// <summary>
    ///     通过类型映射合并对应实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TJKey">连接键类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="destinations">目标数据</param>
    /// <param name="destinationKeySelector">目标键选择器</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <param name="sourceKeySelector">源键选择器</param>
    /// <returns></returns>
    public Task<StoreResult> MergeAsync<TSource, TJKey>(
        IEnumerable<TSource> sources,
        IEnumerable<TEntity> destinations,
        Func<TSource, TJKey> sourceKeySelector,
        Func<TEntity, TJKey> destinationKeySelector,
        TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default);

    #endregion
}