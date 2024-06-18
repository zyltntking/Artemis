namespace Artemis.Data.Shared.Identity;

/// <summary>
///     角色凭据接口
/// </summary>
public interface IRoleClaim : IClaim
{
    /// <summary>
    ///     角色标识
    /// </summary>
    Guid RoleId { get; set; }
}