using Microsoft.AspNetCore.Identity;

namespace Artemis.Services.Identity.Data;

public class ArtemisIdentityUser : IdentityUser<Guid>
{
    public virtual ICollection<ArtemisIdentityUserClaim> Claims { get; set; }
    public virtual ICollection<ArtemisIdentityUserLogin> Logins { get; set; }
    public virtual ICollection<ArtemisIdentityUserToken> Tokens { get; set; }
    public virtual ICollection<ArtemisIdentityUserRole> UserRoles { get; set; }
}