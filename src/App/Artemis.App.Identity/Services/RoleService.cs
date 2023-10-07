using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Extensions.Web.Controller;
using Artemis.Services.Identity.Managers;
using Artemis.Shared.Identity.Services;
using Artemis.Shared.Identity.Transfer;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

namespace Artemis.App.Identity.Services;

/// <summary>
///     角色服务
/// </summary>
[Route("api/Roles")]
public class RoleService : ApiController, IRoleService
{
    /// <summary>
    ///     泛型API控制器
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="logger"></param>
    public RoleService(
        IRoleManager manager,
        ILogger<RoleService> logger) : base(logger)
    {
        RoleManager = manager;
    }

    /// <summary>
    ///     认证管理器
    /// </summary>
    private IRoleManager RoleManager { get; }

    #region ControllerActions

    /// <summary>
    ///     查询角色
    /// </summary>
    /// <param name="nameSearch">角色名称匹配</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <returns>Roles PagedResult</returns>
    /// <remark>GET api/Roles</remark>
    [HttpGet]
    public Task<DataResult<PageResult<RoleInfo>>> FetchRoles(
        string? nameSearch,
        int page = 1,
        int size = 20)
    {
        var request = new PageRequest<FetchRolesFilter>
        {
            Page = page,
            Size = size,
            Filter = new FetchRolesFilter
            {
                RoleNameSearch = nameSearch
            }
        };

        return FetchRolesAsync(request);
    }

    /// <summary>
    ///     获取角色
    /// </summary>
    /// <param name="roleId">角色标识</param>
    /// <returns>Role Result</returns>
    /// <remark>GET api/Roles/{roleId}</remark>
    [HttpGet("{roleId}")]
    public Task<DataResult<RoleInfo>> GetRole(Guid roleId)
    {
        var request = new GetRoleRequest
        {
            RoleId = roleId
        };

        return GetRoleAsync(request);
    }

    /// <summary>
    ///     创建角色
    /// </summary>
    /// <param name="request">创建角色请求</param>
    /// <returns>Create Status</returns>
    /// <remark>POST api/Roles</remark>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public Task<DataResult<RoleInfo>> PostRole([FromBody] [Required] CreateRoleRequest request)
    {
        return CreateRoleAsync(request);
    }

    /// <summary>
    ///     更新角色
    /// </summary>
    /// <param name="roleId">角色名</param>
    /// <param name="rolePack">更新角色信息</param>
    /// <returns>Update Status</returns>
    /// <remark>PUT api/Roles/{roleId}</remark>
    [HttpPut("{roleId}")]
    public Task<DataResult<RoleInfo>> PutRole(Guid roleId, [FromBody] [Required] RolePackage rolePack)
    {
        var request = new UpdateRoleRequest
        {
            RoleId = roleId,
            RolePack = rolePack
        };

        return CreateOrUpdateRoleAsync(request);
    }

    /// <summary>
    ///     删除角色
    /// </summary>
    /// <param name="roleId">角色标识</param>
    /// <returns>Delete Status</returns>
    /// <remarks>DELETE api/Roles/{roleId}</remarks>
    [HttpDelete("{roleId}")]
    public Task<DataResult<EmptyRecord>> DeleteRole(Guid roleId)
    {
        var request = new DeleteRoleRequest
        {
            RoleId = roleId
        };

        return DeleteRoleAsync(request);
    }

    /// <summary>
    ///     角色批量处理
    /// </summary>
    /// <returns>PATCH status</returns>
    /// <remarks>PATCH api/Roles</remarks>
    [HttpPatch]
    public Task<DataResult<EmptyRecord>> PatchRoles()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     查询角色用户列表
    /// </summary>
    /// <param name="roleId">角色名</param>
    /// <param name="userNameSearch">用户名匹配值</param>
    /// <param name="emailSearch">邮箱匹配值</param>
    /// <param name="phoneSearch">电话匹配值</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <returns>GET status</returns>
    /// <remarks>GET api/Roles/{roleId}/Users</remarks>
    [HttpGet("{roleId}/Users")]
    public Task<DataResult<PageResult<UserInfo>>> FetchRoleUsers(
        Guid roleId,
        string? userNameSearch = null,
        string? emailSearch = null,
        string? phoneSearch = null,
        int page = 1,
        int size = 20)
    {
        var request = new PageRequest<FetchRoleUsersFilter>
        {
            Page = page,
            Size = size,
            Filter = new FetchRoleUsersFilter
            {
                RoleId = roleId,
                UserNameSearch = userNameSearch,
                EmailSearch = emailSearch,
                PhoneNumberSearch = phoneSearch
            }
        };

        return FetchRoleUsersAsync(request);
    }

    /// <summary>
    ///     查询角色凭据列表
    /// </summary>
    /// <param name="roleId">角色名</param>
    /// <param name="claimTypeSearch">凭据类型搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <returns></returns>
    /// <remarks>GET api/Roles/{roleName}/Claims</remarks>
    [HttpGet("{roleId}/Claims")]
    public Task<DataResult<PageResult<RoleClaimInfo>>> FetchRoleClaims(
        Guid roleId,
        string? claimTypeSearch,
        int page = 1,
        int size = 20)
    {
        var request = new PageRequest<FetchRoleClaimsFilter>
        {
            Page = page,
            Size = size,
            Filter = new FetchRoleClaimsFilter
            {
                RoleId = roleId,
                ClaimTypeSearch = claimTypeSearch
            }
        };

        return FetchRoleClaimsAsync(request);
    }

    /// <summary>
    ///     获取角色凭据
    /// </summary>
    /// <param name="roleId">角色名</param>
    /// <param name="claimId">凭据标识</param>
    /// <returns></returns>
    /// <remarks>GET api/Roles/{roleId}/Claims/{claimId}</remarks>
    [HttpGet("{roleId}/Claims/{claimId}")]
    public Task<DataResult<RoleClaimInfo>> GetRoleClaim(
        Guid roleId,
        int claimId)
    {
        var request = new GetRoleClaimRequest
        {
            RoleId = roleId,
            ClaimId = claimId
        };

        return GetRoleClaimAsync(request);
    }

    /// <summary>
    ///     创建角色凭据
    /// </summary>
    /// <param name="roleId">角色标识</param>
    /// <param name="claimPack"></param>
    /// <returns>POST api/Roles/{roleId}/Claims</returns>
    [HttpPost("{roleId}/Claims")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public Task<DataResult<RoleClaimInfo>> PostRoleClaim(
        Guid roleId,
        [FromBody] [Required] RoleClaimPackage claimPack)
    {
        var request = new CreateRoleClaimRequest
        {
            RoleId = roleId,
            ClaimPack = claimPack
        };

        return CreateRoleClaimAsync(request);
    }

    /// <summary>
    ///     更新角色凭据
    /// </summary>
    /// <param name="roleId">角色标识</param>
    /// <param name="claimId">凭据标识</param>
    /// <param name="claimPack">凭据信息</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPut("{roleId}/Claims/{claimId}")]
    public Task<DataResult<RoleClaimInfo>> PutRoleClaim(
        Guid roleId,
        int claimId,
        [FromBody] [Required] RoleClaimPackage claimPack)
    {
        var request = new UpdateRoleClaimRequest
        {
            RoleId = roleId,
            ClaimId = claimId,
            ClaimPack = claimPack
        };

        return CreateOrUpdateRoleClaimAsync(request);
    }

    /// <summary>
    ///     删除角色凭据
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="claimId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpDelete("{roleId}/Claims/{claimId}")]
    public Task<DataResult<EmptyRecord>> DeleteRoleClaim(
        Guid roleId,
        int claimId)
    {
        var request = new DeleteRoleClaimRequest
        {
            RoleId = roleId,
            ClaimId = claimId
        };

        return DeleteRoleClaimAsync(request);
    }

    /// <summary>
    ///     角色凭据批量处理
    /// </summary>
    /// <param name="roleId">角色标识</param>
    /// <returns></returns>
    [HttpPatch("{roleId}/Claims")]
    public Task<DataResult<EmptyRecord>> PatchRoleClaim(Guid roleId)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Implementation of IRoleService

    /// <summary>
    ///     查询角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [NonAction]
    public async Task<DataResult<PageResult<RoleInfo>>> FetchRolesAsync(
        PageRequest<FetchRolesFilter> request,
        ServerCallContext? context = default)
    {
        var filter = request.Filter;

        var result = await RoleManager.FetchRolesAsync(
            filter.RoleNameSearch, 
            request.Page, 
            request.Size,
            context?.CancellationToken ?? default);

        return DataResult.Success(result);
    }

    /// <summary>
    ///     获取角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [NonAction]
    public async Task<DataResult<RoleInfo>> GetRoleAsync(
        GetRoleRequest request, 
        ServerCallContext? context = default)
    {
        var result = await RoleManager.GetRoleAsync(
            request.RoleId, 
            context?.CancellationToken ?? default);

        return result is not null 
            ? DataResult.Success(result) 
            : DataResult.Fail<RoleInfo>("未查询到匹配的角色");
    }

    /// <summary>
    ///     创建角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns>创建结果</returns>
    [NonAction]
    public async Task<DataResult<RoleInfo>> CreateRoleAsync(
        CreateRoleRequest request,
        ServerCallContext? context = default)
    {
        var (result, role) = await RoleManager.CreateRoleAsync(
            request, 
            context?.CancellationToken ?? default);

        return result.Succeeded
            ? DataResult.Success(role)!
            : DataResult.Fail<RoleInfo>($"创建失败。{result.DescribeError}");
    }

    /// <summary>
    ///     创建或更新角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [NonAction]
    public async Task<DataResult<RoleInfo>> CreateOrUpdateRoleAsync(
        UpdateRoleRequest request, 
        ServerCallContext? context = default)
    {
        var (result, role) = await RoleManager.CreateOrUpdateRoleAsync(
            request.RoleId, 
            request.RolePack, 
            context?.CancellationToken ?? default);

        return result.Succeeded
            ? DataResult.Success(role)!
            : DataResult.Fail<RoleInfo>($"创建或更新失败。{result.DescribeError}");
    }

    /// <summary>
    ///     删除角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [NonAction]
    public async Task<DataResult<EmptyRecord>> DeleteRoleAsync(
        DeleteRoleRequest request, 
        ServerCallContext? context = default)
    {
        var result = await RoleManager.DeleteRoleAsync(
            request.RoleId,
            context?.CancellationToken ?? default);

        return result.Succeeded
            ? DataResult.Success(new EmptyRecord())
            : DataResult.Fail<EmptyRecord>($"删除失败。{result.DescribeError}");
    }

    /// <summary>
    ///     查询角色用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [NonAction]
    public async Task<DataResult<PageResult<UserInfo>>> FetchRoleUsersAsync(
        PageRequest<FetchRoleUsersFilter> request, 
        ServerCallContext? context = default)
    {
        var filter = request.Filter;

        var result = await RoleManager.FetchRoleUsersAsync(
            filter.RoleId,
            filter.UserNameSearch,
            filter.EmailSearch,
            filter.PhoneNumberSearch,
            request.Page,
            request.Size,
            context?.CancellationToken ?? default);

        return DataResult.Success(result);
    }

    /// <summary>
    ///     查询角色凭据
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [NonAction]
    public async Task<DataResult<PageResult<RoleClaimInfo>>> FetchRoleClaimsAsync(
        PageRequest<FetchRoleClaimsFilter> request, 
        ServerCallContext? context = default)
    {
        var filter = request.Filter;

        var result = await RoleManager.FetchRoleClaimsAsync(
            filter.RoleId,
            filter.ClaimTypeSearch,
            request.Page,
            request.Size,
            context?.CancellationToken ?? default);

        return DataResult.Success(result);
    }

    /// <summary>
    ///     获取角色凭据
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [NonAction]
    public async Task<DataResult<RoleClaimInfo>> GetRoleClaimAsync(
        GetRoleClaimRequest request, 
        ServerCallContext? context = default)
    {
        var result = await RoleManager.GetRoleClaimAsync(
            request.RoleId, 
            request.ClaimId, 
            context?.CancellationToken ?? default);

        return result is not null 
            ? DataResult.Success(result) 
            : DataResult.Fail<RoleClaimInfo>("未查询到对应的凭据");
    }

    /// <summary>
    ///     创建角色凭据
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [NonAction]
    public async Task<DataResult<RoleClaimInfo>> CreateRoleClaimAsync(
        CreateRoleClaimRequest request, 
        ServerCallContext? context = default)
    {
        var (result, roleClaim) = await RoleManager.CreateRoleClaimAsync(
            request.RoleId, 
            request.ClaimPack,
            context?.CancellationToken ?? default);

        return result.Succeeded
            ? DataResult.Success(roleClaim)!
            : DataResult.Fail<RoleClaimInfo>($"创建失败。{result.DescribeError}");
    }

    /// <summary>
    ///     创建或更新角色凭据
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [NonAction]
    public async Task<DataResult<RoleClaimInfo>> CreateOrUpdateRoleClaimAsync(
        UpdateRoleClaimRequest request, 
        ServerCallContext? context = default)
    {
        var (result, roleClaim) = await RoleManager.CreateOrUpdateRoleClaimAsync(
            request.RoleId, 
            request.ClaimId, 
            request.ClaimPack,
            context?.CancellationToken ?? default);

        return result.Succeeded
            ? DataResult.Success(roleClaim)!
            : DataResult.Fail<RoleClaimInfo>($"创建或更新失败。{result.DescribeError}");
    }

    /// <summary>
    ///     删除角色凭据
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [NonAction]
    public async Task<DataResult<EmptyRecord>> DeleteRoleClaimAsync(
        DeleteRoleClaimRequest request, 
        ServerCallContext? context = default)
    {
        var result = await RoleManager.DeleteRoleClaimAsync(
            request.RoleId, 
            request.ClaimId,
            context?.CancellationToken ?? default);

        return result.Succeeded
            ? DataResult.Success(new EmptyRecord())
            : DataResult.Fail<EmptyRecord>($"删除失败。{result.DescribeError}");
    }

    #endregion
}