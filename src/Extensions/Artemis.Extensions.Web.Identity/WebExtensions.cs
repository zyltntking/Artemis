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
    /// <param name="authorizationOptions"></param>
    /// <returns></returns>
    public static IServiceCollection AddArtemisAuthorization(
        this IServiceCollection serviceCollection,
        ArtemisAuthorizationOptions authorizationOptions)
    {
        serviceCollection.AddAuthorization(options =>
        {
            options.AddPolicy(Constants.Anonymous, policy => { policy.Requirements.Add(new AnonymousRequirement()); });

            options.AddPolicy(Constants.Token, policy => { policy.Requirements.Add(new TokenOnlyRequirement()); });

            options.AddPolicy(Constants.Admin, policy =>
            {
                policy.Requirements.Add(new RolesRequirement(new[]
                {
                    Constants.Admin
                }));
            });

            options.AddPolicy(Constants.ActionName,
                policy => { policy.Requirements.Add(new ActionNameClaimRequirement()); });

            options.AddPolicy(Constants.RoutePath,
                policy => { policy.Requirements.Add(new RoutePathClaimRequirement()); });

            if (authorizationOptions.EnableAdvancedPolicy)
            {
                if (authorizationOptions is { RolesBasedPolicyOptions: not null } &&
                    authorizationOptions.RolesBasedPolicyOptions.Any())
                    foreach (var rolesBasedPolicyOption in authorizationOptions.RolesBasedPolicyOptions)
                        options.AddPolicy(rolesBasedPolicyOption.Name,
                            policy => { policy.Requirements.Add(new RolesRequirement(rolesBasedPolicyOption.Roles)); });

                if (authorizationOptions is { ClaimsBasedPolicyOptions: not null } &&
                    authorizationOptions.ClaimsBasedPolicyOptions.Any())
                    foreach (var claimsBasedPolicyOption in authorizationOptions.ClaimsBasedPolicyOptions)
                        options.AddPolicy(claimsBasedPolicyOption.Name,
                            policy =>
                            {
                                policy.Requirements.Add(new ClaimsRequirement(claimsBasedPolicyOption.Claims));
                            });
            }
        });

        serviceCollection.AddSingleton<IAuthorizationHandler, ArtemisIdentityHandler>();

        serviceCollection.Configure<InternalAuthorizationOptions>(options =>
        {
            options.EnableAdvancedPolicy = authorizationOptions.EnableAdvancedPolicy;
            options.HeaderTokenKey = authorizationOptions.HeaderTokenKey;
            options.CacheTokenPrefix = authorizationOptions.CacheTokenPrefix;
        });

        return serviceCollection;
    }
}