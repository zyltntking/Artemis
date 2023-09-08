using Microsoft.AspNetCore.Identity;

namespace Artemis.App.Logic.IdentityLogic.Data;

public class ArtemisIdentityUserToken : IdentityUserToken<Guid>
{
    public virtual ArtemisIdentityUser User { get; set; }
}