using Artemis.Data.Shared.Identity;

namespace Artemis.Data.Shared.Transfer.Identity;

/// <summary>
///     用户凭据信息
/// </summary>
public record UserClaimInfo : ClaimDocument, IUserClaimInfo
{
    #region Implementation of IKeySlot<int>

    /// <summary>
    ///     存储标识
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     用户标识
    /// </summary>
    public required Guid UserId { get; set; }

    #endregion
}

/// <summary>
///     用户凭据数据包
/// </summary>
public record UserClaimPackage : ClaimPackage, IUserClaimPackage;