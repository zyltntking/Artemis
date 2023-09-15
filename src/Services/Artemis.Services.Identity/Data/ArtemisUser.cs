using Artemis.Services.Identity.Data.Configurations;
using Artemis.Shared.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Services.Identity.Data;

/// <summary>
///     ArtemisUser
/// </summary>
[EntityTypeConfiguration(typeof(UserConfiguration))]
public class ArtemisUser : User
{
    /// <summary>
    ///     用户具备的角色
    /// </summary>
    public virtual ICollection<ArtemisRole>? Roles { get; set; }

    /// <summary>
    ///     用户角色映射
    /// </summary>
    public virtual ICollection<ArtemisUserRole>? UserRoles { get; set; }

    /// <summary>
    ///     用户凭据映射
    /// </summary>
    public virtual ICollection<ArtemisUserClaim>? Claims { get; set; }

    /// <summary>
    ///     用户登录映射
    /// </summary>
    public virtual ICollection<ArtemisUserLogin>? Logins { get; set; }

    /// <summary>
    ///     用户令牌映射
    /// </summary>
    public virtual ICollection<ArtemisUserToken>? Tokens { get; set; }
}