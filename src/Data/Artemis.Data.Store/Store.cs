using System.Linq.Expressions;
using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;
using Artemis.Data.Store.Extensions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Artemis.Data.Store;

/// <summary>
///     抽象存储实现
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public abstract class Store<TEntity> : Store<TEntity, Guid>, IStore<TEntity>
    where TEntity : class, IModelBase, IModelBase<Guid>
{
    /// <summary>
    ///     创建一个新的基本存储实例
    /// </summary>
    /// <param name="context">数据访问上下文</param>
    /// <param name="logger">日志依赖</param>
    /// <param name="storeOptions"></param>
    /// <param name="cache">缓存依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    protected Store(
        DbContext context,
        IStoreOptions? storeOptions = null,
        IDistributedCache? cache = null,
        ILogger? logger = null) : base(context, storeOptions, cache, logger)
    {
    }

    #region Overrides of StoreBase<TEntity,Guid>

    /// <summary>
    ///     转换字符串到id
    /// </summary>
    /// <param name="id">id字符串</param>
    /// <returns>id</returns>
    protected override Guid ConvertIdFromString(string id)
    {
        return id.GuidFromString();
    }

    /// <summary>
    ///     转换Id为字符串
    /// </summary>
    /// <param name="id">id</param>
    /// <returns>字符串</returns>
    protected override string ConvertIdToString(Guid id)
    {
        return id.GuidToString();
    }

    #endregion
}

/// <summary>
///     抽象存储实现
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TKey">键类型</typeparam>
public abstract class Store<TEntity, TKey> : KeyWithStore<TEntity, TKey>, IStore<TEntity, TKey>
    where TEntity : class, IModelBase<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     创建一个新的基本存储实例
    /// </summary>
    /// <param name="context">数据访问上下文</param>
    /// <param name="logger">日志依赖</param>
    /// <param name="describer">操作异常描述者</param>
    /// <param name="storeOptions">配置依赖</param>
    /// <param name="cache">缓存依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    protected Store(
        DbContext context,
        IStoreOptions? storeOptions = null,
        IDistributedCache? cache = null,
        ILogger? logger = null,
        StoreErrorDescriber? describer = null) : base(context, cache, storeOptions, logger, describer)
    {
        StoreOptions = storeOptions ?? new ArtemisStoreOptions();
    }

    /// <summary>
    ///     是否被删除
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns>判断结果</returns>
    public override bool IsDeleted(TEntity entity)
    {
        if (MetaDataHosting) return entity.DeletedAt is not null;

        return base.IsDeleted(entity);
    }

    /// <summary>
    ///     是否被删除
    /// </summary>
    /// <param name="entity">实体</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>判断结果</returns>
    public override Task<bool> IsDeletedAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        return MetaDataHosting
            ? Task.FromResult(entity.DeletedAt is not null)
            : base.IsDeletedAsync(entity, cancellationToken);
    }

    #region DataAccess

    /// <summary>
    ///     Entity有追踪访问器
    /// </summary>
    public override IQueryable<TEntity> TrackingQuery =>
        EntitySet.WhereIf(SoftDelete, entity => entity.DeletedAt != null);

    #endregion

    #region IStoreOptions

    private IStoreOptions StoreOptions { get; }

    #region Setting

    /// <summary>
    ///     设置是否启用元数据托管
    /// </summary>
    protected bool MetaDataHosting => StoreOptions.MetaDataHosting || StoreOptions.SoftDelete;

    /// <summary>
    ///     设置是否启用软删除
    /// </summary>
    protected bool SoftDelete => StoreOptions.SoftDelete;

    #endregion

    #endregion

    #region Implementation of IStoreMap<TEntity,TKey>

    #region CreateEntity & CreateEntities

    /// <summary>
    ///     通过类型映射创建一个新实例
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="config">映射配置</param>
    /// <returns>映射后实体</returns>
    public StoreResult CreateNew<TSource>(
        TSource source,
        TypeAdapterConfig? config = null)
    {
        OnActionExecuting(source, nameof(source));
        config ??= IgnoreIdAndMateConfig<TSource>();
        var entity = source!.Adapt<TSource, TEntity>(config);
        if (entity is null)
            throw new MapTargetNullException(nameof(entity));
        AddEntity(entity);
        var result = AttacheChange();
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"CreateNew {typeof(TEntity).Name}");

            CacheEntity(entity);
        }

        return result;
    }

    /// <summary>
    ///     通过类型映射创建一组新实例
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="config">映射配置</param>
    /// <returns>创建结果</returns>
    public StoreResult CreateNew<TSource>(
        IEnumerable<TSource> sources,
        TypeAdapterConfig? config = null)
    {
        OnActionExecuting(sources, nameof(sources));
        config ??= IgnoreIdAndMateConfig<TSource>();
        var entities = sources.Adapt<IEnumerable<TEntity>>(config);
        if (entities is null)
            throw new MapTargetNullException(nameof(entities));
        var list = entities.ToList();
        AddEntities(list);
        var result = AttacheChange();
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"CreateNew {result.EffectRows} {typeof(TEntity).Name} Entities");

            CacheEntities(list);
        }

        return result;
    }

    /// <summary>
    ///     通过类型映射创建一个新实例
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns>创建结果</returns>
    public async Task<StoreResult> CreateNewAsync<TSource>(
        TSource source,
        TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(source, nameof(source), cancellationToken);
        config ??= IgnoreIdAndMateConfig<TSource>();
        var entity = source!.Adapt<TSource, TEntity>(config);
        if (entity is null)
            throw new MapTargetNullException(nameof(entity));
        AddEntity(entity);
        var result = await AttacheChangeAsync(cancellationToken);
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"CreateNew {typeof(TEntity).Name}");

            await CacheEntityAsync(entity, cancellationToken);
        }

        return result;
    }

    /// <summary>
    ///     通过类型映射创建一个新实例
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns>创建结果</returns>
    public async Task<StoreResult> CreateNewAsync<TSource>(
        IEnumerable<TSource> sources,
        TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(sources, nameof(sources), cancellationToken);
        config ??= IgnoreIdAndMateConfig<TSource>();
        var entities = sources.Adapt<IEnumerable<TEntity>>(config);
        if (entities is null)
            throw new MapTargetNullException(nameof(entities));
        var list = entities.ToList();
        AddEntities(list);
        var result = await AttacheChangeAsync(cancellationToken);
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"CreateNew {result.EffectRows} {typeof(TEntity).Name} Entities");

            await CacheEntitiesAsync(list, cancellationToken);
        }

        return result;
    }

    #endregion

    #region OverEntity & OverEntities

    /// <summary>
    ///     通过类型映射覆盖对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="config">映射配置</param>
    /// <returns></returns>
    public StoreResult Over<TSource>(
        TSource source,
        TypeAdapterConfig? config = null) where TSource : IKeySlot<TKey>
    {
        OnActionExecuting(source, nameof(source));
        config ??= IgnoreMetaConfig<TSource>();
        var entity = source.Adapt<TSource, TEntity>(config);
        if (entity is null)
            throw new MapTargetNullException(nameof(entity));
        UpdateEntity(entity);
        var result = AttacheChange();
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Over {typeof(TEntity).Name}");

            CacheEntity(entity);
        }

        return result;
    }

    /// <summary>
    ///     通过类型映射覆盖对应实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="destination">目标数据</param>
    /// <param name="config">映射配置</param>
    /// <returns></returns>
    public StoreResult Over<TSource>(
        TSource source,
        TEntity destination,
        TypeAdapterConfig? config = null)
    {
        OnActionExecuting(source, nameof(source));
        config ??= IgnoreIdAndMateConfig<TSource>();
        source.Adapt(destination, config);
        if (destination is null)
            throw new MapTargetNullException(nameof(destination));
        UpdateEntity(destination);
        var result = AttacheChange();
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Over {typeof(TEntity).Name}");

            CacheEntity(destination);
        }

        return result;
    }

    /// <summary>
    ///     通过类型映射覆盖对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="config">映射配置</param>
    /// <returns></returns>
    public StoreResult Over<TSource>(
        IEnumerable<TSource> sources,
        TypeAdapterConfig? config = null)
        where TSource : IKeySlot<TKey>
    {
        OnActionExecuting(sources, nameof(sources));
        config ??= IgnoreMetaConfig<TSource>();
        var entities = sources.Adapt<IEnumerable<TEntity>>(config);
        if (entities is null)
            throw new MapTargetNullException(nameof(entities));
        var list = entities.ToList();
        UpdateEntities(list);
        var result = AttacheChange();
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Over {result.EffectRows} {typeof(TEntity).Name} Entities");

            CacheEntities(list);
        }

        return result;
    }

    /// <summary>
    ///     通过类型映射覆盖对应实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TJKey">连接键类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="destinations">目标数据</param>
    /// <param name="destinationKeySelector">目标连接键选择器</param>
    /// <param name="config">映射配置</param>
    /// <param name="sourceKeySelector">源连接键选择器</param>
    /// <returns></returns>
    public StoreResult Over<TSource, TJKey>(
        IEnumerable<TSource> sources,
        IEnumerable<TEntity> destinations,
        Func<TSource, TJKey> sourceKeySelector,
        Func<TEntity, TJKey> destinationKeySelector,
        TypeAdapterConfig? config = null)
    {
        var sourceList = sources.ToList();
        OnActionExecuting(sourceList, nameof(sources));
        config ??= IgnoreIdAndMateConfig<TSource>();
        var entities = sourceList.Join(
            destinations,
            sourceKeySelector,
            destinationKeySelector,
            (source, _) => source.Adapt<TSource, TEntity>(config));
        if (entities is null)
            throw new MapTargetNullException(nameof(entities));
        var list = entities.ToList();
        UpdateEntities(list);
        var result = AttacheChange();
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Over {result.EffectRows} {typeof(TEntity).Name} Entities");

            CacheEntities(list);
        }

        return result;
    }

    /// <summary>
    ///     通过类型映射覆盖对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> OverAsync<TSource>(
        TSource source,
        TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default) where TSource : IKeySlot<TKey>
    {
        OnAsyncActionExecuting(source, nameof(source), cancellationToken);
        config ??= IgnoreMetaConfig<TSource>();
        var entity = source.Adapt<TSource, TEntity>(config);
        if (entity is null)
            throw new MapTargetNullException(nameof(entity));
        UpdateEntity(entity);
        var result = await AttacheChangeAsync(cancellationToken);
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Over {typeof(TEntity).Name}");

            await CacheEntityAsync(entity, cancellationToken);
        }

        return result;
    }

    /// <summary>
    ///     通过类型映射覆盖对应实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="destination">目标数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> OverAsync<TSource>(
        TSource source,
        TEntity destination,
        TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(source, nameof(source), cancellationToken);
        config ??= IgnoreIdAndMateConfig<TSource>();
        source.Adapt(destination, config);
        if (destination is null)
            throw new MapTargetNullException(nameof(destination));
        UpdateEntity(destination);
        var result = await AttacheChangeAsync(cancellationToken);
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Over {typeof(TEntity).Name}");

            await CacheEntityAsync(destination, cancellationToken);
        }

        return result;
    }

    /// <summary>
    ///     通过类型映射覆盖对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> OverAsync<TSource>(
        IEnumerable<TSource> sources,
        TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default) where TSource : IKeySlot<TKey>
    {
        OnAsyncActionExecuting(sources, nameof(sources), cancellationToken);
        config ??= IgnoreMetaConfig<TSource>();
        var entities = sources.Adapt<IEnumerable<TEntity>>(config);
        if (entities is null)
            throw new MapTargetNullException(nameof(entities));
        var list = entities.ToList();
        UpdateEntities(list);
        var result = await AttacheChangeAsync(cancellationToken);
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Over {result.EffectRows} {typeof(TEntity).Name} Entities");

            await CacheEntitiesAsync(list, cancellationToken);
        }

        return result;
    }

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
    public async Task<StoreResult> OverAsync<TSource, TJKey>(
        IEnumerable<TSource> sources,
        IEnumerable<TEntity> destinations,
        Func<TSource, TJKey> sourceKeySelector,
        Func<TEntity, TJKey> destinationKeySelector,
        TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default)
    {
        var sourceList = sources.ToList();
        OnAsyncActionExecuting(sourceList, nameof(sources), cancellationToken);
        config ??= IgnoreIdAndMateConfig<TSource>();
        var entities = sourceList.Join(
            destinations,
            sourceKeySelector,
            destinationKeySelector,
            (source, _) => source.Adapt<TSource, TEntity>(config));
        if (entities is null)
            throw new MapTargetNullException(nameof(entities));
        var list = entities.ToList();
        UpdateEntities(list);
        var result = await AttacheChangeAsync(cancellationToken);
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Over {result.EffectRows} {typeof(TEntity).Name} Entities");

            await CacheEntitiesAsync(list, cancellationToken);
        }

        return result;
    }

    #endregion

    #region MergeEntity & MergeEntities

    /// <summary>
    ///     通过类型映射合并对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="config">映射配置</param>
    /// <returns></returns>
    public StoreResult Merge<TSource>(
        TSource source,
        TypeAdapterConfig? config = null) where TSource : IKeySlot<TKey>
    {
        OnActionExecuting(source, nameof(source));
        var entity = FindById(source.Id);
        if (entity is null)
            throw new MapTargetNullException(nameof(entity));
        config ??= IgnoreNullAndMateConfig<TSource>();
        source.Adapt(entity, config);
        UpdateEntity(entity);
        var result = AttacheChange();
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Merge {typeof(TEntity).Name}");

            CacheEntity(entity);
        }

        return result;
    }

    /// <summary>
    ///     通过类型映射合并对应实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="destination">目标数据</param>
    /// <param name="config">映射配置</param>
    /// <returns></returns>
    public StoreResult Merge<TSource>(
        TSource source,
        TEntity destination,
        TypeAdapterConfig? config = null)
    {
        OnActionExecuting(source, nameof(source));
        config ??= IgnoreIdAndNullAndMateConfig<TSource>();
        source.Adapt(destination, config);
        if (destination is null)
            throw new MapTargetNullException(nameof(destination));
        UpdateEntity(destination);
        var result = AttacheChange();
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Merge {typeof(TEntity).Name}");

            CacheEntity(destination);
        }

        return result;
    }

    /// <summary>
    ///     通过类型映射合并对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="config">映射配置</param>
    /// <returns></returns>
    public StoreResult Merge<TSource>(
        IEnumerable<TSource> sources,
        TypeAdapterConfig? config = null)
        where TSource : IKeySlot<TKey>
    {
        var sourceList = sources.ToList();
        OnActionExecuting(sourceList, nameof(sources));
        var destinations = FindByIds(sourceList.Select(source => source.Id));
        if (destinations is null)
            throw new MapTargetNullException(nameof(destinations));
        config ??= IgnoreNullAndMateConfig<TSource>();
        var list = sourceList.Join(
            destinations, source => source.Id,
            destination => destination.Id,
            (source, destination) => source.Adapt(destination, config)).ToList();
        UpdateEntities(list);
        var result = AttacheChange();
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Merge {result.EffectRows} {typeof(TEntity).Name} Entities");

            CacheEntities(list);
        }

        return result;
    }

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
    public StoreResult Merge<TSource, TJKey>(
        IEnumerable<TSource> sources,
        IEnumerable<TEntity> destinations,
        Func<TSource, TJKey> sourceKeySelector,
        Func<TEntity, TJKey> destinationKeySelector,
        TypeAdapterConfig? config = null)
    {
        var sourceList = sources.ToList();
        OnActionExecuting(sourceList, nameof(sources));
        config ??= IgnoreIdAndNullAndMateConfig<TSource>();
        var entities = sourceList.Join(
            destinations,
            sourceKeySelector,
            destinationKeySelector,
            (source, _) => source.Adapt<TSource, TEntity>(config));
        if (entities is null)
            throw new MapTargetNullException(nameof(entities));
        var list = entities.ToList();
        UpdateEntities(list);
        var result = AttacheChange();
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Merge {result.EffectRows} {typeof(TEntity).Name} Entities");

            CacheEntities(list);
        }

        return result;
    }

    /// <summary>
    ///     通过类型映射合并对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> MergeAsync<TSource>(
        TSource source,
        TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default) where TSource : IKeySlot<TKey>
    {
        OnAsyncActionExecuting(source, nameof(source), cancellationToken);
        var entity = await FindByIdAsync(source.Id, cancellationToken);
        if (entity is null)
            throw new MapTargetNullException(nameof(entity));
        config ??= IgnoreNullAndMateConfig<TSource>();
        source.Adapt(entity, config);
        UpdateEntity(entity);
        var result = await AttacheChangeAsync(cancellationToken);
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Merge {typeof(TEntity).Name}");

            await CacheEntityAsync(entity, cancellationToken);
        }

        return result;
    }

    /// <summary>
    ///     通过类型映射合并对应实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="destination">目标数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> MergeAsync<TSource>(
        TSource source,
        TEntity destination,
        TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(source, nameof(source), cancellationToken);
        config ??= IgnoreIdAndNullAndMateConfig<TSource>();
        source.Adapt(destination, config);
        if (destination is null)
            throw new MapTargetNullException(nameof(destination));
        UpdateEntity(destination);
        var result = await AttacheChangeAsync(cancellationToken);
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Merge {typeof(TEntity).Name}");

            await CacheEntityAsync(destination, cancellationToken);
        }

        return result;
    }

    /// <summary>
    ///     通过类型映射合并对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> MergeAsync<TSource>(
        IEnumerable<TSource> sources,
        TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default) where TSource : IKeySlot<TKey>
    {
        var sourceList = sources.ToList();
        OnAsyncActionExecuting(sourceList, nameof(sources), cancellationToken);
        var destinations = await FindByIdsAsync(sourceList.Select(source => source.Id), cancellationToken);
        if (destinations is null)
            throw new MapTargetNullException(nameof(destinations));
        config ??= IgnoreNullAndMateConfig<TSource>();
        var list = sourceList.Join(
            destinations, source => source.Id,
            destination => destination.Id,
            (source, destination) => source.Adapt(destination, config)).ToList();
        UpdateEntities(list);
        var result = await AttacheChangeAsync(cancellationToken);
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Merge {result.EffectRows} {typeof(TEntity).Name} Entities");

            await CacheEntitiesAsync(list, cancellationToken);
        }

        return result;
    }

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
    public async Task<StoreResult> MergeAsync<TSource, TJKey>(
        IEnumerable<TSource> sources,
        IEnumerable<TEntity> destinations,
        Func<TSource, TJKey> sourceKeySelector,
        Func<TEntity, TJKey> destinationKeySelector,
        TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default)
    {
        var sourceList = sources.ToList();
        OnAsyncActionExecuting(sourceList, nameof(sources), cancellationToken);
        config ??= IgnoreIdAndNullAndMateConfig<TSource>();
        var entities = sourceList.Join(
            destinations,
            sourceKeySelector,
            destinationKeySelector,
            (source, _) => source.Adapt<TSource, TEntity>(config));
        if (entities is null)
            throw new MapTargetNullException(nameof(entities));
        var list = entities.ToList();
        UpdateEntities(list);
        var result = await AttacheChangeAsync(cancellationToken);
        if (result.Succeeded)
        {
            if (DebugLogger) Logger?.LogDebug($"Merge {result.EffectRows} {typeof(TEntity).Name} Entities");

            await CacheEntitiesAsync(list, cancellationToken);
        }

        return result;
    }

    #endregion

    #region MapConfig

    /// <summary>
    ///     忽略标识和空值和元数据缓存
    /// </summary>
    private TypeAdapterConfig? _ignoreIdAndNullAndMateConfig;

    /// <summary>
    ///     忽略源实体的空值属性和目标实体的Id和元数据属性
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    protected TypeAdapterConfig IgnoreIdAndNullAndMateConfig<TSource>()
    {
        if (_ignoreIdAndNullAndMateConfig is not null)
            return _ignoreIdAndNullAndMateConfig;
        _ignoreIdAndNullAndMateConfig = IgnoreIdAndNullConfig<TSource>().Clone();
        _ignoreIdAndNullAndMateConfig.NewConfig<TSource, TEntity>()
            .IgnoreNullValues(true)
            .Ignore(item => item.CreatedAt)
            .Ignore(item => item.UpdatedAt)
            .Ignore(item => item.DeletedAt!);
        return _ignoreIdAndNullAndMateConfig;
    }

    /// <summary>
    ///     忽略空值和元数据缓存
    /// </summary>
    private TypeAdapterConfig? _ignoreNullAndMateConfig;

    /// <summary>
    ///     忽略源实体的空值属性和目标实体的元数据属性
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    protected TypeAdapterConfig IgnoreNullAndMateConfig<TSource>()
    {
        if (_ignoreNullAndMateConfig is not null)
            return _ignoreNullAndMateConfig;
        _ignoreNullAndMateConfig = IgnoreNullConfig<TSource>().Clone();
        _ignoreNullAndMateConfig.NewConfig<TSource, TEntity>()
            .IgnoreNullValues(true)
            .Ignore(item => item.CreatedAt)
            .Ignore(item => item.UpdatedAt)
            .Ignore(item => item.DeletedAt!);
        return _ignoreNullAndMateConfig;
    }

    /// <summary>
    ///     忽略Id和元数据缓存
    /// </summary>
    private TypeAdapterConfig? _ignoreIdAndMateConfig;

    /// <summary>
    ///     忽略目标实体的Id和元数据属性
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    protected TypeAdapterConfig IgnoreIdAndMateConfig<TSource>()
    {
        if (_ignoreIdAndMateConfig is not null)
            return _ignoreIdAndMateConfig;
        _ignoreIdAndMateConfig = IgnoreIdConfig<TSource>().Clone();
        _ignoreIdAndMateConfig.NewConfig<TSource, TEntity>()
            .Ignore(item => item.CreatedAt)
            .Ignore(item => item.UpdatedAt)
            .Ignore(item => item.DeletedAt!);
        return _ignoreIdAndMateConfig;
    }

    /// <summary>
    ///     元数据忽略缓存
    /// </summary>
    private TypeAdapterConfig? _ignoreMetaConfig;

    /// <summary>
    ///     忽略目标实体的元数据属性
    /// </summary>
    /// <typeparam name="TSource">源数据类型</typeparam>
    /// <returns>映射配置</returns>
    protected TypeAdapterConfig IgnoreMetaConfig<TSource>()
    {
        if (_ignoreMetaConfig is not null)
            return _ignoreMetaConfig;
        _ignoreMetaConfig = new TypeAdapterConfig();
        _ignoreMetaConfig.NewConfig<TSource, TEntity>()
            .Ignore(item => item.CreatedAt)
            .Ignore(item => item.UpdatedAt)
            .Ignore(item => item.DeletedAt!);
        return _ignoreMetaConfig;
    }

    #endregion

    #endregion

    #region ActionExecution

    /// <summary>
    ///     添加单个实体
    /// </summary>
    /// <param name="entity">实体</param>
    protected override void AddEntity(TEntity entity)
    {
        if (MetaDataHosting)
        {
            var now = DateTime.Now;
            entity.CreatedAt = now;
            entity.UpdatedAt = now;
        }

        Context.Add(entity);
    }

    /// <summary>
    ///     添加多个实体
    /// </summary>
    /// <param name="entities">实体</param>
    protected override void AddEntities(ICollection<TEntity> entities)
    {
        if (MetaDataHosting)
        {
            var now = DateTime.Now;
            foreach (var entity in entities)
            {
                entity.CreatedAt = now;
                entity.UpdatedAt = now;
            }
        }

        Context.AddRange(entities);
    }

    /// <summary>
    ///     追踪一个实体更新
    /// </summary>
    /// <param name="entity">实体</param>
    protected override void UpdateEntity(TEntity entity)
    {
        Context.Attach(entity);

        if (entity is IConcurrencyStamp concurrency)
            concurrency.ConcurrencyStamp = Guid.NewGuid().ToString();

        if (MetaDataHosting)
        {
            var now = DateTime.Now;
            entity.UpdatedAt = now;
        }

        Context.Update(entity);

        if (MetaDataHosting)
            Context.Entry(entity).Property(item => item.CreatedAt).IsModified = false;
    }

    /// <summary>
    ///     追踪多个实体更新
    /// </summary>
    /// <param name="entities">实体</param>
    protected override void UpdateEntities(ICollection<TEntity> entities)
    {
        Context.AttachRange(entities);

        if (MetaDataHosting)
        {
            var now = DateTime.Now;
            foreach (var entity in entities) entity.UpdatedAt = now;
        }

        Context.UpdateRange(entities);

        if (MetaDataHosting)
            foreach (var entity in entities)
                Context.Entry(entity).Property(item => item.CreatedAt).IsModified = false;
    }

    /// <summary>
    ///     批量删除实体
    /// </summary>
    /// <param name="query">查询</param>
    /// <param name="setter">更新委托</param>
    /// <returns></returns>
    protected override int BatchUpdateEntity(
        IQueryable<TEntity> query,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter)
    {
        if (MetaDataHosting)
            query.ExecuteUpdate(metaSetter => metaSetter.SetProperty(entity => entity.UpdatedAt, DateTime.Now));

        return query.ExecuteUpdate(setter);
    }

    /// <summary>
    ///     批量删除实体
    /// </summary>
    /// <param name="query">查询</param>
    /// <param name="setter">更新委托</param>
    /// <param name="cancellationToken">异步操作取消信号</param>
    /// <returns></returns>
    protected override Task<int> BatchUpdateEntityAsync(
        IQueryable<TEntity> query,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter,
        CancellationToken cancellationToken = default)
    {
        if (MetaDataHosting)
            query.ExecuteUpdate(metaSetter => metaSetter.SetProperty(entity => entity.UpdatedAt, DateTime.Now));

        return query.ExecuteUpdateAsync(setter, cancellationToken);
    }

    /// <summary>
    ///     追踪一个实体删除
    /// </summary>
    /// <param name="entity">实体</param>
    protected override void DeleteEntity(TEntity entity)
    {
        if (SoftDelete)
        {
            Context.Attach(entity);
            var now = DateTime.Now;
            entity.UpdatedAt = now;
            entity.DeletedAt = now;
            Context.Update(entity);

            Context.Entry(entity).Property(item => item.CreatedAt).IsModified = false;
        }
        else
        {
            Context.Remove(entity);
        }
    }

    /// <summary>
    ///     追踪多个实体删除
    /// </summary>
    /// <param name="entities">实体</param>
    protected override void DeleteEntities(ICollection<TEntity> entities)
    {
        if (SoftDelete)
        {
            Context.AttachRange(entities);
            var now = DateTime.Now;
            foreach (var entity in entities)
            {
                entity.UpdatedAt = now;
                entity.DeletedAt = now;
            }

            Context.UpdateRange(entities);

            foreach (var entity in entities)
                Context.Entry(entity).Property(item => item.CreatedAt).IsModified = false;
        }
        else
        {
            Context.RemoveRange(entities);
        }
    }

    /// <summary>
    ///     批量删除实体
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    protected override int BatchDeleteEntity(IQueryable<TEntity> query)
    {
        if (SoftDelete)
            return query.ExecuteUpdate(setter => setter
                .SetProperty(entity => entity.UpdatedAt, DateTime.Now)
                .SetProperty(entity => entity.DeletedAt, DateTime.Now));

        return query.ExecuteDelete();
    }

    /// <summary>
    ///     异步批量删除实体
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected override Task<int> BatchDeleteEntityAsync(
        IQueryable<TEntity> query,
        CancellationToken cancellationToken = default)
    {
        if (SoftDelete)
            return query.ExecuteUpdateAsync(setter => setter
                .SetProperty(entity => entity.UpdatedAt, DateTime.Now)
                .SetProperty(entity => entity.DeletedAt, DateTime.Now), cancellationToken);

        return query.ExecuteDeleteAsync(cancellationToken);
    }

    #endregion
}