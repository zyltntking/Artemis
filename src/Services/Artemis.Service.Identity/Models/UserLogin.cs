using System.ComponentModel.DataAnnotations;
using Artemis.Data.Shared.Identity;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Identity.Models;

/// <summary>
///     用户登录模型
/// </summary>
public class UserLogin : IUserLogin
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    [Comment("用户标识")]
    public required Guid UserId { get; set; }

    /// <summary>
    ///     登录提供程序
    /// </summary>
    [Required]
    [MaxLength(64)]
    [Comment("登录提供程序")]
    public required string LoginProvider { get; set; }

    /// <summary>
    ///     提供程序密钥
    /// </summary>
    [Required]
    [MaxLength(256)]
    [Comment("提供程序密钥")]
    public required string ProviderKey { get; set; }

    /// <summary>
    ///     提供程序显示名称
    /// </summary>
    [MaxLength(128)]
    [Comment("提供程序显示名称")]
    public string? ProviderDisplayName { get; set; }
}