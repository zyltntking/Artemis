﻿using Artemis.Data.Core;
using Microsoft.AspNetCore.Identity;

namespace Artemis.Shared.Identity;

/// <summary>
///     用户凭据
/// </summary>
public class UserClaim : IdentityUserClaim<Guid>, IMateSlot
{
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