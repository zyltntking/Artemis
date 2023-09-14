using System.ComponentModel.DataAnnotations;

namespace Artemis.Extensions.Web.Middleware;

/// <summary>
///     域配置
/// </summary>
public class ServiceDomainOptions
{
    /// <summary>
    ///     域名
    /// </summary>
    [Required]
    public string DomainName { get; set; } = "Artemis";
}