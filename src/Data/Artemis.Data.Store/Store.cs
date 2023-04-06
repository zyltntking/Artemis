using Artemis.Data.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Data.Store;

/// <summary>
/// 抽象存储实现
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public class Store<TEntity> : Store<TEntity, DbContext> 
    where TEntity : ModelBase
{
    /// <summary>
    /// 创建一个新的基本存储实例
    /// </summary>
    /// <param name="context">数据访问上下文</param>
    /// <param name="describer">操作异常描述者</param>
    /// <exception cref="ArgumentNullException"></exception>
    protected Store(DbContext context, IStoreErrorDescriber? describer = null) : base(context, describer)
    {
    }
}

/// <summary>
/// 抽象存储实现
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TContext">数据上下文类型</typeparam>
public abstract class Store<TEntity, TContext> : Store<TEntity, TContext, Guid>, IStore<TEntity>
    where TEntity : ModelBase, IModelBase, IModelBase<Guid> 
    where TContext : DbContext
{
    /// <summary>
    /// 创建一个新的基本存储实例
    /// </summary>
    /// <param name="context">数据访问上下文</param>
    /// <param name="describer">操作异常描述者</param>
    /// <exception cref="ArgumentNullException"></exception>
    protected Store(TContext context, IStoreErrorDescriber? describer = null) : base(context, describer)
    {
    }

    #region Overrides of StoreBase<TEntity,Guid>

    /// <summary>
    /// 转换字符串到id
    /// </summary>
    /// <param name="id">id字符串</param>
    /// <returns>id</returns>
    protected override Guid ConvertIdFromString(string? id)
    {
        return id.GuidFromString();
    }

    /// <summary>
    /// 转换Id为字符串
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
/// 抽象存储实现
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TContext">数据上下文类型</typeparam>
/// <typeparam name="TKey">键类型</typeparam>
public abstract class Store<TEntity, TContext, TKey>: StoreBase<TEntity, TKey>, IStore<TEntity, TKey> 
    where TEntity : ModelBase<TKey>, IModelBase<TKey>
    where TContext : DbContext 
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// 创建一个新的基本存储实例
    /// </summary>
    /// <param name="context">数据访问上下文</param>
    /// <param name="describer">操作异常描述者</param>
    /// <exception cref="ArgumentNullException"></exception>
    protected Store(TContext context, IStoreErrorDescriber? describer = null) : base(describer ?? new StoreErrorDescriber())
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    #region Setting

    /// <summary>
    /// 设置是否自动保存更改
    /// </summary>
    public bool AutoSaveChanges { get; set; } = true;

    /// <summary>
    /// MetaDataHosting标记
    /// </summary>
    private bool _metaDataHosting = true;

    /// <summary>
    /// 设置是否启用元数据托管
    /// </summary>
    public bool MetaDataHosting
    {
        get => _metaDataHosting;
        set => _metaDataHosting = SoftDelete || value;
    }

    /// <summary>
    /// SoftDelete标记
    /// </summary>
    private bool _softDelete;

    /// <summary>
    /// 设置是否启用软删除
    /// </summary>
    public bool SoftDelete
    {
        get => _softDelete;
        set
        {
            _softDelete = value;
            if (_softDelete)
            {
                MetaDataHosting = _softDelete;
            }
        }
    }

    #endregion

    #region DataAccess

    /// <summary>
    /// 数据访问上下文
    /// </summary>
    public virtual TContext Context { get; private set; }

    /// <summary>
    /// EntitySet访问器*Main Store Set*
    /// </summary>
    protected virtual DbSet<TEntity> EntitySet => Context.Set<TEntity>();

    /// <summary>
    /// EntityQuery访问器
    /// </summary>
    protected virtual IQueryable<TEntity> EntityQuery => EntitySet.AsQueryable().Where(item => !SoftDelete || item.DeletedAt != null);

    /// <summary>
    /// Entity无追踪访问器
    /// </summary>
    protected virtual IQueryable<TEntity> NoTrackingQuery => EntityQuery.Where(item => !SoftDelete || item.DeletedAt != null).AsNoTracking();

    #endregion

    #region SaveChanges

    /// <summary>
    /// 保存当前存储
    /// </summary>
    /// <returns></returns>
    private int SaveChanges()
    {
        return AutoSaveChanges ? Context.SaveChanges() : 0;
    }

    /// <summary>
    /// 保存当前存储
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
    /// 在<paramref name="entity"/>存储中创建一个新的实体
    /// </summary>
    /// <param name="entity">被创建实体</param>
    /// <returns></returns>
    public virtual StoreResult Create(TEntity entity)
    {
        OnActionExecuting(entity, nameof(entity));
        if (MetaDataHosting)
        {
            var now = DateTime.Now;
            entity.CreatedAt = now;
            entity.UpdatedAt = now;
        }
        Context.Add(entity);
        var changes = SaveChanges();
        return StoreResult.Success(changes);
    }

    /// <summary>
    /// 在<paramref name="entities"/>存储中创建多个新的实体
    /// </summary>
    /// <param name="entities">被创建实体</param>
    /// <returns></returns>
    public virtual StoreResult Create(IEnumerable<TEntity> entities)
    {
        var list = entities.ToList();
        OnActionExecuting(list, nameof(entities));
        if (MetaDataHosting)
        {
            var now = DateTime.Now;
            foreach (var entity in list)
            {
                entity.CreatedAt = now;
                entity.UpdatedAt = now;
            }
        }
        Context.AddRange(list);
        var changes = SaveChanges();
        return StoreResult.Success(changes);
    }

    /// <summary>
    /// 在<paramref name="entity"/>存储中创建一个新的实体
    /// </summary>
    /// <param name="entity">被创建实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public virtual async Task<StoreResult> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(entity, nameof(entity), cancellationToken);
        if (MetaDataHosting)
        {
            var now = DateTime.Now;
            entity.CreatedAt = now;
            entity.UpdatedAt = now;
        }
        Context.Add(entity);
        var changes = await SaveChangesAsync(cancellationToken);
        return StoreResult.Success(changes);
    }

    /// <summary>
    /// 在<paramref name="entities"/>存储中创建多个新的实体
    /// </summary>
    /// <param name="entities">被创建实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public virtual async Task<StoreResult> CreateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        var list = entities.ToList();
        OnAsyncActionExecuting(list, nameof(entities), cancellationToken);
        if (MetaDataHosting)
        {
            var now = DateTime.Now;
            foreach (var entity in list)
            {
                entity.CreatedAt = now;
                entity.UpdatedAt = now;
            }
        }
        Context.AddRange(list);
        var changes = await SaveChangesAsync(cancellationToken);
        return StoreResult.Success(changes);
    }

    #endregion

    #region UpdateEntity & UpdateEntities

    /// <summary>
    /// 在<paramref name="entity"/>存储中更新已存在的实体
    /// </summary>
    /// <param name="entity">被创建实体</param>
    /// <returns></returns>
    public virtual StoreResult Update(TEntity entity)
    {
        OnActionExecuting(entity, nameof(entity));
        Context.Attach(entity);
        if (MetaDataHosting)
        {
            var now = DateTime.Now;
            entity.UpdatedAt = now;
        }
        Context.Update(entity);
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
    /// 在<paramref name="entities"/>存储中更新多个已存在的实体
    /// </summary>
    /// <param name="entities">被创建实体</param>
    /// <returns></returns>
    public virtual StoreResult Update(IEnumerable<TEntity> entities)
    {

        var list = entities.ToList();
        OnActionExecuting(list, nameof(entities));
        Context.AttachRange(list);
        if (MetaDataHosting)
        {
            var now = DateTime.Now;
            foreach (var entity in list)
            {
                entity.UpdatedAt = now;
            }
        }
        Context.UpdateRange(list);
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
    /// 在<paramref name="entity"/>存储中更新已存在的实体
    /// </summary>
    /// <param name="entity">被创建实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public virtual async Task<StoreResult> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(entity, nameof(entity), cancellationToken);
        Context.Attach(entity);
        if (MetaDataHosting)
        {
            var now = DateTime.Now;
            entity.UpdatedAt = now;
        }
        Context.Update(entity);
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

    /// <summary>
    /// 在<paramref name="entities"/>存储中更新多个已存在的实体
    /// </summary>
    /// <param name="entities">被创建实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public virtual async Task<StoreResult> UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        var list = entities.ToList();
        OnAsyncActionExecuting(list, nameof(entities), cancellationToken);
        Context.AttachRange(list);
        if (MetaDataHosting)
        {
            var now = DateTime.Now;
            foreach (var entity in list)
            {
                entity.UpdatedAt = now;
            }
        }
        Context.UpdateRange(list);
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

    #region DeleteEntity & DeleteEntities

    /// <summary>
    /// 在存储中删除已存在的实体
    /// </summary>
    /// <param name="id">被删除实体的主键</param>
    public virtual StoreResult Delete(TKey id)
    {
        ThrowIfDisposed();
        if (id == null)
        {
            throw new ArgumentNullException(nameof(id));
        }
        var entity = FindEntity(id);
        if (entity == null)
        {
            return StoreResult.Failed(ErrorDescriber.NotFoundId(ConvertIdToString(id)));
        }
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
    /// 在<paramref name="entity"/>存储中删除已存在的实体
    /// </summary>
    /// <param name="entity">被删除实体</param>
    /// <returns></returns>
    public virtual StoreResult Delete(TEntity entity)
    {
        OnActionExecuting(entity, nameof(entity));
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
    /// 在存储中删除已存在的实体
    /// </summary>
    /// <param name="ids">被删除实体的主键</param>
    /// <returns></returns>
    public virtual StoreResult Delete(IEnumerable<TKey> ids)
    {
        ThrowIfDisposed();
        if (ids == null)
        {
            throw new ArgumentNullException(nameof(ids));
        }
        var idList = ids as List<TKey> ?? ids.ToList();
        var entityList = FindEntities(idList).ToList();
        if (!entityList.Any())
        {
            var idsString = string.Join(",", idList.Select(ConvertIdToString));
            return StoreResult.Failed(ErrorDescriber.NotFoundId(idsString));
        }
        if (SoftDelete)
        {
            Context.AttachRange(entityList);
            if (MetaDataHosting)
            {
                var now = DateTime.Now;
                foreach (var entity in entityList)
                {
                    entity.UpdatedAt = now;
                    entity.DeletedAt = now;
                }
            }
            Context.UpdateRange(entityList);
        }
        else
        {
            Context.RemoveRange(entityList);
        }
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
    /// 在<paramref name="entities"/>存储中删除已存在的实体
    /// </summary>
    /// <param name="entities">被删除实体</param>
    /// <returns></returns>
    public virtual StoreResult Delete(IEnumerable<TEntity> entities)
    {
        var list = entities.ToList();
        OnActionExecuting(list, nameof(entities));
        if (SoftDelete)
        {
            Context.AttachRange(list);
            if (MetaDataHosting)
            {
                var now = DateTime.Now;
                foreach (var entity in list)
                {
                    entity.UpdatedAt = now;
                    entity.DeletedAt = now;
                }
            }
            Context.UpdateRange(list);
        }
        else
        {
            Context.RemoveRange(list);
        }
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
    /// 在存储中删除已存在的实体
    /// </summary>
    /// <param name="id">被删除实体的主键</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public virtual async Task<StoreResult> DeleteAsync(TKey id, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        if (id == null)
        {
            throw new ArgumentNullException(nameof(id));
        }
        var entity = await FindEntityAsync(id, cancellationToken);
        if (entity == null)
        {
            return StoreResult.Failed(ErrorDescriber.NotFoundId(ConvertIdToString(id)));
        }
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

    /// <summary>
    /// 在<paramref name="entity"/>存储中删除已存在的实体
    /// </summary>
    /// <param name="entity">被删除实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public virtual async Task<StoreResult> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(entity, nameof(entity), cancellationToken);
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

    /// <summary>
    /// 在存储中删除已存在的实体
    /// </summary>
    /// <param name="ids">被删除实体的主键</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public virtual async Task<StoreResult> DeleteAsync(IEnumerable<TKey> ids, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (ids == null)
        {
            throw new ArgumentNullException(nameof(ids));
        }
        var idList = ids as List<TKey> ?? ids.ToList();
        var entityList = (await FindEntitiesAsync(idList, cancellationToken)).ToList();
        if (!entityList.Any())
        {
            var idsString = string.Join(",", idList.Select(ConvertIdToString));
            return StoreResult.Failed(ErrorDescriber.NotFoundId(idsString));
        }
        if (SoftDelete)
        {
            Context.AttachRange(entityList);
            if (MetaDataHosting)
            {
                var now = DateTime.Now;
                foreach (var entity in entityList)
                {
                    entity.UpdatedAt = now;
                    entity.DeletedAt = now;
                }
            }
            Context.UpdateRange(entityList);
        }
        else
        {
            Context.RemoveRange(entityList);
        }
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

    /// <summary>
    /// 在<paramref name="entities"/>存储中删除已存在的实体
    /// </summary>
    /// <param name="entities">被删除实体</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public virtual async Task<StoreResult> DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        var list = entities.ToList();
        OnAsyncActionExecuting(list, nameof(entities), cancellationToken);
        if (SoftDelete)
        {
            Context.AttachRange(list);
            if (MetaDataHosting)
            {
                var now = DateTime.Now;
                foreach (var entity in list)
                {
                    entity.UpdatedAt = now;
                    entity.DeletedAt = now;
                }
            }
            Context.UpdateRange(list);
        }
        else
        {
            Context.RemoveRange(list);
        }
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

    #region FindEntity & FindEntities

    /// <summary>
    /// 根据Id查找实体
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual TEntity? FindEntity(TKey id)
    {
        return NoTrackingQuery.FirstOrDefault(entity => entity.Id.Equals(id));
    }

    /// <summary>
    /// 根据Id查找实体
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public IEnumerable<TEntity> FindEntities(IEnumerable<TKey> ids)
    {
        return NoTrackingQuery.Where(entity => ids.Contains(entity.Id)).ToList();
    }

    /// <summary>
    /// 根据Id查找实体
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public virtual Task<TEntity?> FindEntityAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return NoTrackingQuery.FirstOrDefaultAsync(entity => entity.Id.Equals(id), cancellationToken);
    }

    /// <summary>
    /// 根据Id查找实体
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<IEnumerable<TEntity>> FindEntitiesAsync(IEnumerable<TKey> ids, CancellationToken cancellationToken = default)
    {
        return await NoTrackingQuery.Where(entity => ids.Contains(entity.Id)).ToListAsync(cancellationToken: cancellationToken);
    }

    #endregion

    #endregion

    #region Implementation of IStoreMap<out TEntity,TKey>

    #region CreateEntity & CreateEntities

    /// <summary>
    /// 通过类型映射创建一个新实例
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <returns>映射后实体</returns>
    public StoreResult CreateNew<TSource>(TSource source)
    {
        ThrowIfDisposed();
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }
        var entity = source.Adapt<TEntity>();
        if (entity == null)
        {
            throw new MapTargetNullException(nameof(entity));
        }
        if (MetaDataHosting)
        {
            var now = DateTime.Now;
            entity.CreatedAt = now;
            entity.UpdatedAt = now;
        }
        Context.Add(entity);
        var changes = SaveChanges();
        return StoreResult.Success(changes);
    }

    /// <summary>
    /// 通过类型映射创建一个新实例
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <returns>创建结果</returns>
    public StoreResult CreateNew<TSource>(IEnumerable<TSource> sources)
    {
        ThrowIfDisposed();
        if (sources == null)
        {
            throw new ArgumentNullException(nameof(sources));
        }
        var entities = sources.Adapt<IEnumerable<TEntity>>();
        if (entities == null)
        {
            throw new MapTargetNullException(nameof(entities));
        }
        var list = entities.ToList();
        if (MetaDataHosting)
        {
            var now = DateTime.Now;
            foreach (var entity in list)
            {
                entity.CreatedAt = now;
                entity.UpdatedAt = now;
            }
        }
        Context.AddRange(list);
        var changes = SaveChanges();
        return StoreResult.Success(changes);
    }

    /// <summary>
    /// 通过类型映射创建一个新实例
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="source">源数据</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns>创建结果</returns>
    public async Task<StoreResult> CreateNewAsync<TSource>(TSource source, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }
        var entity = source.Adapt<TEntity>();
        if (entity == null)
        {
            throw new MapTargetNullException(nameof(entity));
        }
        if (MetaDataHosting)
        {
            var now = DateTime.Now;
            entity.CreatedAt = now;
            entity.UpdatedAt = now;
        }
        Context.Add(entity);
        var changes = await SaveChangesAsync(cancellationToken);
        return StoreResult.Success(changes);
    }

    /// <summary>
    /// 通过类型映射创建一个新实例
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <param name="sources">源数据</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns>创建结果</returns>
    public async Task<StoreResult> CreateNewAsync<TSource>(IEnumerable<TSource> sources, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (sources == null)
        {
            throw new ArgumentNullException(nameof(sources));
        }
        var entities = sources.Adapt<IEnumerable<TEntity>>();
        if (entities == null)
        {
            throw new MapTargetNullException(nameof(entities));
        }
        var list = entities.ToList();
        if (MetaDataHosting)
        {
            var now = DateTime.Now;
            foreach (var entity in list)
            {
                entity.CreatedAt = now;
                entity.UpdatedAt = now;
            }
        }
        Context.AddRange(list);
        var changes = await SaveChangesAsync(cancellationToken);
        return StoreResult.Success(changes);
    }

    #endregion

    #endregion
}