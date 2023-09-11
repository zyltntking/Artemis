using Artemis.App.Logic.IdentityLogic.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Artemis.App.Logic.IdentityLogic;

/// <summary>
/// 认证扩展
/// </summary>
public static class IdentityExtensions
{
    /// <summary>
    /// 添加认证服务
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="logicOptions"></param>
    /// <returns></returns>
    public static IServiceCollection AddIdentityService(this IServiceCollection serviceCollection, IdentityLogicOptions logicOptions)
    {
        serviceCollection.AddDbContext<ArtemisIdentityDbContext>(options =>
            {
                options.UseNpgsql(logicOptions.Connection, npgsqlOption =>
                {
                    npgsqlOption.MigrationsHistoryTable("ArtemisIdentityHistory", "identity");

                    npgsqlOption.MigrationsAssembly(logicOptions.AssemblyName);
                }).UseLazyLoadingProxies();
            }).AddDefaultIdentity<ArtemisIdentityUser>()
            .AddEntityFrameworkStores<ArtemisIdentityDbContext>();

        serviceCollection.AddScoped<IIdentityService, IdentityService>();

        serviceCollection.AddDatabaseDeveloperPageExceptionFilter();

        serviceCollection.Configure<HostOptions>(hostOptions =>
        {
            hostOptions.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
        });

        serviceCollection.Configure<IdentityOptions>(options =>
        {
            // Password settings.
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;

            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User settings.
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = false;
        });

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