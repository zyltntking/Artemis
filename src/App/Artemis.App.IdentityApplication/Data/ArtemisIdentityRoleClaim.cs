using Microsoft.AspNetCore.Identity;

namespace Artemis.App.IdentityApplication.Data;

public class ArtemisIdentityRoleClaim : IdentityRoleClaim<Guid>
{
    public virtual ArtemisIdentityRole Role { get; set; }
}