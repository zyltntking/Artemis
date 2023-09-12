using Microsoft.AspNetCore.Identity;

namespace Artemis.Services.Identity.Data;

public class ArtemisIdentityUserToken : IdentityUserToken<Guid>
{
    public virtual ArtemisIdentityUser User { get; set; }
}