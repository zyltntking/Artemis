﻿using Microsoft.AspNetCore.Identity;

namespace Artemis.App.IdentityApplication.Data;

public class ArtemisIdentityUserToken : IdentityUserToken<Guid>
{
    public virtual ArtemisIdentityUser User { get; set; }
}