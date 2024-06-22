namespace Artemis.Data.Shared.Identity;

/// <summary>
/// 角色凭据接口
/// </summary>
public interface IRoleClaim : IRoleClaimPackage
{
    /// <summary>
    ///     角色标识
    /// </summary>
    Guid RoleId { get; set; }
}

/// <summary>
///     角色凭据数据包接口
/// </summary>
public interface IRoleClaimPackage : IClaimPackage
{
}