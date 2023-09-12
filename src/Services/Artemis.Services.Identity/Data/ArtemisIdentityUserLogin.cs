using Microsoft.AspNetCore.Identity;

namespace Artemis.Services.Identity.Data;

public class ArtemisIdentityUserLogin : IdentityUserLogin<Guid>
{
    public virtual ArtemisIdentityUser User { get; set; }
}