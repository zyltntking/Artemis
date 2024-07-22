using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Artemis.Extensions.Identity;

/// <summary>
///     授权扩展
/// </summary>
public static class AuthorizationExtensions
{
    /// <summary>
    ///     配置授权服务
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder ConfigureAuthorization(
        this IHostApplicationBuilder builder,
        ArtemisIdentityOptions? config = null)
    {
        return builder.ConfigureAuthorization<AuthorizationMiddlewareResultHandler>(config);
    }

    /// <summary>
    ///     配置授权服务
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    private static IHostApplicationBuilder ConfigureAuthorization<TAuthorizationMiddlewareResultHandler>(
        this IHostApplicationBuilder builder,
        ArtemisIdentityOptions? config = null)
        where TAuthorizationMiddlewareResultHandler : class, IAuthorizationMiddlewareResultHandler
    {
        config ??= new ArtemisIdentityOptions();

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(
                AuthorizePolicy.Anonymous,
                policy => { policy.Requirements.Add(new AnonymousRequirement()); });

            options.AddPolicy(
                AuthorizePolicy.Token,
                policy => { policy.Requirements.Add(new TokenOnlyRequirement()); });

            options.AddPolicy(
                AuthorizePolicy.Admin,
                policy => { policy.Requirements.Add(new RolesRequirement(AuthorizePolicy.Admin)); });

            options.AddPolicy(
                AuthorizePolicy.ActionName,
                policy => { policy.Requirements.Add(new ActionNameClaimRequirement()); });

            options.AddPolicy(
                AuthorizePolicy.RoutePath,
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

        builder.Services.Configure<ArtemisIdentityOptions>(options =>
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