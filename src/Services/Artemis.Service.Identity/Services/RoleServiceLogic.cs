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
///     角色服务
/// </summary>
public class RoleServiceLogic : RoleService.RoleServiceBase
{
    /// <summary>
    ///     账户服务
    /// </summary>
    /// <param name="roleManager">角色管理器</param>
    /// <param name="logger">日志记录器</param>
    public RoleServiceLogic(
        IIdentityRoleManager roleManager,
        ILogger<RoleServiceLogic> logger)
    {
        RoleManager = roleManager;
        Logger = logger;
    }

    /// <summary>
    ///     角色管理器
    /// </summary>
    private IIdentityRoleManager RoleManager { get; }

    /// <summary>
    ///     日志依赖
    /// </summary>
    private ILogger<RoleServiceLogic> Logger { get; }

    #region Overrides of RoleServiceBase

    /// <summary>
    /// 查询角色信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查询角色信息")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<SearchRoleInfoResponse> SearchRoleInfo(SearchRoleInfoRequest request, ServerCallContext context)
    {
        var roleInfos = await RoleManager.FetchRolesAsync(
            request.RoleName, 
            request.Page ?? 0, 
            request.Size ?? 0,
            context.CancellationToken);

        return roleInfos.PagedResponse<SearchRoleInfoResponse, RoleInfo>();
    }

    /// <summary>
    /// 读取角色信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("读取角色信息")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<ReadRoleInfoResponse> ReadRoleInfo(ReadRoleInfoRequest request, ServerCallContext context)
    {
        var roleId = Guid.Parse(request.RoleId);

        var roleInfo = await RoleManager.GetRoleAsync(roleId, context.CancellationToken);

        return roleInfo.ReadInfoResponse<ReadRoleInfoResponse, RoleInfo>();
    }

    /// <summary>
    /// 创建角色
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("创建角色")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> CreateRole(CreateRoleRequest request, ServerCallContext context)
    {
        var package = request.Adapt<RolePackage>();

        var result = await RoleManager.CreateRoleAsync(package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 批量创建角色
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量创建角色")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchCreateRole(BatchCreateRoleRequest request, ServerCallContext context)
    {
        var packages = request.Batch.Adapt<IEnumerable<RolePackage>>();

        var result = await RoleManager.CreateRolesAsync(packages, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 更新角色
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("更新角色")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> UpdateRole(UpdateRoleRequest request, ServerCallContext context)
    {
        var roleId = Guid.Parse(request.RoleId);

        var rolePackage = request.Role.Adapt<RolePackage>();

        var result = await RoleManager.UpdateRoleAsync(roleId, rolePackage, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 批量更新角色
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量更新角色")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchUpdateRole(BatchUpdateRoleRequest request, ServerCallContext context)
    {
        var package = request.Batch.ToDictionary(
            item =>  Guid.Parse(item.RoleId), 
            item => item.Role.Adapt<RolePackage>());

        var result = await RoleManager.UpdateRolesAsync(package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 删除角色
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("删除角色")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> DeleteRole(DeleteRoleRequest request, ServerCallContext context)
    {
        var roleId = Guid.Parse(request.RoleId);

        var result = await RoleManager.DeleteRoleAsync(roleId, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 批量删除角色
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量删除角色")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchDeleteRole(BatchDeleteRoleRequest request, ServerCallContext context)
    {
        var roleIds = request.RoleIds.Select(Guid.Parse);

        var result = await RoleManager.DeleteRolesAsync(roleIds, context.CancellationToken);

        return result.AffectedResponse();

    }

    /// <summary>
    /// 查询角色用户信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查询角色用户信息")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<SearchRoleUserInfoResponse> SearchRoleUserInfo(SearchRoleUserInfoRequest request, ServerCallContext context)
    {
        var roleId = Guid.Parse(request.RoleId);

        var userInfos = await RoleManager.FetchRoleUsersAsync(roleId);

        return userInfos.PagedResponse<SearchRoleUserInfoResponse, UserInfo>();
    }

    /// <summary>
    /// 读取用户角色信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("读取用户角色信息")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<ReadRoleUserInfoResponse> ReadRoleUserInfo(ReadRoleUserInfoRequest request, ServerCallContext context)
    {
        var roleId = Guid.Parse(request.RoleId);
        var userId = Guid.Parse(request.UserId);

        var userInfo = await RoleManager.GetRoleUserAsync(roleId, userId, context.CancellationToken);

        return userInfo.ReadInfoResponse<ReadRoleUserInfoResponse, UserInfo>();
    }

    /// <summary>
    /// 添加角色用户
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("添加角色用户")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> AddRoleUser(AddRoleUserRequest request, ServerCallContext context)
    {
        var roleId = Guid.Parse(request.RoleId);
        var userId = Guid.Parse(request.User.UserId);

        var result = await RoleManager.AddRoleUserAsync(roleId, userId, context.CancellationToken);
        
        return result.AffectedResponse();
    }

    /// <summary>
    /// 批量添加角色用户
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量添加角色用户")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchAddRoleUser(BatchAddRoleUserRequest request, ServerCallContext context)
    {

        var roleId = Guid.Parse(request.RoleId);

        var userIds = request.UserIds.Select(Guid.Parse);

        var result = await RoleManager.AddRoleUsersAsync(roleId, userIds, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 移除角色用户
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("移除角色用户")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> RemoveRoleUser(RemoveRoleUserRequest request, ServerCallContext context)
    {
        var roleId = Guid.Parse(request.RoleId);

        var userId = Guid.Parse(request.UserId);

        var result = await RoleManager.RemoveRoleUserAsync(roleId, userId, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 批量移除角色用户
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量移除角色用户")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchRemoveRoleUser(BatchRemoveRoleUserRequest request, ServerCallContext context)
    {
        var roleId = Guid.Parse(request.RoleId);

        var userIds = request.UserIds.Select(Guid.Parse);

        var result = await RoleManager.RemoveRoleUsersAsync(roleId, userIds, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 查询角色凭据信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查询角色凭据信息")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<SearchRoleClaimInfoResponse> SearchRoleClaimInfo(SearchRoleClaimInfoRequest request, ServerCallContext context)
    {
        var roleId = Guid.Parse(request.RoleId);

        var roleClaimInfos = await RoleManager.FetchRoleClaimsAsync(
            roleId,
            request.ClaimType,
            request.ClaimValue,
            request.Page ?? 0,
            request.Size ?? 0,
            context.CancellationToken);

        return roleClaimInfos.PagedResponse<SearchRoleClaimInfoResponse, RoleClaimInfo>();
    }

    /// <summary>
    /// 读取角色凭据信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("读取角色凭据信息")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<ReadRoleClaimInfoResponse> ReadRoleClaimInfo(ReadRoleClaimInfoRequest request, ServerCallContext context)
    {
        var roleId = Guid.Parse(request.RoleId);

        var roleClaimInfo = await RoleManager.GetRoleClaimAsync(roleId, request.ClaimId, context.CancellationToken);

        return roleClaimInfo.ReadInfoResponse<ReadRoleClaimInfoResponse, RoleClaimInfo>();
    }

    /// <summary>
    /// 添加角色凭据
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("添加角色凭据")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> AddRoleClaim(AddRoleClaimRequest request, ServerCallContext context)
    {
        var roleId = Guid.Parse(request.RoleId);

        var package = request.RoleClaim.Adapt<RoleClaimPackage>();

        var result = await RoleManager.AddRoleClaimAsync(roleId, package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 批量添加角色凭据
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量添加角色凭据")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchAddRoleClaim(BatchAddRoleClaimRequest request, ServerCallContext context)
    {
        var roleId = Guid.Parse(request.RoleId);

        var packages = request.Batch.Adapt<IEnumerable<RoleClaimPackage>>();

        var result = await RoleManager.AddRoleClaimsAsync(roleId, packages, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 更新角色凭据
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("更新角色凭据")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> UpdateRoleClaim(UpdateRoleClaimRequest request, ServerCallContext context)
    {
        var roleId = Guid.Parse(request.RoleId);

        var package = request.RoleClaim.Adapt<RoleClaimPackage>();

        var result = await RoleManager.UpdateRoleClaimAsync(roleId, request.ClaimId, package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 批量更新角色凭据
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量更新角色凭据")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchUpdateRoleClaim(BatchUpdateRoleClaimRequest request, ServerCallContext context)
    {
        var roleId = Guid.Parse(request.RoleId);

        var dictionary = request.Batch.ToDictionary(
            item => item.ClaimId, 
            item => item.Claim.Adapt<RoleClaimPackage>());

        var result = await RoleManager.UpdateRoleClaimsAsync(roleId, dictionary, context.CancellationToken);

        return result.AffectedResponse();
    }


    /// <summary>
    /// 删除角色凭据
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("删除角色凭据")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> DeleteRoleClaim(RoleClaimIdRequest request, ServerCallContext context)
    {
        var roleId = Guid.Parse(request.RoleId);

        var result = await RoleManager.RemoveRoleClaimAsync(roleId, request.ClaimId, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 批量删除角色凭据
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量删除角色凭据")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchDeleteRoleClaim(BatchDeleteRoleClaimRequest request, ServerCallContext context)
    {
        var roleId = Guid.Parse(request.RoleId);

        var result = await RoleManager.RemoveRoleClaimsAsync(roleId, request.ClaimIds, context.CancellationToken);

        return result.AffectedResponse();
    }

    #endregion

}