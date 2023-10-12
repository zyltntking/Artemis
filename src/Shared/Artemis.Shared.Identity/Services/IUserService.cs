﻿using System.ComponentModel.DataAnnotations;
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
    Task<DataResult<UserInfo>> CreateUserAsync(
        CreateUserRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     创建或更新用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [OperationContract]
    Task<DataResult<UserInfo>> CreateOrUpdateUserAsync(
        UpdateUserRequest request,
        ServerCallContext? context = default);

    /// <summary>
    ///     删除用户
    /// </summary>
    /// <param name="request">删除用户请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    Task<DataResult<EmptyRecord>> DeleteUserAsync(
        DeleteUserRequest request,
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
public sealed record CreateUserRequest : UserPackage
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
    public required UserPackage UserPack { get; set; }

    /// <summary>
    ///     用户密码
    /// </summary>
    [DataMember(Order = 3)]
    public string? Password { get; set; }
}

/// <summary>
///     删除用户请求
/// </summary>
public sealed record DeleteUserRequest : GetUserRequest
{
    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required Guid UserId { get; set; }
}