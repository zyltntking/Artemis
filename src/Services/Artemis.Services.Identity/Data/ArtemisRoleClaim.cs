using Artemis.Services.Identity.Data.Configurations;
using Artemis.Shared.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Services.Identity.Data;

/// <summary>
///     ArtemisRoleClaim
/// </summary>
[EntityTypeConfiguration(typeof(RoleClaimConfiguration))]
public class ArtemisRoleClaim : RoleClaim
{
    /// <summary>
    ///     凭据所属角色
    /// </summary>
    public virtual ArtemisRole Role { get; set; } = null!;
}