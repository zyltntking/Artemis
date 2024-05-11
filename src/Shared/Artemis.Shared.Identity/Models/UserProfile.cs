using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core.Fundamental.Model;

namespace Artemis.Shared.Identity.Models;

/// <summary>
///     用户档案
/// </summary>
public class UserProfile : MetadataInfo
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    public virtual required Guid UserId { get; set; }

    /// <summary>
    ///     元数据键
    /// </summary>
    [Required]
    [MaxLength(64)]
    public override required string Key { get; set; }

    /// <summary>
    ///     元数据值
    /// </summary>
    [MaxLength(256)]
    public override string? Value { get; set; }
}