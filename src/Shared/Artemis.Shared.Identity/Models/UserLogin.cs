using Artemis.Data.Core;
using Microsoft.AspNetCore.Identity;

namespace Artemis.Shared.Identity.Models;

/// <summary>
///     用户登录信息
/// </summary>
public class UserLogin : IdentityUserLogin<Guid>, IKeySlot<int>
{
    #region Implementation of IKeySlot<int>

    /// <summary>
    ///     存储标识
    /// </summary>
    public int Id { get; set; }

    #endregion
}