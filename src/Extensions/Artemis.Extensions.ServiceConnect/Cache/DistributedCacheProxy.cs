using Artemis.Data.Core;
using Microsoft.Extensions.Caching.Distributed;

namespace Artemis.Extensions.ServiceConnect.Cache;

/// <summary>
///     分布式缓存代理
/// </summary>
public class DistributedCacheProxy : AbstractCacheProxy
{
    /// <summary>
    ///     分布式缓存代理
    /// </summary>
    /// <param name="cache">缓存依赖</param>
    public DistributedCacheProxy(IDistributedCache cache)
    {
        Cache = cache;
    }

    /// <summary>
    ///     缓存依赖
    /// </summary>
    private IDistributedCache Cache { get; }

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
        {
            Cache.SetString(key, value.Serialize());
        }
        else
        {
            var options = new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromSeconds(expire)
            };

            Cache.SetString(key, value.Serialize(), options);
        }
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

        if (expire <= 0) return Cache.SetStringAsync(key, Cache.Serialize(), cancellationToken);

        var options = new DistributedCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromSeconds(expire)
        };

        return Cache.SetStringAsync(key, value.Serialize(), options, cancellationToken);
    }

    /// <summary>
    ///     获取缓存
    /// </summary>
    /// <typeparam name="TEntity">缓存实体类型</typeparam>
    /// <param name="key">键</param>
    /// <returns>值</returns>
    public override TEntity? Get<TEntity>(string key) where TEntity : class
    {
        var value = Cache.GetString(key);

        return value?.Deserialize<TEntity>();
    }

    /// <summary>
    ///     异步获取缓存
    /// </summary>
    /// <typeparam name="TEntity">缓存实体</typeparam>
    /// <param name="key">键</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public override Task<TEntity?> GetAsync<TEntity>(string key, CancellationToken cancellationToken = default)
        where TEntity : class
    {
        cancellationToken.ThrowIfCancellationRequested();

        var value = Cache.GetString(key);
        return value is null
            ? Task.FromResult(default(TEntity))
            : Task.FromResult(value.Deserialize<TEntity>());
    }

    /// <summary>
    ///     移除缓存
    /// </summary>
    /// <param name="key">键</param>
    public override void Remove(string key)
    {
        Cache.Remove(key);
    }

    /// <summary>
    ///     异步移除缓存
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        return Cache.RemoveAsync(key, cancellationToken);
    }

    #endregion
}