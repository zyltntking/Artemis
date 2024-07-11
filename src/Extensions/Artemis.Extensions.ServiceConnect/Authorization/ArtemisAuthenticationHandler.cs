using System.Security.Claims;
using System.Text.Encodings.Web;
using Artemis.Data.Core;
using Artemis.Service.Shared.Transfer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
//////
using ClaimTypes = Artemis.Service.Shared.Transfer.ClaimTypes;

namespace Artemis.Extensions.ServiceConnect.Authorization;

/// <summary>
///     认证处理器
/// </summary>
public class ArtemisAuthenticationHandler : AuthenticationHandler<ArtemisAuthenticationOptions>
{
    /// <summary>
    ///     Initializes a new instance of <see cref="T:Microsoft.AspNetCore.Authentication.AuthenticationHandler`1" />.
    /// </summary>
    /// <param name="options">The monitor for the options instance.</param>
    /// <param name="authorizationOptions"></param>
    /// <param name="cache"></param>
    /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILoggerFactory" />.</param>
    /// <param name="encoder">The <see cref="T:System.Text.Encodings.Web.UrlEncoder" />.</param>
    public ArtemisAuthenticationHandler(
        IOptionsMonitor<ArtemisAuthenticationOptions> options,
        IOptions<ArtemisAuthorizationOptions> authorizationOptions,
        IDistributedCache cache,
        ILoggerFactory logger,
        UrlEncoder encoder) : base(options, logger, encoder)
    {
        Cache = cache;
        AuthorizationOptions = authorizationOptions.Value;
    }

    /// <summary>
    ///     缓存访问器
    /// </summary>
    private IDistributedCache Cache { get; }

    /// <summary>
    ///     授权选项
    /// </summary>
    private ArtemisAuthorizationOptions AuthorizationOptions { get; }

    #region Overrides of AuthenticationHandler<ArtemisAuthenticationOptions>

    /// <summary>Allows derived types to handle authentication.</summary>
    /// <returns>The <see cref="T:Microsoft.AspNetCore.Authentication.AuthenticateResult" />.</returns>
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey(AuthorizationOptions.RequestHeaderTokenKey))
            return AuthenticateResult.Fail("未携带令牌");

        // 获取认证令牌
        string authorizationToken = Request.Headers[AuthorizationOptions.RequestHeaderTokenKey]!;

        if (string.IsNullOrEmpty(authorizationToken)) return AuthenticateResult.Fail("空令牌");

        // 若存在定义的模式，则移除
        if (authorizationToken.StartsWith(AuthorizationOptions.RequestHeaderTokenSchema,
                StringComparison.OrdinalIgnoreCase))
            authorizationToken = authorizationToken[AuthorizationOptions.RequestHeaderTokenSchema.Length..];

        // 认证结果
        try
        {
            return await ValidateToken(authorizationToken.Trim());
        }
        catch (Exception ex)
        {
            return AuthenticateResult.Fail(ex.Message);
        }
    }

    /// <summary>
    ///     认证Token
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    private async Task<AuthenticateResult> ValidateToken(string token)
    {
        var cacheTokenKey = TokenKeyGenerator.CacheTokenKey(AuthorizationOptions.CacheTokenPrefix, token);

        var document =
            await Cache.FetchTokenDocumentAsync<TokenDocument>(cacheTokenKey,
                cancellationToken: Context.RequestAborted);

        if (document != null)
        {
            // 处理多终端
            if (!AuthorizationOptions.EnableMultiEnd)
            {
                var userMapTokenKey = TokenKeyGenerator.CacheUserMapTokenKey(
                    AuthorizationOptions.CacheUserMapTokenPrefix,
                    document.EndType,
                    document.UserId);

                var userMapToken = await Cache.FetchUserMapTokenSymbolAsync(userMapTokenKey);

                if (userMapToken != token) return AuthenticateResult.Fail("不允许多终端登录");
            }

            var user = new List<Claim>
            {
                new(ClaimTypes.Authorization, token),
                new(ClaimTypes.UserId, document.UserId.IdToString() ?? string.Empty),
                new(ClaimTypes.UserName, document.UserName),
                new(ClaimTypes.EndType, document.EndType)
            };

            var roles = document.Roles
                .Select(item => new Claim(ClaimTypes.Role, item.Name));
            user.AddRange(roles);

            //claim document
            var userClaims = document.UserClaims;
            var roleClaims = document.RoleClaims;
            var claims = userClaims.Concat(roleClaims)
                .Select(item => new Claim(item.ClaimType, item.ClaimValue));

            user.AddRange(claims);

            //mate data

            var actionName = string.Empty;

            var routePath = string.Empty;

            if (Context.GetEndpoint() is RouteEndpoint routeEndpoint)
            {
                actionName = routeEndpoint.FetchActionName() ?? string.Empty;

                routePath = routeEndpoint.FetchRoutePath() ?? string.Empty;
            }

            user.Add(new Claim(ClaimTypes.MateActionName, actionName));

            user.Add(new Claim(ClaimTypes.MateRoutePath, routePath));


            var principal = new ClaimsPrincipal(new ClaimsIdentity(user, "Artemis"));
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }

        return AuthenticateResult.Fail("无效令牌");
    }

    #endregion
}