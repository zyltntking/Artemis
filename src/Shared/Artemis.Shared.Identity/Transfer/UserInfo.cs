using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Artemis.Data.Core;
using Artemis.Shared.Identity.Transfer.Base;

namespace Artemis.Shared.Identity.Transfer;

/// <summary>
///     用户信息
/// </summary>
[DataContract]
public record UserInfo : UserBase, IKeySlot
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
    public override bool EmailConfirmed { get; set; }

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
    public override bool PhoneNumberConfirmed { get; set; }
}