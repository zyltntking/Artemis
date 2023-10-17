using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Artemis.Data.Core;
using Artemis.Shared.Identity.Transfer;
using Microsoft.AspNetCore.Identity;

namespace Artemis.Shared.Identity.Models;

/// <summary>
///     用户登录信息
/// </summary>
[DataContract]
public class UserLogin : IdentityUserLogin<Guid>, IKeySlot<int>, IUserLogin
{
    /// <summary>
    ///     存储标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public virtual required int Id { get; set; }

    /// <summary>
    ///     登录提供程序
    /// </summary>
    [Required]
    [MaxLength(32)]
    [DataMember(Order = 3)]
    public override required string LoginProvider { get; set; }

    /// <summary>
    ///     提供程序密钥
    /// </summary>
    [Required]
    [MaxLength(64)]
    [DataMember(Order = 4)]
    public override required string ProviderKey { get; set; }

    /// <summary>
    ///     提供程序显示名称
    /// </summary>
    [MaxLength(32)]
    [DataMember(Order = 5)]
    public override string? ProviderDisplayName { get; set; }

    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public override required Guid UserId { get; set; }
}