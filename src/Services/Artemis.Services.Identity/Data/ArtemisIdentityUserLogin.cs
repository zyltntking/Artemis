using Artemis.Services.Identity.Data.Configurations;
using Artemis.Shared.Identity;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Services.Identity.Data;

/// <summary>
///     ArtemisIdentityUserLogin
/// </summary>
[EntityTypeConfiguration(typeof(UserLoginConfiguration))]
public class ArtemisIdentityUserLogin : UserLogin
{
    /// <summary>
    ///     登录信息所属用户
    /// </summary>
    public virtual ArtemisIdentityUser User { get; set; } = null!;
}