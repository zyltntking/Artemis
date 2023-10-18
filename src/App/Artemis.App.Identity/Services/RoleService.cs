using Artemis.Data.Core;
using Artemis.Services.Identity.Managers;
using Artemis.Shared.Identity.Services;
using Artemis.Shared.Identity.Transfer;
using Grpc.Core;

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
    ///     认证管理器
    /// </summary>
    private IRoleManager RoleManager { get; }


    #region Implementation of IRoleService

    /// <summary>
    ///     搜索角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
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
    /// <returns>角色信息<see cref="RoleInfo" /></returns>
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
    /// <returns></returns>
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
    ///     创建角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    public async Task<DataResult<EmptyRecord>> CreateRolesAsync(
        CreateRolesRequest request,
        ServerCallContext? context = default)
    {
        var result = await RoleManager.CreateRolesAsync(
            request.RolePackages,
            context?.CancellationToken ?? default);

        return result.Succeeded
            ? DataResult.Success(new EmptyRecord())
            : DataResult.Fail<EmptyRecord>($"创建失败。{result.DescribeError}");
    }

    /// <summary>
    ///     更新角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    public async Task<DataResult<RoleInfo>> UpdateRoleAsync(
        UpdateRoleRequest request,
        ServerCallContext? context = default)
    {
        var (result, role) = await RoleManager.UpdateRoleAsync(
            request.RoleId,
            request.RolePackage,
            context?.CancellationToken ?? default);

        return result.Succeeded
            ? DataResult.Success(role)!
            : DataResult.Fail<RoleInfo>($"更新失败。{result.DescribeError}");
    }

    /// <summary>
    ///     更新角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    public async Task<DataResult<EmptyRecord>> UpdateRolesAsync(
        UpdateRolesRequest request,
        ServerCallContext? context = default)
    {
        var result = await RoleManager.UpdateRolesAsync(
            request.RolePackages,
            context?.CancellationToken ?? default);

        return result.Succeeded
            ? DataResult.Success(new EmptyRecord())
            : DataResult.Fail<EmptyRecord>($"更新失败。{result.DescribeError}");
    }

    /// <summary>
    /// 更新或创建角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    public async Task<DataResult<RoleInfo>> UpdateOrCreateRoleAsync(
        UpdateRoleRequest request, 
        ServerCallContext? context = default)
    {
        var (result, role) = await RoleManager.UpdateOrCreateRoleAsync(
            request.RoleId,
            request.RolePackage,
            context?.CancellationToken ?? default);

        return result.Succeeded
            ? DataResult.Success(role)!
            : DataResult.Fail<RoleInfo>($"创建过更新失败。{result.DescribeError}");
    }

    /// <summary>
    ///     删除角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
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
    ///     删除角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    public async Task<DataResult<EmptyRecord>> DeleteRolesAsync(
        DeleteRolesRequest request,
        ServerCallContext? context = default)
    {
        var result = await RoleManager.DeleteRolesAsync(
            request.RoleIds,
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
    ///     获取角色用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    public async Task<DataResult<UserInfo>> GetRoleUserAsync(
        GetRoleUserRequest request,
        ServerCallContext? context = default)
    {
        var result = await RoleManager.GetRoleUserAsync(
            request.RoleId,
            request.UserId,
            context?.CancellationToken ?? default);

        return result is not null
            ? DataResult.Success(result)
            : DataResult.Fail<UserInfo>("未查询到匹配的用户");
    }

    /// <summary>
    ///     添加角色用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    public async Task<DataResult<EmptyRecord>> AddRoleUserAsync(
        AddRoleUserRequest request,
        ServerCallContext? context = default)
    {
        var result = await RoleManager.AddRoleUserAsync(
            request.RoleId,
            request.UserId,
            context?.CancellationToken ?? default);

        return result.Succeeded
            ? DataResult.Success(new EmptyRecord())
            : DataResult.Fail<EmptyRecord>($"添加失败。{result.DescribeError}");
    }

    /// <summary>
    ///     添加角色用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    public async Task<DataResult<EmptyRecord>> AddRoleUsersAsync(
        AddRoleUsersRequest request,
        ServerCallContext? context = default)
    {
        var result = await RoleManager.AddRoleUsersAsync(
            request.RoleId,
            request.UserIds,
            context?.CancellationToken ?? default);

        return result.Succeeded
            ? DataResult.Success(new EmptyRecord())
            : DataResult.Fail<EmptyRecord>($"添加失败。{result.DescribeError}");
    }

    /// <summary>
    ///     删除角色用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    public async Task<DataResult<EmptyRecord>> RemoveRoleUserAsync(
        RemoveRoleUserRequest request,
        ServerCallContext? context = default)
    {
        var result = await RoleManager.RemoveRoleUserAsync(
            request.RoleId,
            request.UserId,
            context?.CancellationToken ?? default);

        return result.Succeeded
            ? DataResult.Success(new EmptyRecord())
            : DataResult.Fail<EmptyRecord>($"删除失败。{result.DescribeError}");
    }

    /// <summary>
    ///     删除角色用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    public async Task<DataResult<EmptyRecord>> RemoveRoleUsersAsync(
        RemoveRoleUsersRequest request,
        ServerCallContext? context = default)
    {
        var result = await RoleManager.RemoveRoleUsersAsync(
            request.RoleId,
            request.UserIds,
            context?.CancellationToken ?? default);

        return result.Succeeded
            ? DataResult.Success(new EmptyRecord())
            : DataResult.Fail<EmptyRecord>($"删除失败。{result.DescribeError}");
    }

    /// <summary>
    ///     查询角色凭据
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
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
    public async Task<DataResult<RoleClaimInfo>> GetRoleClaimAsync(
        GetRoleClaimRequest request,
        ServerCallContext? context = default)
    {
        var result = await RoleManager.GetRoleClaimAsync(
            request.RoleId,
            request.RoleClaimId,
            context?.CancellationToken ?? default);

        return result is not null
            ? DataResult.Success(result)
            : DataResult.Fail<RoleClaimInfo>("未查询到对应的凭据");
    }

    /// <summary>
    ///     添加角色凭据
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    public async Task<DataResult<EmptyRecord>> AddRoleClaimAsync(
        AddRoleClaimRequest request,
        ServerCallContext? context = default)
    {
        var result = await RoleManager.AddRoleClaimAsync(
            request.RoleId,
            request.ClaimPackage,
            context?.CancellationToken ?? default);

        return result.Succeeded
            ? DataResult.Success(new EmptyRecord())
            : DataResult.Fail<EmptyRecord>($"添加失败。{result.DescribeError}");
    }

    /// <summary>
    ///     添加角色凭据
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    public async Task<DataResult<EmptyRecord>> AddRoleClaimsAsync(
        AddRoleClaimsRequest request,
        ServerCallContext? context = default)
    {
        var result = await RoleManager.AddRoleClaimsAsync(
            request.RoleId,
            request.ClaimPackages,
            context?.CancellationToken ?? default);

        return result.Succeeded
            ? DataResult.Success(new EmptyRecord())
            : DataResult.Fail<EmptyRecord>($"添加失败。{result.DescribeError}");
    }

    /// <summary>
    ///     删除角色凭据
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    public async Task<DataResult<EmptyRecord>> RemoveRoleClaimAsync(
        RemoveRoleClaimRequest request,
        ServerCallContext? context = default)
    {
        var result = await RoleManager.RemoveRoleClaimAsync(
            request.RoleId,
            request.RoleClaimId,
            context?.CancellationToken ?? default);

        return result.Succeeded
            ? DataResult.Success(new EmptyRecord())
            : DataResult.Fail<EmptyRecord>($"删除失败。{result.DescribeError}");
    }

    /// <summary>
    ///     删除角色凭据
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    public async Task<DataResult<EmptyRecord>> RemoveRoleClaimsAsync(
        RemoveRoleClaimsRequest request,
        ServerCallContext? context = default)
    {
        var result = await RoleManager.RemoveRoleClaimsAsync(
            request.RoleId,
            request.RoleClaimIds,
            context?.CancellationToken ?? default);

        return result.Succeeded
            ? DataResult.Success(new EmptyRecord())
            : DataResult.Fail<EmptyRecord>($"删除失败。{result.DescribeError}");
    }

    #endregion
}