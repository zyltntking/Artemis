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
///     无元数据模型基本存储
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
public abstract class StoreBase<TEntity, TKey> :
    KeyLessStoreBase<TEntity>,
    IStoreBase<TEntity, TKey>
    where TEntity : class, IKeySlot<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     创建一个新的基本存储实例
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cache"></param>
    /// <param name="storeOptions"></param>
    /// <param name="logger"></param>
    /// <param name="describer"></param>
    /// <exception cref="StoreParameterNullException"></exception>
    protected StoreBase(
        DbContext context,
        IDistributedCache? cache = null,
        IKeyWithStoreOptions? storeOptions = null,
        ILogger? logger = null,
        StoreErrorDescriber? describer = null) : base(context, storeOptions, logger, describer)
    {
        Cache = cache;
        StoreOptions = storeOptions ?? new KeyWithStoreOptions();
    }

    #region Access

    /// <summary>
    ///     缓存依赖
    /// </summary>
    protected IDistributedCache? Cache { get; }

    /// <summary>
    ///     键适配查询
    /// </summary>
    public IQueryable<TEntity> KeyMatchQuery(TKey key)
    {
        return EntityQuery.Where(entity => entity.Id.Equals(key));
    }

    /// <summary>
    ///     键适配查询
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    public IQueryable<TEntity> KeyMatchQuery(IEnumerable<TKey> keys)
    {
        var keyList = keys.ToList();

        var query = EntityQuery;

        foreach (var key in keyList)
        {
            var segment = EntityQuery.Where(entity => entity.Id.Equals(key));

            query = query.Union(segment);
        }

        return query;
    }

    #endregion

    #region Options

    /// <summary>
    ///     无键存储配置
    /// </summary>
    private IKeyWithStoreOptions StoreOptions { get; }

    #region SettingAccessor

    /// <summary>
    ///     是否启用具缓存策略
    /// </summary>
    protected bool CachedStore => StoreOptions is { CachedStore: true, Expires: >= 0 } && Cache is not null;

    /// <summary>
    ///     过期时间(秒)
    /// </summary>
    private int Expires => StoreOptions.Expires;

    #endregion

    #endregion

    #region MapConfigs

    #region Accessor

    /// <summary>
    ///     创建新对象的映射配置
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    protected override TypeAdapterConfig CreateNewConfig<TSource>()
    {
        return IgnoreIdConfig<TSource>();
    }

    /// <summary>
    ///     覆盖已有对象的映射配置
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="alternate">覆盖对象是否明确</param>
    /// <returns></returns>
    protected override TypeAdapterConfig OverConfig<TSource>(bool alternate)
    {
        return alternate ? IgnoreIdConfig<TSource>() : EmptyConfig<TSource>();
    }

    /// <summary>
    ///     合并对象的映射配置
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="alternate">待合并对象是否明确</param>
    /// <returns></returns>
    protected override TypeAdapterConfig MergeConfig<TSource>(bool alternate)
    {
        return alternate ? IgnoreIdAndNullConfig<TSource>() : IgnoreNullConfig<TSource>();
    }

    #endregion

    /// <summary>
    ///     标识忽略缓存
    /// </summary>
    private TypeAdapterConfig? _ignoreIdConfig;

    /// <summary>
    ///     忽略目标实体的Id属性
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    protected TypeAdapterConfig IgnoreIdConfig<TSource>()
    {
        if (_ignoreIdConfig is not null)
            return _ignoreIdConfig;
        _ignoreIdConfig = new TypeAdapterConfig();
        _ignoreIdConfig.NewConfig<TSource, TEntity>()
            .Ignore(dest => dest.Id);

        return _ignoreIdConfig;
    }

    /// <summary>
    ///     忽略空值和标识缓存
    /// </summary>
    private TypeAdapterConfig? _ignoreIdAndNullConfig;

    /// <summary>
    ///     忽略目标实体的Id属性和源实体的空值属性
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    protected TypeAdapterConfig IgnoreIdAndNullConfig<TSource>()
    {
        if (_ignoreIdAndNullConfig is not null)
            return _ignoreIdAndNullConfig;
        _ignoreIdAndNullConfig = IgnoreNullConfig<TSource>().Clone();
        _ignoreIdAndNullConfig.NewConfig<TSource, TEntity>()
            .Ignore(dest => dest.Id);
        return _ignoreIdAndNullConfig;
    }

    /// <summary>
    ///     忽略空字符串和标识缓存
    /// </summary>
    private TypeAdapterConfig? _ignoreIdAndEmptyConfig;

    /// <summary>
    ///     忽略目标实体的Id属性和源实体的空字符串属性
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    protected TypeAdapterConfig IgnoreIdAndEmptyConfig<TSource>()
    {
        if (_ignoreIdAndEmptyConfig is not null)
            return _ignoreIdAndEmptyConfig;
        _ignoreIdAndEmptyConfig = IgnoreEmptyStringConfig<TSource>().Clone();
        _ignoreIdAndEmptyConfig.NewConfig<TSource, TEntity>()
            .Ignore(dest => dest.Id);
        return _ignoreIdAndEmptyConfig;
    }

    /// <summary>
    ///     忽略空值和空字符串和标识缓存
    /// </summary>
    private TypeAdapterConfig? _ignoreIdAndNullAndEmptyConfig;

    /// <summary>
    ///     忽略目标实体的Id属性和源实体的空值和空字符串属性
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    protected TypeAdapterConfig IgnoreIdAndNullAndEmptyConfig<TSource>()
    {
        if (_ignoreIdAndNullAndEmptyConfig is not null)
            return _ignoreIdAndNullAndEmptyConfig;
        _ignoreIdAndNullAndEmptyConfig = IgnoreNullAndEmptyConfig<TSource>().Clone();
        _ignoreIdAndNullAndEmptyConfig.NewConfig<TSource, TEntity>()
            .Ignore(dest => dest.Id);
        return _ignoreIdAndNullAndEmptyConfig;
    }

    #endregion

    #region Cache

    /// <summary>
    ///     缓存前缀
    /// </summary>
    protected virtual string Prefix => "Store";

    /// <summary>
    ///     生成键
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    protected virtual string GenerateKey(TEntity entity)
    {
        //    var list = new List<string>();

        //    if (prefix is not null)
        //        list.Add(prefix); //1

        //    list.Add(GetType().Name); //2

        //    if (space is not null)
        //        list.Add(space); //3

        //    list.Add(key ?? Id.ToString()!);

        //    return string.Join(":", list);

        //return entity.GenerateKey(Prefix);

        return Prefix;
    }

    /// <summary>
    ///     缓存实体
    /// </summary>
    /// <param name="entity"></param>
    protected void CacheEntity(TEntity entity)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            Cache?.Set(GenerateKey(entity), entity, Expires);

            if (DebugLogger)
                Logger?.LogDebug($"{typeof(TEntity).Name} Cached");
        }
    }

    /// <summary>
    ///     缓存实体
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected Task CacheEntityAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            Cache?.SetAsync(GenerateKey(entity), entity, Expires, cancellationToken);

            if (DebugLogger) Logger?.LogDebug($"{typeof(TEntity).Name} Cached");
        }

        return Task.CompletedTask;
    }

    /// <summary>
    ///     缓存实体
    /// </summary>
    /// <param name="entities"></param>
    protected void CacheEntities(IEnumerable<TEntity> entities)
    {
        if (CachedStore)
        {
            if (Cache is null) throw new InstanceNotImplementException(nameof(Cache));

            var count = 0;

            foreach (var entity in entities)
            {
                Cache?.SetAsync(GenerateKey(entity), entity, Expires);
                count++;
            }

            if (DebugLogger) Logger?.LogDebug($"Cached {count} {typeof(TEntity).Name} Entities");
        }
    }

    /// <summary>
    ///     缓存实体
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="cancellationToken"></param>
    protected Task CacheEntitiesAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        if (CachedStore)
        {
            if (Cache is null) throw new InstanceNotImplementException(nameof(Cache));

            var count = 0;

            foreach (var entity in entities)
            {
                Cache?.SetAsync(GenerateKey(entity), entity, Expires, cancellationToken);
                count++;
            }

            if (DebugLogger) Logger?.LogDebug($"Cached {count} {typeof(TEntity).Name} Entities");
        }

        return Task.CompletedTask;
    }

    /// <summary>
    ///     获取实体
    /// </summary>
    /// <param name="key">缓存键</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    protected TEntity? GetEntity(TKey key)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            var entity = Instance.CreateInstance<TEntity>();

            entity.Id = key;

            entity = Cache?.Get<TEntity>(GenerateKey(entity));

            if (DebugLogger) Logger?.LogDebug($"Get {typeof(TEntity).Name} Entity From Cache");

            return entity;
        }

        return default;
    }

    /// <summary>
    ///     获取实体
    /// </summary>
    /// <param name="key">缓存键</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    protected async Task<TEntity?> GetEntityAsync(
        TKey key,
        CancellationToken cancellationToken = default)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            var entity = Instance.CreateInstance<TEntity>();

            entity.Id = key;

            entity = await Cache?.GetAsync<TEntity>(GenerateKey(entity), cancellationToken)!;

            if (DebugLogger) Logger?.LogDebug($"Get {typeof(TEntity).Name} Entity From Cache");

            return entity;
        }

        return default;
    }

    /// <summary>
    ///     获取实体
    /// </summary>
    /// <param name="keys">实体键列表</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    protected IEnumerable<TEntity>? GetEntities(IEnumerable<TKey> keys)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            var list = new List<TEntity>();

            foreach (var key in keys)
            {
                var entity = Instance.CreateInstance<TEntity>();

                entity.Id = key;

                entity = Cache?.Get<TEntity>(GenerateKey(entity));

                if (entity is not null)
                    list.Add(entity);
            }

            if (DebugLogger) Logger?.LogDebug($"Get {list.Count} {typeof(TEntity).Name} Entities From Cache");

            return list;
        }

        return default;
    }

    /// <summary>
    ///     获取实体
    /// </summary>
    /// <param name="keys">实体键列表</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected async Task<IEnumerable<TEntity>?> GetEntitiesAsync(
        IEnumerable<TKey> keys,
        CancellationToken cancellationToken = default)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            var list = new List<TEntity>();

            foreach (var key in keys)
            {
                var entity = Instance.CreateInstance<TEntity>();

                entity.Id = key;

                entity = await Cache?.GetAsync<TEntity>(GenerateKey(entity), cancellationToken)!;

                if (entity is not null)
                    list.Add(entity);
            }

            if (DebugLogger) Logger?.LogDebug($"Get {list.Count} {typeof(TEntity).Name} Entities From Cache");

            return list;
        }

        return default;
    }

    /// <summary>
    ///     移除被缓存的实体
    /// </summary>
    /// <param name="key"></param>
    /// <exception cref="InstanceNotImplementException"></exception>
    protected void RemoveCachedEntity(TKey key)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            var entity = Instance.CreateInstance<TEntity>();

            entity.Id = key;

            Cache?.Remove(GenerateKey(entity));

            if (DebugLogger) Logger?.LogDebug($"{typeof(TEntity).Name} Removed From Cache");
        }
    }

    /// <summary>
    ///     移除被缓存的实体
    /// </summary>
    /// <param name="entity"></param>
    protected void RemoveCachedEntity(TEntity entity)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            Cache?.Remove(GenerateKey(entity));

            if (DebugLogger) Logger?.LogDebug($"{typeof(TEntity).Name} Removed From Cache");
        }
    }

    /// <summary>
    ///     移除被缓存的实体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    protected async Task RemoveCachedEntityAsync(TKey key, CancellationToken cancellationToken = default)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            var entity = Instance.CreateInstance<TEntity>();

            entity.Id = key;

            await Cache?.RemoveAsync(GenerateKey(entity), cancellationToken)!;

            if (DebugLogger) Logger?.LogDebug($"{typeof(TEntity).Name} Removed From Cache");
        }
    }

    /// <summary>
    ///     移除被缓存的实体
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    protected async Task RemoveCachedEntityAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            await Cache?.RemoveAsync(GenerateKey(entity), cancellationToken)!;

            if (DebugLogger) Logger?.LogDebug($"{typeof(TEntity).Name} Removed From Cache");
        }
    }

    /// <summary>
    ///     移除被缓存的实体
    /// </summary>
    /// <param name="keys"></param>
    protected void RemoveCachedEntities(IEnumerable<TKey> keys)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            var count = 0;
            foreach (var key in keys)
            {
                var entity = Instance.CreateInstance<TEntity>();

                entity.Id = key;

                Cache?.Remove(GenerateKey(entity));
                count++;
            }

            if (DebugLogger) Logger?.LogDebug($"{count} {typeof(TEntity).Name} Entities Removed From Cache");
        }
    }

    /// <summary>
    ///     移除被缓存的实体
    /// </summary>
    /// <param name="entities"></param>
    protected void RemoveCachedEntities(IEnumerable<TEntity> entities)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            var count = 0;
            foreach (var entity in entities)
            {
                Cache?.Remove(GenerateKey(entity));
                count++;
            }

            if (DebugLogger) Logger?.LogDebug($"{count} {typeof(TEntity).Name} Entities Removed From Cache");
        }
    }

    /// <summary>
    ///     移除被缓存的实体
    /// </summary>
    /// <param name="keys"></param>
    /// <param name="cancellationToken"></param>
    protected async Task RemoveCachedEntitiesAsync(
        IEnumerable<TKey> keys,
        CancellationToken cancellationToken = default)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            var count = 0;
            foreach (var key in keys)
            {
                var entity = Instance.CreateInstance<TEntity>();

                entity.Id = key;

                await Cache?.RemoveAsync(GenerateKey(entity), cancellationToken)!;
                count++;
            }

            if (DebugLogger) Logger?.LogDebug($"{count} {typeof(TEntity).Name} Entities Removed From Cache");
        }
    }

    /// <summary>
    ///     移除被缓存的实体
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="cancellationToken"></param>
    protected async Task RemoveCachedEntitiesAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        if (CachedStore)
        {
            if (Cache is null)
                throw new InstanceNotImplementException(nameof(Cache));

            var count = 0;
            foreach (var entity in entities)
            {
                await Cache?.RemoveAsync(GenerateKey(entity), cancellationToken)!;
                count++;
            }

            if (DebugLogger) Logger?.LogDebug($"{count} {typeof(TEntity).Name} Entities Removed From Cache");
        }
    }

    #endregion

    #region GetId

    /// <summary>
    ///     转换字符串到id
    /// </summary>
    /// <param name="id">id字符串</param>
    /// <returns>id</returns>
    protected virtual TKey? ConvertIdFromString(string id)
    {
        return id.IdFromString<TKey>();
    }

    /// <summary>
    ///     转换Id为字符串
    /// </summary>
    /// <param name="id">id</param>
    /// <returns>字符串</returns>
    protected virtual string? ConvertIdToString(TKey id)
    {
        return id.IdToString();
    }

    /// <summary>
    ///     获取指定实体Id
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns>Id</returns>
    public TKey GetId(TEntity entity)
    {
        OnActionExecuting(entity, nameof(entity));
        return entity.Id;
    }

    /// <summary>
    ///     获取指定实体Id
    /// </summary>
    /// <param name="entity">实体</param>
    /// <param name="cancellationToken">取消异步操作的信号</param>
    /// <returns>Id</returns>
    public Task<TKey> GetIdAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(entity, nameof(entity), cancellationToken);
        return Task.FromResult(entity.Id);
    }

    /// <summary>
    ///     获取指定实体Id字符串
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns>Id字符串</returns>
    public string GetIdString(TEntity entity)
    {
        OnActionExecuting(entity, nameof(entity));
        return ConvertIdToString(entity.Id)!;
    }

    /// <summary>
    ///     获取指定实体Id字符串
    /// </summary>
    /// <param name="entity">实体</param>
    /// <param name="cancellationToken">取消异步操作信号</param>
    /// <returns></returns>
    public Task<string> GetIdStringAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(entity, nameof(entity), cancellationToken);
        return Task.FromResult(ConvertIdToString(entity.Id)!);
    }

    #endregion

    #region IsDelete

    /// <summary>
    ///     是否被删除
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns>判断结果</returns>
    public virtual bool IsDeleted(TEntity entity)
    {
        return IsDeleted(entity.Id);
    }

    /// <summary>
    ///     是否被删除
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    public bool IsDeleted(TKey key)
    {
        return KeyMatchQuery(key).Any();
    }

    /// <summary>
    ///     是否被删除
    /// </summary>
    /// <param name="entity">实体</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>判断结果</returns>
    public virtual Task<bool> IsDeletedAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return IsDeletedAsync(entity.Id, cancellationToken);
    }

    /// <summary>
    ///     是否被删除
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<bool> IsDeletedAsync(TKey key, CancellationToken cancellationToken = default)
    {
        var exists = await KeyMatchQuery(key).AnyAsync(cancellationToken);

        return !exists;
    }

    #endregion

    #region ActionExecution

    /// <summary>
    ///     根据Id查询实体
    /// </summary>
    /// <param name="id">id</param>
    /// <returns></returns>
    protected TEntity? FindById(TKey id)
    {
        return KeyMatchQuery(id).FirstOrDefault();
    }

    /// <summary>
    ///     根据Id查询实体并映射到指定类型
    /// </summary>
    /// <typeparam name="TMapEntity">映射类型</typeparam>
    /// <param name="id">id</param>
    /// <returns></returns>
    protected TMapEntity? FindById<TMapEntity>(TKey id)
    {
        return KeyMatchQuery(id)
            .ProjectToType<TMapEntity>()
            .FirstOrDefault();
    }

    /// <summary>
    ///     根据Id查询实体
    /// </summary>
    /// <param name="ids">id表</param>
    /// <returns></returns>
    protected IEnumerable<TEntity> FindByIds(IEnumerable<TKey> ids)
    {
        return KeyMatchQuery(ids).ToList();
    }

    /// <summary>
    ///     根据Id查询实体并映射到指定类型
    /// </summary>
    /// <typeparam name="TMapEntity">映射类型</typeparam>
    /// <param name="ids">id表</param>
    /// <returns></returns>
    protected IEnumerable<TMapEntity> FindByIds<TMapEntity>(IEnumerable<TKey> ids)
    {
        return KeyMatchQuery(ids)
            .ProjectToType<TMapEntity>()
            .ToList();
    }

    /// <summary>
    ///     根据Id查找实体
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    protected Task<TEntity?> FindByIdAsync(
        TKey id,
        CancellationToken cancellationToken = default)
    {
        return KeyMatchQuery(id).FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    ///     根据Id查询实体并映射到指定类型
    /// </summary>
    /// <typeparam name="TMapEntity">映射类型</typeparam>
    /// <param name="id"></param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    protected Task<TMapEntity?> FindByIdAsync<TMapEntity>(
        TKey id,
        CancellationToken cancellationToken = default)
    {
        return KeyMatchQuery(id)
            .ProjectToType<TMapEntity>()
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    ///     根据Id查找实体
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    protected Task<List<TEntity>> FindByIdsAsync(
        IEnumerable<TKey> ids,
        CancellationToken cancellationToken = default)
    {
        return KeyMatchQuery(ids)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    ///     根据Id查询实体并映射到指定类型
    /// </summary>
    /// <typeparam name="TMapEntity">映射类型</typeparam>
    /// <param name="ids"></param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    protected Task<List<TMapEntity>> FindByIdsAsync<TMapEntity>(
        IEnumerable<TKey> ids,
        CancellationToken cancellationToken = default)
    {
        return KeyMatchQuery(ids)
            .ProjectToType<TMapEntity>()
            .ToListAsync(cancellationToken);
    }

    #endregion
}

/// <summary>
///     无键模型基本存储
/// </summary>
/// <typeparam name="TEntity">模型类型</typeparam>
public abstract class KeyLessStoreBase<TEntity> : IKeyLessStoreBase<TEntity> where TEntity : class
{
    /// <summary>
    ///     无键模型基本存储实例构造
    /// </summary>
    /// <param name="storeOptions"></param>
    /// <param name="logger"></param>
    /// <param name="describer"></param>
    /// <param name="context"></param>
    /// <exception cref="StoreParameterNullException"></exception>
    protected KeyLessStoreBase(
        DbContext context,
        IKeyLessStoreOptions? storeOptions = null,
        ILogger? logger = null,
        StoreErrorDescriber? describer = null)
    {
        Context = context ?? throw new StoreParameterNullException(nameof(context));
        StoreOptions = storeOptions ?? new KeyLessStoreOptions();
        Logger = logger;
        Describer = describer ?? new StoreErrorDescriber();
    }

    /// <summary>
    ///     规范化键
    /// </summary>
    /// <param name="value">键</param>
    /// <returns></returns>
    public string NormalizeKey(string value)
    {
        return value.StringNormalize();
    }

    /// <summary>
    ///     设置当前发生错误的错误描述者
    /// </summary>
    protected StoreErrorDescriber Describer { get; set; }

    #region Access

    /// <summary>
    ///     数据访问上下文
    /// </summary>
    protected DbContext Context { get; }

    /// <summary>
    ///     日志依赖
    /// </summary>
    protected ILogger? Logger { get; }

    /// <summary>
    ///     EntitySet访问器*Main Store Set*
    /// </summary>
    public DbSet<TEntity> EntitySet => Context.Set<TEntity>();

    /// <summary>
    ///     Entity有追踪访问器
    /// </summary>
    public virtual IQueryable<TEntity> TrackingQuery => EntitySet;

    /// <summary>
    ///     Entity无追踪访问器
    /// </summary>
    public IQueryable<TEntity> EntityQuery => TrackingQuery.AsNoTracking();

    #endregion

    #region Options

    /// <summary>
    ///     无键存储配置
    /// </summary>
    private IKeyLessStoreOptions StoreOptions { get; }

    #region SettingAccessor

    /// <summary>
    ///     是否启用Debug日志
    /// </summary>
    protected bool DebugLogger => StoreOptions.DebugLogger && Logger != null;

    /// <summary>
    ///     设置是否自动保存更改
    /// </summary>
    private bool AutoSaveChanges => StoreOptions.AutoSaveChanges;

    #endregion

    #endregion

    #region MapConfigs

    #region Accessor

    /// <summary>
    ///     创建新对象的映射配置
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    protected virtual TypeAdapterConfig CreateNewConfig<TSource>()
    {
        return EmptyConfig<TSource>();
    }

    /// <summary>
    ///     覆盖已有对象的映射配置
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="alternate">覆盖对象是否明确</param>
    /// <returns></returns>
    protected virtual TypeAdapterConfig OverConfig<TSource>(bool alternate)
    {
        return EmptyConfig<TSource>();
    }

    /// <summary>
    ///     合并对象的映射配置
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="alternate">待合并对象是否明确</param>
    /// <returns></returns>
    protected virtual TypeAdapterConfig MergeConfig<TSource>(bool alternate)
    {
        return IgnoreNullConfig<TSource>();
    }

    #endregion

    /// <summary>
    ///     空配置缓存
    /// </summary>
    private TypeAdapterConfig? _emptyConfig;

    /// <summary>
    ///     空配置
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    protected TypeAdapterConfig EmptyConfig<TSource>()
    {
        if (_emptyConfig is not null)
            return _emptyConfig;
        _emptyConfig = new TypeAdapterConfig();
        _emptyConfig.NewConfig<TSource, TEntity>();
        return _emptyConfig;
    }

    /// <summary>
    ///     空值忽略缓存
    /// </summary>
    private TypeAdapterConfig? _ignoreNullConfig;

    /// <summary>
    ///     忽略源实体的空值属性
    /// </summary>
    /// <typeparam name="TSource">源数据类型</typeparam>
    /// <returns>映射配置</returns>
    protected TypeAdapterConfig IgnoreNullConfig<TSource>()
    {
        if (_ignoreNullConfig is not null)
            return _ignoreNullConfig;
        _ignoreNullConfig = new TypeAdapterConfig();
        _ignoreNullConfig.NewConfig<TSource, TEntity>()
            .IgnoreNullValues(true);
        return _ignoreNullConfig;
    }

    /// <summary>
    ///     空字符串忽略缓存
    /// </summary>
    private TypeAdapterConfig? _ignoreEmptyStringConfig;

    /// <summary>
    ///     忽略源实体的空字符串属性
    /// </summary>
    /// <typeparam name="TSource">源数据类型</typeparam>
    /// <returns>映射配置</returns>
    protected TypeAdapterConfig IgnoreEmptyStringConfig<TSource>()
    {
        if (_ignoreEmptyStringConfig is not null)
            return _ignoreEmptyStringConfig;
        _ignoreEmptyStringConfig = new TypeAdapterConfig();
        _ignoreEmptyStringConfig.NewConfig<TSource, TEntity>()
            .Fork(config =>
                config.ForType<string, string>()
                    .MapToTargetWith((src, dest) =>
                        string.IsNullOrEmpty(src) ? dest : src));
        return _ignoreEmptyStringConfig;
    }

    /// <summary>
    ///     空值和空字符串忽略缓存
    /// </summary>
    private TypeAdapterConfig? _ignoreNullAndEmptyConfig;

    /// <summary>
    ///     忽略源实体中的空值和空字符串属性
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    protected TypeAdapterConfig IgnoreNullAndEmptyConfig<TSource>()
    {
        if (_ignoreNullAndEmptyConfig is not null)
            return _ignoreNullAndEmptyConfig;
        _ignoreNullAndEmptyConfig = IgnoreEmptyStringConfig<TSource>().Clone();
        _ignoreNullAndEmptyConfig.NewConfig<TSource, TEntity>()
            .IgnoreNullValues(true);
        return _ignoreNullAndEmptyConfig;
    }

    #endregion

    #region IDisposable

    /// <summary>
    ///     已释放标识
    /// </summary>
    private bool _disposed;

    /// <summary>
    ///     Throws if this class has been disposed.
    /// </summary>
    /// <exception cref="StoreDisposedException"></exception>
    protected void ThrowIfDisposed()
    {
        if (_disposed) throw new StoreDisposedException(GetType());
    }

    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        _disposed = true;
        GC.SuppressFinalize(this);
    }

    #endregion

    #region ActionExecution

    /// <summary>
    ///     添加单个实体
    /// </summary>
    /// <param name="entity">实体</param>
    protected virtual void AddEntity(TEntity entity)
    {
        Context.Add(entity);
    }

    /// <summary>
    ///     添加多个实体
    /// </summary>
    /// <param name="entities">实体</param>
    protected virtual void AddEntities(ICollection<TEntity> entities)
    {
        Context.AddRange(entities);
    }

    /// <summary>
    ///     追踪一个实体更新
    /// </summary>
    /// <param name="entity">实体</param>
    protected virtual void UpdateEntity(TEntity entity)
    {
        Context.Attach(entity);

        if (entity is IConcurrencyStamp concurrency)
            concurrency.ConcurrencyStamp = Guid.NewGuid().ToString();

        Context.Update(entity);
    }

    /// <summary>
    ///     追踪多个实体更新
    /// </summary>
    /// <param name="entities">实体</param>
    protected virtual void UpdateEntities(ICollection<TEntity> entities)
    {
        Context.AttachRange(entities);

        Context.UpdateRange(entities);
    }

    /// <summary>
    ///     批量删除实体
    /// </summary>
    /// <param name="query">查询</param>
    /// <param name="setter">更新委托</param>
    /// <returns></returns>
    protected virtual int BatchUpdateEntity(
        IQueryable<TEntity> query,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter)
    {
        return query.ExecuteUpdate(setter);
    }

    /// <summary>
    ///     批量删除实体
    /// </summary>
    /// <param name="query">查询</param>
    /// <param name="setter">更新委托</param>
    /// <param name="cancellationToken">异步操作取消信号</param>
    /// <returns></returns>
    protected virtual Task<int> BatchUpdateEntityAsync(
        IQueryable<TEntity> query,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setter,
        CancellationToken cancellationToken = default)
    {
        return query.ExecuteUpdateAsync(setter, cancellationToken);
    }

    /// <summary>
    ///     追踪一个实体删除
    /// </summary>
    /// <param name="entity">实体</param>
    protected virtual void DeleteEntity(TEntity entity)
    {
        Context.Remove(entity);
    }

    /// <summary>
    ///     追踪多个实体删除
    /// </summary>
    /// <param name="entities">实体</param>
    protected virtual void DeleteEntities(ICollection<TEntity> entities)
    {
        Context.RemoveRange(entities);
    }

    /// <summary>
    ///     批量删除实体
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    protected virtual int BatchDeleteEntity(IQueryable<TEntity> query)
    {
        return query.ExecuteDelete();
    }

    /// <summary>
    ///     异步批量删除实体
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected virtual Task<int> BatchDeleteEntityAsync(
        IQueryable<TEntity> query,
        CancellationToken cancellationToken = default)
    {
        return query.ExecuteDeleteAsync(cancellationToken);
    }

    #endregion

    #region OnActionExecution

    /// <summary>
    ///     方法执行前
    /// </summary>
    /// <param name="value">实体</param>
    /// <param name="name">参数名</param>
    /// <exception cref="StoreParameterNullException">空参数异常</exception>
    protected void OnActionExecuting<TValue>(TValue value, string name)
    {
        ThrowIfDisposed();
        if (value is null)
            throw new StoreParameterNullException(name);
    }

    /// <summary>
    ///     异步方法执行前
    /// </summary>
    /// <param name="value">实体</param>
    /// <param name="name">参数名</param>
    /// <param name="cancellationToken">取消信号</param>
    protected void OnAsyncActionExecuting<TValue>(TValue value, string name,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        OnActionExecuting(value, name);
    }

    #endregion

    #region SaveChanges

    /// <summary>
    ///     保存当前存储
    /// </summary>
    /// <returns></returns>
    private int SaveChanges()
    {
        if (DebugLogger) Logger?.LogDebug(nameof(SaveChanges));

        return AutoSaveChanges ? Context.SaveChanges() : 0;
    }

    /// <summary>
    ///     保存当前存储
    /// </summary>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>异步取消结果</returns>
    private Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (DebugLogger) Logger?.LogDebug(nameof(SaveChangesAsync));

        return AutoSaveChanges ? Context.SaveChangesAsync(cancellationToken) : Task.FromResult(0);
    }

    #endregion

    #region AttacheChange

    /// <summary>
    ///     保存追踪
    /// </summary>
    /// <returns></returns>
    protected StoreResult AttacheChange()
    {
        try
        {
            var changes = SaveChanges();
            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(Describer.ConcurrencyFailure());
        }
    }

    /// <summary>
    ///     保存异步追踪
    /// </summary>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    protected async Task<StoreResult> AttacheChangeAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var changes = await SaveChangesAsync(cancellationToken);
            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(Describer.ConcurrencyFailure());
        }
    }

    #endregion
}