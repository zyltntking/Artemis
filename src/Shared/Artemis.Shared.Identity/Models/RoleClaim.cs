﻿using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Shared.Identity.Transfer;
using Microsoft.AspNetCore.Identity;

#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).

namespace Artemis.Shared.Identity.Models;

/// <summary>
///     角色凭据
/// </summary>
public class RoleClaim : IdentityRoleClaim<Guid>, IKeySlot<int>, IRoleClaim
{
    /// <summary>
    ///     标识
    /// </summary>
    [Required]
    public override required int Id { get; set; }

    /// <summary>
    ///     凭据类型
    /// </summary>
    [Required]
    [MaxLength(32)]
    public override required string ClaimType { get; set; }

    /// <summary>
    ///     凭据值
    /// </summary>
    [Required]
    [MaxLength(128)]
    public override required string ClaimValue { get; set; }

    /// <summary>
    ///     校验戳
    /// </summary>
    [Required]
    [MaxLength(64)]
    public virtual required string CheckStamp { get; set; }

    /// <summary>
    ///     凭据描述
    /// </summary>
    [MaxLength(128)]
    public virtual string? Description { get; set; }

    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    public override required Guid RoleId { get; set; }
}