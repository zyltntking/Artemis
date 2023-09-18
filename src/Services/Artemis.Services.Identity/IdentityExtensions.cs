using Artemis.Services.Identity.Data;
using Artemis.Services.Identity.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
    public static IServiceCollection AddIdentityService(this IServiceCollection serviceCollection,
        IdentityServiceOptions serviceOptions, bool isDevelopment)
    {
        serviceCollection.AddDbContextPool<ArtemisIdentityContext>(options =>
            {
                options.EnableServiceProviderCaching()
                    .EnableDetailedErrors(isDevelopment)
                    .EnableSensitiveDataLogging(isDevelopment)
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .UseNpgsql(serviceOptions.Connection, npgsqlOption =>
                    {
                        npgsqlOption.MigrationsHistoryTable("ArtemisIdentityHistory", "identity");

                        npgsqlOption.MigrationsAssembly(serviceOptions.AssemblyName);

                        npgsqlOption.EnableRetryOnFailure(3);
                    }) /*.UseLazyLoadingProxies()*/;
            })
            .AddIdentity<ArtemisUser, ArtemisRole>()
            .AddEntityFrameworkStores<ArtemisIdentityContext>()
            .AddDefaultUI()
            .AddDefaultTokenProviders();

        serviceCollection.TryAddScoped<IArtemisUserStore, ArtemisUserStore>();
        serviceCollection.TryAddScoped<IArtemisUserClaimStore, ArtemisUserClaimStore>();
        serviceCollection.TryAddScoped<IArtemisUserLoginStore, ArtemisUserLoginStore>();
        serviceCollection.TryAddScoped<IArtemisUserTokenStore, ArtemisUserTokenStore>();
        serviceCollection.TryAddScoped<IArtemisRoleStore, ArtemisRoleStore>();
        serviceCollection.TryAddScoped<IArtemisRoleClaimStore, ArtemisRoleClaimStore>();
        serviceCollection.TryAddScoped<IArtemisUserRoleStore, ArtemisUserRoleStore>();

        serviceCollection.TryAddScoped<IIdentityManager, IdentityManager>();

        serviceCollection.TryAddScoped<IIdentityService, IdentityService>();

        if (isDevelopment) serviceCollection.AddDatabaseDeveloperPageExceptionFilter();

        if (serviceOptions.IdentityOptionsAction != null)
            serviceCollection.Configure(serviceOptions.IdentityOptionsAction);

        serviceCollection.ConfigureApplicationCookie(options =>
        {
            // Cookie settings
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

            options.LoginPath = "/Identity/Account/Login";
            options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            options.SlidingExpiration = true;
        });

        return serviceCollection;
    }
}