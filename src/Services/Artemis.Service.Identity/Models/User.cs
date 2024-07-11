using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Service.Shared.Identity;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Identity.Models;

/// <summary>
///     用户模型
/// </summary>
public class User : ConcurrencyModel, IUser
{
    /// <summary>
    ///     用户名
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("用户名")]
    public required string UserName { get; set; }

    /// <summary>
    ///     标准化用户名
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("标准化用户名")]
    public required string NormalizedUserName { get; set; }

    /// <summary>
    ///     电子邮件
    /// </summary>
    [EmailAddress]
    [MaxLength(256)]
    [Comment("电子邮件")]
    public string? Email { get; set; }

    /// <summary>
    ///     标准化电子邮件
    /// </summary>
    [MaxLength(256)]
    [Comment("标准化电子邮件")]
    public string? NormalizedEmail { get; set; }

    /// <summary>
    ///     电话号码
    /// </summary>
    [Phone]
    [MaxLength(32)]
    [Comment("电话号码")]
    public string? PhoneNumber { get; set; }

    /// <summary>
    ///     电子邮件确认戳
    /// </summary>
    [Required]
    [Comment("电子邮件确认戳")]
    public required bool EmailConfirmed { get; set; }

    /// <summary>
    ///     电话号码确认戳
    /// </summary>
    [Required]
    [Comment("电话号码确认戳")]
    public required bool PhoneNumberConfirmed { get; set; }

    /// <summary>
    ///     密码哈希
    /// </summary>
    [MaxLength(256)]
    [Comment("密码哈希")]
    public required string PasswordHash { get; set; }

    /// <summary>
    ///     密码锁
    /// </summary>
    [MaxLength(64)]
    [Comment("密码锁")]
    public string? SecurityStamp { get; set; }

    /// <summary>
    ///     是否启用双因子认证
    /// </summary>
    [Required]
    [Comment("是否启用双因子认证")]
    public required bool TwoFactorEnabled { get; set; }

    /// <summary>
    ///     用户锁定到期时间标记
    /// </summary>
    [Comment("用户锁定到期时间标记")]
    public DateTimeOffset? LockoutEnd { get; set; }

    /// <summary>
    ///     是否启用锁定
    /// </summary>
    [Required]
    [Comment("是否启用锁定")]
    public required bool LockoutEnabled { get; set; }

    /// <summary>
    ///     失败尝试次数
    /// </summary>
    [Required]
    [Comment("失败尝试次数")]
    public required int AccessFailedCount { get; set; }
}