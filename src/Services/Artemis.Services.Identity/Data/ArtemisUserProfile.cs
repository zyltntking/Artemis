using Artemis.Data.Core;
using Artemis.Services.Identity.Data.Configurations;
using Artemis.Shared.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Services.Identity.Data;

/// <summary>
///     ArtemisUserProfile
/// </summary>
[EntityTypeConfiguration(typeof(UserProfileConfiguration))]
public class ArtemisUserProfile : UserProfile, IModelBase
{
    /// <summary>
    ///     登录信息所属用户
    /// </summary>
    public virtual ArtemisUser User { get; set; } = null!;

    #region Implementation of IMateSlot

    /// <summary>
    ///     创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    ///     更新时间
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    ///     删除时间
    /// </summary>
    public DateTime? DeletedAt { get; set; }

    #endregion
}