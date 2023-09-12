using Microsoft.AspNetCore.Identity;

namespace Artemis.Services.Identity.Data;

public class ArtemisIdentityUserClaim : IdentityUserClaim<Guid>
{
    public virtual ArtemisIdentityUser User { get; set; }
}