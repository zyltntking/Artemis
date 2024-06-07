using System.Linq.Expressions;
using Artemis.Data.Core;
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
    protected sealed override Guid ConvertIdFromString(string id)
    {
        return id.GuidFromString();
    }

    /// <summary>
    ///     转换Id为字符串
    /// </summary>
    /// <param name="id">id</param>
    /// <returns>字符串</returns>
    protected sealed override string ConvertIdToString(Guid id)
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
public abstract class Store<TEntity, TKey> : KeyWithStore<TEntity, TKey>
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

    #region MapConfig

    #region Accessor

    /// <summary>
    ///     创建新对象的映射配置
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    protected sealed override TypeAdapterConfig CreateNewConfig<TSource>()
    {
        return IgnoreIdAndMetaConfig<TSource>();
    }

    /// <summary>
    ///     覆盖已有对象的映射配置
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="alternate">覆盖对象是否明确</param>
    /// <returns></returns>
    protected sealed override TypeAdapterConfig OverConfig<TSource>(bool alternate)
    {
        return alternate ? IgnoreIdAndMetaConfig<TSource>() : IgnoreMetaConfig<TSource>();
    }

    /// <summary>
    ///     合并对象的映射配置
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="alternate">待合并对象是否明确</param>
    /// <returns></returns>
    protected sealed override TypeAdapterConfig MergeConfig<TSource>(bool alternate)
    {
        return alternate ? IgnoreIdAndNullAndMetaConfig<TSource>() : IgnoreNullAndMetaConfig<TSource>();
    }

    #endregion

    /// <summary>
    ///     忽略标识和空值和元数据缓存
    /// </summary>
    private TypeAdapterConfig? _ignoreIdAndNullAndMetaConfig;

    /// <summary>
    ///     忽略源实体的空值属性和目标实体的Id和元数据属性
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    protected TypeAdapterConfig IgnoreIdAndNullAndMetaConfig<TSource>()
    {
        if (_ignoreIdAndNullAndMetaConfig is not null)
            return _ignoreIdAndNullAndMetaConfig;
        _ignoreIdAndNullAndMetaConfig = IgnoreIdAndNullConfig<TSource>().Clone();
        _ignoreIdAndNullAndMetaConfig.NewConfig<TSource, TEntity>()
            .IgnoreNullValues(true)
            .Ignore(item => item.CreatedAt)
            .Ignore(item => item.UpdatedAt)
            .Ignore(item => item.DeletedAt!);
        return _ignoreIdAndNullAndMetaConfig;
    }

    /// <summary>
    ///     忽略空值和元数据缓存
    /// </summary>
    private TypeAdapterConfig? _ignoreNullAndMetaConfig;

    /// <summary>
    ///     忽略源实体的空值属性和目标实体的元数据属性
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    protected TypeAdapterConfig IgnoreNullAndMetaConfig<TSource>()
    {
        if (_ignoreNullAndMetaConfig is not null)
            return _ignoreNullAndMetaConfig;
        _ignoreNullAndMetaConfig = IgnoreNullConfig<TSource>().Clone();
        _ignoreNullAndMetaConfig.NewConfig<TSource, TEntity>()
            .IgnoreNullValues(true)
            .Ignore(item => item.CreatedAt)
            .Ignore(item => item.UpdatedAt)
            .Ignore(item => item.DeletedAt!);
        return _ignoreNullAndMetaConfig;
    }

    /// <summary>
    ///     忽略Id和元数据缓存
    /// </summary>
    private TypeAdapterConfig? _ignoreIdAndMetaConfig;

    /// <summary>
    ///     忽略目标实体的Id和元数据属性
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    protected TypeAdapterConfig IgnoreIdAndMetaConfig<TSource>()
    {
        if (_ignoreIdAndMetaConfig is not null)
            return _ignoreIdAndMetaConfig;
        _ignoreIdAndMetaConfig = IgnoreIdConfig<TSource>().Clone();
        _ignoreIdAndMetaConfig.NewConfig<TSource, TEntity>()
            .Ignore(item => item.CreatedAt)
            .Ignore(item => item.UpdatedAt)
            .Ignore(item => item.DeletedAt!);
        return _ignoreIdAndMetaConfig;
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

    #region ActionExecution

    /// <summary>
    ///     添加单个实体
    /// </summary>
    /// <param name="entity">实体</param>
    protected sealed override void AddEntity(TEntity entity)
    {
        if (MetaDataHosting)
        {
            var now = DateTime.Now;
            entity.CreatedAt = now;
            entity.UpdatedAt = now;

            entity.ModifyBy = entity.CreateBy;
        }

        Context.Add(entity);
    }

    /// <summary>
    ///     添加多个实体
    /// </summary>
    /// <param name="entities">实体</param>
    protected sealed override void AddEntities(ICollection<TEntity> entities)
    {
        if (MetaDataHosting)
        {
            var now = DateTime.Now;
            foreach (var entity in entities)
            {
                entity.CreatedAt = now;
                entity.UpdatedAt = now;

                entity.ModifyBy = entity.CreateBy;
            }
        }

        Context.AddRange(entities);
    }

    /// <summary>
    ///     追踪一个实体更新
    /// </summary>
    /// <param name="entity">实体</param>
    protected sealed override void UpdateEntity(TEntity entity)
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
        {
            Context.Entry(entity).Property(item => item.CreatedAt).IsModified = false;
            Context.Entry(entity).Property(item => item.DeletedAt).IsModified = false;

            Context.Entry(entity).Property(item => item.CreateBy).IsModified = false;
            Context.Entry(entity).Property(item => item.RemoveBy).IsModified = false;
        }
    }

    /// <summary>
    ///     追踪多个实体更新
    /// </summary>
    /// <param name="entities">实体</param>
    protected sealed override void UpdateEntities(ICollection<TEntity> entities)
    {
        Context.AttachRange(entities);

        if (MetaDataHosting)
        {
            var now = DateTime.Now;
            foreach (var entity in entities) entity.UpdatedAt = now;
        }

        Context.UpdateRange(entities);

        if (MetaDataHosting)
        {
            foreach (var entity in entities)
            {
                Context.Entry(entity).Property(item => item.CreatedAt).IsModified = false;
                Context.Entry(entity).Property(item => item.DeletedAt).IsModified = false;

                Context.Entry(entity).Property(item => item.CreateBy).IsModified = false;
                Context.Entry(entity).Property(item => item.RemoveBy).IsModified = false;
            }
        }
    }

    /// <summary>
    ///     批量删除实体
    /// </summary>
    /// <param name="query">查询</param>
    /// <param name="setter">更新委托</param>
    /// <returns></returns>
    protected sealed override int BatchUpdateEntity(
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
    protected sealed override Task<int> BatchUpdateEntityAsync(
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
    protected sealed override void DeleteEntity(TEntity entity)
    {
        if (SoftDelete)
        {
            Context.Attach(entity);
            var now = DateTime.Now;
            entity.DeletedAt = now;
            Context.Update(entity);

            Context.Entry(entity).Property(item => item.CreatedAt).IsModified = false;
            Context.Entry(entity).Property(item => item.UpdatedAt).IsModified = false;

            Context.Entry(entity).Property(item => item.CreateBy).IsModified = false;
            Context.Entry(entity).Property(item => item.ModifyBy).IsModified = false;
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
    protected sealed override void DeleteEntities(ICollection<TEntity> entities)
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
            {
                Context.Entry(entity).Property(item => item.CreatedAt).IsModified = false;
                Context.Entry(entity).Property(item => item.UpdatedAt).IsModified = false;

                Context.Entry(entity).Property(item => item.CreateBy).IsModified = false;
                Context.Entry(entity).Property(item => item.ModifyBy).IsModified = false;
            }
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
    protected sealed override int BatchDeleteEntity(IQueryable<TEntity> query)
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
    protected sealed override Task<int> BatchDeleteEntityAsync(
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