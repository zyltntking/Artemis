using Artemis.Data.Core;

namespace Artemis.Data.Shared.Identity;

/// <summary>
///     角色凭据接口
/// </summary>
public interface IRoleClaim : IRoleClaimPackage
{
    /// <summary>
    ///     角色标识
    /// </summary>
    Guid RoleId { get; set; }
}

/// <summary>
///     角色凭据信息接口
/// </summary>
public interface IRoleClaimInfo : IKeySlot<int>, IRoleClaimPackage
{
    /// <summary>
    ///     用户标识
    /// </summary>
    Guid UserId { get; set; }
}

/// <summary>
///     角色凭据文档接口
/// </summary>
public interface IRoleClaimDocument : IRoleClaimPackage, ICheckStamp;

/// <summary>
///     角色凭据数据包接口
/// </summary>
public interface IRoleClaimPackage : IClaimPackage;