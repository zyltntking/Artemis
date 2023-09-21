namespace Artemis.Shared.Identity.Transfer.Interface;

/// <summary>
/// 基本用户凭据接口
/// </summary>
public interface IUserClaim
{
    /// <summary>
    ///     角色标识
    /// </summary>
    Guid UserId { get; set; }

    /// <summary>
    ///     凭据类型
    /// </summary>
    string ClaimType { get; set; }

    /// <summary>
    ///     凭据值
    /// </summary>
    string ClaimValue { get; set; }

    /// <summary>
    ///     校验戳
    /// </summary>
    string CheckStamp { get; set; }

    /// <summary>
    ///     凭据描述
    /// </summary>
    string? Description { get; set; }
}