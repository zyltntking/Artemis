using Artemis.Services.Identity.Data.Configurations;
using Artemis.Shared.Identity;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Services.Identity.Data;

/// <summary>
///     ArtemisIdentityRole
/// </summary>
[EntityTypeConfiguration(typeof(RoleConfiguration))]
public class ArtemisIdentityRole : Role
{
    /// <summary>
    ///     用户角色表映射
    /// </summary>
    public virtual ICollection<ArtemisIdentityUserRole>? UserRoles { get; set; }

    /// <summary>
    ///     角色凭据映射
    /// </summary>
    public virtual ICollection<ArtemisIdentityRoleClaim>? RoleClaims { get; set; }
}