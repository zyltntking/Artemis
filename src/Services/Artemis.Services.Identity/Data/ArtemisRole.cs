using Artemis.Services.Identity.Data.Configurations;
using Artemis.Shared.Identity;
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
    ///     用户角色表映射
    /// </summary>
    public virtual ICollection<ArtemisUserRole>? UserRoles { get; set; }

    /// <summary>
    ///     角色凭据映射
    /// </summary>
    public virtual ICollection<ArtemisRoleClaim>? RoleClaims { get; set; }
}