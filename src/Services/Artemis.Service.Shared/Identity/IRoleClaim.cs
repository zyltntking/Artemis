using Artemis.Data.Core;

namespace Artemis.Service.Shared.Identity;

/// <summary>
///     角色凭据接口
/// </summary>
public interface IRoleClaim : IRoleClaimInfo;

/// <summary>
///     角色凭据信息接口
/// </summary>
public interface IRoleClaimInfo : IRoleClaimPackage, ICheckStamp, IKeySlot<int>
{
    /// <summary>
    ///     角色标识
    /// </summary>
    Guid RoleId { get; set; }
}

/// <summary>
///     角色凭据数据包接口
/// </summary>
public interface IRoleClaimPackage : IClaimPackage;