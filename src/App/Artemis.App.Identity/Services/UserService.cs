using System.ComponentModel;
using Artemis.Data.Core;
using Artemis.Data.Shared.Transfer;
using Artemis.Data.Shared.Transfer.Identity;
using Artemis.Service.Identity.Managers;
using Artemis.Service.Protos;
using Artemis.Service.Protos.Identity;
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
    ///     账户服务
    /// </summary>
    /// <param name="userManager">用户管理器</param>
    /// <param name="logger">日志记录器</param>
    public UserService(
        IIdentityUserManager userManager,
        ILogger<AccountService> logger)
    {
        UserManager = userManager;
        Logger = logger;
    }

    /// <summary>
    ///     用户管理器
    /// </summary>
    private IIdentityUserManager UserManager { get; }

    /// <summary>
    ///     日志依赖
    /// </summary>
    private ILogger<AccountService> Logger { get; }

    #region Overrides of UserBase

    /// <summary>
    ///     查询用户
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查询用户")]
    [Authorize(IdentityPolicy.Token)]
    public override async Task<PagedUserInfoResponse> SearchUserInfo(SearchUserRequest request,
        ServerCallContext context)
    {
        var userInfos = await UserManager.FetchUserAsync(
            request.Search?.UserName ?? null,
            request.Search?.Email ?? null,
            request.Search?.Phone ?? null,
            request.Pagination?.Page ?? 0,
            request.Pagination?.Size ?? 0,
            context.CancellationToken);

        var userReplies = userInfos.Items!.Select(item => item.Adapt<UserInfoPackage>());

        var response = DataResult.Success(userInfos).Adapt<PagedUserInfoResponse>();

        response.Data.Items.Add(userReplies);

        return response;
    }

    /// <summary>
    ///     创建用户
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    [Description("创建用户")]
    [Authorize(IdentityPolicy.Token)]
    public override async Task<EmptyResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        var result = await UserManager.CreateUserAsync(new UserPackage
        {
            UserName = request.Sign.UserName,
            Email = request.Sign?.Email,
            PhoneNumber = request.Sign?.Phone
        }, request.Password, context.CancellationToken);

        if (result.Succeeded) return DataResult.EmptySuccess().Adapt<EmptyResponse>();

        return DataResult.EmptyFail($"创建失败。{result.DescribeError}").Adapt<EmptyResponse>();
    }

    /// <summary>
    ///     更新用户信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("更新用户信息")]
    [Authorize(IdentityPolicy.Token)]
    public override async Task<EmptyResponse> UpdateUserInfo(UpdateUserRequest request, ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var result = await UserManager.UpdateUserAsync(userId, new UserPackage
        {
            UserName = request.Sign.UserName,
            Email = request.Sign?.Email,
            PhoneNumber = request.Sign?.Phone
        }, context.CancellationToken);

        if (result.Succeeded) return DataResult.EmptySuccess().Adapt<EmptyResponse>();

        return DataResult.EmptyFail($"更新失败。{result.DescribeError}").Adapt<EmptyResponse>();
    }

    /// <summary>
    ///     读取用户信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("读取用户信息")]
    [Authorize(IdentityPolicy.Token)]
    public override async Task<UserInfoResponse> ReadUserInfo(UserIdRequest request, ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var userInfo = await UserManager.GetUserAsync(userId, context.CancellationToken);

        if (userInfo is null) return DataResult.Fail<UserInfo>().Adapt<UserInfoResponse>();

        return DataResult.Success(userInfo).Adapt<UserInfoResponse>();
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
    ///     查询用户角色信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查询用户角色信息")]
    [Authorize(IdentityPolicy.Token)]
    public override async Task<PagedRoleInfoResponse> SearchUserRoleInfo(SearchUserRoleRequest request,
        ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var roleInfos = await UserManager.FetchUserRolesAsync(
            userId,
            request.Role?.Search?.RoleName,
            request.Role?.Pagination.Page ?? 0,
            request.Role?.Pagination.Size ?? 0,
            context.CancellationToken);

        var roleReplies = roleInfos.Items!.Select(item => item.Adapt<RoleInfoPackage>());

        var response = DataResult.Success(roleInfos).Adapt<PagedRoleInfoResponse>();

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
    public override async Task<RoleInfoResponse> ReadUserRoleInfo(UserRoleIdRequest request, ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);
        var roleId = Guid.Parse(request.RoleId);

        var roleInfo = await UserManager.GetUserRoleAsync(
            userId,
            roleId,
            context.CancellationToken);

        if (roleInfo is null) return DataResult.Fail<RoleInfo>().Adapt<RoleInfoResponse>();

        return DataResult.Success(roleInfo).Adapt<RoleInfoResponse>();
    }

    /// <summary>
    ///     添加用户角色
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("添加用户角色")]
    [Authorize(IdentityPolicy.Token)]
    public override async Task<EmptyResponse> AddUserRole(AddUserRoleRequest request, ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);
        var roleId = Guid.Parse(request.Role.RoleId);

        var result = await UserManager.AddUserRoleAsync(userId, roleId, context.CancellationToken);

        if (result.Succeeded) return DataResult.EmptySuccess().Adapt<EmptyResponse>();

        return DataResult.EmptyFail($"添加失败。{result.DescribeError}").Adapt<EmptyResponse>();
    }

    /// <summary>
    ///     移除用户角色
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("移除用户角色")]
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