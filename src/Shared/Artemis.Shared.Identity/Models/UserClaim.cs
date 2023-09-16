using Microsoft.AspNetCore.Identity;

namespace Artemis.Shared.Identity.Models;

/// <summary>
///     用户凭据
/// </summary>
public class UserClaim : IdentityUserClaim<Guid>
{
}