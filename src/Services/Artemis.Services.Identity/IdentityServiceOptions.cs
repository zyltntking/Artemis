using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Artemis.Services.Identity;

/// <summary>
///     认证逻辑配置
/// </summary>
public sealed class IdentityServiceOptions
{
    /// <summary>
    ///     连接
    /// </summary>
    [Required]
    public string ContextConnection { get; init; } = null!;

    /// <summary>
    ///     缓存连接
    /// </summary>
    public string? RedisCacheConnection { get; init; }

    /// <summary>
    ///     AssemblyName
    /// </summary>
    [Required]
    public string AssemblyName { get; init; } = null!;

    /// <summary>
    ///     认证配置选项操作
    /// </summary>
    public Action<IdentityOptions>? IdentityOptionsAction { get; set; }
}