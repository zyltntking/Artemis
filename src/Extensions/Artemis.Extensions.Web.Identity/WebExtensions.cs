using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Artemis.Extensions.Web.Identity;

/// <summary>
///     认证核心扩展
/// </summary>
public static class WebExtensions
{
    /// <summary>
    ///     添加Artemis认证鉴权服务
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="identityOptions"></param>
    /// <returns></returns>
    public static IServiceCollection AddArtemisAuthorization(
        this IServiceCollection serviceCollection,
        ArtemisIdentityOptions identityOptions)
    {
        return serviceCollection.AddArtemisAuthorization<AuthorizationResultTransformer>(identityOptions);
    }

    /// <summary>
    ///     添加Artemis认证鉴权服务
    /// </summary>
    /// <typeparam name="TAuthorizationMiddlewareResultHandler">认证结果处理程序</typeparam>
    /// <param name="serviceCollection"></param>
    /// <param name="identityOptions"></param>
    /// <returns></returns>
    public static IServiceCollection AddArtemisAuthorization<TAuthorizationMiddlewareResultHandler>(
        this IServiceCollection serviceCollection,
        ArtemisIdentityOptions identityOptions)
        where TAuthorizationMiddlewareResultHandler : class, IAuthorizationMiddlewareResultHandler
    {
        serviceCollection.AddAuthorization(options =>
        {
            options.AddPolicy(IdentityPolicy.Anonymous,
                policy => { policy.Requirements.Add(new AnonymousRequirement()); });

            options.AddPolicy(IdentityPolicy.Token, policy => { policy.Requirements.Add(new TokenOnlyRequirement()); });

            options.AddPolicy(IdentityPolicy.Admin, policy =>
            {
                policy.Requirements.Add(new RolesRequirement(new[]
                {
                    IdentityPolicy.Admin
                }));
            });

            options.AddPolicy(IdentityPolicy.ActionName,
                policy => { policy.Requirements.Add(new ActionNameClaimRequirement()); });

            options.AddPolicy(IdentityPolicy.RoutePath,
                policy => { policy.Requirements.Add(new RoutePathClaimRequirement()); });

            if (identityOptions.EnableAdvancedPolicy)
            {
                if (identityOptions is { RolesBasedPolicyOptions: not null } &&
                    identityOptions.RolesBasedPolicyOptions.Any())
                    foreach (var rolesBasedPolicyOption in identityOptions.RolesBasedPolicyOptions)
                        options.AddPolicy(rolesBasedPolicyOption.Name,
                            policy => { policy.Requirements.Add(new RolesRequirement(rolesBasedPolicyOption.Roles)); });

                if (identityOptions is { ClaimsBasedPolicyOptions: not null } &&
                    identityOptions.ClaimsBasedPolicyOptions.Any())
                    foreach (var claimsBasedPolicyOption in identityOptions.ClaimsBasedPolicyOptions)
                        options.AddPolicy(claimsBasedPolicyOption.Name,
                            policy =>
                            {
                                policy.Requirements.Add(new ClaimsRequirement(claimsBasedPolicyOption.Claims));
                            });
            }
        });

        serviceCollection.AddSingleton<IAuthorizationMiddlewareResultHandler, TAuthorizationMiddlewareResultHandler>();

        serviceCollection.AddSingleton<IAuthorizationHandler, ArtemisIdentityHandler>();

        serviceCollection.AddHttpContextAccessor();

        serviceCollection.Configure<SharedIdentityOptions>(options =>
        {
            options.EnableAdvancedPolicy = identityOptions.EnableAdvancedPolicy;
            options.HeaderIdentityTokenKey = identityOptions.HeaderIdentityTokenKey;
            options.CacheIdentityTokenPrefix = identityOptions.CacheIdentityTokenPrefix;
            options.UserMapTokenPrefix = identityOptions.UserMapTokenPrefix;
            options.CacheIdentityTokenExpire = identityOptions.CacheIdentityTokenExpire;
            options.EnableMultiEnd = identityOptions.EnableMultiEnd;
        });

        return serviceCollection;
    }
}