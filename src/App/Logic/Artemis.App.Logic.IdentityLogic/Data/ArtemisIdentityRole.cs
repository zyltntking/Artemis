using Microsoft.AspNetCore.Identity;

namespace Artemis.App.Logic.IdentityLogic.Data;

public class ArtemisIdentityRole : IdentityRole<Guid>
{
    public virtual ICollection<ArtemisIdentityUserRole> UserRoles { get; set; }
    public virtual ICollection<ArtemisIdentityRoleClaim> RoleClaims { get; set; }
}