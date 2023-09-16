using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Artemis.Data.Core;
using Microsoft.AspNetCore.Identity;

namespace Artemis.Shared.Identity.Models;

/// <summary>
///     用户令牌
/// </summary>
[DataContract]
public class UserToken : IdentityUserToken<Guid>, IKeySlot<int>
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
    [MaxLength(128)]
    [Required]
    public override string LoginProvider { get; set; } = null!;

    /// <summary>
    ///     令牌名称
    /// </summary>
    [DataMember(Order = 4)]
    [MaxLength(128)]
    [Required]
    public override string Name { get; set; } = null!;

    /// <summary>
    ///     令牌值
    /// </summary>
    [DataMember(Order = 5)]
    [MaxLength(256)]
    public override string? Value { get; set; }

    /// <summary>
    ///     存储标识
    /// </summary>
    [DataMember(Order = 1)]
    public virtual int Id { get; set; }
}