using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
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

#endregion

/// <summary>
///     基本用户信息
/// </summary>
[DataContract]
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
    [MaxLength(32)]
    [DataMember(Order = 1)]
    public virtual required string UserName { get; set; }

    /// <summary>
    ///     电子邮件
    /// </summary>
    [EmailAddress]
    [MaxLength(128)]
    [DataMember(Order = 2)]
    public virtual string? Email { get; set; }

    /// <summary>
    ///     电话号码
    /// </summary>
    [Phone]
    [MaxLength(16)]
    [DataMember(Order = 3)]
    public virtual string? PhoneNumber { get; set; }

    #endregion
}

/// <summary>
///     用户信息
/// </summary>
[DataContract]
public record UserInfo : UserPackage, IKeySlot
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public virtual required Guid Id { get; set; }

    /// <summary>
    ///     用户名
    /// </summary>
    [Required]
    [MaxLength(32)]
    [DataMember(Order = 2)]
    public override required string UserName { get; set; }

    /// <summary>
    ///     电子邮件
    /// </summary>
    [EmailAddress]
    [MaxLength(128)]
    [DataMember(Order = 3)]
    public override string? Email { get; set; }

    /// <summary>
    ///     电子邮件确认戳
    /// </summary>
    [Required]
    [DataMember(Order = 4)]
    public virtual required bool EmailConfirmed { get; set; }

    /// <summary>
    ///     电话号码
    /// </summary>
    [Phone]
    [MaxLength(16)]
    [DataMember(Order = 5)]
    public override string? PhoneNumber { get; set; }

    /// <summary>
    ///     电话号码确认戳
    /// </summary>
    [Required]
    [DataMember(Order = 6)]
    public virtual required bool PhoneNumberConfirmed { get; set; }
}