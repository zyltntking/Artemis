using Artemis.Data.Core;
using Artemis.Service.Shared.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Artemis.Extensions.ServiceConnect.Authorization;

/// <summary>
///     Artemis操作员代理实现
/// </summary>
public sealed class ArtemisHandlerProxy : AbstractHandlerProxy
{
    /// <summary>
    ///     代理实现
    /// </summary>
    /// <param name="httpContextAccessor"></param>
    /// <param name="options"></param>
    public ArtemisHandlerProxy(
        IHttpContextAccessor httpContextAccessor,
        IOptions<ArtemisAuthorizationOptions> options)
    {
        HttpContextAccessor = httpContextAccessor;
        Options = options.Value;
    }

    /// <summary>
    ///     http上下文访问器
    /// </summary>
    private IHttpContextAccessor HttpContextAccessor { get; }

    /// <summary>
    ///     Artemis授权配置
    /// </summary>
    private ArtemisAuthorizationOptions Options { get; }

    #region Overrides of AbstractHandlerProxy<Guid>

    /// <summary>
    ///     操作员
    /// </summary>
    public override string Handler
    {
        get
        {
            var httpContext = HttpContextAccessor.HttpContext;

            if (httpContext?.User is { Claims: not null, Identity.IsAuthenticated: true })
            {
                var userId = httpContext.User
                    .Claims
                    .Where(claim => claim.Type == ClaimTypes.UserId)
                    .Select(claim => claim.Value)
                    .FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(userId)) return userId;
            }

            return Guid.Empty.IdToString()!;
        }
    }

    #endregion
}