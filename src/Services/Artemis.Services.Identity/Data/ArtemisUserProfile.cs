using Artemis.Services.Identity.Data.Configurations;
using Artemis.Shared.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Services.Identity.Data;

/// <summary>
///     ArtemisUserProfile
/// </summary>
[EntityTypeConfiguration(typeof(UserProfileConfiguration))]
public class ArtemisUserProfile : UserProfile
{
    /// <summary>
    ///     登录信息所属用户
    /// </summary>
    public virtual ArtemisUser User { get; set; } = null!;
}