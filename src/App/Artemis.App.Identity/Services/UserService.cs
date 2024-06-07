using System.ComponentModel;
using Artemis.Data.Core;
using Artemis.Extensions.Web.Identity;
using Artemis.Protos.Identity;
using Artemis.Services.Identity.Managers;
using Artemis.Shared.Identity.Transfer;
using Grpc.Core;
using Mapster;
using Microsoft.AspNetCore.Authorization;

namespace Artemis.App.Identity.Services;

/// <summary>
///     用户服务
/// </summary>
public class UserService : User.UserBase
{
    /// <summary>
    ///     用户服务
    /// </summary>
    /// <param name="userManager">用户管理器依赖</param>
    /// <param name="logger">日志依赖</param>
    public UserService(
        IUserManager userManager,
        ILogger<UserService> logger)
    {
        UserManager = userManager;
        Logger = logger;
    }

    /// <summary>
    ///     用户管理器
    /// </summary>
    private IUserManager UserManager { get; }

    /// <summary>
    ///     日志依赖
    /// </summary>
    private ILogger<UserService> Logger { get; }

    #region Overrides of UserBase

    /// <summary>
    ///     获取用户信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("获取用户信息")]
    [Authorize(IdentityPolicy.Token)]
    public override async Task<UserInfosResponse> FetchUserInfo(FetchUserInfosRequest request,
        ServerCallContext context)
    {
        var pagedUserInfos = await UserManager.FetchUserAsync(
            request.NameSearch,
            request.EmailSearch,
            request.PhoneNumberSearch,
            request.Page ?? 1,
            request.Size ?? 20,
            context.CancellationToken);

        var userReplies = pagedUserInfos.Items.Adapt<List<UserInfoReply>>();

        var response = DataResult.Success(pagedUserInfos).Adapt<UserInfosResponse>();

        response.Data.Items.Add(userReplies);

        return response;
    }

    /// <summary>
    ///     获取用户信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("获取用户信息")]
    [Authorize(IdentityPolicy.Token)]
    public override async Task<UserInfoResponse> GetUserInfo(UserIdRequest request, ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var userInfo = await UserManager.GetUserAsync(userId, context.CancellationToken);

        if (userInfo is null) return DataResult.Fail<UserInfo>().Adapt<UserInfoResponse>();

        return DataResult.Success(userInfo).Adapt<UserInfoResponse>();
    }

    /// <summary>
    ///     创建用户
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("创建用户")]
    [Authorize(IdentityPolicy.Token)]
    public override async Task<UserInfoResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        var (result, userInfo) = await UserManager.CreateUserAsync(new UserPackage
        {
            UserName = request.UserName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        }, request.Password, context.CancellationToken);

        if (result.Succeeded) return DataResult.Success(userInfo).Adapt<UserInfoResponse>();

        return DataResult.Fail<UserInfo>().Adapt<UserInfoResponse>();
    }

    /// <summary>
    ///     更新用户信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("更新用户信息")]
    [Authorize(IdentityPolicy.Token)]
    public override async Task<UserInfoResponse> UpdateUserInfo(UpdateUserRequest request, ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var (result, userInfo) = await UserManager.UpdateUserAsync(userId, new UserPackage
        {
            UserName = request.UserName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        }, context.CancellationToken);

        if (result.Succeeded) return DataResult.Success(userInfo).Adapt<UserInfoResponse>();

        return DataResult.Fail<UserInfo>($"更新失败。{result.DescribeError}").Adapt<UserInfoResponse>();
    }

    /// <summary>
    ///     删除用户
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("删除用户")]
    [Authorize(IdentityPolicy.Token)]
    public override async Task<EmptyResponse> DeleteUser(UserIdRequest request, ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var result = await UserManager.DeleteUserAsync(userId, context.CancellationToken);

        if (result.Succeeded) return DataResult.EmptySuccess().Adapt<EmptyResponse>();

        return DataResult.EmptyFail($"删除失败。{result.DescribeError}").Adapt<EmptyResponse>();
    }

    /// <summary>
    ///     获取用户角色信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("获取用户角色信息")]
    [Authorize(IdentityPolicy.Token)]
    public override async Task<RoleInfosResponse> FetchUserRolesInfo(FetchUserRolesRequest request,
        ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var pagedUserRoles = await UserManager.FetchUserRolesAsync(
            userId,
            request.RoleNameSearch,
            request.Page ?? 1,
            request.Size ?? 20,
            context.CancellationToken);

        var roleReplies = pagedUserRoles.Items.Adapt<List<RoleInfoReply>>();

        var response = DataResult.Success(pagedUserRoles).Adapt<RoleInfosResponse>();

        response.Data.Items.Add(roleReplies);

        return response;
    }

    /// <summary>
    ///     获取用户角色信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("获取用户角色信息")]
    [Authorize(IdentityPolicy.Token)]
    public override async Task<RoleInfoResponse> GetUserRoleInfo(UserRoleIdRequest request, ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var roleId = Guid.Parse(request.RoleId);

        var roleInfo = await UserManager.GetUserRoleAsync(userId, roleId, context.CancellationToken);

        if (roleInfo is null) return DataResult.Fail<RoleInfo>().Adapt<RoleInfoResponse>();

        return DataResult.Success(roleInfo).Adapt<RoleInfoResponse>();
    }

    /// <summary>
    ///     对用户添加角色
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("对用户添加角色")]
    [Authorize(IdentityPolicy.Token)]
    public override async Task<EmptyResponse> AddUserRole(UserRoleIdRequest request, ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var roleId = Guid.Parse(request.RoleId);

        var result = await UserManager.AddUserRoleAsync(userId, roleId, context.CancellationToken);

        if (result.Succeeded) return DataResult.EmptySuccess().Adapt<EmptyResponse>();

        return DataResult.EmptyFail($"添加失败。{result.DescribeError}").Adapt<EmptyResponse>();
    }

    /// <summary>
    ///     对用户移除角色
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("对用户移除角色")]
    [Authorize(IdentityPolicy.Token)]
    public override async Task<EmptyResponse> RemoveUserRole(UserRoleIdRequest request, ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var roleId = Guid.Parse(request.RoleId);

        var result = await UserManager.RemoveUserRoleAsync(userId, roleId, context.CancellationToken);

        if (result.Succeeded) return DataResult.EmptySuccess().Adapt<EmptyResponse>();

        return DataResult.EmptyFail($"移除失败。{result.DescribeError}").Adapt<EmptyResponse>();
    }

    #endregion
}