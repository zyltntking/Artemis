using Artemis.Data.Grpc;
using Artemis.Services.Identity.Managers;
using Artemis.Shared.Identity.Services;
using Artemis.Shared.Identity.Transfer;

namespace Artemis.App.Identity.Services;

/// <summary>
///     角色服务
/// </summary>
public class RoleService : IRoleService
{
    /// <summary>
    ///     角色服务
    /// </summary>
    /// <param name="roleManager">角色管理器依赖</param>
    public RoleService(IRoleManager roleManager)
    {
        RoleManager = roleManager;
    }

    /// <summary>
    ///     角色管理器
    /// </summary>
    private IRoleManager RoleManager { get; }

    #region Implementation of IRoleService

    /// <summary>
    ///     搜索角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<RolesResponse> FetchRolesAsync(FetchRolesRequest request)
    {
        var filter = request.Filter;

        var result = await RoleManager.FetchRolesAsync(
            filter.RoleNameSearch,
            request.Page,
            request.Size);

        return new RolesResponse
        {
            Result = GrpcResponse.SuccessResult(),
            Page = GrpcResponse.PageResult(result),
            Data = result.Data
        };
    }

    /// <summary>
    ///     获取角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns>角色信息<see cref="RoleInfo" /></returns>
    public async Task<RoleResponse> GetRoleAsync(GetRoleRequest request)
    {
        var result = await RoleManager.GetRoleAsync(request.RoleId);

        if (result is null)
            return new RoleResponse
            {
                Result = GrpcResponse.FailResult("角色不存在"),
                Data = null
            };

        return new RoleResponse
        {
            Result = GrpcResponse.SuccessResult(),
            Data = result
        };
    }

    /// <summary>
    ///     创建角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<RoleResponse> CreateRoleAsync(CreateRoleRequest request)
    {
        var (result, role) = await RoleManager.CreateRoleAsync(request);

        if (result.Succeeded)
            return new RoleResponse
            {
                Result = GrpcResponse.SuccessResult(),
                Data = role
            };

        return new RoleResponse
        {
            Result = GrpcResponse.FailResult($"创建失败。{result.DescribeError}"),
            Data = null
        };
    }

    /// <summary>
    ///     创建角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<GrpcEmptyResponse> CreateRolesAsync(CreateRolesRequest request)
    {
        var result = await RoleManager.CreateRolesAsync(request.RolePackages);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail($"创建失败。{result.DescribeError}");
    }

    /// <summary>
    ///     更新角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<RoleResponse> UpdateRoleAsync(UpdateRoleRequest request)
    {
        var (result, role) = await RoleManager.UpdateRoleAsync(request.RoleId, request.RolePackage);

        if (result.Succeeded)
            return new RoleResponse
            {
                Result = GrpcResponse.SuccessResult(),
                Data = role
            };

        return new RoleResponse
        {
            Result = GrpcResponse.FailResult($"更新失败。{result.DescribeError}"),
            Data = null
        };
    }

    /// <summary>
    ///     更新角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<GrpcEmptyResponse> UpdateRolesAsync(UpdateRolesRequest request)
    {
        var result = await RoleManager.UpdateRolesAsync(request.RolePackages);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail($"更新失败。{result.DescribeError}");
    }

    /// <summary>
    ///     更新或创建角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<RoleResponse> UpdateOrCreateRoleAsync(UpdateRoleRequest request)
    {
        var (result, role) = await RoleManager.UpdateOrCreateRoleAsync(request.RoleId, request.RolePackage);

        if (result.Succeeded)
            return new RoleResponse
            {
                Result = GrpcResponse.SuccessResult(),
                Data = role
            };

        return new RoleResponse
        {
            Result = GrpcResponse.FailResult($"更新失败。{result.DescribeError}"),
            Data = null
        };
    }

    /// <summary>
    ///     删除角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<GrpcEmptyResponse> DeleteRoleAsync(DeleteRoleRequest request)
    {
        var result = await RoleManager.DeleteRoleAsync(request.RoleId);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail($"删除失败。{result.DescribeError}");
    }

    /// <summary>
    ///     删除角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<GrpcEmptyResponse> DeleteRolesAsync(DeleteRolesRequest request)
    {
        var result = await RoleManager.DeleteRolesAsync(request.RoleIds);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail($"删除失败。{result.DescribeError}");
    }

    /// <summary>
    ///     查询角色用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<RoleUsersResponse> FetchRoleUsersAsync(FetchRoleUsersRequest request)
    {
        var filter = request.Filter;

        var result = await RoleManager.FetchRoleUsersAsync(
            filter.RoleId,
            filter.UserNameSearch,
            filter.EmailSearch,
            filter.PhoneNumberSearch,
            request.Page,
            request.Size);

        return new RoleUsersResponse
        {
            Result = GrpcResponse.SuccessResult(),
            Page = GrpcResponse.PageResult(result),
            Data = result.Data
        };
    }

    /// <summary>
    ///     获取角色用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<RoleUserResponse> GetRoleUserAsync(GetRoleUserRequest request)
    {
        var result = await RoleManager.GetRoleUserAsync(request.RoleId, request.UserId);

        if (result is null)
            return new RoleUserResponse
            {
                Result = GrpcResponse.FailResult("角色用户不存在"),
                Data = null
            };

        return new RoleUserResponse
        {
            Result = GrpcResponse.SuccessResult(),
            Data = result
        };
    }

    /// <summary>
    ///     添加角色用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<GrpcEmptyResponse> AddRoleUserAsync(AddRoleUserRequest request)
    {
        var result = await RoleManager.AddRoleUserAsync(request.RoleId, request.UserId);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail($"添加失败。{result.DescribeError}");
    }

    /// <summary>
    ///     添加角色用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<GrpcEmptyResponse> AddRoleUsersAsync(AddRoleUsersRequest request)
    {
        var result = await RoleManager.AddRoleUsersAsync(request.RoleId, request.UserIds);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail($"添加失败。{result.DescribeError}");
    }

    /// <summary>
    ///     删除角色用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<GrpcEmptyResponse> RemoveRoleUserAsync(RemoveRoleUserRequest request)
    {
        var result = await RoleManager.RemoveRoleUserAsync(request.RoleId, request.UserId);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail($"删除失败。{result.DescribeError}");
    }

    /// <summary>
    ///     删除角色用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<GrpcEmptyResponse> RemoveRoleUsersAsync(RemoveRoleUsersRequest request)
    {
        var result = await RoleManager.RemoveRoleUsersAsync(request.RoleId, request.UserIds);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail($"删除失败。{result.DescribeError}");
    }

    /// <summary>
    ///     查询角色凭据
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<RoleClaimsResponse> FetchRoleClaimsAsync(FetchRoleClaimsRequest request)
    {
        var filter = request.Filter;

        var result = await RoleManager.FetchRoleClaimsAsync(
            filter.RoleId,
            filter.ClaimTypeSearch,
            request.Page,
            request.Size);

        return new RoleClaimsResponse
        {
            Result = GrpcResponse.SuccessResult(),
            Page = GrpcResponse.PageResult(result),
            Data = result.Data
        };
    }

    /// <summary>
    ///     获取角色凭据
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<RoleClaimResponse> GetRoleClaimAsync(GetRoleClaimRequest request)
    {
        var result = await RoleManager.GetRoleClaimAsync(request.RoleId, request.RoleClaimId);

        if (result is null)
            return new RoleClaimResponse
            {
                Result = GrpcResponse.FailResult("角色凭据不存在"),
                Data = null
            };

        return new RoleClaimResponse
        {
            Result = GrpcResponse.SuccessResult(),
            Data = result
        };
    }

    /// <summary>
    ///     添加角色凭据
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<GrpcEmptyResponse> AddRoleClaimAsync(AddRoleClaimRequest request)
    {
        var result = await RoleManager.AddRoleClaimAsync(request.RoleId, request.ClaimPackage);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail($"添加失败。{result.DescribeError}");
    }

    /// <summary>
    ///     添加角色凭据
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<GrpcEmptyResponse> AddRoleClaimsAsync(AddRoleClaimsRequest request)
    {
        var result = await RoleManager.AddRoleClaimsAsync(request.RoleId, request.ClaimPackages);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail($"添加失败。{result.DescribeError}");
    }

    /// <summary>
    ///     删除角色凭据
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<GrpcEmptyResponse> RemoveRoleClaimAsync(RemoveRoleClaimRequest request)
    {
        var result = await RoleManager.RemoveRoleClaimAsync(request.RoleId, request.RoleClaimId);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail($"删除失败。{result.DescribeError}");
    }

    /// <summary>
    ///     删除角色凭据
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<GrpcEmptyResponse> RemoveRoleClaimsAsync(RemoveRoleClaimsRequest request)
    {
        var result = await RoleManager.RemoveRoleClaimsAsync(request.RoleId, request.RoleClaimIds);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail($"删除失败。{result.DescribeError}");
    }

    #endregion
}