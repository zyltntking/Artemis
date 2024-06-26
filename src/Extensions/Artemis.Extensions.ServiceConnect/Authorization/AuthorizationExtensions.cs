﻿using Artemis.Data.Shared.Transfer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Artemis.Extensions.ServiceConnect.Authorization;

/// <summary>
///     授权扩展
/// </summary>
internal static class AuthorizationExtensions
{
    /// <summary>
    ///     配置授权服务
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder ConfigureAuthorization(
        this IHostApplicationBuilder builder,
        ArtemisAuthorizationConfig? config = null)
    {
        return builder.ConfigureAuthorization<AuthorizationMiddlewareResultHandler>(config);
    }

    /// <summary>
    ///     配置授权服务
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder ConfigureRpcAuthorization(
        this IHostApplicationBuilder builder,
        ArtemisAuthorizationConfig? config = null)
    {
        return builder.ConfigureAuthorization<RpcAuthorizationMiddlewareResultHandler>(config);
    }

    /// <summary>
    ///     配置授权服务
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    private static IHostApplicationBuilder ConfigureAuthorization<TAuthorizationMiddlewareResultHandler>(
        this IHostApplicationBuilder builder,
        ArtemisAuthorizationConfig? config = null)
        where TAuthorizationMiddlewareResultHandler : class, IAuthorizationMiddlewareResultHandler
    {
        config ??= new ArtemisAuthorizationConfig();

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(IdentityPolicy.Anonymous,
                policy => { policy.Requirements.Add(new AnonymousRequirement()); });

            options.AddPolicy(IdentityPolicy.Token, policy => { policy.Requirements.Add(new TokenOnlyRequirement()); });

            options.AddPolicy(IdentityPolicy.Admin,
                policy => { policy.Requirements.Add(new RolesRequirement(IdentityPolicy.Admin)); });

            options.AddPolicy(IdentityPolicy.ActionName,
                policy => { policy.Requirements.Add(new ActionNameClaimRequirement()); });

            options.AddPolicy(IdentityPolicy.RoutePath,
                policy => { policy.Requirements.Add(new RoutePathClaimRequirement()); });

            if (config.EnableAdvancedPolicy)
            {
                if (config is { RolesBasedPolicyOptions: not null } && config.RolesBasedPolicyOptions.Any())
                    foreach (var rolesBasedPolicyOption in config.RolesBasedPolicyOptions)
                        options.AddPolicy(rolesBasedPolicyOption.Name,
                            policy => { policy.Requirements.Add(new RolesRequirement(rolesBasedPolicyOption.Roles)); });

                if (config is { ClaimsBasedPolicyOptions: not null } && config.ClaimsBasedPolicyOptions.Any())
                    foreach (var claimsBasedPolicyOption in config.ClaimsBasedPolicyOptions)
                        options.AddPolicy(claimsBasedPolicyOption.Name,
                            policy =>
                            {
                                policy.Requirements.Add(new ClaimsRequirement(claimsBasedPolicyOption.Claims));
                            });
            }
        });

        builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, TAuthorizationMiddlewareResultHandler>();

        builder.Services.AddSingleton<IAuthorizationHandler, ArtemisAuthorizationHandler>();

        builder.Services.Configure<ArtemisAuthorizationConfig>(options =>
        {
            options.ContextItemTokenKey = config.ContextItemTokenKey;
            options.RequestHeaderTokenKey = config.RequestHeaderTokenKey;
            options.CacheTokenPrefix = config.CacheTokenPrefix;
            options.CacheUserMapTokenPrefix = config.CacheUserMapTokenPrefix;
            options.CacheTokenExpire = config.CacheTokenExpire;
            options.EnableMultiEnd = config.EnableMultiEnd;
            options.IdentityServiceProvider = config.IdentityServiceProvider;
            options.IdentityServiceTokenNameSuffix = config.IdentityServiceTokenNameSuffix;
            options.EnableAdvancedPolicy = config.EnableAdvancedPolicy;
            options.RolesBasedPolicyOptions = config.RolesBasedPolicyOptions;
            options.ClaimsBasedPolicyOptions = config.ClaimsBasedPolicyOptions;
        });

        return builder;
    }
}