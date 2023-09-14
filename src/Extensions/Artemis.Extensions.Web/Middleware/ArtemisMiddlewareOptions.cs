using System.ComponentModel.DataAnnotations;

namespace Artemis.Extensions.Web.Middleware;

/// <summary>
///     Artemis中间件配置
/// </summary>
public class ArtemisMiddlewareOptions
{
    /// <summary>
    ///     域配置
    /// </summary>
    [Required] public ServiceDomainOptions ServiceDomain = new();
}