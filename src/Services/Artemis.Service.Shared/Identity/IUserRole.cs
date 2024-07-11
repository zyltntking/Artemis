namespace Artemis.Service.Shared.Identity;

/// <summary>
///     用户角色关系接口
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