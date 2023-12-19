using Artemis.Extensions.Web.Identity;
using Artemis.Shared.Identity.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Artemis.Extensions.Rpc
{
    /// <summary>
    /// Rpc服务模板
    /// </summary>
    public abstract class RpcServiceBase
    {
        /// <summary>
        /// 构造模板
        /// </summary>
        /// <param name="httpContextAccessor">HttpContext访问器</param>
        /// <param name="options"></param>
        protected RpcServiceBase(
            IHttpContextAccessor httpContextAccessor,
            IOptions<InternalAuthorizationOptions> options)
        {
            HttpContext = httpContextAccessor.HttpContext;
            Options = options.Value;
        }

        /// <summary>
        ///     Http上下文访问器
        /// </summary>
        private HttpContext? HttpContext { get; }

        /// <summary>
        ///     内部认证配置项
        /// </summary>
        protected InternalAuthorizationOptions Options { get; }

        /// <summary>
        /// 查询当前用户Token
        /// </summary>
        protected TokenDocument? CurrentToken => HttpContext?.FetchToken();

        /// <summary>
        /// 请求头中的Token
        /// </summary>
        protected string? HeaderToken => HttpContext?.FetchHeaderToken(Options.HeaderTokenKey);

        /// <summary>
        /// 操作取消信号
        /// </summary>
        protected CancellationToken CancellationToken => HttpContext?.RequestAborted ?? default;
    }
}
