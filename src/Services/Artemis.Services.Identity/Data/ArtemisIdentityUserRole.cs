using Microsoft.AspNetCore.Identity;

namespace Artemis.Services.Identity.Data;

public class ArtemisIdentityUserRole : IdentityUserRole<Guid>
{
    public virtual ArtemisIdentityUser User { get; set; }
    public virtual ArtemisIdentityRole Role { get; set; }
}