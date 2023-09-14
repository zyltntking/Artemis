using Artemis.Services.Identity.Data.Configurations;
using Artemis.Shared.Identity;
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

    /// <summary>
    ///     用户角色映射
    /// </summary>
    public virtual ICollection<ArtemisUserRole>? UserRoles { get; set; }

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