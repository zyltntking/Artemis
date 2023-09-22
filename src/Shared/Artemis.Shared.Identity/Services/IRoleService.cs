using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ServiceModel;
using Artemis.Data.Core;
using Artemis.Shared.Identity.Transfer;
using ProtoBuf;

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

    /// <summary>
    ///     查询角色凭据
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [OperationContract]
    Task<DataResult<PageResult<RoleClaimInfo>>> FetchRoleClaimsAsync(PageRequest<FetchRoleClaimsFilter> request);

    /// <summary>
    ///     获取角色凭据
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [OperationContract]
    Task<DataResult<RoleClaimInfo>> GetRoleClaimAsync(GetRoleClaimRequest request);
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
    [NullWrappedValue]
    [DataMember(Order = 1)]
    public string? RoleNameSearch { get; set; }
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
    public virtual required string RoleName { get; set; }
}

/// <summary>
///     创建角色请求
/// </summary>
[DataContract]
public record CreateRoleRequest : RoleBase
{
    #region Implementation of RoleBase

    /// <summary>
    ///     角色名
    /// </summary>
    [Required]
    [MaxLength(32)]
    [DataMember(Order = 1)]
    public override required string Name { get; set; }

    /// <summary>
    ///     角色描述
    /// </summary>
    [MaxLength(128)]
    [NullWrappedValue]
    [DataMember(Order = 2)]
    public override string? Description { get; set; }

    #endregion
}

/// <summary>
///     更新角色请求
/// </summary>
[DataContract]
public record UpdateRoleRequest : GetRoleRequest
{
    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required string RoleName { get; set; }

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
public record DeleteRoleRequest : GetRoleRequest
{
    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required string RoleName { get; set; }
}

/// <summary>
///     查询用户过滤器
/// </summary>
[DataContract]
public record FetchRoleUsersFilter : GetRoleRequest
{
    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required string RoleName { get; set; }

    /// <summary>
    ///     用户名搜索值
    /// </summary>
    [NullWrappedValue]
    [DataMember(Order = 2)]
    public string? UserNameSearch { get; set; }

    /// <summary>
    ///     邮箱搜索值
    /// </summary>
    [NullWrappedValue]
    [DataMember(Order = 3)]
    public string? EmailSearch { get; set; }

    /// <summary>
    ///     电话号码搜索值
    /// </summary>
    [NullWrappedValue]
    [DataMember(Order = 4)]
    public string? PhoneNumberSearch { get; set; }
}

/// <summary>
///     查询角色凭据过滤器
/// </summary>
[DataContract]
public record FetchRoleClaimsFilter : GetRoleRequest
{
    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required string RoleName { get; set; }

    /// <summary>
    ///     凭据类型搜索值
    /// </summary>
    [NullWrappedValue]
    [DataMember(Order = 2)]
    public string? ClaimTypeSearch { get; set; }
}

/// <summary>
///     获取角色凭据请求
/// </summary>
[DataContract]
public record GetRoleClaimRequest : GetRoleRequest
{
    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required string RoleName { get; set; }

    /// <summary>
    ///     凭据标识
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public required int ClaimId { get; set; }
}