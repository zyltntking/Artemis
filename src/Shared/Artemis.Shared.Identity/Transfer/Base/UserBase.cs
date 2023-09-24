﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Artemis.Shared.Identity.Transfer.Interface;

namespace Artemis.Shared.Identity.Transfer.Base;

/// <summary>
///     基本用户信息
/// </summary>
[DataContract]
public record UserBase : IUser
{
    /// <summary>
    /// 生成加密戳
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
    ///     电子邮件确认戳
    /// </summary>
    [Required]
    [DataMember(Order = 3)]
    public virtual bool EmailConfirmed { get; set; }

    /// <summary>
    ///     电话号码
    /// </summary>
    [Phone]
    [MaxLength(16)]
    [DataMember(Order = 4)]
    public virtual string? PhoneNumber { get; set; }

    /// <summary>
    ///     电话号码确认戳
    /// </summary>
    [Required]
    [DataMember(Order = 5)]
    public virtual bool PhoneNumberConfirmed { get; set; }

    #endregion
}