using Artemis.Data.Shared.Identity;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Service.Identity.Models;

/// <summary>
/// 用户令牌模型
/// </summary>
public class UserToken : IUserToken
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    public required Guid UserId { get; set; }

    /// <summary>
    ///     登录提供程序
    /// </summary>
    [Required]
    [MaxLength(64)]
    public required string LoginProvider { get; set; }

    /// <summary>
    ///     令牌名称
    /// </summary>
    [Required]
    [MaxLength(128)]
    public required string Name { get; set; }

    /// <summary>
    ///     令牌值
    /// </summary>
    public string? Value { get; set; }
}