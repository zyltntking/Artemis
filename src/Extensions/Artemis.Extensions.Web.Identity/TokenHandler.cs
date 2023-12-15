using Artemis.Data.Core;
using Artemis.Shared.Identity.Transfer;
using Microsoft.Extensions.Caching.Distributed;

namespace Artemis.Extensions.Web.Identity;

/// <summary>
/// Token处理器
/// </summary>
public static class TokenHandler
{
    /// <summary>
    /// 生成Token缓存键
    /// </summary>
    /// <param name="document">token 文档</param>
    /// <param name="cacheTokenPrefix">缓存键前缀</param>
    /// <returns></returns>
    public static string GenerateTokenKey(
        this TokenDocument document, 
        string cacheTokenPrefix = Constants.CacheTokenPrefix)
    {
        var stamp = DateTime.Now.ToUnixTimeStamp();

        var user = document.UserName;

        var key = Hash.Md5Hash($"{user}:{stamp}");

        return $"{cacheTokenPrefix}:{key}";
    }

    /// <summary>
    /// 缓存Token
    /// </summary>
    /// <param name="cache">缓存依赖</param>
    /// <param name="document">Token文档</param>
    /// <param name="cacheTokenKey">缓存键</param>
    /// <param name="expire">过期时间</param>
    /// <returns></returns>
    public static void CacheToken(this IDistributedCache cache, TokenDocument document, string cacheTokenKey, int expire = 0)
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
    /// 获取Token
    /// </summary>
    /// <param name="cache">缓存依赖</param>
    /// <param name="cacheTokenKey">缓存键</param>
    /// <returns></returns>
    public static TokenDocument? FetchToken(this IDistributedCache cache, string cacheTokenKey)
    {
        var value = cache.GetString(cacheTokenKey);

        if (value == null)
        {
            return null;
        }

        cache.Refresh(cacheTokenKey);

        return value.Deserialize<TokenDocument>();
    }
}