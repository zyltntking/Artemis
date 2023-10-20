using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ServiceModel;
using Artemis.Data.Core;
using Artemis.Shared.Identity.Transfer;
using Grpc.Core;

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
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("搜索用户")]
    Task<DataResult<PageResult<UserInfo>>> FetchUsersAsync(
        PageRequest<FetchUsersFilter> request,
        ServerCallContext? context = default);

    /// <summary>
    ///     获取用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns>角色信息<see cref="UserInfo" /></returns>
    [OperationContract]
    [Description("获取用户")]
    Task<DataResult<UserInfo>> GetUserAsync(
        GetUserRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     创建用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("创建用户")]
    Task<DataResult<UserInfo>> CreateUserAsync(
        CreateUserRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     创建用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("创建用户")]
    Task<DataResult<EmptyRecord>> CreateUsersAsync(
        CreateUsersRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     更新用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("更新用户")]
    Task<DataResult<UserInfo>> UpdateUserAsync(
        UpdateUserRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     更新用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("更新用户")]
    Task<DataResult<EmptyRecord>> UpdateUsersAsync(
        UpdateUsersRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     删除用户
    /// </summary>
    /// <param name="request">删除用户请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("删除用户")]
    Task<DataResult<EmptyRecord>> DeleteUserAsync(
        DeleteUserRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     删除用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("删除用户")]
    Task<DataResult<EmptyRecord>> DeleteUsersAsync(
        DeleteUsersRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     查询用户角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("查询用户角色")]
    Task<DataResult<PageResult<RoleInfo>>> FetchUserRolesAsync(
        PageRequest<FetchUserRolesFilter> request,
        ServerCallContext? context = default);

    /// <summary>
    ///     获取用户角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("获取用户角色")]
    Task<DataResult<RoleInfo>> GetUserRoleAsync(
        GetUserRoleRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     添加用户角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("添加用户角色")]
    Task<DataResult<EmptyRecord>> AddUserRoleAsync(
        AddUserRoleRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     添加用户角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("添加用户角色")]
    Task<DataResult<EmptyRecord>> AddUserRolesAsync(
        AddUserRolesRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     删除用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("删除用户角色")]
    Task<DataResult<EmptyRecord>> RemoveUserRoleAsync(
        RemoveUserRoleRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     删除用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("删除用户角色")]
    Task<DataResult<EmptyRecord>> RemoveUserRolesAsync(
        RemoveUserRolesRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     查询用户凭据信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("查询用户凭据信息")]
    Task<DataResult<PageResult<UserClaimInfo>>> FetchUserClaimsAsync(
        PageRequest<FetchUserClaimsFilter> request,
        ServerCallContext? context = default);

    /// <summary>
    ///     获取用户凭据信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("获取用户凭据信息")]
    Task<DataResult<UserClaimInfo>> GetUserClaimAsync(
        GetUserClaimRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     添加用户凭据信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("添加用户凭据信息")]
    Task<DataResult<EmptyRecord>> AddUserClaimAsync(
        AddUserClaimRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     添加用户凭据信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("添加用户凭据信息")]
    Task<DataResult<EmptyRecord>> AddUserClaimsAsync(
        AddUserClaimsRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     删除用户凭据信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("删除用户凭据信息")]
    Task<DataResult<EmptyRecord>> RemoveUserClaimAsync(
        RemoveUserClaimRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     删除用户凭据信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("删除用户凭据信息")]
    Task<DataResult<EmptyRecord>> RemoveUsersClaimAsync(
        RemoveUserClaimsRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     查询用户登录信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("查询用户登录信息")]
    Task<DataResult<PageResult<UserLoginInfo>>> FetchUserLoginsAsync(
        PageRequest<FetchUserLoginsFilter> request,
        ServerCallContext? context = default);

    /// <summary>
    ///     获取用户登录信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("获取用户登录信息")]
    Task<DataResult<UserLoginInfo>> GetUserLoginAsync(
        GetUserLoginRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     添加用户登录信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("添加用户登录信息")]
    Task<DataResult<EmptyRecord>> AddUserLoginAsync(
        AddUserLoginRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     替换用户登录信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("替换用户登录信息")]
    Task<DataResult<EmptyRecord>> ReplaceUserLoginAsync(
        ReplaceUserLoginRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     删除用户登录信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("删除用户登录信息")]
    Task<DataResult<EmptyRecord>> RemoveUserLoginAsync(
        RemoveUserLoginRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     删除用户登录信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("删除用户登录信息")]
    Task<DataResult<EmptyRecord>> RemoveUserLoginsAsync(
        RemoveUserLoginsRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     查询用户令牌信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("查询用户令牌信息")]
    Task<DataResult<PageResult<UserTokenInfo>>> FetchUserTokensAsync(
        PageRequest<FetchUserTokensFilter> request,
        ServerCallContext? context = default);

    /// <summary>
    ///     获取用户令牌信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("获取用户令牌信息")]
    Task<DataResult<UserTokenInfo>> GetUserTokenAsync(
        GetUserTokenRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     添加用户令牌信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("添加用户令牌信息")]
    Task<DataResult<EmptyRecord>> AddUserTokenAsync(
        AddUserTokenRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     替换用户令牌信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("替换用户令牌信息")]
    Task<DataResult<EmptyRecord>> ReplaceUserTokenAsync(
        ReplaceUserTokenRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     删除用户令牌信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("删除用户令牌信息")]
    Task<DataResult<EmptyRecord>> RemoveUserTokenAsync(
        RemoveUserTokenRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     删除用户令牌信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    [Description("删除用户令牌信息")]
    Task<DataResult<EmptyRecord>> RemoveUserTokensAsync(
        RemoveUserTokensRequest request,
        ServerCallContext? context = default);
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