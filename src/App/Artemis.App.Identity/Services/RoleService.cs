using Artemis.Data.Core;
using Artemis.Extensions.Web.Controller;
using Artemis.Services.Identity.Managers;
using Artemis.Shared.Identity.Models;
using Artemis.Shared.Identity.Records;
using Artemis.Shared.Identity.Services;
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
    /// <param name="roleId">角色标识</param>
    /// <returns></returns>
    [HttpGet("{roleId}")]
    public Task<DataResult<Role>> Get(Guid roleId)
    {
        return GetRoleAsync(new GetRoleRequest { RoleId = roleId });
    }

    /// <summary>
    ///     查询角色
    /// </summary>
    /// <param name="nameSearch">角色名称</param>
    /// <param name="page">页码</param>
    /// <param name="pageSize">条目数</param>
    /// <returns></returns>
    [HttpGet]
    public Task<DataResult<PageResult<Role>>> Get(string? nameSearch, int page = 1, int pageSize = 20)
    {
        return FetchRolesAsync(new PageRequest<FetchRolesFilter>
        {
            Filter = new FetchRolesFilter
            {
                RoleNameSearch = nameSearch
            },
            Page = page,
            Size = pageSize
        });
    }

    /// <summary>
    ///     创建角色
    /// </summary>
    /// <param name="request">创建角色请求</param>
    /// <returns></returns>
    [HttpPost]
    public Task<DataResult<EmptyRecord>> Post([FromBody] CreateRoleRequest request)
    {
        return CreateRoleAsync(request);
    }

    /// <summary>
    ///     更新角色
    /// </summary>
    /// <param name="request">更新角色请求</param>
    /// <returns></returns>
    [HttpPut]
    public Task<DataResult<EmptyRecord>> Put([FromBody] UpdateRoleRequest request)
    {
        return UpdateRoleAsync(request);
    }

    #endregion

    #region Implementation of IRoleService

    /// <summary>
    ///     获取角色
    /// </summary>
    /// <param name="request">角色名</param>
    /// <returns></returns>
    [NonAction]
    public async Task<DataResult<Role>> GetRoleAsync([FromBody] GetRoleRequest request)
    {
        if (ModelState.IsValid)
        {
            var result = await IdentityManager.GetRoleAsync(request.RoleId);

            if (result is not null) return DataResult.Success(result);
        }

        return DataResult.Fail<Role>("未查询到匹配的角色");
    }

    /// <summary>
    ///     查询角色
    /// </summary>
    /// <param name="request">查询角色请求</param>
    /// <returns></returns>
    [NonAction]
    public async Task<DataResult<PageResult<Role>>> FetchRolesAsync([FromBody] PageRequest<FetchRolesFilter> request)
    {
        var filter = request.Filter;

        var nameSearch = filter.RoleNameSearch ?? string.Empty;

        var result = await IdentityManager.FetchRolesAsync(nameSearch, request.Page, request.Size);

        return DataResult.Success(result);
    }

    /// <summary>
    ///     创建角色
    /// </summary>
    /// <param name="request">创建角色请求</param>
    /// <returns></returns>
    [NonAction]
    public async Task<DataResult<EmptyRecord>> CreateRoleAsync([FromBody] CreateRoleRequest request)
    {
        var result = await IdentityManager.CreateRoleAsync(request.Name, request.Description);

        if (result.Result.Succeeded) return DataResult.Success(new EmptyRecord());

        return DataResult.Fail<EmptyRecord>(
            $"创建失败。{string.Join(",", result.Result.Errors.Select(error => error.Description))}");
    }

    /// <summary>
    ///     更新角色
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [NonAction]
    public async Task<DataResult<EmptyRecord>> UpdateRoleAsync([FromBody] UpdateRoleRequest request)
    {
        var result = await IdentityManager.UpdateRoleAsync(request);

        if (result.Succeeded) return DataResult.Success(new EmptyRecord());

        return DataResult.Fail<EmptyRecord>(
            $"更新失败。{string.Join(",", result.Errors.Select(error => error.Description))}");
    }

    /// <summary>
    ///     创建或更新角色
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [NonAction]
    public async Task<DataResult<EmptyRecord>> CreateOrUpdateRoleAsync([FromBody] UpdateRoleRequest request)
    {
        var result = await IdentityManager.CreateOrUpdateRoleAsync(request);

        if (result.Succeeded) return DataResult.Success(new EmptyRecord());

        return DataResult.Fail<EmptyRecord>(
            $"创建或更新失败。{string.Join(",", result.Errors.Select(error => error.Description))}");
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