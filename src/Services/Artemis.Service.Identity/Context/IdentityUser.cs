using Artemis.Service.Identity.Context.Configuration;
using Artemis.Service.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Identity.Context;

/// <summary>
///     认证用户实体
/// </summary>
[EntityTypeConfiguration(typeof(IdentityUserConfiguration))]
public sealed class IdentityUser : User
{
    /// <summary>
    ///     用户具备的角色
    /// </summary>
    public ICollection<IdentityRole>? Roles { get; set; }

    /// <summary>
    ///     用户角色映射
    /// </summary>
    public ICollection<IdentityUserRole>? UserRoles { get; set; }

    /// <summary>
    ///     用户凭据映射
    /// </summary>
    public ICollection<IdentityUserClaim>? UserClaims { get; set; }

    /// <summary>
    ///     用户登录映射
    /// </summary>
    public ICollection<IdentityUserLogin>? UserLogins { get; set; }

    /// <summary>
    ///     用户令牌映射
    /// </summary>
    public ICollection<IdentityUserToken>? UserTokens { get; set; }

    ///// <summary>
    /////     用户信息
    ///// </summary>
    //public ICollection<ArtemisUserProfile> UserProfiles { get; } = new List<ArtemisUserProfile>();
}