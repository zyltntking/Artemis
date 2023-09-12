using Microsoft.AspNetCore.Identity;

namespace Artemis.Services.Identity.Data;

public class ArtemisIdentityRole : IdentityRole<Guid>
{
    public virtual ICollection<ArtemisIdentityUserRole> UserRoles { get; set; }
    public virtual ICollection<ArtemisIdentityRoleClaim> RoleClaims { get; set; }
}