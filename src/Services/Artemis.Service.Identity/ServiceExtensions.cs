using Artemis.Data.Core;
using Artemis.Service.Identity.Managers;
using Artemis.Service.Identity.Stores;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Artemis.Service.Identity;

/// <summary>
///     服务扩展
/// </summary>
public static class ServiceExtensions
{
    /// <summary>
    /// 添加认证服务
    /// </summary>
    /// <param name="services"></param>
    public static IServiceCollection AddIdentityServices(this IServiceCollection services) 
    {
        services.TryAddScoped<IIdentityClaimStore, IdentityClaimStore>();
        services.TryAddScoped<IIdentityUserStore, IdentityUserStore>();
        services.TryAddScoped<IIdentityRoleStore, IdentityRoleStore>();
        services.TryAddScoped<IIdentityRoleClaimStore, IdentityRoleClaimStore>();
        services.TryAddScoped<IIdentityUserRoleStore, IdentityUserRoleStore>();
        services.TryAddScoped<IIdentityUserClaimStore, IdentityUserClaimStore>();
        services.TryAddScoped<IIdentityUserProfileStore, IdentityUserProfileStore>();
        services.TryAddScoped<IIdentityUserLoginStore, IdentityUserLoginStore>();
        services.TryAddScoped<IIdentityUserTokenStore, IdentityUserTokenStore>();

        services.TryAddScoped<IIdentityUserManager, IdentityUserManager>();
        services.TryAddScoped<IIdentityRoleManager, IdentityRoleManager>();
        services.TryAddScoped<IIdentityAccountManager, IdentityAccountManager>();
        services.TryAddScoped<IIdentityResourceManager, IdentityResourceManager>();

        return services;
    }

    /// <summary>
    ///     添加认证服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="enableProxy"></param>
    public static IServiceCollection AddIdentityServices<THandlerProxy>(this IServiceCollection services, bool enableProxy = true) 
        where THandlerProxy : class, IHandlerProxy
    {
        services.AddIdentityServices();

        if (enableProxy)
        {
            services.AddScoped<IHandlerProxy, THandlerProxy>();
        }

        return services;
    }
}