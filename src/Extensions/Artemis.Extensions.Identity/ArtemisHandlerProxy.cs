using Artemis.Data.Core;
using Artemis.Data.Core.Fundamental.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Artemis.Extensions.Identity;

/// <summary>
///     Artemis操作员代理实现
/// </summary>
public sealed class ArtemisHandlerProxy : AbstractHandlerProxy
{
    /// <summary>
    ///     代理实现
    /// </summary>
    /// <param name="httpContextAccessor"></param>
    /// <param name="identityOptions"></param>
    public ArtemisHandlerProxy(
        IHttpContextAccessor httpContextAccessor,
        IOptions<ArtemisIdentityOptions> identityOptions)
    {
        HttpContextAccessor = httpContextAccessor;
        Options = identityOptions.Value;
    }

    /// <summary>
    ///     http上下文访问器
    /// </summary>
    private IHttpContextAccessor HttpContextAccessor { get; }

    /// <summary>
    ///     Artemis认证配置
    /// </summary>
    private ArtemisIdentityOptions Options { get; }

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
                    .Where(claim => claim.Type == ArtemisClaimTypes.UserId.Name)
                    .Select(claim => claim.Value)
                    .FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(userId)) return userId;
            }

            return Guid.Empty.IdToString()!;
        }
    }

    #endregion
}