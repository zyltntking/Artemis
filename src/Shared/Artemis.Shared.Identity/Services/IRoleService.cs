using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ServiceModel;
using Artemis.Data.Core;
using Artemis.Shared.Identity.Transfer;

namespace Artemis.Shared.Identity.Services;

/// <summary>
///     角色服务接口
/// </summary>
[ServiceContract]
public interface IRoleService
{
    /// <summary>
    ///     搜索角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("搜索角色")]
    Task<RolesResponse> FetchRolesAsync(FetchRolesRequest request);

    /// <summary>
    ///     获取角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns>角色信息<see cref="RoleInfo" /></returns>
    [OperationContract]
    [Description("获取角色")]
    Task<RoleResponse> GetRoleAsync(GetRoleRequest request);

    /// <summary>
    ///     创建角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("创建角色")]
    Task<RoleResponse> CreateRoleAsync(CreateRoleRequest request);

    /// <summary>
    ///     创建角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("创建角色")]
    Task<EmptyResult> CreateRolesAsync(CreateRolesRequest request);

    /// <summary>
    ///     更新角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("更新角色")]
    Task<RoleResponse> UpdateRoleAsync(UpdateRoleRequest request);

    /// <summary>
    ///     更新角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("更新角色")]
    Task<EmptyResult> UpdateRolesAsync(UpdateRolesRequest request);

    /// <summary>
    ///     更新或创建角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("更新或创建角色")]
    Task<RoleResponse> UpdateOrCreateRoleAsync(UpdateRoleRequest request);

    /// <summary>
    ///     删除角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("删除角色")]
    Task<EmptyResult> DeleteRoleAsync(DeleteRoleRequest request);

    /// <summary>
    ///     删除角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("删除角色")]
    Task<EmptyResult> DeleteRolesAsync(DeleteRolesRequest request);

    /// <summary>
    ///     查询角色用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("查询角色用户")]
    Task<RoleUsersResponse> FetchRoleUsersAsync(FetchRoleUsersRequest request);

    /// <summary>
    ///     获取角色用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("获取角色用户")]
    Task<RoleUserResponse> GetRoleUserAsync(GetRoleUserRequest request);

    /// <summary>
    ///     添加角色用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("添加角色用户")]
    Task<EmptyResult> AddRoleUserAsync(AddRoleUserRequest request);

    /// <summary>
    ///     添加角色用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("添加角色用户")]
    Task<EmptyResult> AddRoleUsersAsync(AddRoleUsersRequest request);

    /// <summary>
    ///     删除角色用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("删除角色用户")]
    Task<EmptyResult> RemoveRoleUserAsync(RemoveRoleUserRequest request);

    /// <summary>
    ///     删除角色用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("删除角色用户")]
    Task<EmptyResult> RemoveRoleUsersAsync(RemoveRoleUsersRequest request);

    /// <summary>
    ///     查询角色凭据
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("查询角色凭据")]
    Task<RoleClaimsResponse> FetchRoleClaimsAsync(FetchRoleClaimsRequest request);

    /// <summary>
    ///     获取角色凭据
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("获取角色凭据")]
    Task<RoleClaimResponse> GetRoleClaimAsync(GetRoleClaimRequest request);

    /// <summary>
    ///     添加角色凭据
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("添加角色凭据")]
    Task<EmptyResult> AddRoleClaimAsync(AddRoleClaimRequest request);

    /// <summary>
    ///     添加角色凭据
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("添加角色凭据")]
    Task<EmptyResult> AddRoleClaimsAsync(AddRoleClaimsRequest request);

    /// <summary>
    ///     删除角色凭据
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("删除角色凭据")]
    Task<EmptyResult> RemoveRoleClaimAsync(RemoveRoleClaimRequest request);

    /// <summary>
    ///     删除角色凭据
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("删除角色凭据")]
    Task<EmptyResult> RemoveRoleClaimsAsync(RemoveRoleClaimsRequest request);
}

/// <summary>
///     查询角色过滤器
/// </summary>
[DataContract]
public sealed record FetchRolesFilter
{
    /// <summary>
    ///     角色名搜索值
    /// </summary>
    [DataMember(Order = 1)]
    public string? RoleNameSearch { get; set; }
}

/// <summary>
///     查询角色请求
/// </summary>
[DataContract]
public sealed record FetchRolesRequest : PageRequest<FetchRolesFilter>
{
    #region Overrides of GrpcPageRequest<FetchRolesFilter>

    /// <summary>
    ///     当前页码(从1开始)
    /// </summary>
    /// <example>1</example>
    [Required]
    [DataMember(Order = 1)]
    public override required int Page { get; set; }

    /// <summary>
    ///     页面大小
    /// </summary>
    /// <example>20</example>
    [Required]
    [DataMember(Order = 2)]
    public override required int Size { get; set; }

    /// <summary>
    ///     过滤条件
    /// </summary>
    [Required]
    [DataMember(Order = 3)]
    public override required FetchRolesFilter Filter { get; set; }

    #endregion
}

/// <summary>
///     RoleInfo分页响应
/// </summary>
[DataContract]
public sealed record RolesResponse : DataResult<PageResult<RoleInfo>>
{

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
    public virtual required Guid RoleId { get; set; }
}

/// <summary>
///     RoleInfo响应
/// </summary>
[DataContract]
public sealed record RoleResponse : DataResult<RoleInfo>
{
}

/// <summary>
///     创建角色请求
/// </summary>
[DataContract]
public sealed record CreateRoleRequest : RolePackage
{
    #region Implementation of RolePackage

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
    [DataMember(Order = 2)]
    public override string? Description { get; set; }

    #endregion
}

/// <summary>
///     创建角色请求
/// </summary>
[DataContract]
public sealed record CreateRolesRequest
{
    /// <summary>
    ///     角色信息
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public required List<RolePackage> RolePackages { get; set; }
}

/// <summary>
///     更新角色请求
/// </summary>
[DataContract]
public sealed record UpdateRoleRequest : GetRoleRequest
{
    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required Guid RoleId { get; set; }

    /// <summary>
    ///     角色信息
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public required RolePackage RolePackage { get; set; }
}

/// <summary>
///     更新角色请求
/// </summary>
[DataContract]
public sealed record UpdateRolesRequest
{
    /// <summary>
    ///     角色信息
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public required Dictionary<Guid, RolePackage> RolePackages { get; set; }
}

/// <summary>
///     删除角色请求
/// </summary>
[DataContract]
public sealed record DeleteRoleRequest : GetRoleRequest
{
    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required Guid RoleId { get; set; }
}

/// <summary>
///     删除角色请求
/// </summary>
[DataContract]
public sealed record DeleteRolesRequest
{
    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public required List<Guid> RoleIds { get; set; }
}

/// <summary>
///     查询角色用户过滤器
/// </summary>
[DataContract]
public sealed record FetchRoleUsersFilter : GetRoleRequest
{
    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required Guid RoleId { get; set; }

    /// <summary>
    ///     用户名搜索值
    /// </summary>
    [DataMember(Order = 2)]
    public string? UserNameSearch { get; set; }

    /// <summary>
    ///     邮箱搜索值
    /// </summary>
    [DataMember(Order = 3)]
    public string? EmailSearch { get; set; }

    /// <summary>
    ///     电话号码搜索值
    /// </summary>
    [DataMember(Order = 4)]
    public string? PhoneNumberSearch { get; set; }
}

/// <summary>
///     查询角色用户请求
/// </summary>
[DataContract]
public sealed record FetchRoleUsersRequest : DataResult<PageResult<FetchRoleUsersFilter>>
{
}

/// <summary>
///     查询角色用户响应
/// </summary>
[DataContract]
public sealed record RoleUsersResponse : DataResult<PageResult<UserInfo>>
{
}

/// <summary>
///     获取角色用户请求
/// </summary>
[DataContract]
public sealed record GetRoleUserRequest : GetRoleRequest
{
    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required Guid RoleId { get; set; }

    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public required Guid UserId { get; set; }
}

/// <summary>
///     角色用户信息响应
/// </summary>
[DataContract]
public sealed record RoleUserResponse : DataResult<UserInfo>
{
}

/// <summary>
///     添加角色用户请求
/// </summary>
[DataContract]
public sealed record AddRoleUserRequest : GetRoleRequest
{
    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required Guid RoleId { get; set; }

    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public required Guid UserId { get; set; }
}

/// <summary>
///     添加角色用户请求
/// </summary>
[DataContract]
public sealed record AddRoleUsersRequest : GetRoleRequest
{
    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required Guid RoleId { get; set; }

    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public required List<Guid> UserIds { get; set; }
}

/// <summary>
///     删除角色用户请求
/// </summary>
[DataContract]
public sealed record RemoveRoleUserRequest : GetRoleRequest
{
    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required Guid RoleId { get; set; }

    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public required Guid UserId { get; set; }
}

/// <summary>
///     删除角色用户请求
/// </summary>
[DataContract]
public sealed record RemoveRoleUsersRequest : GetRoleRequest
{
    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required Guid RoleId { get; set; }

    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public required List<Guid> UserIds { get; set; }
}

/// <summary>
///     查询角色凭据过滤器
/// </summary>
[DataContract]
public sealed record FetchRoleClaimsFilter : GetRoleRequest
{
    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required Guid RoleId { get; set; }

    /// <summary>
    ///     凭据类型搜索值
    /// </summary>
    [DataMember(Order = 2)]
    public string? ClaimTypeSearch { get; set; }
}

/// <summary>
///     查询角色凭据请求
/// </summary>
[DataContract]
public sealed record FetchRoleClaimsRequest : DataResult<PageResult<FetchRoleClaimsFilter>>
{
    
}

/// <summary>
///     角色凭据信息响应
/// </summary>
[DataContract]
public sealed record RoleClaimsResponse : DataResult<PageResult<RoleClaimInfo>>
{
    
}

/// <summary>
///     获取角色凭据请求
/// </summary>
[DataContract]
public sealed record GetRoleClaimRequest : GetRoleRequest
{
    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required Guid RoleId { get; set; }

    /// <summary>
    ///     凭据标识
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public required int RoleClaimId { get; set; }
}

/// <summary>
///     角色凭据信息响应
/// </summary>
[DataContract]
public sealed record RoleClaimResponse : DataResult<RoleClaimInfo>
{
}

/// <summary>
///     添加角色凭据请求
/// </summary>
[DataContract]
public sealed record AddRoleClaimRequest : GetRoleRequest
{
    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required Guid RoleId { get; set; }

    /// <summary>
    ///     凭据信息
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public required RoleClaimPackage ClaimPackage { get; set; }
}

/// <summary>
///     添加角色凭据请求
/// </summary>
[DataContract]
public sealed record AddRoleClaimsRequest : GetRoleRequest
{
    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required Guid RoleId { get; set; }

    /// <summary>
    ///     凭据信息
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public required List<RoleClaimPackage> ClaimPackages { get; set; }
}

/// <summary>
///     删除角色凭据请求
/// </summary>
[DataContract]
public sealed record RemoveRoleClaimRequest : GetRoleRequest
{
    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required Guid RoleId { get; set; }

    /// <summary>
    ///     凭据标识
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public required int RoleClaimId { get; set; }
}

/// <summary>
///     删除角色凭据请求
/// </summary>
[DataContract]
public sealed record RemoveRoleClaimsRequest : GetRoleRequest
{
    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required Guid RoleId { get; set; }

    /// <summary>
    ///     凭据标识
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public required List<int> RoleClaimIds { get; set; }
}