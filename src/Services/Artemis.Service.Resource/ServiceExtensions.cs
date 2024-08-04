using Artemis.Data.Core;
using Artemis.Service.Resource.Managers;
using Artemis.Service.Resource.Stores;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Artemis.Service.Resource;

/// <summary>
///     服务扩展
/// </summary>
public static class ServiceExtensions
{
    /// <summary>
    ///     配置资源服务
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="path"></param>
    public static void ConfigureResourceService(
        this IHostApplicationBuilder builder,
        string path = "service.Resource.json")
    {
        var resourceServiceConfiguration = builder.Configuration.GetSection("Service:Resource");

        if (Path.Exists(path) && !resourceServiceConfiguration.Exists())
            builder.Configuration.AddJsonFile(path, true, true);

        builder.Services.Configure<ResourceServiceConfig>(resourceServiceConfiguration);
    }


    /// <summary>
    ///     添加资源服务
    /// </summary>
    /// <param name="services"></param>
    public static IServiceCollection AddResourceServices(this IServiceCollection services)
    {
        services.TryAddScoped<IArtemisDataDictionaryStore, ArtemisDataDictionaryStore>();
        services.TryAddScoped<IArtemisDataDictionaryItemStore, ArtemisDataDictionaryItemStore>();
        services.TryAddScoped<IArtemisStandardCatalogStore, ArtemisStandardCatalogStore>();
        services.TryAddScoped<IArtemisStandardItemStore, ArtemisStandardItemStore>();
        services.TryAddScoped<IArtemisDeviceStore, ArtemisDeviceStore>();
        services.TryAddScoped<IArtemisDivisionStore, ArtemisDivisionStore>();
        services.TryAddScoped<IArtemisOrganizationStore, ArtemisOrganizationStore>();

        services.TryAddScoped<IDataDictionaryManager, DataDictionaryManager>();
        services.TryAddScoped<IStandardManager, StandardManager>();
        services.TryAddScoped<IDivisionTreeManager, DivisionTreeManager>();
        services.TryAddScoped<IOrganizationTreeManager, OrganizationTreeManager>();

        return services;
    }

    /// <summary>
    ///     添加资源服务
    /// </summary>
    /// <typeparam name="THandlerProxy">操作者代理</typeparam>
    /// <param name="services"></param>
    /// <param name="enableHandlerProxy"></param>
    /// <returns></returns>
    public static IServiceCollection AddResourceServices<THandlerProxy>(
        this IServiceCollection services,
        bool enableHandlerProxy = true)
        where THandlerProxy : class, IHandlerProxy
    {
        services.AddResourceServices();

        if (enableHandlerProxy)
            services.TryAddScoped<IHandlerProxy, THandlerProxy>();

        return services;
    }

    /// <summary>
    ///     添加资源服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="enableHandlerProxy"></param>
    /// <param name="enableCacheProxy"></param>
    public static IServiceCollection AddResourceServices<THandlerProxy, TCacheProxy>(
        this IServiceCollection services,
        bool enableHandlerProxy = true,
        bool enableCacheProxy = true)
        where THandlerProxy : class, IHandlerProxy
        where TCacheProxy : class, ICacheProxy
    {
        services.AddResourceServices<THandlerProxy>(enableHandlerProxy);

        if (enableCacheProxy)
            services.TryAddScoped<ICacheProxy, TCacheProxy>();

        return services;
    }
}