using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ServiceModel;
using Artemis.Data.Grpc;
using Artemis.Shared.Identity.Transfer;
using Grpc.Core;

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
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("搜索角色")]
    Task<RolesResponse> FetchRolesAsync(
        FetchRolesRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     获取角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns>角色信息<see cref="RoleInfo" /></returns>
    [OperationContract]
    [Description("获取角色")]
    Task<RoleResponse> GetRoleAsync(
        GetRoleRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     创建角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("创建角色")]
    Task<RoleResponse> CreateRoleAsync(
        CreateRoleRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     创建角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("创建角色")]
    Task<GrpcEmptyResponse> CreateRolesAsync(
        CreateRolesRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     更新角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("更新角色")]
    Task<RoleResponse> UpdateRoleAsync(
        UpdateRoleRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     更新角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("更新角色")]
    Task<GrpcEmptyResponse> UpdateRolesAsync(
        UpdateRolesRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     更新或创建角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("更新或创建角色")]
    Task<RoleResponse> UpdateOrCreateRoleAsync(
        UpdateRoleRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     删除角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("删除角色")]
    Task<GrpcEmptyResponse> DeleteRoleAsync(
        DeleteRoleRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     删除角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("删除角色")]
    Task<GrpcEmptyResponse> DeleteRolesAsync(
        DeleteRolesRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     查询角色用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("查询角色用户")]
    Task<RoleUsersResponse> FetchRoleUsersAsync(
        FetchRoleUsersRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     获取角色用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("获取角色用户")]
    Task<RoleUserResponse> GetRoleUserAsync(
        GetRoleUserRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     添加角色用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("添加角色用户")]
    Task<GrpcEmptyResponse> AddRoleUserAsync(
        AddRoleUserRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     添加角色用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("添加角色用户")]
    Task<GrpcEmptyResponse> AddRoleUsersAsync(
        AddRoleUsersRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     删除角色用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("删除角色用户")]
    Task<GrpcEmptyResponse> RemoveRoleUserAsync(
        RemoveRoleUserRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     删除角色用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("删除角色用户")]
    Task<GrpcEmptyResponse> RemoveRoleUsersAsync(
        RemoveRoleUsersRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     查询角色凭据
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("查询角色凭据")]
    Task<RoleClaimsResponse> FetchRoleClaimsAsync(
        FetchRoleClaimsRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     获取角色凭据
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("获取角色凭据")]
    Task<RoleClaimResponse> GetRoleClaimAsync(
        GetRoleClaimRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     添加角色凭据
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("添加角色凭据")]
    Task<GrpcEmptyResponse> AddRoleClaimAsync(
        AddRoleClaimRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     添加角色凭据
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("添加角色凭据")]
    Task<GrpcEmptyResponse> AddRoleClaimsAsync(
        AddRoleClaimsRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     删除角色凭据
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("删除角色凭据")]
    Task<GrpcEmptyResponse> RemoveRoleClaimAsync(
        RemoveRoleClaimRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     删除角色凭据
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("删除角色凭据")]
    Task<GrpcEmptyResponse> RemoveRoleClaimsAsync(
        RemoveRoleClaimsRequest request,
        ServerCallContext? context = default);
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
public sealed record FetchRolesRequest : GrpcPageRequest<FetchRolesFilter>
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
public sealed record RolesResponse : GrpcPageResponse<RoleInfo>
{
    #region Overrides of GrpcPageResponse<RoleInfo>

    /// <summary>
    ///     结果信息
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required GrpcPageResult Result { get; set; }

    /// <summary>
    ///     分页信息
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public override required GrpcPageResult Page { get; set; }

    /// <summary>
    ///     数据集
    /// </summary>
    [Required]
    [DataMember(Order = 3)]
    public override required IEnumerable<RoleInfo> Date { get; set; }

    #endregion
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
public sealed record RoleResponse : GrpcResponse<RoleInfo>
{
    #region Overrides of GrpcResponse<RoleInfo>

    /// <summary>
    ///     结果信息
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required GrpcPageResult Result { get; set; }

    /// <summary>
    ///     结果数据
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public override required RoleInfo Data { get; set; }

    #endregion
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
public sealed record FetchRoleUsersRequest : GrpcPageRequest<FetchRoleUsersFilter>
{
    #region Overrides of GrpcPageRequest<FetchRoleUsersFilter>

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
    public override required FetchRoleUsersFilter Filter { get; set; }

    #endregion
}

/// <summary>
///     查询角色用户响应
/// </summary>
[DataContract]
public sealed record RoleUsersResponse : GrpcPageResponse<UserInfo>
{
    #region Overrides of GrpcPageResponse<RoleInfo>

    /// <summary>
    ///     结果信息
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required GrpcPageResult Result { get; set; }

    /// <summary>
    ///     分页信息
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public override required GrpcPageResult Page { get; set; }

    /// <summary>
    ///     数据集
    /// </summary>
    [Required]
    [DataMember(Order = 3)]
    public override required IEnumerable<UserInfo> Date { get; set; }

    #endregion
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
public sealed record RoleUserResponse : GrpcResponse<UserInfo>
{
    #region Overrides of GrpcResponse<RoleInfo>

    /// <summary>
    ///     结果信息
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required GrpcPageResult Result { get; set; }

    /// <summary>
    ///     结果数据
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public override required UserInfo Data { get; set; }

    #endregion
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
public sealed record FetchRoleClaimsRequest : GrpcPageRequest<FetchRoleClaimsFilter>
{
    #region Overrides of GrpcPageRequest<FetchRoleUsersFilter>

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
    public override required FetchRoleClaimsFilter Filter { get; set; }

    #endregion
}

/// <summary>
///     角色凭据信息响应
/// </summary>
[DataContract]
public sealed record RoleClaimsResponse : GrpcPageResponse<RoleClaimInfo>
{
    #region Overrides of GrpcPageResponse<RoleInfo>

    /// <summary>
    ///     结果信息
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required GrpcPageResult Result { get; set; }

    /// <summary>
    ///     分页信息
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public override required GrpcPageResult Page { get; set; }

    /// <summary>
    ///     数据集
    /// </summary>
    [Required]
    [DataMember(Order = 3)]
    public override required IEnumerable<RoleClaimInfo> Date { get; set; }

    #endregion
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
public sealed record RoleClaimResponse : GrpcResponse<RoleClaimInfo>
{
    #region Overrides of GrpcResponse<RoleInfo>

    /// <summary>
    ///     结果信息
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required GrpcPageResult Result { get; set; }

    /// <summary>
    ///     结果数据
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public override required RoleClaimInfo Data { get; set; }

    #endregion
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