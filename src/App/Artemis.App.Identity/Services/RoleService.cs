using Artemis.Data.Core;
using Artemis.Extensions.Web.Controller;
using Artemis.Services.Identity.Managers;
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
    ///     查询角色
    /// </summary>
    /// <param name="nameSearch">角色名称匹配</param>
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
    ///     创建角色
    /// </summary>
    /// <param name="roleRequest">创建角色请求</param>
    /// <returns>Create Status</returns>
    /// <remark>POST api/Roles</remark>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
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
    ///     删除角色
    /// </summary>
    /// <param name="roleName"></param>
    /// <returns>Delete Status</returns>
    /// <remarks>DELETE api/Roles/{roleName}</remarks>
    [HttpDelete("{roleName}")]
    public Task<DataResult<EmptyRecord>> Delete(string roleName)
    {
        var request = new DeleteRoleRequest
        {
            RoleName = roleName
        };

        return DeleteRoleAsync(request);
    }

    /// <summary>
    ///     角色批量处理
    /// </summary>
    /// <returns>PATCH status</returns>
    /// <remarks>PATCH api/Roles</remarks>
    [HttpPatch]
    public Task<DataResult<EmptyRecord>> Patch()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     角色用户列表
    /// </summary>
    /// <param name="roleName">角色名</param>
    /// <param name="userNameSearch">用户名匹配值</param>
    /// <param name="emailSearch">邮箱匹配值</param>
    /// <param name="phoneSearch">电话匹配值</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <returns>GET status</returns>
    /// <remarks>GET api/Roles/{roleName}/Users</remarks>
    [HttpGet("{roleName}/Users")]
    public Task<DataResult<PageResult<UserInfo>>> FetchRoleUsers(
        string roleName,
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
                RoleName = roleName,
                UserNameSearch = userNameSearch,
                EmailSearch = emailSearch,
                PhoneNumberSearch = phoneSearch
            }
        };

        return FetchRoleUsersAsync(request);
    }

    /// <summary>
    ///     角色凭据列表
    /// </summary>
    /// <param name="roleName">角色名</param>
    /// <param name="claimTypeSearch">凭据类型搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <returns></returns>
    /// <remarks>GET api/Roles/{roleName}/Claims</remarks>
    [HttpGet("{roleName}/Claims")]
    public Task<DataResult<PageResult<RoleClaimInfo>>> FetchRoleClaims(
        string roleName,
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
                RoleName = roleName,
                ClaimTypeSearch = claimTypeSearch
            }
        };

        return FetchRoleClaimsAsync(request);
    }

    /// <summary>
    ///     获取角色凭据
    /// </summary>
    /// <param name="roleName">角色名</param>
    /// <param name="claimId">凭据标识</param>
    /// <returns></returns>
    /// <remarks>GET api/Roles/{roleName}/Claims/{claimId}</remarks>
    [HttpGet("{roleName}/Claims/{claimId}")]
    public Task<DataResult<RoleClaimInfo>> GetRoleClaim(string roleName, int claimId)
    {
        var request = new GetRoleClaimRequest
        {
            RoleName = roleName,
            ClaimId = claimId
        };

        return GetRoleClaimAsync(request);
    }

    #endregion

    #region Implementation of IRoleService

    /// <summary>
    ///     查询角色
    /// </summary>
    /// <param name="request">查询角色请求</param>
    /// <returns></returns>
    [NonAction]
    public async Task<DataResult<PageResult<RoleInfo>>> FetchRolesAsync(
        [FromBody] PageRequest<FetchRolesFilter> request)
    {
        var filter = request.Filter;

        var nameSearch = filter.RoleNameSearch ?? string.Empty;

        var result = await IdentityManager.FetchRolesAsync(nameSearch, request.Page, request.Size);

        return DataResult.Success(result);
    }

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

        return DataResult.Fail<EmptyRecord>(
            $"创建失败。{string.Join(",", result.Errors.Select(error => error.Description))}");
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
        var result = await IdentityManager.DeleteRoleAsync(request.RoleName);

        if (result.Succeeded)
            return DataResult.Success(new EmptyRecord());

        return DataResult.Fail<EmptyRecord>(
            $"删除失败。{string.Join(",", result.Errors.Select(error => error.Description))}");
    }


    /// <summary>
    ///     查询角色用户
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [NonAction]
    public async Task<DataResult<PageResult<UserInfo>>> FetchRoleUsersAsync(PageRequest<FetchRoleUsersFilter> request)
    {
        var filter = request.Filter;

        var roleName = filter.RoleName;

        var userNameSearch = filter.UserNameSearch ?? string.Empty;

        var emailSearch = filter.EmailSearch ?? string.Empty;

        var phoneSearch = filter.PhoneNumberSearch ?? string.Empty;

        var result = await IdentityManager.FetchRoleUsersAsync(
            roleName,
            userNameSearch,
            emailSearch,
            phoneSearch,
            request.Page,
            request.Size);

        return DataResult.Success(result);
    }

    /// <summary>
    ///     查询角色凭据
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [NonAction]
    public async Task<DataResult<PageResult<RoleClaimInfo>>> FetchRoleClaimsAsync(
        PageRequest<FetchRoleClaimsFilter> request)
    {
        var filter = request.Filter;

        var roleName = filter.RoleName;

        var claimTypeSearch = filter.ClaimTypeSearch;

        var result = await IdentityManager.FetchRoleClaimsAsync(roleName, claimTypeSearch, request.Page, request.Size);

        return DataResult.Success(result);
    }

    /// <summary>
    ///     获取角色凭据
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [NonAction]
    public async Task<DataResult<RoleClaimInfo>> GetRoleClaimAsync(GetRoleClaimRequest request)
    {
        var result = await IdentityManager
            .GetRoleClaimAsync(request.RoleName, request.ClaimId);

        return result is not null ? DataResult.Success(result) : DataResult.Fail<RoleClaimInfo>("未查询到对应的凭据");
    }

    #endregion
}