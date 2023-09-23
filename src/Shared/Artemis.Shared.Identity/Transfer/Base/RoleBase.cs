using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Artemis.Shared.Identity.Transfer.Interface;

namespace Artemis.Shared.Identity.Transfer.Base;

/// <summary>
///     基本角色信息
/// </summary>
[DataContract]
public record RoleBase : IRole
{
    #region Implementation of IRole

    /// <summary>
    ///     角色名
    /// </summary>
    [Required]
    [MaxLength(32)]
    [DataMember(Order = 1)]
    public virtual required string Name { get; set; }

    /// <summary>
    ///     角色描述
    /// </summary>
    [MaxLength(128)]
    [DataMember(Order = 2)]
    public virtual string? Description { get; set; }

    #endregion
}