using Artemis.Services.Identity.Data.Configurations;
using Artemis.Shared.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Services.Identity.Data;

/// <summary>
///     ArtemisRole
/// </summary>
[EntityTypeConfiguration(typeof(RoleConfiguration))]
public class ArtemisRole : Role
{
    /// <summary>
    ///     具备该角色的用户
    /// </summary>
    public virtual ICollection<ArtemisUser> Users { get; } = new List<ArtemisUser>();

    /// <summary>
    ///     用户角色表映射
    /// </summary>
    public virtual ICollection<ArtemisUserRole> UserRoles { get; } = new List<ArtemisUserRole>();

    /// <summary>
    ///     角色凭据映射
    /// </summary>
    public virtual ICollection<ArtemisRoleClaim> RoleClaims { get; } = new List<ArtemisRoleClaim>();
}