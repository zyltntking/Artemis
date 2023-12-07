using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Services.Identity;

/// <summary>
///     认证逻辑配置
/// </summary>
public sealed class IdentityServiceOptions
{
    /// <summary>
    ///     是否启用缓存
    /// </summary>
    public bool EnableCache { get; set; }

    /// <summary>
    ///     缓存连接
    /// </summary>
    public string? RedisCacheConnection { get; init; }

    /// <summary>
    ///     设置数据库操作
    /// </summary>
    [Required]
    public required Action<DbContextOptionsBuilder> RegisterDbAction { get; init; }

    /// <summary>
    ///     认证配置选项操作
    /// </summary>
    public Action<IdentityOptions>? IdentityOptionsAction { get; set; }
}