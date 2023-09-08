using Microsoft.AspNetCore.Identity;

namespace Artemis.App.Logic.IdentityLogic.Data;

public class ArtemisIdentityUserClaim : IdentityUserClaim<Guid>
{
    public virtual ArtemisIdentityUser User { get; set; }
}