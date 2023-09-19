using System.Runtime.Serialization;
using Artemis.Data.Core;
using Microsoft.AspNetCore.Identity;

namespace Artemis.Shared.Identity.Models;

/// <summary>
///     用户角色映射
/// </summary>
[DataContract]
public class UserRole : IdentityUserRole<Guid>, IKeySlot<int>
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [DataMember(Order = 2)]
    public override Guid UserId { get; set; }

    /// <summary>
    ///     角色标识
    /// </summary>
    [DataMember(Order = 3)]
    public override Guid RoleId { get; set; }

    /// <summary>
    ///     存储标识
    /// </summary>
    [DataMember(Order = 1)]
    public virtual int Id { get; set; }
}