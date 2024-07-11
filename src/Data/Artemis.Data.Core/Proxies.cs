namespace Artemis.Data.Core;

#region HandlerProxy

/// <summary>
///     操作者代理接口
/// </summary>
public interface IHandlerProxy
{
    /// <summary>
    ///     操作员
    /// </summary>
    string Handler { get; }
}

/// <summary>
///     抽象操作员代理实现
/// </summary>
public abstract class AbstractHandlerProxy : IHandlerProxy
{
    #region Implementation of IHandlerProxy

    /// <summary>
    ///     操作员
    /// </summary>
    public abstract string Handler { get; }

    #endregion
}

#endregion

#region CacheProxy

/// <summary>
///     缓存代理接口
/// </summary>
public interface ICacheProxy
{
    /// <summary>
    ///     设置缓存
    /// </summary>
    /// <typeparam name="TEntity">缓存实体类型</typeparam>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    /// <param name="expire">过期时间</param>
    void Set<TEntity>(string key, TEntity value, int expire = 0) where TEntity : class;

    /// <summary>
    ///     异步设置缓存
    /// </summary>
    /// <typeparam name="TEntity">缓存实体类型</typeparam>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    /// <param name="expire">过期时间</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task SetAsync<TEntity>(string key, TEntity value, int expire = 0, CancellationToken cancellationToken = default)
        where TEntity : class;

    /// <summary>
    ///     获取缓存
    /// </summary>
    /// <typeparam name="TEntity">缓存实体类型</typeparam>
    /// <param name="key">键</param>
    /// <returns>值</returns>
    TEntity? Get<TEntity>(string key) where TEntity : class;

    /// <summary>
    ///     异步获取缓存
    /// </summary>
    /// <typeparam name="TEntity">缓存实体</typeparam>
    /// <param name="key">键</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<TEntity?> GetAsync<TEntity>(string key, CancellationToken cancellationToken = default) where TEntity : class;

    /// <summary>
    ///     移除缓存
    /// </summary>
    /// <param name="key">键</param>
    void Remove(string key);

    /// <summary>
    ///     异步移除缓存
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);
}

/// <summary>
///     抽象缓存代理实现
/// </summary>
/// <typeparam name="TCacheImplement"></typeparam>
public abstract class AbstractCacheProxy<TCacheImplement> : ICacheProxy
{
    /// <summary>
    ///     抽象缓存代理实现
    /// </summary>
    /// <param name="cache"></param>
    protected AbstractCacheProxy(TCacheImplement cache)
    {
        Cache = cache;
    }

    /// <summary>
    ///     缓存依赖
    /// </summary>
    protected TCacheImplement Cache { get; }

    #region Implementation of ICacheProxy

    /// <summary>
    ///     设置缓存
    /// </summary>
    /// <typeparam name="TEntity">缓存实体</typeparam>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    /// <param name="expire">过期时间</param>
    public abstract void Set<TEntity>(string key, TEntity value, int expire = 0) where TEntity : class;

    /// <summary>
    ///     异步设置缓存
    /// </summary>
    /// <typeparam name="TEntity">缓存实体类型</typeparam>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    /// <param name="expire">过期时间</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public abstract Task SetAsync<TEntity>(string key, TEntity value, int expire = 0,
        CancellationToken cancellationToken = default) where TEntity : class;

    /// <summary>
    ///     获取缓存
    /// </summary>
    /// <typeparam name="TEntity">缓存实体类型</typeparam>
    /// <param name="key">键</param>
    /// <returns>值</returns>
    public abstract TEntity? Get<TEntity>(string key) where TEntity : class;

    /// <summary>
    ///     异步获取缓存
    /// </summary>
    /// <typeparam name="TEntity">缓存实体</typeparam>
    /// <param name="key">键</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public abstract Task<TEntity?> GetAsync<TEntity>(string key, CancellationToken cancellationToken = default)
        where TEntity : class;

    /// <summary>
    ///     移除缓存
    /// </summary>
    /// <param name="key">键</param>
    public abstract void Remove(string key);

    /// <summary>
    ///     异步移除缓存
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public abstract Task RemoveAsync(string key, CancellationToken cancellationToken = default);

    #endregion
}

#endregion