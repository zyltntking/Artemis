using Artemis.Data.Core;
using Artemis.Data.Shared.Transfer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Distributed;
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
    /// <param name="cache">缓存依赖</param>
    /// <param name="httpContextAccessor"></param>
    /// <param name="options">配置</param>
    /// <param name="logger">日志依赖</param>
    public ArtemisAuthorizationHandler(
        IDistributedCache cache,
        IHttpContextAccessor httpContextAccessor,
        IOptions<ArtemisAuthorizationConfig> options,
        ILogger<ArtemisAuthorizationHandler> logger)
    {
        Cache = cache;
        HttpContextAccessor = httpContextAccessor;
        Config = options.Value;
        Logger = logger;
    }

    /// <summary>
    ///     缓存访问器
    /// </summary>
    private IDistributedCache Cache { get; }

    /// <summary>
    ///     配置访问器
    /// </summary>
    private ArtemisAuthorizationConfig Config { get; }

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
        IArtemisAuthorizationRequirement requirement)
    {
        var httpContext = HttpContextAccessor.HttpContext;

        if (requirement is AnonymousRequirement)
        {
            context.Succeed(requirement);

            return Task.CompletedTask;
        }

        var message = "未授权";

        if (requirement is TokenRequirement)
        {
            if (httpContext is not null)
            {
                var tokenSymbol = httpContext.FetchTokenSymbol(Config.RequestHeaderTokenKey);

                if (!string.IsNullOrWhiteSpace(tokenSymbol))
                {
                    var cacheTokenKey = TokenKeyGenerator.CacheTokenKey(Config.CacheTokenPrefix, tokenSymbol);

                    var document = Cache.FetchTokenDocument<TokenDocument>(cacheTokenKey);

                    if (document is not null)
                    {
                        httpContext.CacheTokenDocument(Config.ContextItemTokenKey, document);

                        var continueHandler = true;

                        // 处理多终端登录逻辑
                        if (!Config.EnableMultiEnd)
                        {
                            var userMapTokenKey = TokenKeyGenerator.CacheUserMapTokenKey(
                                Config.CacheUserMapTokenPrefix,
                                document.EndType,
                                document.UserId);

                            var userMapToken = Cache.FetchUserMapTokenSymbol(userMapTokenKey);

                            if (userMapToken != tokenSymbol)
                                continueHandler = false;
                        }

                        if (continueHandler)
                        {
                            if (requirement is TokenOnlyRequirement)
                            {
                                context.Succeed(requirement);

                                return Task.CompletedTask;
                            }

                            if (HandleRolesRequirement(requirement, document, ref message))
                            {
                                context.Succeed(requirement);

                                return Task.CompletedTask;
                            }


                            if (HandleClaimRequirement(requirement, document, ref message))
                            {
                                context.Succeed(requirement);

                                return Task.CompletedTask;
                            }


                            if (HandleActionNameClaimRequirement(requirement, httpContext, document, ref message))
                            {
                                context.Succeed(requirement);

                                return Task.CompletedTask;
                            }

                            if (HandleRoutePathClaimRequirement(requirement, httpContext, document, ref message))
                            {
                                context.Succeed(requirement);

                                return Task.CompletedTask;
                            }
                        }
                        else
                        {
                            message = "该用户已在其他终端登录";
                        }
                    }
                    else
                    {
                        message = "无效令牌";
                    }
                }
                else
                {
                    message = "请求未携带令牌";
                }
            }
            else
            {
                message = "无法获取传输的令牌信息";
            }
        }

        Logger.LogWarning(message);

        HttpContextAccessor.HttpContext?.Items.Add(SharedKey.AuthorizationMessage, message);

        context.Fail(new AuthorizationFailureReason(this, message));

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
    private bool HandleRolesRequirement(IArtemisAuthorizationRequirement requirement, TokenDocument? document,
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
    private bool HandleClaimRequirement(IArtemisAuthorizationRequirement requirement, TokenDocument? document,
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
    private bool HandleActionNameClaimRequirement(IArtemisAuthorizationRequirement requirement, HttpContext context,
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
                    var userClaimValues = document
                        .UserClaims
                        .Where(claim => claim.ClaimType == ClaimTypes.ActionName)
                        .Select(claim => claim.ClaimValue);

                    var roleClaimValues = document
                        .RoleClaims
                        .Where(claim => claim.ClaimType == ClaimTypes.ActionName)
                        .Select(claim => claim.ClaimValue);

                    var claimValues = userClaimValues.Union(roleClaimValues);

                    if (claimValues.Contains(actionName)) return true;

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
    private bool HandleRoutePathClaimRequirement(IArtemisAuthorizationRequirement requirement, HttpContext context,
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
                        var userClaimValues = document
                            .UserClaims
                            .Where(claim => claim.ClaimType == ClaimTypes.RoutePath)
                            .Select(claim => claim.ClaimValue);

                        var roleClaimValues = document
                            .RoleClaims
                            .Where(claim => claim.ClaimType == ClaimTypes.RoutePath)
                            .Select(claim => claim.ClaimValue);

                        var claimValues = userClaimValues.Union(roleClaimValues);

                        if (claimValues.Contains(routePath)) return true;

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