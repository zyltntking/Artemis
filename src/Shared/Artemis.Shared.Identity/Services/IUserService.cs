using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ServiceModel;
using Artemis.Data.Grpc;
using Artemis.Shared.Identity.Transfer;

namespace Artemis.Shared.Identity.Services;

/// <summary>
///     用户服务接口
/// </summary>
[ServiceContract]
public interface IUserService
{
    /// <summary>
    ///     搜索用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("搜索用户")]
    Task<UsersResponse> FetchUsersAsync(FetchUsersRequest request);

    /// <summary>
    ///     获取用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns>角色信息<see cref="UserInfo" /></returns>
    [OperationContract]
    [Description("获取用户")]
    Task<UserResponse> GetUserAsync(GetUserRequest request);

    /// <summary>
    ///     创建用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("创建用户")]
    Task<UserResponse> CreateUserAsync(CreateUserRequest request);

    /// <summary>
    ///     创建用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("创建用户")]
    Task<GrpcEmptyResponse> CreateUsersAsync(CreateUsersRequest request);

    /// <summary>
    ///     更新用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("更新用户")]
    Task<UserResponse> UpdateUserAsync(UpdateUserRequest request);

    /// <summary>
    ///     更新用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("更新用户")]
    Task<GrpcEmptyResponse> UpdateUsersAsync(UpdateUsersRequest request);

    /// <summary>
    ///     删除用户
    /// </summary>
    /// <param name="request">删除用户请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("删除用户")]
    Task<GrpcEmptyResponse> DeleteUserAsync(DeleteUserRequest request);

    /// <summary>
    ///     删除用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("删除用户")]
    Task<GrpcEmptyResponse> DeleteUsersAsync(DeleteUsersRequest request);

    /// <summary>
    ///     查询用户角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("查询用户角色")]
    Task<UserRolesResponse> FetchUserRolesAsync(FetchUserRolesRequest request);

    /// <summary>
    ///     获取用户角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("获取用户角色")]
    Task<UserRoleResponse> GetUserRoleAsync(GetUserRoleRequest request);

    /// <summary>
    ///     添加用户角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("添加用户角色")]
    Task<GrpcEmptyResponse> AddUserRoleAsync(AddUserRoleRequest request);

    /// <summary>
    ///     添加用户角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("添加用户角色")]
    Task<GrpcEmptyResponse> AddUserRolesAsync(AddUserRolesRequest request);

    /// <summary>
    ///     删除用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("删除用户角色")]
    Task<GrpcEmptyResponse> RemoveUserRoleAsync(RemoveUserRoleRequest request);

    /// <summary>
    ///     删除用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("删除用户角色")]
    Task<GrpcEmptyResponse> RemoveUserRolesAsync(RemoveUserRolesRequest request);

    /// <summary>
    ///     查询用户凭据信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("查询用户凭据信息")]
    Task<UserClaimsResponse> FetchUserClaimsAsync(FetchUserClaimsRequest request);

    /// <summary>
    ///     获取用户凭据信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("获取用户凭据信息")]
    Task<UserClaimResponse> GetUserClaimAsync(GetUserClaimRequest request);

    /// <summary>
    ///     添加用户凭据信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("添加用户凭据信息")]
    Task<GrpcEmptyResponse> AddUserClaimAsync(AddUserClaimRequest request);

    /// <summary>
    ///     添加用户凭据信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("添加用户凭据信息")]
    Task<GrpcEmptyResponse> AddUserClaimsAsync(AddUserClaimsRequest request);

    /// <summary>
    ///     删除用户凭据信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("删除用户凭据信息")]
    Task<GrpcEmptyResponse> RemoveUserClaimAsync(RemoveUserClaimRequest request);

    /// <summary>
    ///     删除用户凭据信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("删除用户凭据信息")]
    Task<GrpcEmptyResponse> RemoveUsersClaimAsync(RemoveUserClaimsRequest request);

    /// <summary>
    ///     查询用户登录信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("查询用户登录信息")]
    Task<UserLoginsResponse> FetchUserLoginsAsync(FetchUserLoginsRequest request);

    /// <summary>
    ///     获取用户登录信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("获取用户登录信息")]
    Task<UserLoginResponse> GetUserLoginAsync(GetUserLoginRequest request);

    /// <summary>
    ///     添加用户登录信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("添加用户登录信息")]
    Task<GrpcEmptyResponse> AddUserLoginAsync(AddUserLoginRequest request);

    /// <summary>
    ///     替换用户登录信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("替换用户登录信息")]
    Task<GrpcEmptyResponse> ReplaceUserLoginAsync(ReplaceUserLoginRequest request);

    /// <summary>
    ///     删除用户登录信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("删除用户登录信息")]
    Task<GrpcEmptyResponse> RemoveUserLoginAsync(RemoveUserLoginRequest request);

    /// <summary>
    ///     删除用户登录信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("删除用户登录信息")]
    Task<GrpcEmptyResponse> RemoveUserLoginsAsync(RemoveUserLoginsRequest request);

    /// <summary>
    ///     查询用户令牌信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("查询用户令牌信息")]
    Task<UserTokensResponse> FetchUserTokensAsync(FetchUserTokensRequest request);

    /// <summary>
    ///     获取用户令牌信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("获取用户令牌信息")]
    Task<UserTokenResponse> GetUserTokenAsync(GetUserTokenRequest request);

    /// <summary>
    ///     添加用户令牌信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("添加用户令牌信息")]
    Task<GrpcEmptyResponse> AddUserTokenAsync(AddUserTokenRequest request);

    /// <summary>
    ///     替换用户令牌信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("替换用户令牌信息")]
    Task<GrpcEmptyResponse> ReplaceUserTokenAsync(ReplaceUserTokenRequest request);

    /// <summary>
    ///     删除用户令牌信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("删除用户令牌信息")]
    Task<GrpcEmptyResponse> RemoveUserTokenAsync(RemoveUserTokenRequest request);

    /// <summary>
    ///     删除用户令牌信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("删除用户令牌信息")]
    Task<GrpcEmptyResponse> RemoveUserTokensAsync(RemoveUserTokensRequest request);
}

/// <summary>
///     查询用户过滤器
/// </summary>
[DataContract]
public sealed record FetchUsersFilter
{
    /// <summary>
    ///     用户名搜索值
    /// </summary>
    [DataMember(Order = 1)]
    public string? NameSearch { get; init; }

    /// <summary>
    ///     邮箱搜索值
    /// </summary>
    [DataMember(Order = 2)]
    public string? EmailSearch { get; init; }

    /// <summary>
    ///     电话号码搜索值
    /// </summary>
    [DataMember(Order = 3)]
    public string? PhoneNumberSearch { get; init; }
}

/// <summary>
///     查询用户请求
/// </summary>
[DataContract]
public sealed record FetchUsersRequest : GrpcPageRequest<FetchUsersFilter>
{
    #region Overrides of GrpcPageRequest<FetchUsersFilter>

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
    public override required FetchUsersFilter Filter { get; set; }

    #endregion
}

/// <summary>
///     查询用户响应
/// </summary>
[DataContract]
public sealed record UsersResponse : GrpcPageResponse<UserInfo>
{
    #region Overrides of GrpcPageResponse<RoleInfo>

    /// <summary>
    ///     结果信息
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required GrpcResult Result { get; set; }

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
    public override required IEnumerable<UserInfo>? Date { get; set; }

    #endregion
}

/// <summary>
///     获取用户请求
/// </summary>
[DataContract]
public record GetUserRequest
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public virtual required Guid UserId { get; set; }
}

/// <summary>
///     用户信息响应
/// </summary>
[DataContract]
public sealed record UserResponse : GrpcResponse<UserInfo>
{
    #region Overrides of GrpcResponse<RoleInfo>

    /// <summary>
    ///     结果信息
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required GrpcResult Result { get; set; }

    /// <summary>
    ///     结果数据
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public override required UserInfo? Data { get; set; }

    #endregion
}

/// <summary>
///     创建用户请求
/// </summary>
[DataContract]
public record CreateUserRequest : UserPackage
{
    #region Implementation of RolePackage

    /// <summary>
    ///     用户名
    /// </summary>
    [Required]
    [MaxLength(32)]
    [DataMember(Order = 1)]
    public override required string UserName { get; set; }

    /// <summary>
    ///     密码
    /// </summary>
    [Required]
    [StringLength(32, MinimumLength = 6)]
    [DataMember(Order = 2)]
    public required string Password { get; set; }

    /// <summary>
    ///     电子邮件
    /// </summary>
    [EmailAddress]
    [MaxLength(128)]
    [DataMember(Order = 3)]
    public override string? Email { get; set; }

    /// <summary>
    ///     电话号码
    /// </summary>
    [Phone]
    [MaxLength(16)]
    [DataMember(Order = 4)]
    public override string? PhoneNumber { get; set; }

    #endregion
}

/// <summary>
///     创建用户请求
/// </summary>
[DataContract]
public sealed record CreateUsersRequest
{
    /// <summary>
    ///     用户信息
    /// </summary>
    /// <remarks>key: userPackage, value: password</remarks>
    [Required]
    [DataMember(Order = 1)]
    public required Dictionary<UserPackage, string> UserPackages { get; set; }
}

/// <summary>
///     更新用户请求
/// </summary>
[DataContract]
public sealed record UpdateUserRequest : GetUserRequest
{
    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required Guid UserId { get; set; }

    /// <summary>
    ///     用户信息
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public required UserPackage UserPackage { get; set; }
}

/// <summary>
///     更新用户请求
/// </summary>
[DataContract]
public sealed record UpdateUsersRequest
{
    /// <summary>
    ///     用户信息
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public required Dictionary<Guid, UserPackage> UserPackages { get; set; }
}

/// <summary>
///     删除用户请求
/// </summary>
[DataContract]
public sealed record DeleteUserRequest : GetUserRequest
{
    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required Guid UserId { get; set; }
}

/// <summary>
///     删除用户请求
/// </summary>
[DataContract]
public sealed record DeleteUsersRequest
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public required List<Guid> UserIds { get; set; }
}

/// <summary>
///     查询角色过滤器
/// </summary>
[DataContract]
public sealed record FetchUserRolesFilter : GetUserRequest
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required Guid UserId { get; set; }

    /// <summary>
    ///     用户名搜索值
    /// </summary>
    [DataMember(Order = 2)]
    public string? RoleNameSearch { get; set; }
}

/// <summary>
///     查询用户角色请求
/// </summary>
[DataContract]
public sealed record FetchUserRolesRequest : GrpcPageRequest<FetchUserRolesFilter>
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
    public override required FetchUserRolesFilter Filter { get; set; }

    #endregion
}

/// <summary>
///     查询用户角色响应
/// </summary>
[DataContract]
public sealed record UserRolesResponse : GrpcPageResponse<RoleInfo>
{
    #region Overrides of GrpcPageResponse<RoleInfo>

    /// <summary>
    ///     结果信息
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required GrpcResult Result { get; set; }

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
    public override required IEnumerable<RoleInfo>? Date { get; set; }

    #endregion
}

/// <summary>
///     获取用户角色请求
/// </summary>
[DataContract]
public sealed record GetUserRoleRequest : GetUserRequest
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required Guid UserId { get; set; }

    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public required Guid RoleId { get; set; }
}

/// <summary>
///     用户角色信息响应
/// </summary>
[DataContract]
public sealed record UserRoleResponse : GrpcResponse<RoleInfo>
{
    #region Overrides of GrpcResponse<RoleInfo>

    /// <summary>
    ///     结果信息
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required GrpcResult Result { get; set; }

    /// <summary>
    ///     结果数据
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public override required RoleInfo? Data { get; set; }

    #endregion
}

/// <summary>
///     添加用户角色请求
/// </summary>
[DataContract]
public sealed record AddUserRoleRequest : GetUserRequest
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required Guid UserId { get; set; }

    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public required Guid RoleId { get; set; }
}

/// <summary>
///     添加角色用户请求
/// </summary>
[DataContract]
public sealed record AddUserRolesRequest : GetUserRequest
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required Guid UserId { get; set; }

    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public required List<Guid> RoleIds { get; set; }
}

/// <summary>
///     删除用户角色请求
/// </summary>
[DataContract]
public sealed record RemoveUserRoleRequest : GetUserRequest
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required Guid UserId { get; set; }

    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public required Guid RoleId { get; set; }
}

/// <summary>
///     删除角色用户请求
/// </summary>
[DataContract]
public sealed record RemoveUserRolesRequest : GetUserRequest
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required Guid UserId { get; set; }

    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public required List<Guid> RoleIds { get; set; }
}

/// <summary>
///     查询用户凭据过滤器
/// </summary>
[DataContract]
public sealed record FetchUserClaimsFilter : GetUserRequest
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required Guid UserId { get; set; }

    /// <summary>
    ///     凭据类型搜索值
    /// </summary>
    [DataMember(Order = 2)]
    public string? ClaimTypeSearch { get; set; }
}

/// <summary>
///     查询用户凭据请求
/// </summary>
[DataContract]
public sealed record FetchUserClaimsRequest : GrpcPageRequest<FetchUserClaimsFilter>
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
    public override required FetchUserClaimsFilter Filter { get; set; }

    #endregion
}

/// <summary>
///     查询用户凭据响应
/// </summary>
[DataContract]
public sealed record UserClaimsResponse : GrpcPageResponse<UserClaimInfo>
{
    #region Overrides of GrpcPageResponse<RoleInfo>

    /// <summary>
    ///     结果信息
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required GrpcResult Result { get; set; }

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
    public override required IEnumerable<UserClaimInfo>? Date { get; set; }

    #endregion
}

/// <summary>
///     获取用户凭据请求
/// </summary>
[DataContract]
public sealed record GetUserClaimRequest : GetUserRequest
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required Guid UserId { get; set; }

    /// <summary>
    ///     凭据标识
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public required int UserClaimId { get; set; }
}

/// <summary>
///     用户凭据信息响应
/// </summary>
[DataContract]
public sealed record UserClaimResponse : GrpcResponse<UserClaimInfo>
{
    #region Overrides of GrpcResponse<RoleInfo>

    /// <summary>
    ///     结果信息
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required GrpcResult Result { get; set; }

    /// <summary>
    ///     结果数据
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public override required UserClaimInfo? Data { get; set; }

    #endregion
}

/// <summary>
///     添加用户凭据请求
/// </summary>
[DataContract]
public sealed record AddUserClaimRequest : GetUserRequest
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required Guid UserId { get; set; }

    /// <summary>
    ///     凭据信息
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public required UserClaimPackage ClaimPackage { get; set; }
}

/// <summary>
///     添加用户凭据请求
/// </summary>
[DataContract]
public sealed record AddUserClaimsRequest : GetUserRequest
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required Guid UserId { get; set; }

    /// <summary>
    ///     凭据信息
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public required List<UserClaimPackage> ClaimPackages { get; set; }
}

/// <summary>
///     删除用户凭据请求
/// </summary>
[DataContract]
public sealed record RemoveUserClaimRequest : GetUserRequest
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required Guid UserId { get; set; }

    /// <summary>
    ///     凭据标识
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public required int UserClaimId { get; set; }
}

/// <summary>
///     删除用户凭据请求
/// </summary>
[DataContract]
public sealed record RemoveUserClaimsRequest : GetUserRequest
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required Guid UserId { get; set; }

    /// <summary>
    ///     凭据标识
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public required List<int> UserClaimIds { get; set; }
}

/// <summary>
///     查询用户登录信息过滤器
/// </summary>
[DataContract]
public sealed record FetchUserLoginsFilter : GetUserRequest
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [DataMember(Order = 1)]
    public override required Guid UserId { get; set; }

    /// <summary>
    ///     登录类型搜索值
    /// </summary>
    [DataMember(Order = 2)]
    public string? LoginProviderSearch { get; init; }
}

/// <summary>
///     查询用户登录请求
/// </summary>
[DataContract]
public sealed record FetchUserLoginsRequest : GrpcPageRequest<FetchUserLoginsFilter>
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
    public override required FetchUserLoginsFilter Filter { get; set; }

    #endregion
}

/// <summary>
///     查询用户登录信息响应
/// </summary>
[DataContract]
public sealed record UserLoginsResponse : GrpcPageResponse<UserLoginInfo>
{
    #region Overrides of GrpcPageResponse<RoleInfo>

    /// <summary>
    ///     结果信息
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required GrpcResult Result { get; set; }

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
    public override required IEnumerable<UserLoginInfo>? Date { get; set; }

    #endregion
}

/// <summary>
///     获取用户登录信息请求
/// </summary>
[DataContract]
public sealed record GetUserLoginRequest : GetUserRequest
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [DataMember(Order = 1)]
    public override required Guid UserId { get; set; }

    /// <summary>
    ///     用户登录信息标识
    /// </summary>
    [DataMember(Order = 2)]
    public required int UserLoginId { get; set; }
}

/// <summary>
///     用户登录信息响应
/// </summary>
[DataContract]
public sealed record UserLoginResponse : GrpcResponse<UserLoginInfo>
{
    #region Overrides of GrpcResponse<RoleInfo>

    /// <summary>
    ///     结果信息
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required GrpcResult Result { get; set; }

    /// <summary>
    ///     结果数据
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public override required UserLoginInfo? Data { get; set; }

    #endregion
}

/// <summary>
///     添加用户登录信息请求
/// </summary>
[DataContract]
public sealed record AddUserLoginRequest : GetUserRequest
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [DataMember(Order = 1)]
    public override required Guid UserId { get; set; }

    /// <summary>
    ///     用户登录信息标识
    /// </summary>
    [DataMember(Order = 2)]
    public required UserLoginPackage UserLoginPackage { get; set; }
}

/// <summary>
///     替换用户登录信息请求
/// </summary>
[DataContract]
public sealed record ReplaceUserLoginRequest : GetUserRequest
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [DataMember(Order = 1)]
    public override required Guid UserId { get; set; }

    /// <summary>
    ///     用户标识
    /// </summary>
    [DataMember(Order = 2)]
    public required int UserLoginId { get; set; }

    /// <summary>
    ///     用户登录信息标识
    /// </summary>
    [DataMember(Order = 3)]
    public required UserLoginPackage UserLoginPackage { get; set; }
}

/// <summary>
///     删除用户登录信息请求
/// </summary>
[DataContract]
public sealed record RemoveUserLoginRequest : GetUserRequest
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [DataMember(Order = 1)]
    public override required Guid UserId { get; set; }

    /// <summary>
    ///     用户登录信息标识
    /// </summary>
    [DataMember(Order = 2)]
    public required int UserLoginId { get; set; }
}

/// <summary>
///     删除用户登录信息请求
/// </summary>
[DataContract]
public sealed record RemoveUserLoginsRequest : GetUserRequest
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [DataMember(Order = 1)]
    public override required Guid UserId { get; set; }

    /// <summary>
    ///     用户登录信息标识
    /// </summary>
    [DataMember(Order = 2)]
    public required List<int> UserLoginIds { get; set; }
}

/// <summary>
///     查询用户令牌信息过滤器
/// </summary>
[DataContract]
public sealed record FetchUserTokensFilter : GetUserRequest
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [DataMember(Order = 1)]
    public override required Guid UserId { get; set; }

    /// <summary>
    ///     登录类型搜索值
    /// </summary>
    [DataMember(Order = 2)]
    public string? LoginProviderSearch { get; init; }

    /// <summary>
    ///     令牌名搜索值
    /// </summary>
    [DataMember(Order = 3)]
    public string? NameSearch { get; init; }
}

/// <summary>
///     查询用户令牌请求
/// </summary>
[DataContract]
public sealed record FetchUserTokensRequest : GrpcPageRequest<FetchUserTokensFilter>
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
    public override required FetchUserTokensFilter Filter { get; set; }

    #endregion
}

/// <summary>
///     用户令牌分页响应
/// </summary>
[DataContract]
public sealed record UserTokensResponse : GrpcPageResponse<UserTokenInfo>
{
    #region Overrides of GrpcPageResponse<RoleInfo>

    /// <summary>
    ///     结果信息
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required GrpcResult Result { get; set; }

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
    public override required IEnumerable<UserTokenInfo>? Date { get; set; }

    #endregion
}

/// <summary>
///     获取用户令牌信息请求
/// </summary>
[DataContract]
public sealed record GetUserTokenRequest : GetUserRequest
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [DataMember(Order = 1)]
    public override required Guid UserId { get; set; }

    /// <summary>
    ///     用户令牌信息标识
    /// </summary>
    [DataMember(Order = 2)]
    public required int UserTokenId { get; set; }
}

/// <summary>
///     用户令牌信息响应
/// </summary>
[DataContract]
public sealed record UserTokenResponse : GrpcResponse<UserTokenInfo>
{
    #region Overrides of GrpcResponse<RoleInfo>

    /// <summary>
    ///     结果信息
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required GrpcResult Result { get; set; }

    /// <summary>
    ///     结果数据
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public override required UserTokenInfo? Data { get; set; }

    #endregion
}

/// <summary>
///     添加用户令牌信息请求
/// </summary>
[DataContract]
public sealed record AddUserTokenRequest : GetUserRequest
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [DataMember(Order = 1)]
    public override required Guid UserId { get; set; }

    /// <summary>
    ///     用户令牌信息标识
    /// </summary>
    [DataMember(Order = 2)]
    public required UserTokenPackage UserTokenPackage { get; set; }
}

/// <summary>
///     替换用户令牌信息请求
/// </summary>
[DataContract]
public sealed record ReplaceUserTokenRequest : GetUserRequest
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [DataMember(Order = 1)]
    public override required Guid UserId { get; set; }

    /// <summary>
    ///     用户标识
    /// </summary>
    [DataMember(Order = 2)]
    public required int UserTokenId { get; set; }

    /// <summary>
    ///     用户登录信息标识
    /// </summary>
    [DataMember(Order = 3)]
    public required UserTokenPackage UserTokenPackage { get; set; }
}

/// <summary>
///     删除用户登录信息请求
/// </summary>
[DataContract]
public sealed record RemoveUserTokenRequest : GetUserRequest
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [DataMember(Order = 1)]
    public override required Guid UserId { get; set; }

    /// <summary>
    ///     用户令牌信息标识
    /// </summary>
    [DataMember(Order = 2)]
    public required int UserTokenId { get; set; }
}

/// <summary>
///     删除用户令牌信息请求
/// </summary>
[DataContract]
public sealed record RemoveUserTokensRequest : GetUserRequest
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [DataMember(Order = 1)]
    public override required Guid UserId { get; set; }

    /// <summary>
    ///     用户令牌信息标识
    /// </summary>
    [DataMember(Order = 2)]
    public required List<int> UserTokenIds { get; set; }
}