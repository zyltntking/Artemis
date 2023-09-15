using Artemis.Data.Store;
using Artemis.Services.Identity.Data;
using Artemis.Services.Identity.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
        serviceCollection.AddDbContext<ArtemisIdentityContext>(options =>
            {
                options.UseNpgsql(serviceOptions.Connection, npgsqlOption =>
                {
                    npgsqlOption.MigrationsHistoryTable("ArtemisIdentityHistory", "identity");

                    npgsqlOption.MigrationsAssembly(serviceOptions.AssemblyName);
                }).UseLazyLoadingProxies();
            })
            .AddDefaultIdentity<ArtemisUser>()
            .AddEntityFrameworkStores<ArtemisIdentityContext>();

        serviceCollection.AddScoped<IArtemisUserStore, ArtemisUserStore>();
        serviceCollection.AddScoped<IArtemisUserClaimStore, ArtemisUserClaimStore>();
        serviceCollection.AddScoped<IArtemisUserLoginStore, ArtemisUserLoginStore>();
        serviceCollection.AddScoped<IArtemisUserTokenStore, ArtemisUserTokenStore>();
        serviceCollection.AddScoped<IArtemisRoleStore, ArtemisRoleStore>();
        serviceCollection.AddScoped<IArtemisRoleClaimStore, ArtemisRoleClaimStore>();
        serviceCollection.AddScoped<IArtemisUserRoleStore, ArtemisUserRoleStore>();

        serviceCollection.AddScoped<IArtemisManager, ArtemisManager>();

        serviceCollection.AddScoped<IIdentityService, IdentityService>();

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