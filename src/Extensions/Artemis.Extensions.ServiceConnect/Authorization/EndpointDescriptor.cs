using System.ComponentModel;
using Grpc.AspNetCore.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;

namespace Artemis.Extensions.ServiceConnect.Authorization;

/// <summary>
///     端描述器
/// </summary>
internal static class EndpointDescriptor
{
    /// <summary>
    ///     查询操作名称
    /// </summary>
    /// <param name="endpoint"></param>
    /// <returns></returns>
    public static string? FetchActionName(this RouteEndpoint endpoint)
    {
        if (endpoint.HasMetadata<PageModelAttribute>()) return endpoint.RoutePatternActionName();

        if (endpoint.HasMetadata<ApiControllerAttribute>()) return endpoint.ApiActionName();

        if (endpoint.HasMetadata<GrpcMethodMetadata>()) return endpoint.RoutePatternActionName();

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
    ///     查询端描述
    /// </summary>
    /// <param name="endpoint"></param>
    /// <returns></returns>
    public static string? FetchDescription(this RouteEndpoint endpoint)
    {
        var descriptionMetadata = endpoint.FetchMetadata<DescriptionAttribute>();

        return descriptionMetadata?.Description;
    }

    #region MatadataTypeFilter

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

    /// <summary>
    ///     查询指定类型的元数据
    /// </summary>
    /// <typeparam name="TMetadata"></typeparam>
    /// <param name="endpoint"></param>
    /// <returns></returns>
    private static TMetadata? FetchMetadata<TMetadata>(this Endpoint endpoint)
    {
        return endpoint.Metadata.OfType<TMetadata>().FirstOrDefault();
    }

    #endregion

    #region ActionNamePicker

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