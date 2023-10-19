using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ServiceModel;
using Artemis.Data.Core;
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
    Task<DataResult<PageResult<RoleInfo>>> FetchRolesAsync(
        PageRequest<FetchRolesFilter> request,
        ServerCallContext? context = default);

    /// <summary>
    ///     获取角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns>角色信息<see cref="RoleInfo" /></returns>
    [OperationContract]
    Task<DataResult<RoleInfo>> GetRoleAsync(
        GetRoleRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     创建角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    Task<DataResult<RoleInfo>> CreateRoleAsync(
        CreateRoleRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     创建角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    Task<DataResult<EmptyRecord>> CreateRolesAsync(
        CreateRolesRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     更新角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    Task<DataResult<RoleInfo>> UpdateRoleAsync(
        UpdateRoleRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     更新角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    Task<DataResult<EmptyRecord>> UpdateRolesAsync(
        UpdateRolesRequest request,
        ServerCallContext? context = default);

    /// <summary>
    /// 更新或创建角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    Task<DataResult<RoleInfo>> UpdateOrCreateRoleAsync(
        UpdateRoleRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     删除角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    Task<DataResult<EmptyRecord>> DeleteRoleAsync(
        DeleteRoleRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     删除角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    Task<DataResult<EmptyRecord>> DeleteRolesAsync(
        DeleteRolesRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     查询角色用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    Task<DataResult<PageResult<UserInfo>>> FetchRoleUsersAsync(
        PageRequest<FetchRoleUsersFilter> request,
        ServerCallContext? context = default);

    /// <summary>
    ///     获取角色用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    Task<DataResult<UserInfo>> GetRoleUserAsync(
        GetRoleUserRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     添加角色用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    Task<DataResult<EmptyRecord>> AddRoleUserAsync(
        AddRoleUserRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     添加角色用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    Task<DataResult<EmptyRecord>> AddRoleUsersAsync(
        AddRoleUsersRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     删除角色用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    Task<DataResult<EmptyRecord>> RemoveRoleUserAsync(
        RemoveRoleUserRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     删除角色用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    Task<DataResult<EmptyRecord>> RemoveRoleUsersAsync(
        RemoveRoleUsersRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     查询角色凭据
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    Task<DataResult<PageResult<RoleClaimInfo>>> FetchRoleClaimsAsync(
        PageRequest<FetchRoleClaimsFilter> request,
        ServerCallContext? context = default);

    /// <summary>
    ///     获取角色凭据
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    Task<DataResult<RoleClaimInfo>> GetRoleClaimAsync(
        GetRoleClaimRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     添加角色凭据
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    Task<DataResult<EmptyRecord>> AddRoleClaimAsync(
        AddRoleClaimRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     添加角色凭据
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    Task<DataResult<EmptyRecord>> AddRoleClaimsAsync(
        AddRoleClaimsRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     删除角色凭据
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    Task<DataResult<EmptyRecord>> RemoveRoleClaimAsync(
        RemoveRoleClaimRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     删除角色凭据
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    Task<DataResult<EmptyRecord>> RemoveRoleClaimsAsync(
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
///     查询用户过滤器
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