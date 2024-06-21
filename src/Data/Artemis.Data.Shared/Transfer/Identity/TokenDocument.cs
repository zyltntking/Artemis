namespace Artemis.Data.Shared.Transfer.Identity;

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
    ///     端类型
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