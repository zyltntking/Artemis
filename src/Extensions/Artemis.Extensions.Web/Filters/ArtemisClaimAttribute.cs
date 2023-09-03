using Artemis.Data.Core;
using Artemis.Extensions.Web.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Artemis.Extensions.Web.Filters;

/// <summary>
/// ArtemisClaim过滤器属性
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
public class ArtemisClaimAttribute : TypeFilterAttribute
{
    /// <summary>
    /// Instantiates a new <see cref="T:Microsoft.AspNetCore.Mvc.TypeFilterAttribute" /> instance.
    /// </summary>
    /// <param name="claim">是否生效</param>
    public ArtemisClaimAttribute(string? claim = null) : base(typeof(ArtemisClaimFilter))
    {
        if (claim != null) 
            Arguments = new object[] { claim };
    }

    /// <summary>
    /// ArtemisClaimFilter内部实现
    /// </summary>
    private class ArtemisClaimFilter : IAsyncAuthorizationFilter
    {
        /// <summary>
        /// ArtemisClaimFilter构造
        /// </summary>
        /// <param name="logger">日志</param>
        /// <param name="cache">缓存</param>
        /// <param name="claim">是否生效</param>
        public ArtemisClaimFilter(ILogger<ArtemisClaimFilter> logger, IDistributedCache cache, string? claim = null)
        {
            Logger = logger;
            Cache = cache;
            Claim = claim;
        }

        /// <summary>
        /// 日志
        /// </summary>
        private ILogger<ArtemisClaimFilter> Logger { get; }

        /// <summary>
        /// 缓存
        /// </summary>
        private IDistributedCache Cache { get; }

        /// <summary>
        /// 是否生效
        /// </summary>
        private string? Claim { get; set; }

        #region Implementation of IAsyncAuthorizationFilter

        /// <summary>
        /// Called early in the filter pipeline to confirm request is authorized.
        /// </summary>
        /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext" />.</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task" /> that on completion indicates the filter has executed.
        /// </returns>
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            Claim ??= context.HttpContext.Request.Path.Value;

            if (Claim == null)
            {
                var resp = DataResult.Exception<ClaimNullException>(new ClaimNullException());
                context.Result = new ObjectResult(resp);
            }

            Logger.LogInformation("开始校验凭据。");

            var token = Guid.NewGuid().ToString();

            Cache.SetString($"Identity:Token:{token}", token, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(4000)
            });


            return Task.CompletedTask;
        }

        #endregion
    }
}