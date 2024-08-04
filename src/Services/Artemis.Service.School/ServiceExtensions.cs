using Artemis.Data.Core;
using Artemis.Service.School.Managers;
using Artemis.Service.School.Stores;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Artemis.Service.School;

/// <summary>
///     服务扩展
/// </summary>
public static class ServiceExtensions
{
    /// <summary>
    ///     添加学校服务
    /// </summary>
    /// <param name="services"></param>
    public static IServiceCollection AddSchoolServices(this IServiceCollection services)
    {
        services.TryAddScoped<IArtemisSchoolStore, ArtemisSchoolStore>();
        services.TryAddScoped<IArtemisClassStore, ArtemisClassStore>();
        services.TryAddScoped<IArtemisTeacherStore, ArtemisTeacherStore>();
        services.TryAddScoped<IArtemisStudentStore, ArtemisStudentStore>();

        services.TryAddScoped<ISchoolManager, SchoolManager>();
        services.TryAddScoped<ISchoolTeacherManager, SchoolTeacherManager>();
        services.TryAddScoped<ISchoolStudentManager, SchoolStudentManager>();

        return services;
    }

    /// <summary>
    ///     添加学校服务
    /// </summary>
    /// <typeparam name="THandlerProxy">操作者代理</typeparam>
    /// <param name="services"></param>
    /// <param name="enableHandlerProxy"></param>
    /// <returns></returns>
    public static IServiceCollection AddSchoolServices<THandlerProxy>(
        this IServiceCollection services,
        bool enableHandlerProxy = true)
        where THandlerProxy : class, IHandlerProxy
    {
        services.AddSchoolServices();

        if (enableHandlerProxy)
            services.TryAddScoped<IHandlerProxy, THandlerProxy>();

        return services;
    }

    /// <summary>
    ///     添加学校服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="enableHandlerProxy"></param>
    /// <param name="enableCacheProxy"></param>
    public static IServiceCollection AddSchoolServices<THandlerProxy, TCacheProxy>(
        this IServiceCollection services,
        bool enableHandlerProxy = true,
        bool enableCacheProxy = true)
        where THandlerProxy : class, IHandlerProxy
        where TCacheProxy : class, ICacheProxy
    {
        services.AddSchoolServices<THandlerProxy>(enableHandlerProxy);

        if (enableCacheProxy)
            services.TryAddScoped<ICacheProxy, TCacheProxy>();

        return services;
    }
}