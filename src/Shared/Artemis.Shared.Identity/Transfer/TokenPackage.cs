using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Artemis.Shared.Identity.Transfer;

#region Interface

/// <summary>
///     Token对象接口
/// </summary>
file interface IToken
{
    /// <summary>
    ///     Token
    /// </summary>
    public string Token { get; }

    /// <summary>
    ///     过期时间
    /// </summary>
    public DateTime Expire { get; set; }
}

#endregion

/// <summary>
///     Token对象
/// </summary>
[DataContract]
public record TokenPackage : IToken
{
    #region Implementation of IToken

    /// <summary>
    ///     Token
    /// </summary>
    [MaxLength(32)]
    [DataMember(Order = 1)]
    public virtual string Token { get; } = null!;

    /// <summary>
    ///     过期时间
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public virtual required DateTime Expire { get; set; }

    #endregion
}

/// <summary>
///     Token信息
/// </summary>
public sealed record TokenInfo
{
    /// <summary>
    ///     用户标识
    /// </summary>
    public required Guid UserId { get; set; }

    /// <summary>
    ///     用户凭据
    /// </summary>
    public required List<ClaimItem> UserClaims { get; set; }

    /// <summary>
    ///     用户名
    /// </summary>
    public required string UserName { get; set; }

    /// <summary>
    ///     角色标识
    /// </summary>
    public required IEnumerable<Guid> RoleIds { get; set; }

    /// <summary>
    ///     角色名
    /// </summary>
    public required IEnumerable<string> RoleNames { get; set; }

    /// <summary>
    ///     角色凭据
    /// </summary>
    public required List<ClaimItem> RoleClaims { get; set; }
}