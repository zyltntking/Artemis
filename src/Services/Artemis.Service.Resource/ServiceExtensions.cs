using Artemis.Data.Core;
using Artemis.Service.Resource.Managers;
using Artemis.Service.Resource.Stores;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Artemis.Service.Resource;

/// <summary>
///     服务扩展
/// </summary>
public static class ServiceExtensions
{
    /// <summary>
    ///     添加资源服务
    /// </summary>
    /// <param name="services"></param>
    public static IServiceCollection AddResourceServices(this IServiceCollection services)
    {
        services.TryAddScoped<IArtemisDataDictionaryStore, ArtemisDataDictionaryStore>();
        services.TryAddScoped<IArtemisDataDictionaryItemStore, ArtemisDataDictionaryItemStore>();
        services.TryAddScoped<IArtemisDeviceStore, ArtemisDeviceStore>();
        services.TryAddScoped<IArtemisDivisionStore, ArtemisDivisionStore>();
        services.TryAddScoped<IArtemisOrganizationStore, ArtemisOrganizationStore>();

        services.TryAddScoped<IDivisionTreeManager, DivisionTreeManager>();
        services.TryAddScoped<IOrganizationTreeManager, OrganizationTreeManager>();


        return services;
    }

    /// <summary>
    ///     添加资源服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="enableProxy"></param>
    public static IServiceCollection AddResourceServices<THandlerProxy>(this IServiceCollection services,
        bool enableProxy = true)
        where THandlerProxy : class, IHandlerProxy
    {
        services.AddResourceServices();

        if (enableProxy) services.AddScoped<IHandlerProxy, THandlerProxy>();

        return services;
    }
}