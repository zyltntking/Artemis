using System.Security.Claims;
using System.Text.RegularExpressions;
using Artemis.Extensions.Web.Exceptions;
using Artemis.Extensions.Web.Feature;
using Artemis.Extensions.Web.Fundamental;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Artemis.Extensions.Web.Filter;

/// <summary>
///     ArtemisClaim过滤器属性
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
public class ArtemisClaimAttribute : TypeFilterAttribute
{
    /// <summary>
    ///     Instantiates a new <see cref="T:Microsoft.AspNetCore.Mvc.TypeFilterAttribute" /> instance.
    /// </summary>
    /// <param name="claim">是否生效</param>
    public ArtemisClaimAttribute(string? claim = null) : base(typeof(ArtemisClaimFilter))
    {
        if (claim != null)
            Arguments = new object[] { claim };
    }

    /// <summary>
    ///     ArtemisClaimFilter内部实现
    /// </summary>
    private class ArtemisClaimFilter : IAsyncAuthorizationFilter
    {
        /// <summary>
        ///     ArtemisClaimFilter构造
        /// </summary>
        /// <param name="logger">日志</param>
        /// <param name="cache">缓存</param>
        /// <param name="claim">是否生效</param>
        public ArtemisClaimFilter(ILogger<ArtemisClaimFilter> logger, IDistributedCache cache, string? claim = null)
        {
            Logger = logger;
            Cache = cache;
            SignatureClaim = claim ?? string.Empty;
        }

        /// <summary>
        ///     日志
        /// </summary>
        private ILogger<ArtemisClaimFilter> Logger { get; }

        /// <summary>
        ///     缓存
        /// </summary>
        private IDistributedCache Cache { get; }

        /// <summary>
        ///     是否生效
        /// </summary>
        private string SignatureClaim { get; }

        #region Implementation of IAsyncAuthorizationFilter

        /// <summary>
        ///     Called early in the filter pipeline to confirm request is authorized.
        /// </summary>
        /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext" />.</param>
        /// <returns>
        ///     A <see cref="T:System.Threading.Tasks.Task" /> that on completion indicates the filter has executed.
        /// </returns>
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var routePathClaim = context.HttpContext.Request.Path.Value ?? string.Empty;

            var actionNameClaim = ActionNameClaim(context.ActionDescriptor.DisplayName);

            Logger.LogInformation("从缓存中获取凭据...");

            var claimList = Cache.FetchClaims().ToList();

            Logger.LogInformation("开始校验凭据...");

            bool signatureValid = false, routePathValid = false, actionNameValid = false;

            if (!string.IsNullOrEmpty(SignatureClaim))
            {
                Logger.LogInformation("校验签名凭据...");

                var claimType = IdentityClaimType.Signature.ToString();

                signatureValid = VerifyClaim(claimList, claimType, SignatureClaim);
            }

            if (signatureValid) return Task.CompletedTask;

            if (!string.IsNullOrEmpty(routePathClaim))
            {
                Logger.LogInformation("校验路由凭据...");

                var claimType = IdentityClaimType.RoutePath.ToString();

                routePathValid = VerifyClaim(claimList, claimType, routePathClaim);
            }

            if (routePathValid) return Task.CompletedTask;

            if (!string.IsNullOrEmpty(actionNameClaim))
            {
                Logger.LogInformation("校验操作名凭据...");

                var claimType = IdentityClaimType.ActionName.ToString();

                actionNameValid = VerifyClaim(claimList, claimType, actionNameClaim);
            }

            if (actionNameValid) return Task.CompletedTask;

            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;

            var actionName = descriptor?.ActionName;

            if (string.IsNullOrWhiteSpace(actionName)) throw new ClaimInvalidException();

            throw new ClaimInvalidException(actionName);
        }

        #endregion

        /// <summary>
        ///     操作名凭据
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        private static string ActionNameClaim(string? actionName)
        {
            var regex = new Regex(@"\((.*?)\)");

            return regex.Replace(actionName ?? string.Empty, string.Empty).Trim();
        }

        /// <summary>
        ///     校验凭据
        /// </summary>
        /// <param name="claims">凭据表</param>
        /// <param name="claimType">凭据类型</param>
        /// <param name="claimValue">凭据值</param>
        /// <returns></returns>
        private static bool VerifyClaim(IEnumerable<Claim> claims, string claimType, string claimValue)
        {
            return claims.Where(item => item.Type.Equals(claimType, StringComparison.OrdinalIgnoreCase))
                .Select(item => item.Value)
                .Contains(claimValue);
        }
    }
}