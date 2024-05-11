using Artemis.Data.Core;
using Artemis.Shared.Identity.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;

namespace Artemis.Extensions.Web.Identity;

/// <summary>
///     Token处理器
/// </summary>
public static class TokenHandler
{
    /// <summary>
    ///     生成Token缓存键
    /// </summary>
    /// <param name="document">token 文档</param>
    /// <returns></returns>
    public static string GenerateTokenKey(this TokenDocument document)
    {
        var stamp = DateTime.Now.ToUnixTimeStamp();

        var (id, user) = (document.UserId, document.UserName);

        return Hash.Md5Hash($"{id}|{user}:{stamp}");
    }

    /// <summary>
    ///     缓存字符串
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="expire"></param>
    private static void CacheString(this IDistributedCache cache, string key, string value, int expire)
    {
        if (expire <= 0)
        {
            cache.SetString(key, value);
        }
        else
        {
            var options = new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromSeconds(expire)
            };

            cache.SetString(key, value, options);
        }
    }

    /// <summary>
    ///     异步缓存字符串
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="expire"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private static Task CacheStringAsync(this IDistributedCache cache, string key, string value, int expire,
        CancellationToken cancellationToken = default)
    {
        if (expire <= 0) return cache.SetStringAsync(key, value, cancellationToken);

        var options = new DistributedCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromSeconds(expire)
        };

        return cache.SetStringAsync(key, value, options, cancellationToken);
    }

    /// <summary>
    ///     缓存Token
    /// </summary>
    /// <param name="cache">缓存依赖</param>
    /// <param name="document">Token文档</param>
    /// <param name="cacheTokenKey">缓存键</param>
    /// <param name="expire">过期时间</param>
    /// <returns></returns>
    public static void CacheToken(
        this IDistributedCache cache,
        TokenDocument document,
        string cacheTokenKey,
        int expire = 0)
    {
        cache.CacheString(cacheTokenKey, document.Serialize(), expire);
    }

    /// <summary>
    ///     缓存Token
    /// </summary>
    /// <param name="cache">缓存依赖</param>
    /// <param name="document">Token文档</param>
    /// <param name="cacheTokenKey">缓存键</param>
    /// <param name="expire">过期时间</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static Task CacheTokenAsync(
        this IDistributedCache cache,
        TokenDocument document,
        string cacheTokenKey,
        int expire = 0,
        CancellationToken cancellationToken = default)
    {
        return cache.CacheStringAsync(cacheTokenKey, document.Serialize(), expire, cancellationToken);
    }

    /// <summary>
    ///     缓存用户对Token的映射
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="cacheKey"></param>
    /// <param name="token"></param>
    /// <param name="expire"></param>
    public static void CacheUserMapToken(
        this IDistributedCache cache,
        string cacheKey,
        string token,
        int expire = 0)
    {
        cache.CacheString(cacheKey, token, expire);
    }

    /// <summary>
    ///     缓存用户对Token的映射
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="cacheKey"></param>
    /// <param name="token"></param>
    /// <param name="expire"></param>
    /// <param name="cancellationToken"></param>
    public static Task CacheUserMapTokenAsync(
        this IDistributedCache cache,
        string cacheKey,
        string token,
        int expire = 0,
        CancellationToken cancellationToken = default)
    {
        return cache.CacheStringAsync(cacheKey, token, expire, cancellationToken);
    }

    /// <summary>
    ///     缓存token
    /// </summary>
    /// <param name="context"></param>
    /// <param name="document"></param>
    public static void CacheToken(
        this HttpContext context,
        TokenDocument document)
    {
        if (context.Items.ContainsKey(Constants.ContextIdentityItemKey))
            context.Items[Constants.ContextIdentityItemKey] = document;

        context.Items.Add(Constants.ContextIdentityItemKey, document);
    }

    /// <summary>
    ///     移除上下文中的Token
    /// </summary>
    /// <param name="context"></param>
    public static void RemoveToken(this HttpContext context)
    {
        if (context.Items.ContainsKey(Constants.ContextIdentityItemKey))
            context.Items.Remove(Constants.ContextIdentityItemKey);
    }

    /// <summary>
    ///     获取Token
    /// </summary>
    /// <param name="cache">缓存依赖</param>
    /// <param name="cacheTokenKey">缓存键</param>
    /// <param name="refreshToken">是否刷新缓存</param>
    /// <returns></returns>
    public static TokenDocument? FetchToken(
        this IDistributedCache cache,
        string cacheTokenKey,
        bool refreshToken = true)
    {
        var value = cache.GetString(cacheTokenKey);

        if (value == null) return null;

        // 刷新缓存
        if (refreshToken) cache.Refresh(cacheTokenKey);

        return value.Deserialize<TokenDocument>();
    }

    /// <summary>
    ///     获取Token
    /// </summary>
    /// <param name="cache">缓存依赖</param>
    /// <param name="cacheTokenKey">缓存键</param>
    /// <param name="refreshToken">是否刷新缓存</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<TokenDocument?> FetchTokenAsync(
        this IDistributedCache cache,
        string cacheTokenKey,
        bool refreshToken = true,
        CancellationToken cancellationToken = default)
    {
        var value = await cache.GetStringAsync(cacheTokenKey, cancellationToken);

        if (value == null)
            return null;

        // 刷新缓存
        if (refreshToken)
            await cache.RefreshAsync(cacheTokenKey, cancellationToken);

        return value.Deserialize<TokenDocument>();
    }

    /// <summary>
    ///     获取用户对Token的映射
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="cacheKey"></param>
    /// <param name="refreshToken"></param>
    /// <returns></returns>
    public static string? FetchUserMapToken(
        this IDistributedCache cache,
        string cacheKey,
        bool refreshToken = true)
    {
        var value = cache.GetString(cacheKey);

        if (value == null) return null;

        // 刷新缓存
        if (refreshToken) cache.Refresh(cacheKey);

        return value;
    }

    /// <summary>
    ///     获取用户对Token的映射
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="cacheKey"></param>
    /// <param name="refreshToken"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<string?> FetchUserMapTokenAsync(
        this IDistributedCache cache,
        string cacheKey,
        bool refreshToken = true,
        CancellationToken cancellationToken = default)
    {
        var value = await cache.GetStringAsync(cacheKey, cancellationToken);

        if (value == null)
            return null;

        // 刷新缓存
        if (refreshToken)
            await cache.RefreshAsync(cacheKey, cancellationToken);

        return value;
    }

    /// <summary>
    ///     从上下文中获取Token
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static TokenDocument? FetchToken(this HttpContext context)
    {
        if (context.Items.TryGetValue(Constants.ContextIdentityItemKey, out var document))
            if (document is TokenDocument tokenDocument)
                return tokenDocument;

        return null;
    }
}