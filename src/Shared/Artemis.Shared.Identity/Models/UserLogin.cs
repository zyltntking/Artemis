using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Artemis.Data.Core;
using Microsoft.AspNetCore.Identity;

namespace Artemis.Shared.Identity.Models;

/// <summary>
///     用户登录信息
/// </summary>
[DataContract]
public class UserLogin : IdentityUserLogin<Guid>, IKeySlot<int>
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [DataMember(Order = 2)]
    public override Guid UserId { get; set; }

    /// <summary>
    ///     登录提供程序
    /// </summary>
    [DataMember(Order = 3)]
    [MaxLength(32)]
    [Required]
    public override string LoginProvider { get; set; } = null!;

    /// <summary>
    ///     提供程序密钥
    /// </summary>
    [DataMember(Order = 4)]
    [MaxLength(64)]
    [Required]
    public override string ProviderKey { get; set; } = null!;

    /// <summary>
    ///     提供程序显示名称
    /// </summary>
    [DataMember(Order = 5)]
    [MaxLength(32)]
    public override string? ProviderDisplayName { get; set; }

    /// <summary>
    ///     存储标识
    /// </summary>
    [DataMember(Order = 1)]
    public virtual int Id { get; set; }
}