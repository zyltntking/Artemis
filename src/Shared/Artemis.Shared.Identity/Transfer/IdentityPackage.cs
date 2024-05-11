namespace Artemis.Shared.Identity.Transfer;

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
    /// 端类型
    /// </summary>
    public required string EndType { get; set; }

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