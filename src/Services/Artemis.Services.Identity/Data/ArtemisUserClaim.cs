using Artemis.Data.Core;
using Artemis.Services.Identity.Data.Configurations;
using Artemis.Shared.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Services.Identity.Data;

/// <summary>
///     ArtemisUserClaim
/// </summary>
[EntityTypeConfiguration(typeof(UserClaimConfiguration))]
public class ArtemisUserClaim : UserClaim, IModelBase<int>
{
    /// <summary>
    ///     所属用户
    /// </summary>
    public virtual ArtemisUser User { get; set; } = null!;

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