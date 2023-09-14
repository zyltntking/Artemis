using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Artemis.Extensions.Web.Middleware;

/// <summary>
///     Artemis中间件扩展
/// </summary>
public static class ArtemisMiddlewareExtension
{
    /// <summary>
    ///     添加Artemis中间件组
    /// </summary>
    /// <param name="services"></param>
    /// <param name="optionAction"></param>
    /// <returns></returns>
    public static IServiceCollection AddArtemisMiddleWares(this IServiceCollection services,
        Action<ArtemisMiddlewareOptions> optionAction)
    {
        services.Configure(optionAction);

        services.AddScoped<ServiceDomainMiddleware>();
        services.AddScoped<ExceptionResultMiddleware>();

        return services;
    }

    /// <summary>
    ///     使用Artemis中间件组
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseArtemisMiddleWares(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<ServiceDomainMiddleware>();
        builder.UseMiddleware<ExceptionResultMiddleware>();

        return builder;
    }
}