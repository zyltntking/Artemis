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
    /// <param name="internalAuthorizationOptions"></param>
    /// <returns></returns>
    public static IServiceCollection AddArtemisAuthorization(
        this IServiceCollection serviceCollection,
        ArtemisAuthorizationOptions internalAuthorizationOptions)
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

            if (internalAuthorizationOptions.EnableAdvancedPolicy)
            {
                if (internalAuthorizationOptions is { RolesBasedPolicyOptions: not null } &&
                    internalAuthorizationOptions.RolesBasedPolicyOptions.Any())
                    foreach (var rolesBasedPolicyOption in internalAuthorizationOptions.RolesBasedPolicyOptions)
                        options.AddPolicy(rolesBasedPolicyOption.Name,
                            policy => { policy.Requirements.Add(new RolesRequirement(rolesBasedPolicyOption.Roles)); });

                if (internalAuthorizationOptions is { ClaimsBasedPolicyOptions: not null } &&
                    internalAuthorizationOptions.ClaimsBasedPolicyOptions.Any())
                    foreach (var claimsBasedPolicyOption in internalAuthorizationOptions.ClaimsBasedPolicyOptions)
                        options.AddPolicy(claimsBasedPolicyOption.Name,
                            policy =>
                            {
                                policy.Requirements.Add(new ClaimsRequirement(claimsBasedPolicyOption.Claims));
                            });
            }
        }).AddSingleton<IAuthorizationMiddlewareResultHandler, AuthorizationResultTransformer>();

        serviceCollection.AddSingleton<IAuthorizationHandler, ArtemisIdentityHandler>();

        serviceCollection.AddHttpContextAccessor();

        serviceCollection.Configure<InternalAuthorizationOptions>(options =>
        {
            options.EnableAdvancedPolicy = internalAuthorizationOptions.EnableAdvancedPolicy;
            options.HeaderTokenKey = internalAuthorizationOptions.HeaderTokenKey;
            options.CacheTokenPrefix = internalAuthorizationOptions.CacheTokenPrefix;
            options.Expire = internalAuthorizationOptions.Expire;
        });

        return serviceCollection;
    }
}