using Microsoft.AspNetCore.Identity;

namespace Artemis.App.Logic.IdentityLogic.Data;

public class ArtemisIdentityRoleClaim : IdentityRoleClaim<Guid>
{
    public virtual ArtemisIdentityRole Role { get; set; }
}