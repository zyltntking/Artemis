﻿using Artemis.Data.Core;
using Artemis.Services.Identity.Data.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Services.Identity.Data;

/// <summary>
/// ArtemisIdentityUserClaim
/// </summary>
[EntityTypeConfiguration(typeof(UserClaimConfiguration))]
public sealed class ArtemisIdentityUserClaim : IdentityUserClaim<Guid>, IMateSlot
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
    /// 所属用户
    /// </summary>
    public ArtemisIdentityUser User { get; set; } = null!;
}