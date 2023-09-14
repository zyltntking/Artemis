using Artemis.Services.Identity.Data.Configurations;
using Artemis.Shared.Identity;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Services.Identity.Data;

/// <summary>
///     ArtemisUserClaim
/// </summary>
[EntityTypeConfiguration(typeof(UserClaimConfiguration))]
public class ArtemisUserClaim : UserClaim
{
    /// <summary>
    ///     所属用户
    /// </summary>
    public virtual ArtemisUser User { get; set; } = null!;
}