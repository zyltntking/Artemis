using System.ComponentModel;
using Artemis.Extensions.Identity;
using Artemis.Service.Identity.Managers;
using Artemis.Service.Identity.Protos;
using Artemis.Service.Shared.Identity.Transfer;
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
    ///     查询用户信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查询用户信息")]
    [Authorize(AuthorizePolicy.Admin)]
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

        return userInfos.PagedResponse<SearchUserInfoResponse, UserInfo>();
    }

    /// <summary>
    ///     读取用户信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("读取用户信息")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<ReadUserInfoResponse> ReadUserInfo(ReadUserInfoRequest request,
        ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var userInfo = await UserManager.GetUserAsync(userId, context.CancellationToken);

        return userInfo.ReadInfoResponse<ReadUserInfoResponse, UserInfo>();
    }

    /// <summary>
    ///     创建用户
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    [Description("创建用户")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        var result = await UserManager.CreateUserAsync(new UserSign
        {
            UserName = request.UserName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        }, request.Password, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量创建用户
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量创建用户")]
    [Authorize(AuthorizePolicy.Admin)]
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

        return result.AffectedResponse();
    }

    /// <summary>
    ///     更新用户信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("更新用户信息")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> UpdateUser(UpdateUserRequest request, ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var result = await UserManager.UpdateUserAsync(userId, new UserPackage
        {
            UserName = request.User.UserName,
            Email = request.User.Email,
            PhoneNumber = request.User.PhoneNumber
        }, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量更新用户信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量更新用户信息")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchUpdateUser(
        BatchUpdateUserRequest request,
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

        return result.AffectedResponse();
    }

    /// <summary>
    ///     删除用户
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("删除用户")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> DeleteUser(DeleteUserRequest request, ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var result = await UserManager.DeleteUserAsync(userId, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量删除用户
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量删除用户")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchDeleteUser(BatchDeleteUserRequest request,
        ServerCallContext context)
    {
        var ids = request.UserIds.Select(Guid.Parse);

        var result = await UserManager.DeleteUsersAsync(ids, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     查询用户角色信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查询用户角色信息")]
    [Authorize(AuthorizePolicy.Admin)]
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

        return roleInfos.PagedResponse<SearchUserRoleInfoResponse, RoleInfo>();
    }

    /// <summary>
    ///     获取用户角色信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("获取用户角色信息")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<ReadUserRoleInfoResponse> ReadUserRoleInfo(ReadUserRoleInfoRequest request,
        ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);
        var roleId = Guid.Parse(request.RoleId);

        var roleInfo = await UserManager.GetUserRoleAsync(
            userId,
            roleId,
            context.CancellationToken);

        return roleInfo.ReadInfoResponse<ReadUserRoleInfoResponse, RoleInfo>();
    }

    /// <summary>
    ///     添加用户角色
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("添加用户角色")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> AddUserRole(AddUserRoleRequest request, ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);
        var roleId = Guid.Parse(request.Role.RoleId);

        var result = await UserManager.AddUserRoleAsync(userId, roleId, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量添加用户角色
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量添加用户角色")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchAddUserRole(BatchAddUserRoleRequest request,
        ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var roleIds = request.RoleIds.Select(Guid.Parse);

        var result = await UserManager.AddUserRolesAsync(userId, roleIds, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     移除用户角色
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("移除用户角色")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> RemoveUserRole(RemoveUserRoleRequest request,
        ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);
        var roleId = Guid.Parse(request.RoleId);

        var result = await UserManager.RemoveUserRoleAsync(userId, roleId, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量移除用户角色
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量移除用户角色")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchRemoveUserRole(BatchRemoveUserRoleRequest request,
        ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);
        var roleIds = request.RoleIds.Select(Guid.Parse).ToList();

        var result = await UserManager.RemoveUserRolesAsync(userId, roleIds, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     查询用户凭据信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查询用户凭据信息")]
    [Authorize(AuthorizePolicy.Admin)]
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

        return userClaimInfos.PagedResponse<SearchUserClaimInfoResponse, UserClaimInfo>();
    }

    /// <summary>
    ///     获取用户凭据信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("获取用户凭据信息")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<ReadUserClaimInfoResponse> ReadUserClaimInfo(ReadUserClaimInfoRequest request,
        ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var userClaimInfo = await UserManager.GetUserClaimAsync(userId, request.ClaimId, context.CancellationToken);

        return userClaimInfo.ReadInfoResponse<ReadUserClaimInfoResponse, UserClaimInfo>();
    }

    /// <summary>
    ///     添加用户凭据
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("添加用户凭据")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> AddUserClaim(AddUserClaimRequest request, ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var package = request.UserClaim.Adapt<UserClaimPackage>();

        var result = await UserManager.AddUserClaimAsync(userId, package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量添加用户凭据
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量添加用户凭据")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchAddUserClaim(BatchAddUserClaimRequest request,
        ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var packages = request.Batch.Adapt<IEnumerable<UserClaimPackage>>();

        var result = await UserManager.AddUserClaimsAsync(userId, packages, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     更新用户凭据
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("更新用户凭据")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> UpdateUserClaim(UpdateUserClaimRequest request,
        ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var package = request.UserClaim.Adapt<UserClaimPackage>();

        var result = await UserManager.UpdateUserClaimAsync(userId, request.ClaimId, package, context.CancellationToken);

        return result.AffectedResponse();
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

        return result.AffectedResponse();
    }

    /// <summary>
    ///     删除用户凭据
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("删除用户凭据")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> DeleteUserClaim(UserClaimIdRequest request, ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var result = await UserManager.RemoveUserClaimAsync(userId, request.ClaimId, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量删除用户凭据
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量删除用户凭据")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchDeleteUserClaim(BatchDeleteUserClaimRequest request,
        ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var result = await UserManager.RemoveUserClaimsAsync(userId, request.ClaimIds, context.CancellationToken);

        return result.AffectedResponse();
    }

    #endregion
}