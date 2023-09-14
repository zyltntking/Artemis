using Artemis.Services.Identity.Data.Configurations;
using Artemis.Shared.Identity;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Services.Identity.Data;

/// <summary>
///     ArtemisIdentityUserClaim
/// </summary>
[EntityTypeConfiguration(typeof(UserClaimConfiguration))]
public class ArtemisIdentityUserClaim : UserClaim
{
    /// <summary>
    ///     所属用户
    /// </summary>
    public virtual ArtemisIdentityUser User { get; set; } = null!;
}