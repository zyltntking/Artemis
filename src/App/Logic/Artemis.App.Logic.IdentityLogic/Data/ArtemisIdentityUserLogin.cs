using Microsoft.AspNetCore.Identity;

namespace Artemis.App.Logic.IdentityLogic.Data;

public class ArtemisIdentityUserLogin : IdentityUserLogin<Guid>
{
    public virtual ArtemisIdentityUser User { get; set; }
}