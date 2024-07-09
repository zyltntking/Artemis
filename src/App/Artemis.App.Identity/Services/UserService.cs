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
            request.UserName,
            request.Email,
            request.Phone,
            request.Page ?? 0,
            request.Size ?? 0,
            context.CancellationToken);

        var userReplies = userInfos.Items!.Select(item => item.Adapt<UserInfoPackage>());

        var response = DataResult.Success(userInfos).Adapt<PagedUserInfoResponse>();

        response.Data.Items.Add(userReplies);

        return response;
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
    ///     创建用户
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    [Description("创建用户")]
    [Authorize(IdentityPolicy.Token)]
    public override async Task<EmptyResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        var result = await UserManager.CreateUserAsync(new UserSign
        {
            UserName = request.Sign.UserName,
            Email = request.Sign?.Email,
            PhoneNumber = request.Sign?.Phone
        }, request.Password, context.CancellationToken);

        if (result.Succeeded) return DataResult.EmptySuccess().Adapt<EmptyResponse>();

        return DataResult.EmptyFail($"创建失败。{result.DescribeError}").Adapt<EmptyResponse>();
    }

    /// <summary>
    ///     批量创建用户
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量创建用户")]
    [Authorize(IdentityPolicy.Token)]
    public override async Task<EmptyResponse> BatchCreateUser(BatchCreateUserRequest request, ServerCallContext context)
    {
        var dictionary = request.Batch.ToDictionary(
            create => new UserSign
            {
                UserName = create.Sign.UserName,
                Email = create.Sign?.Email,
                PhoneNumber = create.Sign?.Phone
            },
            create => create.Password);

        var result = await UserManager.CreateUsersAsync(dictionary, context.CancellationToken);

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
    ///     批量更新用户信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    public override async Task<EmptyResponse> BatchUpdateUserInfo(BatchUpdateUserRequest request,
        ServerCallContext context)
    {
        var dictionary = request.Batch.ToDictionary(
            update => Guid.Parse(update.UserId),
            update => new UserPackage
            {
                UserName = update.Sign.UserName,
                Email = update.Sign?.Email,
                PhoneNumber = update.Sign?.Phone
            });

        var result = await UserManager.UpdateUsersAsync(dictionary, context.CancellationToken);

        if (result.Succeeded) return DataResult.EmptySuccess().Adapt<EmptyResponse>();

        return DataResult.EmptyFail($"更新失败。{result.DescribeError}").Adapt<EmptyResponse>();
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
    ///     批量删除用户
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量删除用户")]
    [Authorize(IdentityPolicy.Token)]
    public override async Task<EmptyResponse> BatchDeleteUser(BatchDeleteUserRequest request, ServerCallContext context)
    {
        var idList = request.UserIds.Select(Guid.Parse).ToList();

        var result = await UserManager.DeleteUsersAsync(idList, context.CancellationToken);

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
            request.RoleName,
            request.Page ?? 0,
            request.Size ?? 0,
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
    ///     批量添加用户角色
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量添加用户角色")]
    [Authorize(IdentityPolicy.Token)]
    public override async Task<EmptyResponse> BatchAddUserRole(BatchUserRoleIdRequest request,
        ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var roleIds = request.RoleIds.Select(Guid.Parse).ToList();

        var result = await UserManager.AddUserRolesAsync(userId, roleIds, context.CancellationToken);

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

    /// <summary>
    ///     批量移除用户角色
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量移除用户角色")]
    [Authorize(IdentityPolicy.Token)]
    public override async Task<EmptyResponse> BatchRemoveUserRole(BatchUserRoleIdRequest request,
        ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);
        var roleIds = request.RoleIds.Select(Guid.Parse).ToList();

        var result = await UserManager.RemoveUserRolesAsync(userId, roleIds, context.CancellationToken);

        if (result.Succeeded) return DataResult.EmptySuccess().Adapt<EmptyResponse>();

        return DataResult.EmptyFail($"移除失败。{result.DescribeError}").Adapt<EmptyResponse>();
    }

    /// <summary>
    ///     查询用户凭据信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查询用户凭据信息")]
    [Authorize(IdentityPolicy.Token)]
    public override async Task<PagedUserClaimInfoResponse> SearchUserClaimInfo(SearchUserClaimRequest request,
        ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var userClaimInfos = await UserManager.FetchUserClaimsAsync(
            userId,
            request.ClaimType,
            request.ClaimValue,
            request.Page ?? 0,
            request.Size ?? 0,
            context.CancellationToken);

        var userClaimReplies = userClaimInfos.Items!.Select(item => item.Adapt<UserClaimInfoPackage>());

        var response = DataResult.Success(userClaimInfos).Adapt<PagedUserClaimInfoResponse>();

        response.Data.Items.Add(userClaimReplies);

        return response;
    }

    /// <summary>
    ///     获取用户凭据信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("获取用户凭据信息")]
    [Authorize(IdentityPolicy.Token)]
    public override async Task<UserClaimInfoResponse> ReadUserClaimInfo(UserClaimIdRequest request,
        ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var userClaimInfo = await UserManager.GetUserClaimAsync(userId, request.ClaimId, context.CancellationToken);

        if (userClaimInfo is null) return DataResult.Fail<UserClaimInfo>().Adapt<UserClaimInfoResponse>();

        return DataResult.Success(userClaimInfo).Adapt<UserClaimInfoResponse>();
    }

    /// <summary>
    ///     添加用户凭据
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("添加用户凭据")]
    [Authorize(IdentityPolicy.Token)]
    public override async Task<EmptyResponse> AddUserClaim(AddUserClaimRequest request, ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var result = await UserManager.AddUserClaimAsync(
            userId,
            new UserClaimPackage
            {
                ClaimType = request.Claim.ClaimType,
                ClaimValue = request.Claim.ClaimValue,
                Description = request.Claim.Description
            },
            context.CancellationToken);

        if (result.Succeeded) return DataResult.EmptySuccess().Adapt<EmptyResponse>();

        return DataResult.EmptyFail($"添加失败。{result.DescribeError}").Adapt<EmptyResponse>();
    }

    /// <summary>
    ///     批量添加用户凭据
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量添加用户凭据")]
    [Authorize(IdentityPolicy.Token)]
    public override async Task<EmptyResponse> BatchAddUserClaim(BatchAddUserClaimRequest request,
        ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var userClaimPackages = request.Batch.Select(claim => new UserClaimPackage
        {
            ClaimType = claim.ClaimType,
            ClaimValue = claim.ClaimValue,
            Description = claim.Description
        }).ToList();

        var result = await UserManager.AddUserClaimsAsync(userId, userClaimPackages, context.CancellationToken);

        if (result.Succeeded) return DataResult.EmptySuccess().Adapt<EmptyResponse>();

        return DataResult.EmptyFail($"添加失败。{result.DescribeError}").Adapt<EmptyResponse>();
    }

    /// <summary>
    ///     更新用户凭据
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("更新用户凭据")]
    [Authorize(IdentityPolicy.Token)]
    public override async Task<EmptyResponse> UpdateUserClaim(UpdateUserClaimRequest request, ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var result = await UserManager.UpdateUserClaimAsync(
            userId,
            request.ClaimId,
            new UserClaimPackage
            {
                ClaimType = request.Claim.ClaimType,
                ClaimValue = request.Claim.ClaimValue,
                Description = request.Claim.Description
            },
            context.CancellationToken);

        if (result.Succeeded) return DataResult.EmptySuccess().Adapt<EmptyResponse>();

        return DataResult.EmptyFail($"更新失败。{result.DescribeError}").Adapt<EmptyResponse>();
    }

    /// <summary>
    ///     批量更新用户凭据
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    public override async Task<EmptyResponse> BatchUpdateUserClaim(BatchUpdateUserClaimRequest request,
        ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var dictionary = request.Batch.ToDictionary(
            update => update.ClaimId,
            update => new UserClaimPackage
            {
                ClaimType = update.Claim.ClaimType,
                ClaimValue = update.Claim.ClaimValue,
                Description = update.Claim.Description
            });

        var result = await UserManager.UpdateUserClaimsAsync(userId, dictionary, context.CancellationToken);

        if (result.Succeeded) return DataResult.EmptySuccess().Adapt<EmptyResponse>();

        return DataResult.EmptyFail($"更新失败。{result.DescribeError}").Adapt<EmptyResponse>();
    }

    /// <summary>
    ///     删除用户凭据
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("删除用户凭据")]
    [Authorize(IdentityPolicy.Token)]
    public override async Task<EmptyResponse> DeleteUserClaim(UserClaimIdRequest request, ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var result = await UserManager.RemoveUserClaimAsync(userId, request.ClaimId, context.CancellationToken);

        if (result.Succeeded) return DataResult.EmptySuccess().Adapt<EmptyResponse>();

        return DataResult.EmptyFail($"删除失败。{result.DescribeError}").Adapt<EmptyResponse>();
    }

    /// <summary>
    ///     批量删除用户凭据
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量删除用户凭据")]
    [Authorize(IdentityPolicy.Token)]
    public override async Task<EmptyResponse> BatchDeleteUserClaim(BatchDeleteUserClaimRequest request,
        ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var result = await UserManager.RemoveUserClaimsAsync(userId, request.ClaimIds, context.CancellationToken);

        if (result.Succeeded) return DataResult.EmptySuccess().Adapt<EmptyResponse>();

        return DataResult.EmptyFail($"删除失败。{result.DescribeError}").Adapt<EmptyResponse>();
    }

    #endregion
}