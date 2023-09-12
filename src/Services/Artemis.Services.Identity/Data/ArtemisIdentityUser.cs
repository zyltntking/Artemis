using Artemis.Data.Core;
using Artemis.Services.Identity.Data.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Services.Identity.Data;

/// <summary>
/// ArtemisIdentityUser
/// </summary>
[EntityTypeConfiguration(typeof(UserConfiguration))]
public sealed class ArtemisIdentityUser : IdentityUser<Guid>, IMateSlot
{
    #region Implementation of IMateSlot

    /// <summary>
    ///     创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    ///     更新时间
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    /// <summary>
    ///     删除时间
    /// </summary>
    public DateTime? DeletedAt { get; set; }

    #endregion

    /// <summary>
    /// 用户凭据映射
    /// </summary>
    public ICollection<ArtemisIdentityUserClaim>? Claims { get; set; }

    /// <summary>
    /// 用户登录映射
    /// </summary>
    public ICollection<ArtemisIdentityUserLogin>? Logins { get; set; }

    /// <summary>
    /// 用户令牌映射
    /// </summary>
    public ICollection<ArtemisIdentityUserToken>? Tokens { get; set; }

    /// <summary>
    /// 用户角色映射
    /// </summary>
    public ICollection<ArtemisIdentityUserRole>? UserRoles { get; set; }
}