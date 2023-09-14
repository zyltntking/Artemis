using Artemis.Services.Identity.Data.Configurations;
using Artemis.Shared.Identity;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Services.Identity.Data;

/// <summary>
///     ArtemisIdentityUserToken
/// </summary>
[EntityTypeConfiguration(typeof(UserTokenConfiguration))]
public class ArtemisIdentityUserToken : UserToken
{
    /// <summary>
    ///     凭据所属用户
    /// </summary>
    public virtual ArtemisIdentityUser User { get; set; } = null!;
}