using Artemis.Services.Identity.Data.Configurations;
using Artemis.Shared.Identity;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Services.Identity.Data;

/// <summary>
///     ArtemisIdentityRoleClaim
/// </summary>
[EntityTypeConfiguration(typeof(RoleClaimConfiguration))]
public class ArtemisIdentityRoleClaim : RoleClaim
{
    /// <summary>
    ///     凭据所属角色
    /// </summary>
    public virtual ArtemisIdentityRole Role { get; set; } = null!;
}