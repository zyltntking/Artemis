using System.Security.Claims;
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
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
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

        /// <summary>
        /// 凭据类型索引
        /// </summary>
        private const string ClaimTypeKey = "ClaimType";

        /// <summary>
        /// 域标识
        /// </summary>
        private const string DomainKey = SharedKeys.DomainKey;

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
            if (context.HttpContext.Items.ContainsKey(ClaimTypeKey))
            {
                //不改变上个筛选器的校验结果
                return Task.CompletedTask;
            }

            var routePathClaim = context.HttpContext.Request.Path.Value ?? string.Empty;

            var actionName = ActionNameClaim(context.ActionDescriptor as ControllerActionDescriptor);

            Logger.LogInformation("从缓存中获取凭据...");

            var claimList = Cache.FetchClaims().ToList();

            Logger.LogInformation("开始校验凭据...");

            if (!string.IsNullOrEmpty(SignatureClaim))
            {
                Logger.LogInformation("校验签名凭据...");

                var claimType = IdentityClaimType.Signature.ToString();

                var signatureValid = VerifyClaim(claimList, claimType, SignatureClaim);

                if (signatureValid)
                {
                    context.HttpContext.Items.Add(ClaimTypeKey, claimType);
                    return Task.CompletedTask;
                }
            }

            if (!string.IsNullOrEmpty(routePathClaim))
            {
                Logger.LogInformation("校验路由凭据...");

                var claimType = IdentityClaimType.RoutePath.ToString();

                var routePathValid = VerifyClaim(claimList, claimType, routePathClaim);

                if (routePathValid)
                {
                    context.HttpContext.Items.Add(ClaimTypeKey, claimType);
                    return Task.CompletedTask;
                }
            }

            var domain = context.HttpContext.Items[DomainKey] as string;

            var actionNameClaim = $"{domain}.{actionName}";

            if (!string.IsNullOrEmpty(actionName))
            {
                Logger.LogInformation("校验操作名凭据...");

                var claimType = IdentityClaimType.ActionName.ToString();

                var actionNameValid = VerifyClaim(claimList, claimType, actionNameClaim);

                if (actionNameValid)
                {
                    context.HttpContext.Items.Add(ClaimTypeKey, claimType);
                    return Task.CompletedTask;
                }
            }

            // 提示凭据异常所在操作
            Logger.LogInformation($"domain:{domain} action:{actionName},凭据校验未通过...");

            if (string.IsNullOrWhiteSpace(actionName)) 
                throw new ClaimInvalidException();

            throw new ClaimInvalidException("actionNameClaim");
        }

        #endregion

        /// <summary>
        ///     操作名凭据
        /// </summary>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        private static string ActionNameClaim(ControllerActionDescriptor? descriptor)
        {
            if (descriptor == null)
            {
                return string.Empty;
            }

            var controller = descriptor.ControllerName;

            var action = descriptor.ActionName;

            return $"{controller}.{action}";
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