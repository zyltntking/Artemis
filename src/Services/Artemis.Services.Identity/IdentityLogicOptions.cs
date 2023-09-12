using System.ComponentModel.DataAnnotations;

namespace Artemis.Services.Identity;

/// <summary>
///     认证逻辑配置
/// </summary>
public sealed class IdentityLogicOptions
{
    /// <summary>
    ///     连接
    /// </summary>
    [Required]
    public string Connection { get; init; } = null!;

    /// <summary>
    ///     AssemblyName
    /// </summary>
    [Required]
    public string AssemblyName { get; init; } = null!;
}