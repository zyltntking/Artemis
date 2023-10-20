using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Shared.Identity.Transfer;

namespace Artemis.Shared.Identity.Models;

/// <summary>
///     凭据字典
/// </summary>
public class Claim : IKeySlot<Guid>, IClaim
{
    /// <summary>
    ///     凭据类型
    /// </summary>
    [Required]
    [MaxLength(32)]
    public required string ClaimType { get; set; }

    /// <summary>
    ///     凭据值
    /// </summary>
    [Required]
    [MaxLength(128)]
    public required string ClaimValue { get; set; }

    /// <summary>
    ///     校验戳
    /// </summary>
    [Required]
    [MaxLength(64)]
    public required string CheckStamp { get; set; }

    /// <summary>
    ///     凭据描述
    /// </summary>
    [MaxLength(128)]
    public string? Description { get; set; }

    /// <summary>
    ///     凭据标识
    /// </summary>
    [Required]
    public required Guid Id { get; set; }
}