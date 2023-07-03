using Artemis.Data.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Data.Store;

/// <summary>
///     抽象存储实现
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public class Store<TEntity> : Store<TEntity, DbContext>
    where TEntity : ModelBase
{
    /// <summary>
    ///     创建一个新的基本存储实例
    /// </summary>
    /// <param name="context">数据访问上下文</param>
    /// <param name="describer">操作异常描述者</param>
    /// <exception cref="ArgumentNullException"></exception>
    public Store(DbContext context, IStoreErrorDescriber? describer = null) : base(context, describer)
    {
    }
}

/// <summary>
///     抽象存储实现
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TContext">数据上下文类型</typeparam>
public abstract class Store<TEntity, TContext> : Store<TEntity, TContext, Guid>, IStore<TEntity>
    where TEntity : ModelBase, IModelBase, IModelBase<Guid>
    where TContext : DbContext
{
    /// <summary>
    ///     创建一个新的基本存储实例
    /// </summary>
    /// <param name="context">数据访问上下文</param>
    /// <param name="describer">操作异常描述者</param>
    /// <exception cref="ArgumentNullException"></exception>
    protected Store(TContext context, IStoreErrorDescriber? describer = null) : base(context, describer)
    {
    }

    #region Overrides of StoreBase<TEntity,Guid>

    /// <summary>
    ///     转换字符串到id
    /// </summary>
    /// <param name="id">id字符串</param>
    /// <returns>id</returns>
    protected override Guid ConvertIdFromString(string? id)
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
/// <typeparam name="TContext">数据上下文类型</typeparam>
/// <typeparam name="TKey">键类型</typeparam>
public abstract class Store<TEntity, TContext, TKey> : StoreBase<TEntity, TKey>, IStore<TEntity, TKey>
    where TEntity : ModelBase<TKey>, IModelBase<TKey>
    where TContext : DbContext
    where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     创建一个新的基本存储实例
    /// </summary>
    /// <param name="context">数据访问上下文</param>
    /// <param name="describer">操作异常描述者</param>
    /// <exception cref="ArgumentNullException"></exception>
    protected Store(TContext context, IStoreErrorDescriber? describer = null) : base(describer ??
        new StoreErrorDescriber())
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    #region Setting

    /// <summary>
    ///     设置是否自动保存更改
    /// </summary>
    public bool AutoSaveChanges { get; set; } = true;

    /// <summary>
    ///     MetaDataHosting标记
    /// </summary>
    private bool _metaDataHosting = true;

    /// <summary>
    ///     设置是否启用元数据托管
    /// </summary>
    public bool MetaDataHosting
    {
        get => _metaDataHosting;
        set => _metaDataHosting = SoftDelete || value;
    }

    /// <summary>
    ///     SoftDelete标记
    /// </summary>
    private bool _softDelete;

    /// <summary>
    ///     设置是否启用软删除
    /// </summary>
    public bool SoftDelete
    {
        get => _softDelete;
        set
        {
            _softDelete = value;
            if (_softDelete) MetaDataHosting = _softDelete;
        }
    }

    #endregion

    #region DataAccess

    /// <summary>
    ///     数据访问上下文
    /// </summary>
    protected virtual TContext Context { get; } = null!;

    /// <summary>
    ///     EntitySet访问器*Main Store Set*
    /// </summary>
    public virtual DbSet<TEntity> EntitySet => Context.Set<TEntity>();

    /// <summary>
    ///     Entity有追踪访问器
    /// </summary>
    public virtual IQueryable<TEntity> TrackingQuery => EntitySet.Where(item => !SoftDelete || item.DeletedAt != null);

    /// <summary>
    ///     Entity无追踪访问器
    /// </summary>
    public virtual IQueryable<TEntity> EntityQuery =>
        EntitySet.Where(item => !SoftDelete || item.DeletedAt != null).AsNoTracking();

    #endregion

    #region SaveChanges

    /// <summary>
    ///     保存当前存储
    /// </summary>
    /// <returns></returns>
    private int SaveChanges()
    {
        return AutoSaveChanges ? Context.SaveChanges() : 0;
    }

    /// <summary>
    ///     保存当前存储
    /// </summary>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>异步取消结果</returns>
    private Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return AutoSaveChanges ? Context.SaveChangesAsync(cancellationToken) : Task.FromResult(0);
    }

    #endregion

    #region Implementation of IStoreCommon<TEntity,in TKey>

    #region CreateEntity & CreateEntities

    /// <summary>
    ///     在<paramref name="entity" />存储中创建一个新的实体
    /// </summary>
    /// <param name="entity">被创建实体</param>
    /// <returns></returns>
    public StoreResult Create(TEntity entity)
    {
        OnActionExecuting(entity, nameof(entity));
        AddEntity(entity);
        var changes = SaveChanges();
        return StoreResult.Success(changes);
    }

    /// <summary>
    ///     在<paramref name="entities" />存储中创建多个新的实体
    /// </summary>
    /// <param name="entities">被创建实体</param>
    /// <returns></returns>
    public StoreResult Create(IEnumerable<TEntity> entities)
    {
        var list = entities.ToList();
        OnActionExecuting(list, nameof(entities));
        AddEntities(list);
        var changes = SaveChanges();
        return StoreResult.Success(changes);
    }

    /// <summary>
    ///     在<paramref name="entity" />存储中创建一个新的实体
    /// </summary>
    /// <param name="entity">被创建实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(entity, nameof(entity), cancellationToken);
        AddEntity(entity);
        var changes = await SaveChangesAsync(cancellationToken);
        return StoreResult.Success(changes);
    }

    /// <summary>
    ///     在<paramref name="entities" />存储中创建多个新的实体
    /// </summary>
    /// <param name="entities">被创建实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> CreateAsync(IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        var list = entities.ToList();
        OnAsyncActionExecuting(list, nameof(entities), cancellationToken);
        AddEntities(list);
        var changes = await SaveChangesAsync(cancellationToken);
        return StoreResult.Success(changes);
    }

    #endregion

    #region UpdateEntity & UpdateEntities

    /// <summary>
    ///     在<paramref name="entity" />存储中更新已存在的实体
    /// </summary>
    /// <param name="entity">被创建实体</param>
    /// <returns></returns>
    public StoreResult Update(TEntity entity)
    {
        OnActionExecuting(entity, nameof(entity));
        UpdateEntity(entity);
        return AttacheChange();
    }

    /// <summary>
    ///     在<paramref name="entities" />存储中更新多个已存在的实体
    /// </summary>
    /// <param name="entities">被创建实体</param>
    /// <returns></returns>
    public StoreResult Update(IEnumerable<TEntity> entities)
    {
        var list = entities.ToList();
        OnActionExecuting(list, nameof(entities));
        UpdateEntities(list);
        return AttacheChange();
    }

    /// <summary>
    ///     在<paramref name="entity" />存储中更新已存在的实体
    /// </summary>
    /// <param name="entity">被创建实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public Task<StoreResult> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(entity, nameof(entity), cancellationToken);
        UpdateEntity(entity);
        return AttacheChangeAsync(cancellationToken);
    }

    /// <summary>
    ///     在<paramref name="entities" />存储中更新多个已存在的实体
    /// </summary>
    /// <param name="entities">被创建实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public Task<StoreResult> UpdateAsync(IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        var list = entities.ToList();
        OnAsyncActionExecuting(list, nameof(entities), cancellationToken);
        UpdateEntities(list);
        return AttacheChangeAsync(cancellationToken);
    }

    #endregion

    #region DeleteEntity & DeleteEntities

    /// <summary>
    ///     在存储中删除已存在的实体
    /// </summary>
    /// <param name="id">被删除实体的主键</param>
    public StoreResult Delete(TKey id)
    {
        OnActionExecuting(id, nameof(id));
        var entity = FindEntity(id);
        if (entity == null) return StoreResult.Failed(ErrorDescriber.NotFoundId(ConvertIdToString(id)));
        DeleteEntity(entity);
        return AttacheChange();
    }

    /// <summary>
    ///     在<paramref name="entity" />存储中删除已存在的实体
    /// </summary>
    /// <param name="entity">被删除实体</param>
    /// <returns></returns>
    public StoreResult Delete(TEntity entity)
    {
        OnActionExecuting(entity, nameof(entity));
        DeleteEntity(entity);
        return AttacheChange();
    }

    /// <summary>
    ///     在存储中删除已存在的实体
    /// </summary>
    /// <param name="ids">被删除实体的主键</param>
    /// <returns></returns>
    public StoreResult Delete(IEnumerable<TKey> ids)
    {
        var idList = ids as List<TKey> ?? ids.ToList();
        OnActionExecuting(idList, nameof(ids));
        var entityList = FindEntities(idList).ToList();
        if (!entityList.Any())
        {
            var idsString = string.Join(",", idList.Select(ConvertIdToString));
            return StoreResult.Failed(ErrorDescriber.NotFoundId(idsString));
        }

        DeleteEntities(entityList);
        return AttacheChange();
    }

    /// <summary>
    ///     在<paramref name="entities" />存储中删除已存在的实体
    /// </summary>
    /// <param name="entities">被删除实体</param>
    /// <returns></returns>
    public StoreResult Delete(IEnumerable<TEntity> entities)
    {
        var list = entities.ToList();
        OnActionExecuting(list, nameof(entities));
        DeleteEntities(list);
        return AttacheChange();
    }

    /// <summary>
    ///     在存储中删除已存在的实体
    /// </summary>
    /// <param name="id">被删除实体的主键</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> DeleteAsync(TKey id, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(id, nameof(id), cancellationToken);
        var entity = await FindEntityAsync(id, cancellationToken);
        if (entity == null) return StoreResult.Failed(ErrorDescriber.NotFoundId(ConvertIdToString(id)));
        DeleteEntity(entity);
        return await AttacheChangeAsync(cancellationToken);
    }

    /// <summary>
    ///     在<paramref name="entity" />存储中删除已存在的实体
    /// </summary>
    /// <param name="entity">被删除实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public Task<StoreResult> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(entity, nameof(entity), cancellationToken);
        DeleteEntity(entity);
        return AttacheChangeAsync(cancellationToken);
    }

    /// <summary>
    ///     在存储中删除已存在的实体
    /// </summary>
    /// <param name="ids">被删除实体的主键</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> DeleteAsync(IEnumerable<TKey> ids,
        CancellationToken cancellationToken = default)
    {
        var idList = ids as List<TKey> ?? ids.ToList();
        OnAsyncActionExecuting(idList, nameof(ids), cancellationToken);
        var entityList = (await FindEntitiesAsync(idList, cancellationToken)).ToList();
        if (!entityList.Any())
        {
            var idsString = string.Join(",", idList.Select(ConvertIdToString));
            return StoreResult.Failed(ErrorDescriber.NotFoundId(idsString));
        }

        DeleteEntities(entityList);
        return await AttacheChangeAsync(cancellationToken);
    }

    /// <summary>
    ///     在<paramref name="entities" />存储中删除已存在的实体
    /// </summary>
    /// <param name="entities">被删除实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public Task<StoreResult> DeleteAsync(IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        var list = entities.ToList();
        OnAsyncActionExecuting(list, nameof(entities), cancellationToken);
        DeleteEntities(list);
        return AttacheChangeAsync(cancellationToken);
    }

    #endregion

    #region FindEntity & FindEntities

    /// <summary>
    ///     根据Id查找实体
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public TEntity? FindEntity(TKey id)
    {
        OnActionExecuting(id, nameof(id));
        return FindById(id);
    }

    /// <summary>
    ///     根据Id查找实体
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public IEnumerable<TEntity> FindEntities(IEnumerable<TKey> ids)
    {
        var idArray = ids as TKey[] ?? ids.ToArray();
        OnActionExecuting(idArray, nameof(ids));
        return FindByIds(idArray);
    }

    /// <summary>
    ///     根据Id查找实体
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public Task<TEntity?> FindEntityAsync(TKey id, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(id, nameof(id), cancellationToken);
        return FindByIdAsync(id, cancellationToken);
    }

    /// <summary>
    ///     根据Id查找实体
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public Task<IEnumerable<TEntity>> FindEntitiesAsync(IEnumerable<TKey> ids,
        CancellationToken cancellationToken = default)
    {
        var idArray = ids as TKey[] ?? ids.ToArray();
        OnAsyncActionExecuting(idArray, nameof(ids), cancellationToken);
        return FindByIdsAsync(idArray, cancellationToken);
    }

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
    public StoreResult CreateNew<TSource>(TSource source, TypeAdapterConfig? config = null)
    {
        OnActionExecuting(source, nameof(source));
        config ??= IgnoreIdConfig<TSource>();
        var entity = source!.Adapt<TSource, TEntity>(config);
        if (entity == null) throw new MapTargetNullException(nameof(entity));
        AddEntity(entity);
        var changes = SaveChanges();
        return StoreResult.Success(changes);
    }

    /// <summary>
    ///     通过类型映射创建一组新实例
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="config">映射配置</param>
    /// <returns>创建结果</returns>
    public StoreResult CreateNew<TSource>(IEnumerable<TSource> sources, TypeAdapterConfig? config = null)
    {
        OnActionExecuting(sources, nameof(sources));
        config ??= IgnoreIdConfig<TSource>();
        var entities = sources.Adapt<IEnumerable<TEntity>>(config);
        if (entities == null) throw new MapTargetNullException(nameof(entities));
        var list = entities.ToList();
        AddEntities(list);
        var changes = SaveChanges();
        return StoreResult.Success(changes);
    }

    /// <summary>
    ///     通过类型映射创建一个新实例
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns>创建结果</returns>
    public async Task<StoreResult> CreateNewAsync<TSource>(TSource source, TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(source, nameof(source), cancellationToken);
        config ??= IgnoreIdConfig<TSource>();
        var entity = source!.Adapt<TSource, TEntity>(config);
        if (entity == null) throw new MapTargetNullException(nameof(entity));
        AddEntity(entity);
        var changes = await SaveChangesAsync(cancellationToken);
        return StoreResult.Success(changes);
    }

    /// <summary>
    ///     通过类型映射创建一个新实例
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns>创建结果</returns>
    public async Task<StoreResult> CreateNewAsync<TSource>(IEnumerable<TSource> sources,
        TypeAdapterConfig? config = null, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(sources, nameof(sources), cancellationToken);
        config ??= IgnoreIdConfig<TSource>();
        var entities = sources.Adapt<IEnumerable<TEntity>>(config);
        if (entities == null) throw new MapTargetNullException(nameof(entities));
        var list = entities.ToList();
        AddEntities(list);
        var changes = await SaveChangesAsync(cancellationToken);
        return StoreResult.Success(changes);
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
    public StoreResult Over<TSource>(TSource source, TypeAdapterConfig? config = null) where TSource : IKeySlot<TKey>
    {
        OnActionExecuting(source, nameof(source));
        config ??= IgnoreMetaConfig<TSource>();
        var entity = source.Adapt<TSource, TEntity>(config);
        if (entity == null) throw new MapTargetNullException(nameof(entity));
        UpdateEntity(entity);
        return AttacheChange();
    }

    /// <summary>
    ///     通过类型映射覆盖对应实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="destination">目标数据</param>
    /// <param name="config">映射配置</param>
    /// <returns></returns>
    public StoreResult Over<TSource>(TSource source, TEntity destination, TypeAdapterConfig? config = null)
    {
        OnActionExecuting(source, nameof(source));
        config ??= IgnoreIdConfig<TSource>();
        source.Adapt(destination, config);
        if (destination == null) throw new MapTargetNullException(nameof(destination));
        UpdateEntity(destination);
        return AttacheChange();
    }

    /// <summary>
    ///     通过类型映射覆盖对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="config">映射配置</param>
    /// <returns></returns>
    public StoreResult Over<TSource>(IEnumerable<TSource> sources, TypeAdapterConfig? config = null)
        where TSource : IKeySlot<TKey>
    {
        OnActionExecuting(sources, nameof(sources));
        config ??= IgnoreMetaConfig<TSource>();
        var entities = sources.Adapt<IEnumerable<TEntity>>(config);
        if (entities == null) throw new MapTargetNullException(nameof(entities));
        var list = entities.ToList();
        UpdateEntities(list);
        return AttacheChange();
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
        config ??= IgnoreIdConfig<TSource>();
        var entities = sourceList.Join(destinations, sourceKeySelector, destinationKeySelector,
            (source, _) => source.Adapt<TSource, TEntity>(config));
        if (entities == null) throw new MapTargetNullException(nameof(entities));
        var list = entities.ToList();
        UpdateEntities(list);
        return AttacheChange();
    }

    /// <summary>
    ///     通过类型映射覆盖对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public Task<StoreResult> OverAsync<TSource>(TSource source, TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default) where TSource : IKeySlot<TKey>
    {
        OnAsyncActionExecuting(source, nameof(source), cancellationToken);
        config ??= IgnoreMetaConfig<TSource>();
        var entity = source.Adapt<TSource, TEntity>(config);
        if (entity == null) throw new MapTargetNullException(nameof(entity));
        UpdateEntity(entity);
        return AttacheChangeAsync(cancellationToken);
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
    public Task<StoreResult> OverAsync<TSource>(TSource source, TEntity destination, TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(source, nameof(source), cancellationToken);
        config ??= IgnoreIdConfig<TSource>();
        source.Adapt(destination, config);
        if (destination == null) throw new MapTargetNullException(nameof(destination));
        UpdateEntity(destination);
        return AttacheChangeAsync(cancellationToken);
    }

    /// <summary>
    ///     通过类型映射覆盖对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public Task<StoreResult> OverAsync<TSource>(IEnumerable<TSource> sources, TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default) where TSource : IKeySlot<TKey>
    {
        OnAsyncActionExecuting(sources, nameof(sources), cancellationToken);
        config ??= IgnoreMetaConfig<TSource>();
        var entities = sources.Adapt<IEnumerable<TEntity>>(config);
        if (entities == null) throw new MapTargetNullException(nameof(entities));
        var list = entities.ToList();
        UpdateEntities(list);
        return AttacheChangeAsync(cancellationToken);
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
    public Task<StoreResult> OverAsync<TSource, TJKey>(
        IEnumerable<TSource> sources,
        IEnumerable<TEntity> destinations,
        Func<TSource, TJKey> sourceKeySelector,
        Func<TEntity, TJKey> destinationKeySelector,
        TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default)
    {
        var sourceList = sources.ToList();
        OnAsyncActionExecuting(sourceList, nameof(sources), cancellationToken);
        config ??= IgnoreIdConfig<TSource>();
        var entities = sourceList.Join(destinations, sourceKeySelector, destinationKeySelector,
            (source, _) => source.Adapt<TSource, TEntity>(config));
        if (entities == null) throw new MapTargetNullException(nameof(entities));
        var list = entities.ToList();
        UpdateEntities(list);
        return AttacheChangeAsync(cancellationToken);
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
    public StoreResult Merge<TSource>(TSource source, TypeAdapterConfig? config = null) where TSource : IKeySlot<TKey>
    {
        OnActionExecuting(source, nameof(source));
        var entity = FindById(source.Id);
        if (entity == null) throw new MapTargetNullException(nameof(entity));
        config ??= IgnoreNullConfig<TSource>();
        source.Adapt(entity, config);
        UpdateEntity(entity);
        return AttacheChange();
    }

    /// <summary>
    ///     通过类型映射合并对应实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="destination">目标数据</param>
    /// <param name="config">映射配置</param>
    /// <returns></returns>
    public StoreResult Merge<TSource>(TSource source, TEntity destination, TypeAdapterConfig? config = null)
    {
        OnActionExecuting(source, nameof(source));
        config ??= IgnoreIdAndNullConfig<TSource>();
        source.Adapt(destination, config);
        if (destination == null) throw new MapTargetNullException(nameof(destination));
        UpdateEntity(destination);
        return AttacheChange();
    }

    /// <summary>
    ///     通过类型映射合并对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="config">映射配置</param>
    /// <returns></returns>
    public StoreResult Merge<TSource>(IEnumerable<TSource> sources, TypeAdapterConfig? config = null)
        where TSource : IKeySlot<TKey>
    {
        var sourceList = sources.ToList();
        OnActionExecuting(sourceList, nameof(sources));
        var destinations = FindByIds(sourceList.Select(source => source.Id));
        if (destinations == null) throw new MapTargetNullException(nameof(destinations));
        config ??= IgnoreNullConfig<TSource>();
        var entities = sourceList.Join(destinations, source => source.Id, destination => destination.Id,
            (source, destination) => source.Adapt(destination, config)).ToList();
        UpdateEntities(entities);
        return AttacheChange();
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
    public StoreResult Merge<TSource, TJKey>(IEnumerable<TSource> sources, IEnumerable<TEntity> destinations,
        Func<TSource, TJKey> sourceKeySelector,
        Func<TEntity, TJKey> destinationKeySelector, TypeAdapterConfig? config = null)
    {
        var sourceList = sources.ToList();
        OnActionExecuting(sourceList, nameof(sources));
        config ??= IgnoreIdAndNullConfig<TSource>();
        var entities = sourceList.Join(destinations, sourceKeySelector, destinationKeySelector,
            (source, _) => source.Adapt<TSource, TEntity>(config));
        if (entities == null) throw new MapTargetNullException(nameof(entities));
        var list = entities.ToList();
        UpdateEntities(list);
        return AttacheChange();
    }

    /// <summary>
    ///     通过类型映射合并对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> MergeAsync<TSource>(TSource source, TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default) where TSource : IKeySlot<TKey>
    {
        OnAsyncActionExecuting(source, nameof(source), cancellationToken);
        var entity = await FindByIdAsync(source.Id, cancellationToken);
        if (entity == null) throw new MapTargetNullException(nameof(entity));
        config ??= IgnoreNullConfig<TSource>();
        source.Adapt(entity, config);
        UpdateEntity(entity);
        return await AttacheChangeAsync(cancellationToken);
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
    public Task<StoreResult> MergeAsync<TSource>(TSource source, TEntity destination, TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(source, nameof(source), cancellationToken);
        config ??= IgnoreIdAndNullConfig<TSource>();
        source.Adapt(destination, config);
        if (destination == null) throw new MapTargetNullException(nameof(destination));
        UpdateEntity(destination);
        return AttacheChangeAsync(cancellationToken);
    }

    /// <summary>
    ///     通过类型映射合并对应Id的实体
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="config">映射配置</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> MergeAsync<TSource>(IEnumerable<TSource> sources, TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default) where TSource : IKeySlot<TKey>
    {
        var sourceList = sources.ToList();
        OnAsyncActionExecuting(sourceList, nameof(sources), cancellationToken);
        var destinations = await FindByIdsAsync(sourceList.Select(source => source.Id), cancellationToken);
        if (destinations == null) throw new MapTargetNullException(nameof(destinations));
        config ??= IgnoreNullConfig<TSource>();
        var entities = sourceList.Join(destinations, source => source.Id, destination => destination.Id,
            (source, destination) => source.Adapt(destination, config)).ToList();
        UpdateEntities(entities);
        return await AttacheChangeAsync(cancellationToken);
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
    public Task<StoreResult> MergeAsync<TSource, TJKey>(IEnumerable<TSource> sources, IEnumerable<TEntity> destinations,
        Func<TSource, TJKey> sourceKeySelector,
        Func<TEntity, TJKey> destinationKeySelector, TypeAdapterConfig? config = null,
        CancellationToken cancellationToken = default)
    {
        var sourceList = sources.ToList();
        OnAsyncActionExecuting(sourceList, nameof(sources), cancellationToken);
        config ??= IgnoreIdAndNullConfig<TSource>();
        var entities = sourceList.Join(destinations, sourceKeySelector, destinationKeySelector,
            (source, _) => source.Adapt<TSource, TEntity>(config));
        if (entities == null) throw new MapTargetNullException(nameof(entities));
        var list = entities.ToList();
        UpdateEntities(list);
        return AttacheChangeAsync(cancellationToken);
    }

    #endregion

    #region MapConfig

    /// <summary>
    ///     id和空值忽略缓存
    /// </summary>
    private TypeAdapterConfig? _ignoreIdAndNullConfig;

    /// <summary>
    ///     空值或空字符值忽略缓存
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public TypeAdapterConfig IgnoreIdAndNullConfig<TSource>()
    {
        if (_ignoreIdAndNullConfig != null) return _ignoreIdAndNullConfig;
        _ignoreIdAndNullConfig = IgnoreNullConfig<TSource>().Clone();
        _ignoreIdAndNullConfig.NewConfig<TSource, TEntity>()
            .IgnoreNullValues(true);

        return _ignoreIdAndNullConfig;
    }

    /// <summary>
    ///     空值或空字符值忽略缓存
    /// </summary>
    private TypeAdapterConfig? _ignoreNullConfig;

    /// <summary>
    ///     空值或空字符值忽略缓存
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public TypeAdapterConfig IgnoreNullConfig<TSource>()
    {
        if (_ignoreNullConfig != null) return _ignoreNullConfig;
        _ignoreNullConfig = IgnoreMetaConfig<TSource>().Clone();
        _ignoreNullConfig.NewConfig<TSource, TEntity>()
            .IgnoreNullValues(true);

        return _ignoreNullConfig;
    }

    /// <summary>
    ///     Id忽略缓存
    /// </summary>
    private TypeAdapterConfig? _ignoreIdConfig;

    /// <summary>
    ///     Id忽略访问器
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public TypeAdapterConfig IgnoreIdConfig<TSource>()
    {
        if (_ignoreIdConfig != null) return _ignoreIdConfig;
        _ignoreIdConfig = IgnoreMetaConfig<TSource>().Clone();
        _ignoreIdConfig.NewConfig<TSource, TEntity>()
            .Ignore(item => item.Id);
        return _ignoreIdConfig;
    }

    /// <summary>
    ///     元数据忽略缓存
    /// </summary>
    private TypeAdapterConfig? _ignoreMetaConfig;

    /// <summary>
    ///     元数据忽略访问器
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public TypeAdapterConfig IgnoreMetaConfig<TSource>()
    {
        if (_ignoreMetaConfig != null) return _ignoreMetaConfig;
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
    private void AddEntity(TEntity entity)
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
    private void AddEntities(ICollection<TEntity> entities)
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
    private void UpdateEntity(TEntity entity)
    {
        Context.Attach(entity);
        if (MetaDataHosting)
        {
            var now = DateTime.Now;
            entity.UpdatedAt = now;
        }

        Context.Update(entity);
    }

    /// <summary>
    ///     追踪多个实体更新
    /// </summary>
    /// <param name="entities">实体</param>
    private void UpdateEntities(ICollection<TEntity> entities)
    {
        Context.AttachRange(entities);
        if (MetaDataHosting)
        {
            var now = DateTime.Now;
            foreach (var entity in entities) entity.UpdatedAt = now;
        }

        Context.UpdateRange(entities);
    }

    /// <summary>
    ///     追踪一个实体删除
    /// </summary>
    /// <param name="entity">实体</param>
    private void DeleteEntity(TEntity entity)
    {
        if (SoftDelete)
        {
            Context.Attach(entity);
            if (MetaDataHosting)
            {
                var now = DateTime.Now;
                entity.UpdatedAt = now;
                entity.DeletedAt = now;
            }

            Context.Update(entity);
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
    private void DeleteEntities(ICollection<TEntity> entities)
    {
        if (SoftDelete)
        {
            Context.AttachRange(entities);
            if (MetaDataHosting)
            {
                var now = DateTime.Now;
                foreach (var entity in entities)
                {
                    entity.UpdatedAt = now;
                    entity.DeletedAt = now;
                }
            }

            Context.UpdateRange(entities);
        }
        else
        {
            Context.RemoveRange(entities);
        }
    }

    /// <summary>
    ///     根据Id查询实体
    /// </summary>
    /// <param name="id">id</param>
    /// <returns></returns>
    private TEntity? FindById(TKey id)
    {
        return EntityQuery.FirstOrDefault(entity => entity.Id.Equals(id));
    }

    /// <summary>
    ///     根据Id查询实体
    /// </summary>
    /// <param name="ids">id</param>
    /// <returns></returns>
    private IEnumerable<TEntity> FindByIds(IEnumerable<TKey> ids)
    {
        return EntityQuery.Where(entity => ids.Contains(entity.Id)).ToList();
    }

    /// <summary>
    ///     根据Id查找实体
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    private Task<TEntity?> FindByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return EntityQuery.FirstOrDefaultAsync(entity => entity.Id.Equals(id), cancellationToken);
    }

    /// <summary>
    ///     根据Id查找实体
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    private async Task<IEnumerable<TEntity>> FindByIdsAsync(IEnumerable<TKey> ids,
        CancellationToken cancellationToken = default)
    {
        return await EntityQuery.Where(entity => ids.Contains(entity.Id)).ToListAsync(cancellationToken);
    }

    #endregion

    #region OnActionExecution

    /// <summary>
    ///     保存追踪
    /// </summary>
    /// <returns></returns>
    private StoreResult AttacheChange()
    {
        try
        {
            var changes = SaveChanges();
            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(ErrorDescriber.ConcurrencyFailure());
        }
    }

    /// <summary>
    ///     保存异步追踪
    /// </summary>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    private async Task<StoreResult> AttacheChangeAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var changes = await SaveChangesAsync(cancellationToken);
            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(ErrorDescriber.ConcurrencyFailure());
        }
    }

    #endregion
}