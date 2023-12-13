using Artemis.Data.Core;
using Artemis.Shared.Identity.Transfer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Artemis.Extensions.Web.Identity;

/// <summary>
///     Artemis凭据处理器
/// </summary>
public class ArtemisIdentityHandler : AuthorizationHandler<IArtemisIdentityRequirement>
{
    /// <summary>
    ///     构造
    /// </summary>
    /// <param name="httpContextAccessor">HttpContext访问器依赖</param>
    /// <param name="cache">缓存依赖</param>
    /// <param name="logger">日志依赖</param>
    public ArtemisIdentityHandler(
        IHttpContextAccessor httpContextAccessor, 
        IDistributedCache cache,
        ILogger<ArtemisIdentityHandler> logger)
    {
        HttpContextAccessor = httpContextAccessor;
        Cache = cache;
        Logger = logger;
    }

    /// <summary>
    ///     HttpContext访问器
    /// </summary>
    private IHttpContextAccessor HttpContextAccessor { get; }

    /// <summary>
    ///     缓存访问器
    /// </summary>
    private IDistributedCache Cache { get; }
    
    /// <summary>
    /// 日志访问器
    /// </summary>
    private ILogger Logger { get; }

    #region Overrides of AuthorizationHandler<IdentityRequirement>

    /// <summary>
    ///     Makes a decision if authorization is allowed based on a specific requirement.
    /// </summary>
    /// <param name="context">The authorization context.</param>
    /// <param name="requirement">The requirement to evaluate.</param>
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        IArtemisIdentityRequirement requirement)
    {
        if (requirement is AnonymousRequirement)
        {
            context.Succeed(requirement);

            return Task.CompletedTask;
        }

        var failMessage = "未授权";

        if (requirement is TokenRequirement tokenRequirement)
        {
            var headerToken = FetchHeaderToken(tokenRequirement.HeaderTokenKey);

            if (!string.IsNullOrWhiteSpace(headerToken))
            {
                var cacheToken = FetchCacheToken($"{tokenRequirement.CacheTokenPrefix}:{headerToken}");

                if (!string.IsNullOrWhiteSpace(cacheToken))
                {
                    var tokenDocument = cacheToken.Deserialize<TokenDocument>();

                    if (requirement is RolesRequirement rolesRequirement)
                    {
                        if (tokenDocument is { Roles: not null } && tokenDocument.Roles.Any())
                        {
                            var roles = tokenDocument.Roles.Select(role => role.Name);

                            var roleMatch = roles.Any(
                                role => rolesRequirement
                                    .Roles
                                    .Contains(role.StringNormalize())
                                );

                            if (roleMatch)
                            {
                                context.Succeed(requirement);

                                return Task.CompletedTask;
                            }

                            failMessage = "无有效角色";
                        }
                        else
                        {
                            failMessage = "用户未分配角色";
                        }
                    }

                    if (requirement is ClaimRequirement claimRequirement)
                    {
                        if (tokenDocument is { UserClaims: not null, RoleClaims: not null} 
                            && (tokenDocument.UserClaims.Any() || tokenDocument.RoleClaims.Any()) )
                        {
                            var claims = tokenDocument
                                .UserClaims
                                .Select(claim => claim.CheckStamp)
                                .Union(tokenDocument.RoleClaims.Select(claim => claim.CheckStamp));

                            var claimMatch = claims.Any(
                                claim => claimRequirement
                                    .Claims
                                    .Select(pair => Hash.Md5Hash($"{pair.Key}:{pair.Value}"))
                                    .Contains(claim));

                            if (claimMatch)
                            {
                                context.Succeed(requirement);

                                return Task.CompletedTask;
                            }

                            failMessage = "无有效凭据";

                        }
                        else
                        {
                            failMessage = "用户及其角色未分配凭据";
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

        Logger.LogWarning(failMessage);

        context.Fail(new AuthorizationFailureReason(this, failMessage));

        return Task.CompletedTask;
    }

    #endregion

    /// <summary>
    ///     获取Token
    /// </summary>
    /// <param name="headerKey">header 键</param>
    /// <returns></returns>
    private string? FetchHeaderToken(string headerKey)
    {
        var headers = HttpContextAccessor.HttpContext?.Request.Headers;

        if (headers != null && headers.TryGetValue(headerKey, out var token)) return token;

        return null;
    }

    /// <summary>
    ///     获取缓存Token
    /// </summary>
    /// <param name="cacheTokenKey">缓存token键</param>
    /// <returns></returns>
    private string? FetchCacheToken(string cacheTokenKey)
    {
        var cacheToken = Cache.GetString(cacheTokenKey);

        return cacheToken;
    }
}