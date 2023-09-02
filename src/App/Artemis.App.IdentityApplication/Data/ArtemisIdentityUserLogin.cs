using Microsoft.AspNetCore.Identity;

namespace Artemis.App.IdentityApplication.Data;

public class ArtemisIdentityUserLogin : IdentityUserLogin<Guid>
{
    public virtual ArtemisIdentityUser User { get; set; }
}