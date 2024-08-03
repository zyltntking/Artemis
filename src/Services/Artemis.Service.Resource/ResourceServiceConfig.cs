using Artemis.Data.Core;

namespace Artemis.Service.Resource;

/// <summary>
///     资源服务配置
/// </summary>
public record ResourceServiceConfig
{
    /// <summary>
    ///     机构区域
    /// </summary>
    public required string OrganizationRegion { get; set; } = InitVal.Unknown;
}