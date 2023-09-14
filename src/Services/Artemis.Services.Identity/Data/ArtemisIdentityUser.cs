using Artemis.Services.Identity.Data.Configurations;
using Artemis.Shared.Identity;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Services.Identity.Data;

/// <summary>
///     ArtemisIdentityUser
/// </summary>
[EntityTypeConfiguration(typeof(UserConfiguration))]
public class ArtemisIdentityUser : User
{
    /// <summary>
    ///     用户凭据映射
    /// </summary>
    public virtual ICollection<ArtemisIdentityUserClaim>? Claims { get; set; }

    /// <summary>
    ///     用户登录映射
    /// </summary>
    public virtual ICollection<ArtemisIdentityUserLogin>? Logins { get; set; }

    /// <summary>
    ///     用户令牌映射
    /// </summary>
    public virtual ICollection<ArtemisIdentityUserToken>? Tokens { get; set; }

    /// <summary>
    ///     用户角色映射
    /// </summary>
    public virtual ICollection<ArtemisIdentityUserRole>? UserRoles { get; set; }

    #region Implementation of IMateSlot

    /// <summary>
    ///     创建时间
    /// </summary>
    public virtual DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    ///     更新时间
    /// </summary>
    public virtual DateTime UpdatedAt { get; set; } = DateTime.Now;

    /// <summary>
    ///     删除时间
    /// </summary>
    public virtual DateTime? DeletedAt { get; set; }

    #endregion
}