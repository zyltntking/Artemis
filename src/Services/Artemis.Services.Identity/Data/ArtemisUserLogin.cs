using Artemis.Services.Identity.Data.Configurations;
using Artemis.Shared.Identity;
using Artemis.Shared.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Services.Identity.Data;

/// <summary>
///     ArtemisUserLogin
/// </summary>
[EntityTypeConfiguration(typeof(UserLoginConfiguration))]
public class ArtemisUserLogin : UserLogin
{
    /// <summary>
    ///     登录信息所属用户
    /// </summary>
    public virtual ArtemisUser User { get; set; } = null!;
}