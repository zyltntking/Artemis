using Artemis.Data.Core;
using Artemis.Data.Shared.Transfer.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;

namespace Artemis.Extensions.ServiceConnect.Authorization;

/// <summary>
/// 令牌处理器扩展
/// </summary>
public static class TokenHandlerExtensions
{
    #region Cache

    /// <summary>
    ///     从缓存获取Token
    /// </summary>
    /// <param name="cache">缓存依赖</param>
    /// <param name="cacheTokenKey">缓存键</param>
    /// <param name="refreshToken">是否刷新缓存</param>
    /// <returns></returns>
    public static TTokenDocument? FetchTokenFromCache<TTokenDocument>(
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
    ///     从缓存获取用户对Token的映射
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="cacheKey"></param>
    /// <param name="refreshToken"></param>
    /// <returns></returns>
    public static string? FetchUserMapTokenFromCache(
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

    #endregion

    #region HttpContext

    /// <summary>
    ///     从上下文中获取Token文档
    /// </summary>
    /// <param name="context"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static TTokenDocument? FetchTokenDocumentFromContext<TTokenDocument>(this HttpContext context, string key) where TTokenDocument : class
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
    public static void CacheToken(
        this HttpContext context, string key, TokenDocument document)
    {
        if (context.Items.ContainsKey(key))
            context.Items[key] = document;

        context.Items.Add(key, document);
    }

    ///// <summary>
    /////     移除上下文中的Token
    ///// </summary>
    ///// <param name="context"></param>
    //public static void RemoveToken(this HttpContext context)
    //{
    //    if (context.Items.ContainsKey(Constants.ContextIdentityItemKey))
    //        context.Items.Remove(Constants.ContextIdentityItemKey);
    //}


    /// <summary>
    /// 从请求头中获取Token串
    /// </summary>
    /// <param name="context"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string? FetchTokenFromRequestHeader(this HttpContext context, string key)
    {
        var headers = context.Request.Headers;

        if (headers.TryGetValue(key, out var token)) return token;

        return null;
    }

    #endregion
}