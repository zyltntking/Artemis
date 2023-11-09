using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Shared.Identity.Transfer;
using Microsoft.AspNetCore.Identity;
#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).

namespace Artemis.Shared.Identity.Models;

/// <summary>
///     用户
/// </summary>
public class User : IdentityUser<Guid>, IKeySlot, IConcurrencyStamp, IUser, IIdentityUserDocument
{
    /// <summary>
    ///     并发锁
    /// </summary>
    [MaxLength(64)]
    public override string? ConcurrencyStamp { get; set; }

    /// <summary>
    ///     密码哈希
    /// </summary>
    [MaxLength(128)]
    public override required string PasswordHash { get; set; }

    /// <summary>
    ///     标识
    /// </summary>
    [Required]
    public override required Guid Id { get; set; }

    /// <summary>
    ///     用户名
    /// </summary>
    [Required]
    [MaxLength(32)]
    public override required string UserName { get; set; }

    /// <summary>
    ///     电子邮件
    /// </summary>
    [EmailAddress]
    [MaxLength(128)]
    public override string? Email { get; set; }

    /// <summary>
    ///     电话号码
    /// </summary>
    [Phone]
    [MaxLength(16)]
    public override string? PhoneNumber { get; set; }

    /// <summary>
    ///     标准化用户名
    /// </summary>
    [Required]
    [MaxLength(32)]
    public override required string NormalizedUserName { get; set; }

    /// <summary>
    ///     标准化电子邮件
    /// </summary>
    [MaxLength(128)]
    public override string? NormalizedEmail { get; set; }

    /// <summary>
    ///     电子邮件确认戳
    /// </summary>
    [Required]
    public override required bool EmailConfirmed { get; set; }

    /// <summary>
    ///     标准化电话号码
    /// </summary>
    [MaxLength(16)]
    public virtual string? NormalizedPhoneNumber { get; set; }

    /// <summary>
    ///     电话号码确认戳
    /// </summary>
    [Required]
    public override required bool PhoneNumberConfirmed { get; set; }

    /// <summary>
    ///     密码锁
    /// </summary>
    [MaxLength(64)]
    public override string? SecurityStamp { get; set; }

    /// <summary>
    ///     是否启用双因子认证
    /// </summary>
    [Required]
    public override required bool TwoFactorEnabled { get; set; }

    /// <summary>
    ///     是否启用锁定
    /// </summary>
    [Required]
    public override required bool LockoutEnabled { get; set; }

    /// <summary>
    ///     失败尝试次数
    /// </summary>
    [Required]
    public override required int AccessFailedCount { get; set; }

    /// <summary>
    ///     用户锁定到期时间标记
    /// </summary>
    public override DateTimeOffset? LockoutEnd { get; set; }
}