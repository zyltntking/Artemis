using Grpc.AspNetCore.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Extensions.Caching.Distributed;

namespace Artemis.Extensions.Web.Identity;

/// <summary>
///     操作描述器
/// </summary>
public static class IdentityDescriptor
{
    /// <summary>
    ///     查询操作名称
    /// </summary>
    /// <param name="endpoint"></param>
    /// <returns></returns>
    public static string? FetchActionName(this RouteEndpoint endpoint)
    {
        var pageAction = endpoint.HasMetadata<PageModelAttribute>();

        var apiAction = endpoint.HasMetadata<ApiControllerAttribute>();

        var grpcAction = endpoint.HasMetadata<GrpcMethodMetadata>();

        if (pageAction) return endpoint.RoutePatternActionName();

        if (apiAction) return endpoint.ApiActionName();

        if (grpcAction) return endpoint.RoutePatternActionName();

        return null;
    }

    /// <summary>
    ///     查询路由路径
    /// </summary>
    /// <param name="endpoint"></param>
    /// <returns></returns>
    public static string? FetchRoutePath(this RouteEndpoint endpoint)
    {
        return endpoint.RoutePattern.RawText;
    }

    /// <summary>
    ///     获取Token
    /// </summary>
    /// <param name="context">http 上下文</param>
    /// <param name="headerTokenKey">header 键</param>
    /// <returns></returns>
    public static string? FetchHeaderToken(HttpContext context, string headerTokenKey)
    {
        var headers = context.Request.Headers;

        if (headers.TryGetValue(headerTokenKey, out var token)) return token;

        return null;
    }

    /// <summary>
    ///     获取缓存Token
    /// </summary>
    /// <param name="cache">缓存</param>
    /// <param name="cacheTokenKey">缓存token键</param>
    /// <returns></returns>
    public static string? FetchCacheToken(IDistributedCache cache, string cacheTokenKey)
    {
        var cacheToken = cache.GetString(cacheTokenKey);

        return cacheToken;
    }

    #region ActionTyppeFilter

    /// <summary>
    ///     判断元数据中是否包含指定类型
    /// </summary>
    /// <typeparam name="TMetadata"></typeparam>
    /// <param name="endpoint"></param>
    /// <returns></returns>
    private static bool HasMetadata<TMetadata>(this Endpoint endpoint)
    {
        return endpoint.Metadata.OfType<TMetadata>().Any();
    }

    #endregion

    #region ActionNames

    /// <summary>
    ///     Api控制器操作名
    /// </summary>
    /// <param name="endpoint"></param>
    /// <returns></returns>
    private static string ApiActionName(this RouteEndpoint endpoint)
    {
        var actionParts = endpoint
            .RoutePattern
            .Defaults
            .Select(item => item.Value as string);

        return string.Join(".", actionParts);
    }

    /// <summary>
    ///     路由匹配模式操作名
    /// </summary>
    /// <param name="endpoint"></param>
    /// <returns></returns>
    private static string RoutePatternActionName(this RouteEndpoint endpoint)
    {
        var actionParts = endpoint.RoutePattern
            .PathSegments
            .SelectMany(item => item.Parts)
            .Select(item =>
            {
                return item switch
                {
                    RoutePatternLiteralPart literalPart => literalPart.Content,
                    RoutePatternParameterPart parameterPart => parameterPart.Name,
                    RoutePatternSeparatorPart separatorPart => separatorPart.Content,
                    _ => string.Empty
                };
            });

        return string.Join(".", actionParts);
    }

    #endregion
}