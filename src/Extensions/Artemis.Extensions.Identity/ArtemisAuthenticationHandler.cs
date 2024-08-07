using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Artemis.Extensions.Identity;

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
        IOptions<ArtemisIdentityOptions> authorizationOptions,
        IDistributedCache cache,
        ILoggerFactory logger,
        UrlEncoder encoder) : base(options, logger, encoder)
    {
        Cache = cache;
        IdentityOptions = authorizationOptions.Value;
    }

    /// <summary>
    ///     缓存访问器
    /// </summary>
    private IDistributedCache Cache { get; }

    /// <summary>
    ///     授权选项
    /// </summary>
    private ArtemisIdentityOptions IdentityOptions { get; }

    #region Overrides of AuthenticationHandler<ArtemisAuthenticationOptions>

    /// <summary>Allows derived types to handle authentication.</summary>
    /// <returns>The <see cref="T:Microsoft.AspNetCore.Authentication.AuthenticateResult" />.</returns>
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var authorizationToken = Request.FetchRequestTokenSymbol(
            IdentityOptions.RequestHeaderTokenKey,
            IdentityOptions.RequestHeaderTokenSchema);

        if (authorizationToken == null) return AuthenticateResult.Fail("未携带令牌或空令牌");

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
        var cacheTokenKey = TokenKeyGenerator.CacheTokenKey(IdentityOptions.CacheTokenPrefix, token);

        var document = await Cache.FetchTokenRecordAsync(cacheTokenKey, true, Context.RequestAborted);

        if (document != null)
        {
            // 处理多终端
            if (!IdentityOptions.EnableMultiEnd)
            {
                var userMapTokenKey = TokenKeyGenerator.CacheUserMapTokenKey(
                    IdentityOptions.CacheUserMapTokenPrefix,
                    document.EndType,
                    document.UserId);

                var userMapToken = await Cache.FetchUserMapTokenSymbolAsync(userMapTokenKey);

                if (userMapToken != token) 
                    return AuthenticateResult.Fail("不允许多终端登录");
            }

            var actionName = string.Empty;

            var routePath = string.Empty;

            if (Context.GetEndpoint() is RouteEndpoint routeEndpoint)
            {
                actionName = routeEndpoint.FetchActionName() ?? string.Empty;

                routePath = routeEndpoint.FetchRoutePath() ?? string.Empty;
            }

            var user = document.GetClaims(token, actionName, routePath);

            var principal = new ClaimsPrincipal(new ClaimsIdentity(user, "Artemis"));
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }

        return AuthenticateResult.Fail("无效令牌");
    }

    #endregion
}