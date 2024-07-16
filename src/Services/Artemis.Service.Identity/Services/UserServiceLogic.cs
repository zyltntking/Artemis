using System.ComponentModel;
using Artemis.Data.Core;
using Artemis.Extensions.Identity;
using Artemis.Service.Identity.Managers;
using Artemis.Service.Identity.Protos;
using Artemis.Service.Shared.Transfer.Identity;
using Grpc.Core;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.Identity.Services;

/// <summary>
///     用户服务
/// </summary>
public class UserServiceLogic : UserService.UserServiceBase
{
    /// <summary>
    ///     账户服务
    /// </summary>
    /// <param name="userManager">用户管理器</param>
    /// <param name="logger">日志记录器</param>
    public UserServiceLogic(
        IIdentityUserManager userManager,
        ILogger<AccountServiceLogic> logger)
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
    private ILogger<AccountServiceLogic> Logger { get; }

    #region Overrides of UserBase

    /// <summary>
    ///     查询用户
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查询用户")]
    [Authorize(ArtemisAuthorizePolicy.Token)]
    public override async Task<SearchUserInfoResponse> SearchUserInfo(SearchUserInfoRequest request,
        ServerCallContext context)
    {
        var userInfos = await UserManager.FetchUserAsync(
            request.UserName,
            request.Email,
            request.Phone,
            request.Page ?? 0,
            request.Size ?? 0,
            context.CancellationToken);

        var userReplies = userInfos.Items!.Select(item => item.Adapt<UserInfoPacket>());

        var response = DataResult.Success(userInfos).Adapt<SearchUserInfoResponse>();

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
    [Authorize(ArtemisAuthorizePolicy.Token)]
    public override async Task<ReadUserInfoResponse> ReadUserInfo(ReadUserInfoRequest request,
        ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var userInfo = await UserManager.GetUserAsync(userId, context.CancellationToken);

        if (userInfo is null) return DataResult.Fail<UserInfo>().Adapt<ReadUserInfoResponse>();

        return DataResult.Success(userInfo).Adapt<ReadUserInfoResponse>();
    }

    /// <summary>
    ///     创建用户
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    [Description("创建用户")]
    [Authorize(ArtemisAuthorizePolicy.Token)]
    public override async Task<AffectedResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        var result = await UserManager.CreateUserAsync(new UserSign
        {
            UserName = request.UserName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        }, request.Password, context.CancellationToken);

        if (result.Succeeded) return DataResult.EmptySuccess().Adapt<AffectedResponse>();

        return DataResult.EmptyFail($"创建失败。{result.DescribeError}").Adapt<AffectedResponse>();
    }

    /// <summary>
    ///     批量创建用户
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量创建用户")]
    [Authorize(ArtemisAuthorizePolicy.Token)]
    public override async Task<AffectedResponse> BatchCreateUser(BatchCreateUserRequest request,
        ServerCallContext context)
    {
        var dictionary = request.Batch.ToDictionary(
            create => new UserSign
            {
                UserName = create.UserName,
                Email = create.Email,
                PhoneNumber = create.PhoneNumber
            },
            create => create.Password);

        var result = await UserManager.CreateUsersAsync(dictionary, context.CancellationToken);

        if (result.Succeeded) return DataResult.EmptySuccess().Adapt<AffectedResponse>();

        return DataResult.EmptyFail($"创建失败。{result.DescribeError}").Adapt<AffectedResponse>();
    }

    /// <summary>
    ///     更新用户信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("更新用户信息")]
    [Authorize(ArtemisAuthorizePolicy.Token)]
    public override async Task<AffectedResponse> UpdateUser(UpdateUserRequest request, ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var result = await UserManager.UpdateUserAsync(userId, new UserPackage
        {
            UserName = request.User.UserName,
            Email = request.User.Email,
            PhoneNumber = request.User.PhoneNumber
        }, context.CancellationToken);

        if (result.Succeeded) return DataResult.EmptySuccess().Adapt<AffectedResponse>();

        return DataResult.EmptyFail($"更新失败。{result.DescribeError}").Adapt<AffectedResponse>();
    }

    /// <summary>
    ///     批量更新用户信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    public override async Task<AffectedResponse> BatchUpdateUser(BatchUpdateUserRequest request,
        ServerCallContext context)
    {
        var dictionary = request.Batch.ToDictionary(
            update => Guid.Parse(update.UserId),
            update => new UserPackage
            {
                UserName = update.User.UserName,
                Email = update.User.Email,
                PhoneNumber = update.User.PhoneNumber
            });

        var result = await UserManager.UpdateUsersAsync(dictionary, context.CancellationToken);

        if (result.Succeeded) return DataResult.EmptySuccess().Adapt<AffectedResponse>();

        return DataResult.EmptyFail($"更新失败。{result.DescribeError}").Adapt<AffectedResponse>();
    }

    /// <summary>
    ///     删除用户
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("删除用户")]
    [Authorize(ArtemisAuthorizePolicy.Token)]
    public override async Task<AffectedResponse> DeleteUser(DeleteUserRequest request, ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var result = await UserManager.DeleteUserAsync(userId, context.CancellationToken);

        if (result.Succeeded) return DataResult.EmptySuccess().Adapt<AffectedResponse>();

        return DataResult.EmptyFail($"删除失败。{result.DescribeError}").Adapt<AffectedResponse>();
    }

    /// <summary>
    ///     批量删除用户
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量删除用户")]
    [Authorize(ArtemisAuthorizePolicy.Token)]
    public override async Task<AffectedResponse> BatchDeleteUser(BatchDeleteUserRequest request,
        ServerCallContext context)
    {
        var idList = request.UserIds.Select(Guid.Parse).ToList();

        var result = await UserManager.DeleteUsersAsync(idList, context.CancellationToken);

        if (result.Succeeded) return DataResult.EmptySuccess().Adapt<AffectedResponse>();

        return DataResult.EmptyFail($"删除失败。{result.DescribeError}").Adapt<AffectedResponse>();
    }

    /// <summary>
    ///     查询用户角色信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查询用户角色信息")]
    [Authorize(ArtemisAuthorizePolicy.Token)]
    public override async Task<SearchUserRoleInfoResponse> SearchUserRoleInfo(SearchUserRoleInfoRequest request,
        ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var roleInfos = await UserManager.FetchUserRolesAsync(
            userId,
            request.RoleName,
            request.Page ?? 0,
            request.Size ?? 0,
            context.CancellationToken);

        var roleReplies = roleInfos.Items!.Select(item => item.Adapt<UserRoleInfoPacket>());

        var response = DataResult.Success(roleInfos).Adapt<SearchUserRoleInfoResponse>();

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
    [Authorize(ArtemisAuthorizePolicy.Token)]
    public override async Task<ReadUserRoleInfoResponse> ReadUserRoleInfo(ReadUserRoleInfoRequest request,
        ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);
        var roleId = Guid.Parse(request.RoleId);

        var roleInfo = await UserManager.GetUserRoleAsync(
            userId,
            roleId,
            context.CancellationToken);

        if (roleInfo is null) return DataResult.Fail<RoleInfo>().Adapt<ReadUserRoleInfoResponse>();

        return DataResult.Success(roleInfo).Adapt<ReadUserRoleInfoResponse>();
    }

    /// <summary>
    ///     添加用户角色
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("添加用户角色")]
    [Authorize(ArtemisAuthorizePolicy.Token)]
    public override async Task<AffectedResponse> AddUserRole(AddUserRoleRequest request, ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);
        var roleId = Guid.Parse(request.Role.RoleId);

        var result = await UserManager.AddUserRoleAsync(userId, roleId, context.CancellationToken);

        if (result.Succeeded) return DataResult.EmptySuccess().Adapt<AffectedResponse>();

        return DataResult.EmptyFail($"添加失败。{result.DescribeError}").Adapt<AffectedResponse>();
    }

    /// <summary>
    ///     批量添加用户角色
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量添加用户角色")]
    [Authorize(ArtemisAuthorizePolicy.Token)]
    public override async Task<AffectedResponse> BatchAddUserRole(BatchAddUserRoleRequest request,
        ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var roleIds = request.RoleIds.Select(Guid.Parse).ToList();

        var result = await UserManager.AddUserRolesAsync(userId, roleIds, context.CancellationToken);

        if (result.Succeeded) return DataResult.EmptySuccess().Adapt<AffectedResponse>();

        return DataResult.EmptyFail($"添加失败。{result.DescribeError}").Adapt<AffectedResponse>();
    }

    /// <summary>
    ///     移除用户角色
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("移除用户角色")]
    [Authorize(ArtemisAuthorizePolicy.Token)]
    public override async Task<AffectedResponse> RemoveUserRole(RemoveUserRoleRequest request,
        ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);
        var roleId = Guid.Parse(request.RoleId);

        var result = await UserManager.RemoveUserRoleAsync(userId, roleId, context.CancellationToken);

        if (result.Succeeded) return DataResult.EmptySuccess().Adapt<AffectedResponse>();

        return DataResult.EmptyFail($"移除失败。{result.DescribeError}").Adapt<AffectedResponse>();
    }

    /// <summary>
    ///     批量移除用户角色
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量移除用户角色")]
    [Authorize(ArtemisAuthorizePolicy.Token)]
    public override async Task<AffectedResponse> BatchRemoveUserRole(BatchRemoveUserRoleRequest request,
        ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);
        var roleIds = request.RoleIds.Select(Guid.Parse).ToList();

        var result = await UserManager.RemoveUserRolesAsync(userId, roleIds, context.CancellationToken);

        if (result.Succeeded) return DataResult.EmptySuccess().Adapt<AffectedResponse>();

        return DataResult.EmptyFail($"移除失败。{result.DescribeError}").Adapt<AffectedResponse>();
    }

    /// <summary>
    ///     查询用户凭据信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查询用户凭据信息")]
    [Authorize(ArtemisAuthorizePolicy.Token)]
    public override async Task<SearchUserClaimInfoResponse> SearchUserClaimInfo(SearchUserClaimInfoRequest request,
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

        var userClaimReplies = userClaimInfos.Items!.Select(item => item.Adapt<UserClaimInfoPacket>());

        var response = DataResult.Success(userClaimInfos).Adapt<SearchUserClaimInfoResponse>();

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
    [Authorize(ArtemisAuthorizePolicy.Token)]
    public override async Task<ReadUserClaimInfoResponse> ReadUserClaimInfo(ReadUserClaimInfoRequest request,
        ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var userClaimInfo = await UserManager.GetUserClaimAsync(userId, request.ClaimId, context.CancellationToken);

        if (userClaimInfo is null) return DataResult.Fail<UserClaimInfo>().Adapt<ReadUserClaimInfoResponse>();

        return DataResult.Success(userClaimInfo).Adapt<ReadUserClaimInfoResponse>();
    }

    /// <summary>
    ///     添加用户凭据
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("添加用户凭据")]
    [Authorize(ArtemisAuthorizePolicy.Token)]
    public override async Task<AffectedResponse> AddUserClaim(AddUserClaimRequest request, ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var result = await UserManager.AddUserClaimAsync(
            userId,
            new UserClaimPackage
            {
                ClaimType = request.UserClaim.ClaimType,
                ClaimValue = request.UserClaim.ClaimValue,
                Description = request.UserClaim.Description
            },
            context.CancellationToken);

        if (result.Succeeded) return DataResult.EmptySuccess().Adapt<AffectedResponse>();

        return DataResult.EmptyFail($"添加失败。{result.DescribeError}").Adapt<AffectedResponse>();
    }

    /// <summary>
    ///     批量添加用户凭据
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量添加用户凭据")]
    [Authorize(ArtemisAuthorizePolicy.Token)]
    public override async Task<AffectedResponse> BatchAddUserClaim(BatchAddUserClaimRequest request,
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

        if (result.Succeeded) return DataResult.EmptySuccess().Adapt<AffectedResponse>();

        return DataResult.EmptyFail($"添加失败。{result.DescribeError}").Adapt<AffectedResponse>();
    }

    /// <summary>
    ///     更新用户凭据
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("更新用户凭据")]
    [Authorize(ArtemisAuthorizePolicy.Token)]
    public override async Task<AffectedResponse> UpdateUserClaim(UpdateUserClaimRequest request,
        ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var result = await UserManager.UpdateUserClaimAsync(
            userId,
            request.ClaimId,
            new UserClaimPackage
            {
                ClaimType = request.UserClaim.ClaimType,
                ClaimValue = request.UserClaim.ClaimValue,
                Description = request.UserClaim.Description
            },
            context.CancellationToken);

        if (result.Succeeded) return DataResult.EmptySuccess().Adapt<AffectedResponse>();

        return DataResult.EmptyFail($"更新失败。{result.DescribeError}").Adapt<AffectedResponse>();
    }

    /// <summary>
    ///     批量更新用户凭据
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    public override async Task<AffectedResponse> BatchUpdateUserClaim(BatchUpdateUserClaimRequest request,
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

        if (result.Succeeded) return DataResult.EmptySuccess().Adapt<AffectedResponse>();

        return DataResult.EmptyFail($"更新失败。{result.DescribeError}").Adapt<AffectedResponse>();
    }

    /// <summary>
    ///     删除用户凭据
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("删除用户凭据")]
    [Authorize(ArtemisAuthorizePolicy.Token)]
    public override async Task<AffectedResponse> DeleteUserClaim(UserClaimIdRequest request, ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var result = await UserManager.RemoveUserClaimAsync(userId, request.ClaimId, context.CancellationToken);

        if (result.Succeeded) return DataResult.EmptySuccess().Adapt<AffectedResponse>();

        return DataResult.EmptyFail($"删除失败。{result.DescribeError}").Adapt<AffectedResponse>();
    }

    /// <summary>
    ///     批量删除用户凭据
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量删除用户凭据")]
    [Authorize(ArtemisAuthorizePolicy.Token)]
    public override async Task<AffectedResponse> BatchDeleteUserClaim(BatchDeleteUserClaimRequest request,
        ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var result = await UserManager.RemoveUserClaimsAsync(userId, request.ClaimIds, context.CancellationToken);

        if (result.Succeeded) return DataResult.EmptySuccess().Adapt<AffectedResponse>();

        return DataResult.EmptyFail($"删除失败。{result.DescribeError}").Adapt<AffectedResponse>();
    }

    #endregion
}