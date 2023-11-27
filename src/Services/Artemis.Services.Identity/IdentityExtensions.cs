﻿using Artemis.Data.Store;
using Artemis.Services.Identity.Data;
using Artemis.Services.Identity.Managers;
using Artemis.Services.Identity.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

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
    public static IServiceCollection AddIdentityService(
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

        if (serviceOptions.RedisCacheConnection is not null && serviceOptions.RedisCacheConnection != string.Empty)
        {
            serviceCollection.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = serviceOptions.RedisCacheConnection;
                options.InstanceName = "Artemis:Identity:";
            });

            serviceCollection.TryAddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(serviceOptions.RedisCacheConnection));

            serviceCollection.Configure<ArtemisStoreOptions>(option =>
            {
                option.CachedManager = true;
                option.CachedStore = true;
            });
        }

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
}