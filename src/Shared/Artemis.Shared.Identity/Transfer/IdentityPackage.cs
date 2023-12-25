using System.ComponentModel.DataAnnotations;

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
    public string? Token { get; set; }

    /// <summary>
    ///     过期时间
    /// </summary>
    public long Expire { get; set; }
}

#endregion

/// <summary>
///     Token对象
/// </summary>
public record TokenPackage : IToken
{
    #region Implementation of IToken

    /// <summary>
    ///     Token
    /// </summary>
    [MaxLength(32)]
    public string? Token { get; set; }

    /// <summary>
    ///     过期时间
    /// </summary>
    [Required]
    public required long Expire { get; set; }

    #endregion
}

/// <summary>
///     认证基本信息
/// </summary>
public record SignInPackage
{
    /// <summary>
    ///     用户签名
    /// </summary>
    /// <remarks>用户名|电话号码|邮箱地址</remarks>
    [Required]
    public required string UserSign { get; set; }

    /// <summary>
    ///     密码
    /// </summary>
    [Required]
    public required string Password { get; set; }
}

/// <summary>
///     签入结果
/// </summary>
public sealed record SignResult
{
    /// <summary>
    ///     是否认证成功
    /// </summary>
    public bool Succeeded { get; set; }

    /// <summary>
    ///     认证消息
    /// </summary>
    public required string Message { get; set; }
}

/// <summary>
///     Token信息
/// </summary>
public sealed record TokenDocument
{
    /// <summary>
    ///     用户标识
    /// </summary>
    public required Guid UserId { get; set; }

    /// <summary>
    ///     用户名
    /// </summary>
    public required string UserName { get; set; }

    /// <summary>
    ///     用户凭据
    /// </summary>
    public required IEnumerable<ClaimPackage> UserClaims { get; set; }

    /// <summary>
    ///     角色标识
    /// </summary>
    public required IEnumerable<RoleInfo> Roles { get; set; }

    /// <summary>
    ///     角色凭据
    /// </summary>
    public required IEnumerable<ClaimPackage> RoleClaims { get; set; }
}