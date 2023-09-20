using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ServiceModel;
using Artemis.Data.Core;
using Artemis.Shared.Identity.Models;
using Artemis.Shared.Identity.Records;

namespace Artemis.Shared.Identity.Services;

/// <summary>
///     角色服务接口
/// </summary>
[ServiceContract]
public interface IRoleService
{
    /// <summary>
    ///     获取角色
    /// </summary>
    /// <param name="request">获取角色请求</param>
    /// <returns></returns>
    [OperationContract]
    Task<DataResult<Role>> GetRoleAsync(GetRoleRequest request);

    /// <summary>
    ///     查询角色
    /// </summary>
    /// <param name="request">查询角色请求</param>
    /// <returns></returns>
    [OperationContract]
    Task<DataResult<PageResult<Role>>> FetchRolesAsync(PageRequest<FetchRolesFilter> request);

    /// <summary>
    ///     创建角色
    /// </summary>
    /// <param name="request">创建角色请求</param>
    /// <returns></returns>
    [OperationContract]
    Task<DataResult<EmptyRecord>> CreateRoleAsync(CreateRoleRequest request);

    /// <summary>
    ///     更新角色
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [OperationContract]
    Task<DataResult<EmptyRecord>> UpdateRoleAsync(UpdateRoleRequest request);

    /// <summary>
    ///     创建或更新角色
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [OperationContract]
    Task<DataResult<EmptyRecord>> CreateOrUpdateRoleAsync(UpdateRoleRequest request);

    /// <summary>
    ///     删除角色
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [OperationContract]
    Task<DataResult<EmptyRecord>> DeleteRoleAsync(DeleteRoleRequest request);

    /// <summary>
    ///     删除角色
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [OperationContract]
    Task<DataResult<EmptyRecord>> DeleteRolesAsync(DeleteRolesRequest request);

    /// <summary>
    ///     查询角色用户
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [OperationContract]
    Task<DataResult<PageResult<User>>> FetchRoleUsersAsync(PageRequest<FetchRoleUsersFilter> request);
}

/// <summary>
///     获取角色请求
/// </summary>
[DataContract]
public record GetRoleRequest
{
    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public Guid RoleId { get; set; }
}

/// <summary>
///     查询角色过滤器
/// </summary>
[DataContract]
public record FetchRolesFilter
{
    /// <summary>
    ///     角色名搜索值
    /// </summary>
    [DataMember(Order = 1)]
    public string? RoleNameSearch { get; set; }
}

/// <summary>
///     创建角色请求
/// </summary>
[DataContract]
public record CreateRoleRequest
{
    /// <summary>
    ///     角色名
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public string Name { get; set; } = null!;

    /// <summary>
    ///     角色描述
    /// </summary>
    [DataMember(Order = 2)]
    public string? Description { get; set; }
}

/// <summary>
///     更新角色请求
/// </summary>
[DataContract]
public class UpdateRoleRequest : Role
{
}

/// <summary>
///     删除角色请求
/// </summary>
[DataContract]
public record DeleteRoleRequest
{
    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public Guid RoleId { get; set; }
}

/// <summary>
///     删除角色请求
/// </summary>
public record DeleteRolesRequest
{
    /// <summary>
    ///     角色标识列表
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public IEnumerable<Guid> RoleIds { get; set; } = null!;
}

/// <summary>
///     查询用户过滤器
/// </summary>
[DataContract]
public record FetchRoleUsersFilter
{
    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public Guid RoleId { get; set; }


    /// <summary>
    ///     用户名搜索值
    /// </summary>
    [DataMember(Order = 2)]
    public string? UserNameSearch { get; set; }

    /// <summary>
    ///     必要字段
    /// </summary>
    [Required]
    [DataMember(Order = 3)]
    public string RequiredParameter { get; set; } = null!;
}