using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ServiceModel;
using Artemis.Data.Core;
using Artemis.Shared.Identity.Records;
using Artemis.Shared.Identity.Transfer;

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
    /// <returns>角色信息<see cref="RoleInfo" /></returns>
    [OperationContract]
    Task<DataResult<RoleInfo>> GetRoleAsync(GetRoleRequest request);

    /// <summary>
    ///     查询角色
    /// </summary>
    /// <param name="request">查询角色请求</param>
    /// <returns></returns>
    [OperationContract]
    Task<DataResult<PageResult<RoleInfo>>> FetchRolesAsync(PageRequest<FetchRolesFilter> request);

    /// <summary>
    ///     创建角色
    /// </summary>
    /// <param name="roleRequest">创建角色请求</param>
    /// <returns></returns>
    [OperationContract]
    Task<DataResult<EmptyRecord>> CreateRoleAsync(CreateRoleRequest roleRequest);

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
    ///     查询角色用户
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [OperationContract]
    Task<DataResult<PageResult<UserInfo>>> FetchRoleUsersAsync(PageRequest<FetchRoleUsersFilter> request);
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
    public required string RoleName { get; set; } = null!;
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
public record CreateRoleRequest : RoleBase
{
    /// <summary>
    ///     角色名
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required string Name { get; set; } = null!;

    /// <summary>
    ///     角色描述
    /// </summary>
    [DataMember(Order = 2)]
    public override required string? Description { get; set; }
}

/// <summary>
///     更新角色请求
/// </summary>
[DataContract]
public record UpdateRoleRequest
{
    /// <summary>
    ///     角色名
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public required string RoleName { get; set; } = null!;

    /// <summary>
    ///     角色信息
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public required RoleBase RoleInfo { get; set; } = null!;
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
    public required string RoleName { get; set; } = null!;
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
    public required string RoleName { get; set; }


    /// <summary>
    ///     用户名搜索值
    /// </summary>
    [DataMember(Order = 2)]
    public string? UserNameSearch { get; set; }
}