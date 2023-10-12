using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Artemis.Data.Core;
using Artemis.Shared.Identity.Transfer;
using Microsoft.AspNetCore.Identity;

namespace Artemis.Shared.Identity.Models;

/// <summary>
///     用户角色映射
/// </summary>
[DataContract]
public class UserRole : IdentityUserRole<Guid>, IKeySlot<int>, IUserRole
{
    /// <summary>
    ///     存储标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public virtual required int Id { get; set; }

    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public override required Guid UserId { get; set; }

    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 3)]
    public override required Guid RoleId { get; set; }
}