using Artemis.Data.Core;
using StackExchange.Redis;

namespace Artemis.Extensions.ServiceConnect.Cache;

/// <summary>
///     ConnectionMultiplexer缓存代理
/// </summary>
public class ConnectionMultiplexerCacheProxy : AbstractCacheProxy<IConnectionMultiplexer>
{
    /// <summary>
    ///     抽象缓存代理实现
    /// </summary>
    /// <param name="cache"></param>
    public ConnectionMultiplexerCacheProxy(IConnectionMultiplexer cache) : base(cache)
    {
    }

    #region Overrides of AbstractCacheProxy

    /// <summary>
    ///     设置缓存
    /// </summary>
    /// <typeparam name="TEntity">缓存实体</typeparam>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    /// <param name="expire">过期时间</param>
    public override void Set<TEntity>(string key, TEntity value, int expire = 0)
    {
        if (expire <= 0)
            Cache.GetDatabase().StringSet(key, value.Serialize());
        else
            Cache.GetDatabase().StringSet(key, value.Serialize(), TimeSpan.FromSeconds(expire));
    }

    /// <summary>
    ///     异步设置缓存
    /// </summary>
    /// <typeparam name="TEntity">缓存实体类型</typeparam>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    /// <param name="expire">过期时间</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public override Task SetAsync<TEntity>(string key, TEntity value, int expire = 0,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (expire <= 0)
            return Cache.GetDatabase().StringSetAsync(key, value.Serialize());

        return Cache.GetDatabase().StringSetAsync(key, value.Serialize(), TimeSpan.FromSeconds(expire));
    }

    /// <summary>
    ///     获取缓存
    /// </summary>
    /// <typeparam name="TEntity">缓存实体类型</typeparam>
    /// <param name="key">键</param>
    /// <returns>值</returns>
    public override TEntity? Get<TEntity>(string key) where TEntity : class
    {
        var value = Cache.GetDatabase().StringGet(key);

        return value.HasValue ? value.ToString().Deserialize<TEntity>() : null;
    }

    /// <summary>
    ///     异步获取缓存
    /// </summary>
    /// <typeparam name="TEntity">缓存实体</typeparam>
    /// <param name="key">键</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public override async Task<TEntity?> GetAsync<TEntity>(string key, CancellationToken cancellationToken = default)
        where TEntity : class
    {
        cancellationToken.ThrowIfCancellationRequested();

        var value = await Cache.GetDatabase().StringGetAsync(key);

        return value.HasValue ? value.ToString().Deserialize<TEntity>() : null;
    }

    /// <summary>
    ///     移除缓存
    /// </summary>
    /// <param name="key">键</param>
    public override void Remove(string key)
    {
        Cache.GetDatabase().KeyDelete(key);
    }

    /// <summary>
    ///     异步移除缓存
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Cache.GetDatabase().KeyDeleteAsync(key);
    }

    #endregion
}