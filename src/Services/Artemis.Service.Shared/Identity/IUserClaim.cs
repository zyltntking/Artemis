using Artemis.Data.Core;

namespace Artemis.Service.Shared.Identity;

/// <summary>
///     用户凭据接口
/// </summary>
public interface IUserClaim : IUserClaimInfo;

/// <summary>
///     用户凭据信息接口
/// </summary>
public interface IUserClaimInfo : IUserClaimPackage, ICheckStamp, IKeySlot<int>
{
    /// <summary>
    ///     用户标识
    /// </summary>
    Guid UserId { get; set; }
}

/// <summary>
///     用户凭据数据包接口
/// </summary>
public interface IUserClaimPackage : IClaimPackage
{
}