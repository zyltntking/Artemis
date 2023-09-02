using Microsoft.AspNetCore.Identity;

namespace Artemis.App.IdentityApplication.Data;

public class ArtemisIdentityRole : IdentityRole<Guid>
{
    public virtual ICollection<ArtemisIdentityUserRole> UserRoles { get; set; }
    public virtual ICollection<ArtemisIdentityRoleClaim> RoleClaims { get; set; }
}