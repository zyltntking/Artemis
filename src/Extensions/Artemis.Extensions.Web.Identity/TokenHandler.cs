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

        var user = document.UserName;

        return Hash.Md5Hash($"{user}:{stamp}");
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
        if (expire <= 0)
        {
            cache.SetString(cacheTokenKey, document.Serialize());
        }
        else
        {
            var options = new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromSeconds(expire)
            };

            cache.SetString(cacheTokenKey, document.Serialize(), options);
        }
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
        if (context.Items.ContainsKey(Constants.ContextItemKey)) context.Items[Constants.ContextItemKey] = document;

        context.Items.Add(Constants.ContextItemKey, document);
    }

    /// <summary>
    ///     移除缓存中的Token
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="cacheTokenKey"></param>
    public static void RemoveToken(
        this IDistributedCache cache,
        string cacheTokenKey)
    {
        cache.Remove(cacheTokenKey);
    }

    /// <summary>
    ///     移除上下文中的Token
    /// </summary>
    /// <param name="context"></param>
    public static void RemoveToken(this HttpContext context)
    {
        if (context.Items.ContainsKey(Constants.ContextItemKey)) context.Items.Remove(Constants.ContextItemKey);
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
    ///     从上下文中获取Token
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static TokenDocument? FetchToken(this HttpContext context)
    {
        if (context.Items.TryGetValue(Constants.ContextItemKey, out var document))
            if (document is TokenDocument tokenDocument)
                return tokenDocument;

        return null;
    }
}