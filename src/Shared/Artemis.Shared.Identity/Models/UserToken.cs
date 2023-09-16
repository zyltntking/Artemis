using Artemis.Data.Core;
using Microsoft.AspNetCore.Identity;

namespace Artemis.Shared.Identity.Models;

/// <summary>
///     用户令牌
/// </summary>
public class UserToken : IdentityUserToken<Guid>, IKeySlot<int>
{
    #region Implementation of IKeySlot<int>

    /// <summary>
    ///     存储标识
    /// </summary>
    public int Id { get; set; }

    #endregion
}