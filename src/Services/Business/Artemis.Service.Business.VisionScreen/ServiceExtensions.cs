using Artemis.Data.Core;
using Artemis.Service.Business.VisionScreen.Managers;
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
    ///     添加原始数据服务
    /// </summary>
    /// <param name="services"></param>
    public static IServiceCollection AddRawDataServices(this IServiceCollection services)
    {
        services.TryAddScoped<IArtemisOptometerStore, ArtemisOptometerStore>();
        services.TryAddScoped<IArtemisVisualChartStore, ArtemisVisualChartStore>();

        services.TryAddScoped<IArtemisRawDataManager, ArtemisRawDataManager>();

        return services;
    }

    /// <summary>
    ///     添加原始数据服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="enableProxy"></param>
    public static IServiceCollection AddRawDataServices<THandlerProxy>(this IServiceCollection services,
        bool enableProxy = true)
        where THandlerProxy : class, IHandlerProxy
    {
        AddRawDataServices(services);

        if (enableProxy) services.AddScoped<IHandlerProxy, THandlerProxy>();

        return services;
    }
}