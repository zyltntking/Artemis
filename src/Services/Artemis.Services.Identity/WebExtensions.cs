using Artemis.Services.Identity.Data;
using Artemis.Services.Identity.Logic;
using Artemis.Services.Identity.Managers;
using Artemis.Services.Identity.Stores;
using Artemis.Shared.Identity.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Artemis.Services.Identity;

/// <summary>
///     认证扩展
/// </summary>
public static class IdentityExtensions
{
    /// <summary>
    ///     添加认证服务
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="serviceOptions"></param>
    /// <param name="isDevelopment"></param>
    /// <returns></returns>
    public static IServiceCollection AddArtemisIdentityService(
        this IServiceCollection serviceCollection,
        IdentityServiceOptions serviceOptions,
        bool isDevelopment)
    {
        var registerDbAction = serviceOptions.RegisterDbAction;

        serviceCollection.AddDbContextPool<ArtemisIdentityContext>(options =>
            {
                var contextBuilder = options.EnableServiceProviderCaching()
                    .EnableDetailedErrors(isDevelopment)
                    .EnableSensitiveDataLogging(isDevelopment)
                    .LogTo(Console.WriteLine, LogLevel.Information);
                registerDbAction(contextBuilder);
            })
            .AddIdentity<ArtemisUser, ArtemisRole>()
            .AddEntityFrameworkStores<ArtemisIdentityContext>()
            .AddDefaultTokenProviders();

        serviceCollection.TryAddScoped<IArtemisClaimStore, ArtemisClaimStore>();
        serviceCollection.TryAddScoped<IArtemisUserStore, ArtemisUserStore>();
        serviceCollection.TryAddScoped<IArtemisRoleStore, ArtemisRoleStore>();
        serviceCollection.TryAddScoped<IArtemisRoleClaimStore, ArtemisRoleClaimStore>();
        serviceCollection.TryAddScoped<IArtemisUserRoleStore, ArtemisUserRoleStore>();
        serviceCollection.TryAddScoped<IArtemisUserClaimStore, ArtemisUserClaimStore>();
        serviceCollection.TryAddScoped<IArtemisUserLoginStore, ArtemisUserLoginStore>();
        serviceCollection.TryAddScoped<IArtemisUserTokenStore, ArtemisUserTokenStore>();

        serviceCollection.TryAddScoped<IClaimManager, ClaimManager>();
        serviceCollection.TryAddScoped<IUserManager, UserManager>();
        serviceCollection.TryAddScoped<IRoleManager, RoleManager>();
        serviceCollection.TryAddScoped<IAccountManager, AccountManager>();

        if (isDevelopment)
            serviceCollection.AddDatabaseDeveloperPageExceptionFilter();

        if (serviceOptions.IdentityOptionsAction is not null)
            serviceCollection.Configure(serviceOptions.IdentityOptionsAction);

        return serviceCollection;
    }

    /// <summary>
    ///    添加认证服务
    /// </summary>
    /// <typeparam name="TUserService"></typeparam>
    /// <typeparam name="TRoleService"></typeparam>
    /// <typeparam name="TAccountService"></typeparam>
    /// <param name="application"></param>
    public static void MapArtemisIdentityGrpcServices<TUserService, TRoleService, TAccountService>(this WebApplication application)
        where TUserService : class, IUserService
        where TRoleService : class, IRoleService
        where TAccountService : class, IAccountService
    {

        application.MapGrpcService<TUserService>();
        application.MapGrpcService<TRoleService>();
        application.MapGrpcService<TAccountService>();
    }

    /// <summary>
    /// 添加默认认证服务
    /// </summary>
    /// <param name="application">Web应用创建器</param>
    public static void MapDefaultArtemisIdentityGrpcServices(this WebApplication application)
    {
        application.MapArtemisIdentityGrpcServices<
            UserService, 
            RoleService, 
            AccountService>();
    }
}