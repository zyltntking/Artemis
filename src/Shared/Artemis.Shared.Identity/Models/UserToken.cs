using System.ComponentModel.DataAnnotations;
using Artemis.Shared.Identity.Transfer;
using Microsoft.AspNetCore.Identity;

namespace Artemis.Shared.Identity.Models;

/// <summary>
///     用户令牌
/// </summary>
public class UserToken : IdentityUserToken<Guid>, IUserToken
{
    /// <summary>
    ///     登录提供程序
    /// </summary>
    [Required]
    [MaxLength(32)]
    public override required string LoginProvider { get; set; }

    /// <summary>
    ///     令牌名称
    /// </summary>
    [Required]
    [MaxLength(32)]
    public override required string Name { get; set; }

    /// <summary>
    ///     令牌值
    /// </summary>
    public override string? Value { get; set; }

    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    public override required Guid UserId { get; set; }
}