using System.ComponentModel.DataAnnotations;
using Artemis.Data.Shared.Identity;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Identity.Models;

/// <summary>
///     用户凭据模型
/// </summary>
public class UserClaim : StandardClaim, IUserClaim
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    [Comment("用户标识")]
    public Guid UserId { get; set; }
}