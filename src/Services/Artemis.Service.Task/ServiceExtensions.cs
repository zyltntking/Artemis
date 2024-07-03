using Artemis.Data.Core;
using Artemis.Service.Task.Managers;
using Artemis.Service.Task.Stores;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Artemis.Service.Task;

/// <summary>
///     服务扩展
/// </summary>
public static class ServiceExtensions
{
    /// <summary>
    ///     添加学校服务
    /// </summary>
    /// <param name="services"></param>
    public static IServiceCollection AddTaskServices(this IServiceCollection services)
    {
        services.TryAddScoped<IArtemisTaskStore, ArtemisTaskStore>();
        services.TryAddScoped<IArtemisAgentStore, ArtemisAgentStore>();
        services.TryAddScoped<IArtemisTaskUnitStore, ArtemisTaskUnitStore>();
        services.TryAddScoped<IArtemisTaskTargetStore, ArtemisTaskTargetStore>();
        services.TryAddScoped<IArtemisTaskAgentStores, ArtemisTaskAgentStores>();

        services.TryAddScoped<IArtemisTaskManager, ArtemisTaskManager>();

        return services;
    }

    /// <summary>
    ///     添加学校服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="enableProxy"></param>
    public static IServiceCollection AddTaskServices<THandlerProxy>(this IServiceCollection services,
        bool enableProxy = true)
        where THandlerProxy : class, IHandlerProxy
    {
        services.AddTaskServices();

        if (enableProxy) services.AddScoped<IHandlerProxy, THandlerProxy>();

        return services;
    }
}