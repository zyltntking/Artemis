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
    /// <param name="services"></param>
    /// <param name="enableProxy"></param>
    public static IServiceCollection AddSchoolServices<THandlerProxy>(this IServiceCollection services,
        bool enableProxy = true)
        where THandlerProxy : class, IHandlerProxy
    {
        services.AddSchoolServices();

        if (enableProxy) services.AddScoped<IHandlerProxy, THandlerProxy>();

        return services;
    }
}