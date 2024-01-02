using Artemis.Data.Core;
using Microsoft.Extensions.Caching.Distributed;

namespace Artemis.Data.Store.Extensions;

/// <summary>
///     分布式缓存扩展
/// </summary>
public static class DistributedCacheExtensions
{
    /// <summary>
    ///     设置缓存
    /// </summary>
    /// <typeparam name="T">存储类型</typeparam>
    /// <param name="cache">缓存依赖</param>
    /// <param name="key">缓存键</param>
    /// <param name="entity">实体</param>
    /// <param name="expire">过期时间</param>
    public static void Set<T>(
        this IDistributedCache cache,
        string key, 
        T entity,
        int expire = 0) where T : class
    {
        if (expire <= 0)
        {
            cache.SetString(key, entity.Serialize());
        }
        else
        {
            var options = new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromSeconds(expire)
            };

            cache.SetString(key, entity.Serialize(), options);
        }
    }


    /// <summary>
    ///     设置缓存
    /// </summary>
    /// <typeparam name="T">存储类型</typeparam>
    /// <param name="cache">缓存依赖</param>
    /// <param name="key">缓存键</param>
    /// <param name="entity">缓存实体</param>
    /// <param name="expire">过期时间</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public static Task SetAsync<T>(
        this IDistributedCache cache,
        string key,
        T entity,
        int expire = 0,
        CancellationToken cancellationToken = default) where T : class
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (expire <= 0) return cache.SetStringAsync(key, entity.Serialize(), cancellationToken);

        var options = new DistributedCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromSeconds(expire)
        };

        return cache.SetStringAsync(key, entity.Serialize(), options, cancellationToken);
    }

    /// <summary>
    ///     获取缓存
    /// </summary>
    /// <typeparam name="T">存储类型</typeparam>
    /// <param name="cache">缓存依赖</param>
    /// <param name="key">缓存键</param>
    /// <returns></returns>
    public static T? Get<T>(this IDistributedCache cache, string key) where T : class
    {
        var value = cache.GetString(key);

        return value?.Deserialize<T>();
    }

    /// <summary>
    ///     获取缓存
    /// </summary>
    /// <typeparam name="T">存储类型</typeparam>
    /// <param name="cache">缓存依赖</param>
    /// <param name="key">缓存键</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public static Task<T?> GetAsync<T>(
        this IDistributedCache cache,
        string key,
        CancellationToken cancellationToken = default) where T : class
    {
        cancellationToken.ThrowIfCancellationRequested();

        var value = cache.GetString(key);
        return value is null
            ? Task.FromResult(default(T))
            : Task.FromResult(value.Deserialize<T>());
    }
}