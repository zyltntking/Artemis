using Artemis.Service.Identity.Context.Configuration;
using Artemis.Service.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Identity.Context;

/// <summary>
/// 认证用户档案实体
/// </summary>
[EntityTypeConfiguration(typeof(IdentityUserProfileConfiguration))]
public sealed class IdentityUserProfile : UserProfile
{
    /// <summary>
    ///     所属用户
    /// </summary>
    public required IdentityUser User { get; set; }
}