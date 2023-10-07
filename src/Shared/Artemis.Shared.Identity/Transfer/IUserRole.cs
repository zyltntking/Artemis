namespace Artemis.Shared.Identity.Transfer;

/// <summary>
///     基本用户角色信息接口
/// </summary>
public interface IUserRole
{
    /// <summary>
    ///     用户标识
    /// </summary>
    Guid UserId { get; set; }

    /// <summary>
    ///     角色标识
    /// </summary>
    Guid RoleId { get; set; }
}