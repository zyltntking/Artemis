﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Identity;

namespace Artemis.Shared.Identity.Models;

/// <summary>
///     角色
/// </summary>
[DataContract]
public class Role : IdentityRole<Guid>
{
    /// <summary>
    ///     标识
    /// </summary>
    [DataMember(Order = 1)]
    public override Guid Id { get; set; }

    /// <summary>
    ///     角色名
    /// </summary>
    [DataMember(Order = 2)]
    [Required]
    [MaxLength(128)]
    public override string Name { get; set; } = null!;

    /// <summary>
    ///     规范化角色名
    /// </summary>
    [DataMember(Order = 3)]
    [Required]
    [MaxLength(128)]
    public override string NormalizedName { get; set; } = null!;

    /// <summary>
    ///     并发锁
    /// </summary>
    [DataMember(Order = 4)]
    [MaxLength(128)]
    public override string? ConcurrencyStamp { get; set; }

    /// <summary>
    ///     角色描述
    /// </summary>
    [DataMember(Order = 5)]
    [MaxLength(256)]
    public virtual string? Descripcion { get; set; }
}