using Artemis.Data.Core;
using Artemis.Shared.Identity;
using Artemis.Shared.Identity.Transfer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Artemis.Extensions.Web.Identity;

/// <summary>
///     Artemis凭据处理器
/// </summary>
public class ArtemisIdentityHandler : AuthorizationHandler<IArtemisIdentityRequirement>
{
    /// <summary>
    ///     构造
    /// </summary>
    /// <param name="cache">缓存依赖</param>
    /// <param name="httpContextAccessor"></param>
    /// <param name="options">配置</param>
    /// <param name="logger">日志依赖</param>
    public ArtemisIdentityHandler(
        IDistributedCache cache,
        IHttpContextAccessor httpContextAccessor,
        IOptions<InternalAuthorizationOptions> options,
        ILogger<ArtemisIdentityHandler> logger)
    {
        Cache = cache;
        HttpContextAccessor = httpContextAccessor;
        Options = options.Value;
        Logger = logger;
    }

    /// <summary>
    ///     缓存访问器
    /// </summary>
    private IDistributedCache Cache { get; }

    /// <summary>
    ///     配置访问器
    /// </summary>
    private InternalAuthorizationOptions Options { get; }

    /// <summary>
    ///     日志访问器
    /// </summary>
    private ILogger Logger { get; }

    /// <summary>
    ///     Http上下文访问器
    /// </summary>
    private IHttpContextAccessor HttpContextAccessor { get; }

    #region Overrides of AuthorizationHandler<IdentityRequirement>

    /// <summary>
    ///     Makes a decision if authorization is allowed based on a specific requirement.
    /// </summary>
    /// <param name="context">The authorization context.</param>
    /// <param name="requirement">The requirement to evaluate.</param>
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        IArtemisIdentityRequirement requirement)
    {
        var httpContext = HttpContextAccessor.HttpContext;

        if (requirement is AnonymousRequirement)
        {
            context.Succeed(requirement);

            return Task.CompletedTask;
        }

        var failMessage = "未授权";

        if (requirement is TokenRequirement)
        {
            if (httpContext is not null)
            {
                var headerToken = httpContext.FetchHeaderToken(Options.HeaderTokenKey);

                if (!string.IsNullOrWhiteSpace(headerToken))
                {
                    var document = Cache.FetchToken($"{Options.CacheTokenPrefix}:{headerToken}");

                    if (document is not null)
                    {
                        var continueHandler = true;

                        // 处理不允许多终端的逻辑
                        if (!Options.EnableMultiEnd)
                        {
                            var userMapToken = Cache.FetchUserMapToken($"{Options.UserMapTokenPrefix}:{document.UserId}");

                            if (userMapToken != headerToken)
                            {
                                continueHandler = false;
                            }
                        }

                        if (continueHandler)
                        {
                            httpContext.CacheToken(document);

                            if (requirement is TokenOnlyRequirement)
                            {
                                context.Succeed(requirement);

                                return Task.CompletedTask;
                            }

                            if (HandleRolesRequirement(requirement, document, ref failMessage))
                            {
                                context.Succeed(requirement);

                                return Task.CompletedTask;
                            }


                            if (HandleClaimRequirement(requirement, document, ref failMessage))
                            {
                                context.Succeed(requirement);

                                return Task.CompletedTask;
                            }


                            if (HandleActionNameClaimRequirement(requirement, httpContext, document, ref failMessage))
                            {
                                context.Succeed(requirement);

                                return Task.CompletedTask;
                            }

                            if (HandleRoutePathClaimRequirement(requirement, httpContext, document, ref failMessage))
                            {
                                context.Succeed(requirement);

                                return Task.CompletedTask;
                            }
                        }
                    }
                    else
                    {
                        failMessage = "无效令牌";
                    }
                }
                else
                {
                    failMessage = "请求未携带令牌";
                }
            }
            else
            {
                failMessage = "无法获取传输的令牌信息";
            }
        }

        Logger.LogWarning(failMessage);

        HttpContextAccessor.HttpContext?.Items.Add(SharedKey.AuthMessage, failMessage);

        context.Fail(new AuthorizationFailureReason(this, failMessage));

        return Task.CompletedTask;
    }

    #endregion

    /// <summary>
    ///     处理角色要求
    /// </summary>
    /// <param name="requirement">要求对象</param>
    /// <param name="document">令牌文档</param>
    /// <param name="message">失败消息</param>
    /// <returns></returns>
    private bool HandleRolesRequirement(IArtemisIdentityRequirement requirement, TokenDocument? document,
        ref string message)
    {
        if (requirement is not RolesRequirement rolesRequirement)
            return false;
        if (document is { Roles: not null } && document.Roles.Any())
        {
            var roles = document.Roles.Select(role => role.Name);

            var roleMatch = roles.Any(
                role => rolesRequirement
                    .Roles
                    .Contains(role.StringNormalize())
            );

            if (roleMatch) return true;

            message = "无有效角色";
        }
        else
        {
            message = "用户未分配角色";
        }

        return false;
    }

    /// <summary>
    ///     处理凭据要求
    /// </summary>
    /// <param name="requirement">要求对象</param>
    /// <param name="document">令牌文档</param>
    /// <param name="message">失败消息</param>
    /// <returns></returns>
    private bool HandleClaimRequirement(IArtemisIdentityRequirement requirement, TokenDocument? document,
        ref string message)
    {
        if (requirement is not ClaimsRequirement claimRequirement)
            return false;
        if (document is { UserClaims: not null, RoleClaims: not null } &&
            (document.UserClaims.Any() || document.RoleClaims.Any()))
        {
            var requireClaimKeys = claimRequirement.Claims.Select(item => item.Key);

            var requireClaimStamps =
                claimRequirement.Claims.Select(Normalize.KeyValuePairStampNormalize);

            var userClaimStamps = document
                .UserClaims
                .Where(claim => requireClaimKeys.Contains(claim.ClaimType))
                .Select(claim => claim.CheckStamp);

            var roleClaimStamps = document
                .RoleClaims
                .Where(claim => requireClaimKeys.Contains(claim.ClaimType))
                .Select(claim => claim.CheckStamp);

            var claimStamps = userClaimStamps.Union(roleClaimStamps);

            var claimMatch = claimStamps.Any(requireClaimStamps.Contains);

            if (claimMatch) return true;

            message = "无有效凭据";
        }
        else
        {
            message = "用户及其角色未分配凭据";
        }

        return false;
    }

    /// <summary>
    ///     处理ActionName凭据要求
    /// </summary>
    /// <param name="requirement">要求对象</param>
    /// <param name="context">http上下文</param>
    /// <param name="document">令牌文档</param>
    /// <param name="message">失败消息</param>
    /// <returns></returns>
    private bool HandleActionNameClaimRequirement(IArtemisIdentityRequirement requirement, HttpContext context,
        TokenDocument? document, ref string message)
    {
        if (requirement is not ActionNameClaimRequirement)
            return false;
        if (context.GetEndpoint() is RouteEndpoint routeEndpoint)
        {
            var actionName = routeEndpoint.FetchActionName();

            if (!string.IsNullOrWhiteSpace(actionName))
            {
                if (document is { UserClaims: not null, RoleClaims: not null } &&
                    (document.UserClaims.Any() || document.RoleClaims.Any()))
                {
                    var userClaims = document
                        .UserClaims
                        .Where(claim => claim.ClaimType == ArtemisClaimTypes.ActionName)
                        .Any(claim => claim.ClaimValue == actionName);

                    var roleClaims = document
                        .RoleClaims
                        .Where(claim => claim.ClaimType == ArtemisClaimTypes.ActionName)
                        .Any(claim => claim.ClaimValue == actionName);

                    if (userClaims || roleClaims) return true;

                    message = "无有效操作名凭据";
                }
                else
                {
                    message = "用户及其角色未分配该操作需要的操作名凭据";
                }
            }
            else
            {
                message = "无法解析操作名称";
            }
        }
        else
        {
            message = "无法解析路由类型";
        }

        return false;
    }

    /// <summary>
    ///     处理RoutePath凭据要求
    /// </summary>
    /// <param name="requirement">要求对象</param>
    /// <param name="context">http上下文</param>
    /// <param name="document">令牌文档</param>
    /// <param name="message">失败消息</param>
    /// <returns></returns>
    private bool HandleRoutePathClaimRequirement(IArtemisIdentityRequirement requirement, HttpContext context,
        TokenDocument? document, ref string message)
    {
        if (requirement is RoutePathClaimRequirement)
        {
            if (context.GetEndpoint() is RouteEndpoint routeEndpoint)
            {
                var routePath = routeEndpoint.FetchRoutePath();

                if (!string.IsNullOrWhiteSpace(routePath))
                {
                    if (document is { UserClaims: not null, RoleClaims: not null } &&
                        (document.UserClaims.Any() || document.RoleClaims.Any()))
                    {
                        var userClaims = document
                            .UserClaims
                            .Where(claim => claim.ClaimType == ArtemisClaimTypes.RoutePath)
                            .Any(claim => claim.ClaimValue == routePath);

                        var roleClaims = document
                            .RoleClaims
                            .Where(claim => claim.ClaimType == ArtemisClaimTypes.RoutePath)
                            .Any(claim => claim.ClaimValue == routePath);

                        if (userClaims || roleClaims) return true;

                        message = "无有效路由路径凭据";
                    }
                    else
                    {
                        message = "用户及其角色未分配该操作需要的路由路径凭据";
                    }
                }
                else
                {
                    message = "无法解析路由路径";
                }
            }
            else
            {
                message = "无法解析路由类型";
            }
        }

        return false;
    }
}