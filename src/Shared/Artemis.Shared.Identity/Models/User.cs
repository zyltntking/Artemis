using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Artemis.Data.Core;
using Artemis.Shared.Identity.Transfer.Interface;
using Microsoft.AspNetCore.Identity;

namespace Artemis.Shared.Identity.Models;

/// <summary>
///     用户
/// </summary>
[DataContract]
public class User : IdentityUser<Guid>, IKeySlot, IUser
{
    /// <summary>
    ///     标识
    /// </summary>
    [DataMember(Order = 1)]
    public override Guid Id { get; set; }

    /// <summary>
    ///     用户名
    /// </summary>
    [Required]
    [MaxLength(32)]
    [DataMember(Order = 2)]
    public override string UserName { get; set; } = null!;

    /// <summary>
    ///     电子邮件
    /// </summary>
    [EmailAddress]
    [MaxLength(128)]
    [DataMember(Order = 4)]
    public override string? Email { get; set; }

    /// <summary>
    ///     电子邮件确认戳
    /// </summary>
    [DataMember(Order = 6)]
    public override bool EmailConfirmed { get; set; }

    /// <summary>
    ///     电话号码
    /// </summary>
    [Phone]
    [MaxLength(16)]
    [DataMember(Order = 7)]
    public override string? PhoneNumber { get; set; }

    /// <summary>
    ///     电话号码确认戳
    /// </summary>
    [DataMember(Order = 8)]
    public override bool PhoneNumberConfirmed { get; set; }

    /// <summary>
    ///     标准化用户名
    /// </summary>
    [Required]
    [MaxLength(32)]
    [DataMember(Order = 3)]
    public override string NormalizedUserName { get; set; } = null!;

    /// <summary>
    ///     标准化电子邮件
    /// </summary>
    [MaxLength(128)]
    [DataMember(Order = 5)]
    public override string? NormalizedEmail { get; set; }

    /// <summary>
    ///     密码哈希
    /// </summary>
    [EmailAddress]
    [MaxLength(128)]
    [DataMember(Order = 9)]
    public override string PasswordHash { get; set; } = null!;

    /// <summary>
    ///     密码锁
    /// </summary>
    [DataMember(Order = 10)]
    [MaxLength(64)]
    public override string? SecurityStamp { get; set; }

    /// <summary>
    ///     并发锁
    /// </summary>
    [DataMember(Order = 11)]
    [MaxLength(64)]
    public override string? ConcurrencyStamp { get; set; }

    /// <summary>
    ///     是否启用双因子认证
    /// </summary>
    [DataMember(Order = 12)]
    public override bool TwoFactorEnabled { get; set; }

    /// <summary>
    ///     是否启用锁定
    /// </summary>
    [DataMember(Order = 13)]
    public override bool LockoutEnabled { get; set; }

    /// <summary>
    ///     失败尝试次数
    /// </summary>
    [DataMember(Order = 14)]
    public override int AccessFailedCount { get; set; }

    /// <summary>
    ///     用户锁定到期时间标记
    /// </summary>
    [DataMember(Order = 15)]
    public override DateTimeOffset? LockoutEnd { get; set; }
}