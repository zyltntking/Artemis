using Artemis.Data.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;

namespace Artemis.Extensions.ServiceConnect.Authorization;

/// <summary>
///     令牌处理器扩展
/// </summary>
public static class TokenHandlerExtensions
{
    #region Cache

    /// <summary>
    ///     缓存Token文档
    /// </summary>
    /// <typeparam name="TTokenDocument"></typeparam>
    /// <param name="cache"></param>
    /// <param name="cacheTokenKey"></param>
    /// <param name="document"></param>
    /// <param name="expire"></param>
    public static void CacheTokenDocument<TTokenDocument>(
        this IDistributedCache cache,
        string cacheTokenKey,
        TTokenDocument document,
        int expire = 0) where TTokenDocument : class
    {
        var value = document.Serialize();

        cache.CacheString(cacheTokenKey, value, expire);
    }

    /// <summary>
    ///     异步缓存Token文档
    /// </summary>
    /// <typeparam name="TTokenDocument"></typeparam>
    /// <param name="cache"></param>
    /// <param name="cacheTokenKey"></param>
    /// <param name="document"></param>
    /// <param name="expire"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static Task CacheTokenDocumentAsync<TTokenDocument>(
        this IDistributedCache cache,
        string cacheTokenKey,
        TTokenDocument document,
        int expire = 0,
        CancellationToken cancellationToken = default) where TTokenDocument : class
    {
        var value = document.Serialize();

        return cache.CacheStringAsync(cacheTokenKey, value, expire, cancellationToken);
    }

    /// <summary>
    ///     从缓存获取Token
    /// </summary>
    /// <param name="cache">缓存依赖</param>
    /// <param name="cacheTokenKey">缓存键</param>
    /// <param name="refreshToken">是否刷新缓存</param>
    /// <returns></returns>
    public static TTokenDocument? FetchTokenDocument<TTokenDocument>(
        this IDistributedCache cache,
        string cacheTokenKey,
        bool refreshToken = true) where TTokenDocument : class
    {
        var value = cache.GetString(cacheTokenKey);

        if (value == null)
            return null;

        // 刷新缓存
        if (refreshToken) cache.Refresh(cacheTokenKey);

        return value.Deserialize<TTokenDocument>();
    }

    /// <summary>
    ///     从缓存中获取Token文档
    /// </summary>
    /// <typeparam name="TTokenDocument"></typeparam>
    /// <param name="cache"></param>
    /// <param name="cacheTokenKey"></param>
    /// <param name="refreshToken"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<TTokenDocument?> FetchTokenDocumentAsync<TTokenDocument>(
        this IDistributedCache cache,
        string cacheTokenKey,
        bool refreshToken = true,
        CancellationToken cancellationToken = default) where TTokenDocument : class
    {
        var value = await cache.GetStringAsync(cacheTokenKey, cancellationToken);

        if (value == null)
            return null;

        // 刷新缓存
        if (refreshToken) await cache.RefreshAsync(cacheTokenKey, cancellationToken);

        return value.Deserialize<TTokenDocument>();
    }

    /// <summary>
    ///     缓存用户对Token的映射
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="cacheKey"></param>
    /// <param name="tokenSymbol"></param>
    /// <param name="expire"></param>
    public static void CacheUserMapTokenSymbol(
        this IDistributedCache cache,
        string cacheKey,
        string tokenSymbol,
        int expire = 0)
    {
        cache.CacheString(cacheKey, tokenSymbol, expire);
    }

    /// <summary>
    ///     缓存用户对Token的映射
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="cacheKey"></param>
    /// <param name="tokenSymbol"></param>
    /// <param name="expire"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static Task CacheUserMapTokenSymbolAsync(
        this IDistributedCache cache,
        string cacheKey,
        string tokenSymbol,
        int expire = 0,
        CancellationToken cancellationToken = default)
    {
        return cache.CacheStringAsync(cacheKey, tokenSymbol, expire, cancellationToken);
    }

    /// <summary>
    ///     从缓存获取用户对Token的映射
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="cacheKey"></param>
    /// <param name="refreshToken"></param>
    /// <returns></returns>
    public static string? FetchUserMapTokenSymbol(
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
    ///     从缓存获取用户对Token的映射
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="cacheKey"></param>
    /// <param name="refreshToken"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<string?> FetchUserMapTokenSymbolAsync(
        this IDistributedCache cache,
        string cacheKey,
        bool refreshToken = true,
        CancellationToken cancellationToken = default)
    {
        var value = await cache.GetStringAsync(cacheKey, cancellationToken);

        if (value == null) return null;

        // 刷新缓存
        if (refreshToken) await cache.RefreshAsync(cacheKey, cancellationToken);

        return value;
    }

    #region Loginc

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

    #endregion

    #endregion

    #region HttpContext

    /// <summary>
    ///     从上下文中获取Token文档
    /// </summary>
    /// <param name="context"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static TTokenDocument? FetchTokenDocument<TTokenDocument>(
        this HttpContext context,
        string key) where TTokenDocument : class
    {
        if (context.Items.TryGetValue(key, out var document))
            if (document is TTokenDocument tokenDocument)
                return tokenDocument;

        return null;
    }

    /// <summary>
    ///     缓存token到上下文
    /// </summary>
    /// <param name="context"></param>
    /// <param name="key"></param>
    /// <param name="document"></param>
    public static void CacheTokenDocument<TTokenDocument>(
        this HttpContext context,
        string key,
        TTokenDocument document) where TTokenDocument : class
    {
        if (context.Items.ContainsKey(key))
            context.Items[key] = document;

        context.Items.Add(key, document);
    }

    /// <summary>
    ///     移除上下文中的Token
    /// </summary>
    /// <param name="context"></param>
    /// <param name="key"></param>
    public static void RemoveTokenDocument(this HttpContext context, string key)
    {
        if (context.Items.ContainsKey(key))
            context.Items.Remove(key);
    }

    /// <summary>
    ///     从请求头中获取Token串
    /// </summary>
    /// <param name="context"></param>
    /// <param name="key"></param>
    /// <param name="schema"></param>
    /// <returns></returns>
    public static string? FetchTokenSymbol(this HttpContext context, string key, string schema)
    {
        var headers = context.Request.Headers;

        if (headers.ContainsKey(key))
        {
            string token = headers[key]!;

            if (token.StartsWith(schema, StringComparison.OrdinalIgnoreCase))
            {
                token = token[schema.Length..];
            }

            return token;
        }

        return null;
    }

    #endregion
}

/// <summary>
///     TokenKey生成器
/// </summary>
public static class TokenKeyGenerator
{
    /// <summary>
    ///     缓存Token键
    /// </summary>
    /// <param name="prefix">前缀</param>
    /// <param name="symbol">Token符号</param>
    /// <returns></returns>
    public static string CacheTokenKey(string prefix, string symbol)
    {
        return $"{prefix}:{symbol}";
    }

    /// <summary>
    ///     缓存用户映射Token键
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="prefix"></param>
    /// <param name="end"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static string CacheUserMapTokenKey<TKey>(string prefix, string end, TKey id) where TKey : IEquatable<TKey>
    {
        return $"{prefix}:{end}:{id}";
    }

    /// <summary>
    ///     登录提供键
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="end">端类型</param>
    /// <returns></returns>
    public static string LoginProviderKey<TKey>(TKey id, string end) where TKey : IEquatable<TKey>
    {
        return $"{id}:{end}";
    }

    /// <summary>
    ///     令牌名称
    /// </summary>
    /// <param name="end"></param>
    /// <param name="suffix"></param>
    /// <returns></returns>
    public static string ProviderTokenName(string end, string suffix)
    {
        return $"{end}:{suffix}";
    }
}