using System.ComponentModel.DataAnnotations;
using Artemis.Shared.Identity.Transfer;
using Microsoft.AspNetCore.Identity;

namespace Artemis.Shared.Identity.Models;

/// <summary>
///     用户登录信息
/// </summary>
public class UserLogin : IdentityUserLogin<Guid>, IUserLogin
{
    /// <summary>
    ///     登录提供程序
    /// </summary>
    [Required]
    [MaxLength(32)]
    public override required string LoginProvider { get; set; }

    /// <summary>
    ///     提供程序密钥
    /// </summary>
    [Required]
    [MaxLength(64)]
    public override required string ProviderKey { get; set; }

    /// <summary>
    ///     提供程序显示名称
    /// </summary>
    [MaxLength(32)]
    public override string? ProviderDisplayName { get; set; }

    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    public override required Guid UserId { get; set; }
}