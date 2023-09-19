using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ServiceModel;
using Artemis.Data.Core;
using Artemis.Shared.Identity.Models;
using Grpc.Core;

namespace Artemis.Shared.Identity;

/// <summary>
/// 角色服务接口
/// </summary>
[ServiceContract]
public interface IRoleService
{
    /// <summary>
    /// 获取角色
    /// </summary>
    /// <param name="request">角色名</param>
    /// <param name="context"></param>
    /// <returns></returns>
    [OperationContract]
    Task<DataResult<Role>> GetRole(GetRoleRequest request);
}

/// <summary>
/// 获取角色请求
/// </summary>
[DataContract]
public record GetRoleRequest
{
    /// <summary>
    /// 角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public Guid RoleId { get; set; }
}