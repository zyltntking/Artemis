using Microsoft.AspNetCore.Identity;

namespace Artemis.App.IdentityApplication.Data;

public class ArtemisIdentityUserClaim : IdentityUserClaim<Guid>
{
    public virtual ArtemisIdentityUser User { get; set; }
}