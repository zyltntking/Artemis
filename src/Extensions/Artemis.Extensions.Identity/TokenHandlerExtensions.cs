using System.Security.Claims;
using Artemis.Data.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;

namespace Artemis.Extensions.Identity;

/// <summary>
///     令牌处理器扩展
/// </summary>
public static class TokenHandlerExtensions
{
    /// <summary>
    ///     从请求头中获取Token串
    /// </summary>
    /// <param name="request"></param>
    /// <param name="key"></param>
    /// <param name="schema"></param>
    /// <returns></returns>
    internal static string? FetchRequestTokenSymbol(this HttpRequest request, string key, string schema)
    {
        var headers = request.Headers;

        if (headers.ContainsKey(key))
        {
            string token = headers[key]!;

            if (token.StartsWith(schema, StringComparison.OrdinalIgnoreCase))
            {
                token = token[schema.Length..];
                return token;
            }
        }

        return null;
    }

    #region Cache

    /// <summary>
    ///     缓存Token记录
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="cacheTokenKey"></param>
    /// <param name="record"></param>
    public static void CacheTokenRecord(
        this IDistributedCache cache,
        string cacheTokenKey,
        TokenRecord record)
    {
        var value = record.Serialize();

        cache.CacheString(cacheTokenKey, value, record.Expire);
    }

    /// <summary>
    ///     异步缓存Token记录
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="cacheTokenKey"></param>
    /// <param name="record"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static Task CacheTokenRecordAsync(
        this IDistributedCache cache,
        string cacheTokenKey,
        TokenRecord record,
        CancellationToken cancellationToken = default)
    {
        var value = record.Serialize();

        return cache.CacheStringAsync(cacheTokenKey, value, record.Expire, cancellationToken);
    }

    /// <summary>
    ///     从缓存获取Token记录
    /// </summary>
    /// <param name="cache">缓存依赖</param>
    /// <param name="cacheTokenKey">缓存键</param>
    /// <param name="refreshToken">是否刷新缓存</param>
    /// <returns></returns>
    public static TokenRecord? FetchTokenRecord(
        this IDistributedCache cache,
        string cacheTokenKey,
        bool refreshToken = true)
    {
        var value = cache.GetString(cacheTokenKey);

        if (value == null)
            return null;

        // 刷新缓存
        if (refreshToken)
            cache.Refresh(cacheTokenKey);

        return value.Deserialize<TokenRecord>();
    }

    /// <summary>
    ///     从缓存中获取Token记录
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="cacheTokenKey"></param>
    /// <param name="refreshToken"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<TokenRecord?> FetchTokenRecordAsync(
        this IDistributedCache cache,
        string cacheTokenKey,
        bool refreshToken = true,
        CancellationToken cancellationToken = default)
    {
        var value = await cache.GetStringAsync(cacheTokenKey, cancellationToken);

        if (value == null)
            return null;

        // 刷新缓存
        if (refreshToken) await cache.RefreshAsync(cacheTokenKey, cancellationToken);

        return value.Deserialize<TokenRecord>();
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
    /// <param name="prefix"></param>
    /// <param name="end"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static string CacheUserMapTokenKey(string prefix, string end, Guid id)
    {
        return $"{prefix}:{end}:{id:D}";
    }

    /// <summary>
    ///     登录提供键
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="end">端类型</param>
    /// <returns></returns>
    public static string LoginProviderKey(Guid id, string end)
    {
        return $"{id:D}:{end}";
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