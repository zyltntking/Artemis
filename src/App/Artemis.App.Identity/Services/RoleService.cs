using Artemis.Data.Core;
using Artemis.Extensions.Web.Controller;
using Artemis.Services.Identity.Managers;
using Artemis.Shared.Identity.Models;
using Artemis.Shared.Identity.Records;
using Artemis.Shared.Identity.Services;
using Artemis.Shared.Identity.Transfer;
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
        IIdentityManager manager,
        ILogger<RoleService> logger) : base(logger)
    {
        IdentityManager = manager;
    }

    /// <summary>
    ///     认证管理器
    /// </summary>
    private IIdentityManager IdentityManager { get; }

    #region ControllerActions

    /// <summary>
    ///     获取角色
    /// </summary>
    /// <param name="roleName">角色标识</param>
    /// <returns>Role Result</returns>
    /// <remark>GET api/Roles/{roleName}</remark>
    [HttpGet("{roleName}")]
    public Task<DataResult<RoleInfo>> Get(string roleName)
    {
        var request = new GetRoleRequest
        {
            RoleName = roleName
        };

        return GetRoleAsync(request);
    }

    /// <summary>
    ///     查询角色
    /// </summary>
    /// <param name="nameSearch">角色名称</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <returns>Roles PagedResult</returns>
    /// <remark>GET api/Roles</remark>
    [HttpGet]
    public Task<DataResult<PageResult<RoleInfo>>> Fetch(
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
    ///     创建角色
    /// </summary>
    /// <param name="roleRequest">创建角色请求</param>
    /// <returns>Create Status</returns>
    /// <remark>POST api/Roles</remark>
    [HttpPost]
    public Task<DataResult<EmptyRecord>> Post([FromBody] CreateRoleRequest roleRequest)
    {
        return CreateRoleAsync(roleRequest);
    }

    /// <summary>
    ///     更新角色
    /// </summary>
    /// <param name="roleName">角色名</param>
    /// <param name="roleInfo">更新角色信息</param>
    /// <returns>Update Status</returns>
    /// <remark>PUT api/Roles/{roleName}</remark>
    [HttpPut("{roleName}")]
    public Task<DataResult<EmptyRecord>> Put(string roleName, [FromBody] RoleBase roleInfo)
    {
        var request = new UpdateRoleRequest
        {
            RoleName = roleName,
            RoleInfo = roleInfo
        };

        return CreateOrUpdateRoleAsync(request);
    }

    /// <summary>
    /// 删除角色
    /// </summary>
    /// <param name="roleName"></param>
    /// <returns>Delete Status</returns>
    /// <remarks>DELETE api/Roles/{roleName}</remarks>
    [HttpDelete("{roleName}")]
    public Task<DataResult<EmptyRecord>> Delete(string roleName)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Implementation of IRoleService

    /// <summary>
    ///     获取角色
    /// </summary>
    /// <param name="request">角色名</param>
    /// <returns></returns>
    [NonAction]
    public async Task<DataResult<RoleInfo>> GetRoleAsync([FromBody] GetRoleRequest request)
    {
        var result = await IdentityManager.GetRoleAsync(request.RoleName);

        return result is not null ? DataResult.Success(result) : DataResult.Fail<RoleInfo>("未查询到匹配的角色");
    }

    /// <summary>
    ///     查询角色
    /// </summary>
    /// <param name="request">查询角色请求</param>
    /// <returns></returns>
    [NonAction]
    public async Task<DataResult<PageResult<RoleInfo>>> FetchRolesAsync([FromBody] PageRequest<FetchRolesFilter> request)
    {
        var filter = request.Filter;

        var nameSearch = filter.RoleNameSearch ?? string.Empty;

        var result = await IdentityManager.FetchRolesAsync(nameSearch, request.Page, request.Size);

        return DataResult.Success(result);
    }

    /// <summary>
    ///     创建角色
    /// </summary>
    /// <param name="roleRequest">创建角色请求</param>
    /// <returns>创建结果</returns>
    [NonAction]
    public async Task<DataResult<EmptyRecord>> CreateRoleAsync([FromBody] CreateRoleRequest roleRequest)
    {
        var result = await IdentityManager.CreateRoleAsync(roleRequest.Name, roleRequest.Description);

        if (result.Succeeded) 
            return DataResult.Success(new EmptyRecord());

        return DataResult.Fail<EmptyRecord>($"创建失败。{string.Join(",", result.Errors.Select(error => error.Description))}");
    }

    /// <summary>
    ///     创建或更新角色
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [NonAction]
    public async Task<DataResult<EmptyRecord>> CreateOrUpdateRoleAsync([FromBody] UpdateRoleRequest request)
    {
        var result = await IdentityManager.CreateOrUpdateRoleAsync(request.RoleName, request.RoleInfo);

        if (result.Succeeded) 
            return DataResult.Success(new EmptyRecord());

        return DataResult.Fail<EmptyRecord>($"创建或更新失败。{string.Join(",", result.Errors.Select(error => error.Description))}");
    }

    /// <summary>
    ///     删除角色
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [NonAction]
    public async Task<DataResult<EmptyRecord>> DeleteRoleAsync(DeleteRoleRequest request)
    {
        var result = await IdentityManager.DeleteRoleAsync(request.RoleId);

        if (result.Succeeded) return DataResult.Success(new EmptyRecord());

        return DataResult.Fail<EmptyRecord>(
            $"删除失败。{string.Join(",", result.Errors.Select(error => error.Description))}");
    }

    /// <summary>
    ///     删除角色
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [NonAction]
    public async Task<DataResult<EmptyRecord>> DeleteRolesAsync(DeleteRolesRequest request)
    {
        var result = await IdentityManager.DeleteRolesAsync(request.RoleIds);

        if (result.Succeeded) return DataResult.Success(new EmptyRecord());

        return DataResult.Fail<EmptyRecord>(
            $"删除失败。{string.Join(",", result.Errors.Select(error => error.Description))}");
    }

    /// <summary>
    ///     查询角色用户
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [NonAction]
    public async Task<DataResult<PageResult<User>>> FetchRoleUsersAsync(PageRequest<FetchRoleUsersFilter> request)
    {
        var filter = request.Filter;

        var roleId = filter.RoleId;

        var nameSearch = filter.UserNameSearch ?? string.Empty;

        var result = await IdentityManager.FetchRoleUsersAsync(roleId, nameSearch, request.Page, request.Size);

        return DataResult.Success(result);
    }

    #endregion
}