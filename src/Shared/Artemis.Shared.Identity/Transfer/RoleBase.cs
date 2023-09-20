using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Artemis.Shared.Identity.Transfer.Interface;

namespace Artemis.Shared.Identity.Transfer;

/// <summary>
/// 基本角色信息
/// </summary>
[DataContract]
public abstract record RoleBase : IRole
{
    #region Implementation of IRole

    /// <summary>
    /// 角色名
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public virtual required string Name { get; set; } = null!;

    /// <summary>
    /// 角色描述
    /// </summary>
    [DataMember(Order = 2)]
    public virtual string? Description { get; set; }

    #endregion
}