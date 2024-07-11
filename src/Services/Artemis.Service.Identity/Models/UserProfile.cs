using System.ComponentModel.DataAnnotations;
using Artemis.Service.Shared.Identity;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Identity.Models;

/// <summary>
///     用户档案模型
/// </summary>
public class UserProfile : IUserProfile
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    [Comment("用户标识")]
    public Guid UserId { get; set; }

    /// <summary>
    ///     数据键
    /// </summary>
    [Required]
    [MaxLength(64)]
    [Comment("用户档案数据键")]
    public required string Key { get; set; }

    /// <summary>
    ///     数据值
    /// </summary>
    [MaxLength(256)]
    [Comment("用户档案数据值")]
    public string? Value { get; set; }
}