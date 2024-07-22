namespace Artemis.Service.Shared.Identity.Transfer;

/// <summary>
///     用户凭据信息
/// </summary>
public record UserClaimInfo : UserClaimPackage, IUserClaimInfo
{
    /// <summary>
    ///     存储标识
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    ///     用户标识
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    ///     校验戳
    /// </summary>
    public required string CheckStamp { get; set; }
}

/// <summary>
///     用户凭据数据包
/// </summary>
public record UserClaimPackage : ClaimPackage, IUserClaimPackage;