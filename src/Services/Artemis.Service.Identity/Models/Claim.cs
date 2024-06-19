using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Data.Shared.Identity;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Identity.Models;

/// <summary>
///     凭据模型
/// </summary>
public class Claim : KeySlot<int>, IClaim, ICheckStamp
{
    /// <summary>
    ///     凭据类型
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("凭据类型")]
    public required string ClaimType { get; set; }

    /// <summary>
    ///     凭据值
    /// </summary>
    [Required]
    [MaxLength(512)]
    [Comment("凭据值")]
    public required string ClaimValue { get; set; }

    /// <summary>
    ///     校验戳
    /// </summary>
    [Required]
    [MaxLength(64)]
    [Comment("校验戳")]
    public required string CheckStamp { get; set; }

    /// <summary>
    ///     凭据描述
    /// </summary>
    [MaxLength(128)]
    [Comment("凭据描述")]
    public string? Description { get; set; }
}