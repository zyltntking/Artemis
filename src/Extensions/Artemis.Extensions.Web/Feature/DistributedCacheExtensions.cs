using System.Security.Claims;
using Microsoft.Extensions.Caching.Distributed;

namespace Artemis.Extensions.Web.Feature;

/// <summary>
///     分布式缓存扩展
/// </summary>
public static class DistributedCacheExtensions
{
    /// <summary>
    ///     获取凭据列表
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<Claim> FetchClaims(this IDistributedCache cache)
    {
        // todo add inner imp
        return new List<Claim>();
    }
}