using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;

namespace Artemis.Shared.Identity.Transfer;

#region Interface

/// <summary>
///     基本用户接口
/// </summary>
internal interface IUser
{
    /// <summary>
    ///     用户名
    /// </summary>
    string UserName { get; set; }

    /// <summary>
    ///     电子邮件
    /// </summary>
    string? Email { get; set; }

    /// <summary>
    ///     电话号码
    /// </summary>
    string? PhoneNumber { get; set; }
}

/// <summary>
///     认证必要信息
/// </summary>
public interface IIdentityUser
{
    /// <summary>
    ///     用户名
    /// </summary>
    string UserName { get; set; }

    /// <summary>
    ///     密码哈希
    /// </summary>
    string PasswordHash { get; set; }
}

#endregion

/// <summary>
///     基本用户信息
/// </summary>
public record UserPackage : IUser
{
    /// <summary>
    ///     生成加密戳
    /// </summary>
    public string GenerateSecurityStamp => Guid.NewGuid().ToString("N").ToUpper();

    #region Implementation of IUser

    /// <summary>
    ///     用户名
    /// </summary>
    [Required]
    [MaxLength(128)]
    public required string UserName { get; set; }

    /// <summary>
    ///     电子邮件
    /// </summary>
    [EmailAddress]
    [MaxLength(256)]
    public string? Email { get; set; }

    /// <summary>
    ///     电话号码
    /// </summary>
    [Phone]
    [MaxLength(32)]
    public string? PhoneNumber { get; set; }

    #endregion
}

/// <summary>
///     用户信息
/// </summary>
public sealed record UserInfo : UserPackage, IKeySlot
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    public required Guid Id { get; set; }

    /// <summary>
    ///     电子邮件确认戳
    /// </summary>
    [Required]
    public required bool EmailConfirmed { get; set; }

    /// <summary>
    ///     电话号码确认戳
    /// </summary>
    [Required]
    public required bool PhoneNumberConfirmed { get; set; }
}

/// <summary>
///     认证用户文档
/// </summary>
public sealed record UserDocument : IIdentityUser, IKeySlot
{
    #region Implementation of IIdentityUserDocument

    /// <summary>
    ///     标识
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    ///     用户名
    /// </summary>
    public required string UserName { get; set; }

    /// <summary>
    ///     密码哈希
    /// </summary>
    public required string PasswordHash { get; set; }

    #endregion
}