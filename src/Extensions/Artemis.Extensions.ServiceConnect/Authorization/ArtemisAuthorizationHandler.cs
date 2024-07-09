using Artemis.Data.Core;
using Artemis.Data.Shared.Transfer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Artemis.Extensions.ServiceConnect.Authorization;

/// <summary>
///     Artemis凭据处理器
/// </summary>
internal class ArtemisAuthorizationHandler : AuthorizationHandler<IArtemisAuthorizationRequirement>
{
    /// <summary>
    ///     构造
    /// </summary>
    /// <param name="options">配置</param>
    /// <param name="logger">日志依赖</param>
    public ArtemisAuthorizationHandler(
        IOptions<ArtemisAuthorizationOptions> options,
        ILogger<ArtemisAuthorizationHandler> logger)
    {
        Options = options.Value;
        Logger = logger;
    }

    /// <summary>
    ///     配置访问器
    /// </summary>
    private ArtemisAuthorizationOptions Options { get; }

    /// <summary>
    ///     日志访问器
    /// </summary>
    private ILogger Logger { get; }

    #region Overrides of AuthorizationHandler<IdentityRequirement>

    /// <summary>
    ///     Makes a decision if authorization is allowed based on a specific requirement.
    /// </summary>
    /// <param name="context">The authorization context.</param>
    /// <param name="requirement">The requirement to evaluate.</param>
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        IArtemisAuthorizationRequirement requirement)
    {
        if (requirement is AnonymousRequirement)
        {
            context.Succeed(requirement);

            return Task.CompletedTask;
        }

        var message = "未授权";

        if (requirement is TokenRequirement)
        {
            var user = context.User;

            if (user.Identity != null)
            {
                if (user.Identity.IsAuthenticated)
                {
                    if (requirement is TokenOnlyRequirement)
                    {
                        context.Succeed(requirement);

                        return Task.CompletedTask;
                    }

                    if (requirement is RolesRequirement rolesRequirement)
                    {
                        var roles = user.Claims
                            .Where(claim => claim.Type == ClaimTypes.Role)
                            .Select(claim => claim.Value.StringNormalize())
                            .ToList();

                        if (roles.Any())
                        {
                            var roleMatch = roles.Any(role => rolesRequirement
                                .Roles
                                .Contains(role.StringNormalize()));

                            if (roleMatch)
                            {
                                context.Succeed(requirement);

                                return Task.CompletedTask;
                            }

                            message = "该用户无有效角色";
                        }
                    }

                    if (requirement is ClaimsRequirement claimsRequirement)
                    {
                        var requireCheckStamps = claimsRequirement
                            .Claims
                            .Select(claim => claim.KeyValuePairStamp());

                        var claimStamps = user
                            .Claims
                            .Select(claim => Normalize.KeyValuePairStamp(claim.Type, claim.Value));

                        var hitClaims = requireCheckStamps.Intersect(claimStamps);

                        if (hitClaims.Any())
                        {
                            context.Succeed(requirement);

                            return Task.CompletedTask;
                        }

                        message = "该用户无有效凭据";
                    }

                    if (requirement is ActionNameClaimRequirement)
                    {

                        var requireActionName = user
                            .Claims
                            .Where(claim => claim.Type == ClaimTypes.MateActionName)
                            .Select(claim => claim.Value)
                            .FirstOrDefault();

                        var claimActionName = user
                            .Claims
                            .Where(claim => claim.Type == ClaimTypes.MateActionName)
                            .Select(claim => claim.Value)
                            .FirstOrDefault();

                        if (string.Equals(requireActionName, claimActionName, StringComparison.OrdinalIgnoreCase))
                        {
                            context.Succeed(requirement);

                            return Task.CompletedTask;
                        }

                        message = "该用户无有效操作名凭据";
                    }

                    if (requirement is RoutePathClaimRequirement)
                    {
                        var requireRoutePath = user
                            .Claims
                            .Where(claim => claim.Type == ClaimTypes.MateRoutePath)
                            .Select(claim => claim.Value)
                            .FirstOrDefault();

                        var claimRoutePath = user
                            .Claims
                            .Where(claim => claim.Type == ClaimTypes.MateRoutePath)
                            .Select(claim => claim.Value)
                            .FirstOrDefault();

                        if (string.Equals(requireRoutePath, claimRoutePath, StringComparison.OrdinalIgnoreCase))
                        {
                            context.Succeed(requirement);

                            return Task.CompletedTask;
                        }

                        message = "该用户无有效路由路径凭据";
                    }


                }
                else
                {
                    message = "未通过认证，请尝试重新登录";
                }

            }

        }

        Logger.LogWarning(message);

        context.Fail(new AuthorizationFailureReason(this, message));

        return Task.CompletedTask;
    }

    #endregion

}