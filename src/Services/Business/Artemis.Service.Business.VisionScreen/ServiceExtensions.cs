using Artemis.Data.Core;
using Artemis.Service.Business.VisionScreen.Stores;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Artemis.Service.Business.VisionScreen;

/// <summary>
///     服务扩展
/// </summary>
public static class ServiceExtensions
{
    /// <summary>
    ///     添加业务服务
    /// </summary>
    /// <param name="services"></param>
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.TryAddScoped<IArtemisVisionScreenRecordStore, ArtemisVisionScreenRecordStore>();
        services.TryAddScoped<IArtemisOptometerStore, ArtemisOptometerStore>();
        services.TryAddScoped<IArtemisVisualChartStore, ArtemisVisualChartStore>();
        services.TryAddScoped<IArtemisStudentRelationBindingStore, ArtemisStudentRelationBindingStore>();
        services.TryAddScoped<IArtemisRecordFeedbackStore, ArtemisRecordFeedbackStore>();

        return services;
    }

    /// <summary>
    ///     添加业务服务
    /// </summary>
    /// <typeparam name="THandlerProxy">操作者代理</typeparam>
    /// <param name="services"></param>
    /// <param name="enableHandlerProxy"></param>
    /// <returns></returns>
    public static IServiceCollection AddBusinessServices<THandlerProxy>(
        this IServiceCollection services,
        bool enableHandlerProxy = true)
        where THandlerProxy : class, IHandlerProxy
    {
        services.AddBusinessServices();

        if (enableHandlerProxy)
            services.TryAddScoped<IHandlerProxy, THandlerProxy>();

        return services;
    }

    /// <summary>
    ///     添加业务服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="enableHandlerProxy"></param>
    /// <param name="enableCacheProxy"></param>
    public static IServiceCollection AddBusinessServices<THandlerProxy, TCacheProxy>(
        this IServiceCollection services,
        bool enableHandlerProxy = true,
        bool enableCacheProxy = true)
        where THandlerProxy : class, IHandlerProxy
        where TCacheProxy : class, ICacheProxy
    {
        services.AddBusinessServices<THandlerProxy>(enableHandlerProxy);

        if (enableCacheProxy)
            services.TryAddScoped<ICacheProxy, TCacheProxy>();

        return services;
    }
}