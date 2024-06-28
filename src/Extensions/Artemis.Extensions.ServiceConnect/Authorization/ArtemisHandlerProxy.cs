using Artemis.Data.Core;
using Artemis.Data.Shared.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Artemis.Extensions.ServiceConnect.Authorization;

/// <summary>
/// Artemis操作员代理实现
/// </summary>
public sealed class ArtemisHandlerProxy : AbstractHandlerProxy
{
    /// <summary>
    /// 代理实现
    /// </summary>
    /// <param name="httpContextAccessor"></param>
    /// <param name="options"></param>
    public ArtemisHandlerProxy(
        IHttpContextAccessor httpContextAccessor,
        IOptions<ArtemisAuthorizationConfig> options)
    {
        HttpContextAccessor = httpContextAccessor;
        Options = options.Value;
    }

    /// <summary>
    /// http上下文访问器
    /// </summary>
    private IHttpContextAccessor HttpContextAccessor { get; }

    /// <summary>
    /// Artemis授权配置
    /// </summary>
    private ArtemisAuthorizationConfig Options { get; }

    #region Overrides of AbstractHandlerProxy<Guid>

    /// <summary>
    ///     操作员
    /// </summary>
    public override Guid Handler
    {
        get
        {
            var httpContext = HttpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return Guid.Empty;
            }

            var token = httpContext.FetchTokenDocument<TokenDocument>(Options.ContextItemTokenKey);

            return token?.UserId ?? Guid.Empty;
        }
    }

    #endregion
}