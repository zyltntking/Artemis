using Microsoft.AspNetCore.Identity;

namespace Artemis.Services.Identity.Data;

public class ArtemisIdentityRoleClaim : IdentityRoleClaim<Guid>
{
    public virtual ArtemisIdentityRole Role { get; set; }
}