using Artemis.Data.Core;
using Artemis.Services.Identity.Data.Configurations;
using Artemis.Shared.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Services.Identity.Data;

/// <summary>
///     ArtemisUser
/// </summary>
[EntityTypeConfiguration(typeof(UserConfiguration))]
public class ArtemisUser : User, IConcurrencyModelBase
{
    /// <summary>
    ///     用户具备的角色
    /// </summary>
    public virtual ICollection<ArtemisRole> Roles { get; } = new List<ArtemisRole>();

    /// <summary>
    ///     用户角色映射
    /// </summary>
    public virtual ICollection<ArtemisUserRole> UserRoles { get; } = new List<ArtemisUserRole>();

    /// <summary>
    ///     用户凭据映射
    /// </summary>
    public virtual ICollection<ArtemisUserClaim> UserClaims { get; } = new List<ArtemisUserClaim>();

    /// <summary>
    ///     用户登录映射
    /// </summary>
    public virtual ICollection<ArtemisUserLogin> UserLogins { get; } = new List<ArtemisUserLogin>();

    /// <summary>
    ///     用户令牌映射
    /// </summary>
    public virtual ICollection<ArtemisUserToken> UserTokens { get; } = new List<ArtemisUserToken>();

    /// <summary>
    ///     用户信息
    /// </summary>
    public virtual ICollection<ArtemisUserProfile> UserProfiles { get; } = new List<ArtemisUserProfile>();

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