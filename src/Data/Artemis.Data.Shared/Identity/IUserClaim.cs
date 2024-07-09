using Artemis.Data.Core;

namespace Artemis.Data.Shared.Identity;

/// <summary>
///     用户凭据接口
/// </summary>
public interface IUserClaim : IUserClaimPackage
{
    /// <summary>
    ///     用户标识
    /// </summary>
    Guid UserId { get; set; }
}

/// <summary>
///     用户凭据信息接口
/// </summary>
public interface IUserClaimInfo : IKeySlot<int>, IUserClaimPackage
{
    /// <summary>
    ///     用户标识
    /// </summary>
    Guid UserId { get; set; }
}

/// <summary>
///     用户凭据文档接口
/// </summary>
public interface IUserClaimDocument : IUserClaimPackage, ICheckStamp;

/// <summary>
///     用户凭据数据包接口
/// </summary>
public interface IUserClaimPackage : IClaimPackage
{
}