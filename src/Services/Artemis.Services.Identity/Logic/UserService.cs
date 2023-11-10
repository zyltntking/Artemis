using Artemis.Data.Grpc;
using Artemis.Services.Identity.Managers;
using Artemis.Shared.Identity.Services;
using Artemis.Shared.Identity.Transfer;

namespace Artemis.Services.Identity.Logic;

/// <summary>
///     用户服务
/// </summary>
internal class UserService : IUserService
{
    /// <summary>
    ///     泛型API控制器
    /// </summary>
    /// <param name="manager"></param>
    public UserService(IUserManager manager)
    {
        UserManager = manager;
    }

    /// <summary>
    ///     用户管理器
    /// </summary>
    private IUserManager UserManager { get; }

    #region Implementation of IUserService

    /// <summary>
    ///     搜索用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<UsersResponse> FetchUsersAsync(FetchUsersRequest request)
    {
        var filter = request.Filter;

        var result = await UserManager.FetchUserAsync(
            filter.NameSearch,
            filter.EmailSearch,
            filter.PhoneNumberSearch,
            request.Page,
            request.Size);

        return new UsersResponse
        {
            Result = GrpcResponse.SuccessResult(),
            Page = GrpcResponse.PageResult(result),
            Data = result.Data
        };
    }

    /// <summary>
    ///     获取用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns>角色信息<see cref="UserInfo" /></returns>
    public async Task<UserResponse> GetUserAsync(GetUserRequest request)
    {
        var result = await UserManager.GetUserAsync(request.UserId);

        if (result is null)
            return new UserResponse
            {
                Result = GrpcResponse.FailResult("用户不存在"),
                Data = null
            };

        return new UserResponse
        {
            Result = GrpcResponse.SuccessResult(),
            Data = result
        };
    }

    /// <summary>
    ///     创建用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<UserResponse> CreateUserAsync(CreateUserRequest request)
    {
        var (result, user) = await UserManager.CreateUserAsync(request, request.Password);

        if (result.Succeeded)
            return new UserResponse
            {
                Result = GrpcResponse.SuccessResult(),
                Data = user
            };

        return new UserResponse
        {
            Result = GrpcResponse.FailResult($"创建失败。{result.DescribeError}"),
            Data = null
        };
    }

    /// <summary>
    ///     创建用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<GrpcEmptyResponse> CreateUsersAsync(CreateUsersRequest request)
    {
        var packages = request.UserPackages.ToList();

        var result = await UserManager.CreateUsersAsync(packages);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail($"创建失败。{result.DescribeError}");
    }

    /// <summary>
    ///     更新用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<UserResponse> UpdateUserAsync(UpdateUserRequest request)
    {
        var (result, user) = await UserManager.UpdateUserAsync(request.UserId, request.UserPackage);

        if (result.Succeeded)
            return new UserResponse
            {
                Result = GrpcResponse.SuccessResult(),
                Data = user
            };

        return new UserResponse
        {
            Result = GrpcResponse.FailResult($"更新失败。{result.DescribeError}"),
            Data = null
        };
    }

    /// <summary>
    ///     更新用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<GrpcEmptyResponse> UpdateUsersAsync(UpdateUsersRequest request)
    {
        var result = await UserManager.UpdateUsersAsync(request.UserPackages);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail($"更新失败。{result.DescribeError}");
    }

    /// <summary>
    ///     删除用户
    /// </summary>
    /// <param name="request">删除用户请求</param>
    /// <returns></returns>
    public async Task<GrpcEmptyResponse> DeleteUserAsync(DeleteUserRequest request)
    {
        var result = await UserManager.DeleteUserAsync(request.UserId);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail($"删除失败。{result.DescribeError}");
    }

    /// <summary>
    ///     删除用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<GrpcEmptyResponse> DeleteUsersAsync(DeleteUsersRequest request)
    {
        var result = await UserManager.DeleteUsersAsync(request.UserIds);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail($"删除失败。{result.DescribeError}");
    }

    /// <summary>
    ///     查询用户角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<UserRolesResponse> FetchUserRolesAsync(FetchUserRolesRequest request)
    {
        var filter = request.Filter;

        var result = await UserManager.FetchUserRolesAsync(
            filter.UserId,
            filter.RoleNameSearch,
            request.Page,
            request.Size);

        return new UserRolesResponse
        {
            Result = GrpcResponse.SuccessResult(),
            Page = GrpcResponse.PageResult(result),
            Data = result.Data
        };
    }

    /// <summary>
    ///     获取用户角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<UserRoleResponse> GetUserRoleAsync(GetUserRoleRequest request)
    {
        var result = await UserManager.GetUserRoleAsync(request.UserId, request.RoleId);

        if (result is null)
            return new UserRoleResponse
            {
                Result = GrpcResponse.FailResult("用户角色不存在"),
                Data = null
            };

        return new UserRoleResponse
        {
            Result = GrpcResponse.SuccessResult(),
            Data = result
        };
    }

    /// <summary>
    ///     添加用户角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<GrpcEmptyResponse> AddUserRoleAsync(AddUserRoleRequest request)
    {
        var result = await UserManager.AddUserRoleAsync(request.UserId, request.RoleId);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail($"添加失败。{result.DescribeError}");
    }

    /// <summary>
    ///     添加用户角色
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<GrpcEmptyResponse> AddUserRolesAsync(AddUserRolesRequest request)
    {
        var result = await UserManager.AddUserRolesAsync(request.UserId, request.RoleIds);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail($"添加失败。{result.DescribeError}");
    }

    /// <summary>
    ///     删除用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<GrpcEmptyResponse> RemoveUserRoleAsync(RemoveUserRoleRequest request)
    {
        var result = await UserManager.RemoveUserRoleAsync(request.UserId, request.RoleId);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail($"删除失败。{result.DescribeError}");
    }

    /// <summary>
    ///     删除用户
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<GrpcEmptyResponse> RemoveUserRolesAsync(RemoveUserRolesRequest request)
    {
        var result = await UserManager.RemoveUserRolesAsync(request.UserId, request.RoleIds);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail($"删除失败。{result.DescribeError}");
    }

    /// <summary>
    ///     查询用户凭据信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<UserClaimsResponse> FetchUserClaimsAsync(FetchUserClaimsRequest request)
    {
        var filter = request.Filter;

        var result = await UserManager.FetchUserClaimsAsync(
            filter.UserId,
            filter.ClaimTypeSearch,
            request.Page,
            request.Size);

        return new UserClaimsResponse
        {
            Result = GrpcResponse.SuccessResult(),
            Page = GrpcResponse.PageResult(result),
            Data = result.Data
        };
    }

    /// <summary>
    ///     获取用户凭据信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<UserClaimResponse> GetUserClaimAsync(GetUserClaimRequest request)
    {
        var result = await UserManager.GetUserClaimAsync(request.UserId, request.UserClaimId);

        if (result is null)
            return new UserClaimResponse
            {
                Result = GrpcResponse.FailResult("用户凭据信息不存在"),
                Data = null
            };

        return new UserClaimResponse
        {
            Result = GrpcResponse.SuccessResult(),
            Data = result
        };
    }

    /// <summary>
    ///     添加用户凭据信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<GrpcEmptyResponse> AddUserClaimAsync(AddUserClaimRequest request)
    {
        var result = await UserManager.AddUserClaimAsync(request.UserId, request.ClaimPackage);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail($"添加失败。{result.DescribeError}");
    }

    /// <summary>
    ///     添加用户凭据信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<GrpcEmptyResponse> AddUserClaimsAsync(AddUserClaimsRequest request)
    {
        var result = await UserManager.AddUserClaimsAsync(request.UserId, request.ClaimPackages);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail($"添加失败。{result.DescribeError}");
    }

    /// <summary>
    ///     删除用户凭据信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<GrpcEmptyResponse> RemoveUserClaimAsync(RemoveUserClaimRequest request)
    {
        var result = await UserManager.RemoveUserClaimAsync(request.UserId, request.UserClaimId);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail($"删除失败。{result.DescribeError}");
    }

    /// <summary>
    ///     删除用户凭据信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<GrpcEmptyResponse> RemoveUsersClaimAsync(RemoveUserClaimsRequest request)
    {
        var result = await UserManager.RemoveUserClaimsAsync(request.UserId, request.UserClaimIds);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail($"删除失败。{result.DescribeError}");
    }

    /// <summary>
    ///     查询用户登录信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<UserLoginsResponse> FetchUserLoginsAsync(FetchUserLoginsRequest request)
    {
        var filter = request.Filter;

        var result = await UserManager.FetchUserLoginsAsync(
            filter.UserId,
            filter.LoginProviderSearch,
            request.Page,
            request.Size);

        return new UserLoginsResponse
        {
            Result = GrpcResponse.SuccessResult(),
            Page = GrpcResponse.PageResult(result),
            Data = result.Data
        };
    }

    /// <summary>
    ///     获取用户登录信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<UserLoginResponse> GetUserLoginAsync(GetUserLoginRequest request)
    {
        var result = await UserManager.GetUserLoginAsync(request.UserId, request.UserLoginId);

        if (result is null)
            return new UserLoginResponse
            {
                Result = GrpcResponse.FailResult("用户登录信息不存在"),
                Data = null
            };

        return new UserLoginResponse
        {
            Result = GrpcResponse.SuccessResult(),
            Data = result
        };
    }

    /// <summary>
    ///     添加用户登录信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<GrpcEmptyResponse> AddUserLoginAsync(AddUserLoginRequest request)
    {
        var result = await UserManager.AddUserLoginAsync(request.UserId, request.UserLoginPackage);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail($"添加失败。{result.DescribeError}");
    }

    /// <summary>
    ///     替换用户登录信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<GrpcEmptyResponse> ReplaceUserLoginAsync(ReplaceUserLoginRequest request)
    {
        var result = await UserManager.ReplaceUserLoginAsync(
            request.UserId,
            request.UserLoginId,
            request.UserLoginPackage);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail($"替换失败。{result.DescribeError}");
    }

    /// <summary>
    ///     删除用户登录信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<GrpcEmptyResponse> RemoveUserLoginAsync(RemoveUserLoginRequest request)
    {
        var result = await UserManager.RemoveUserLoginAsync(request.UserId, request.UserLoginId);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail($"删除失败。{result.DescribeError}");
    }

    /// <summary>
    ///     删除用户登录信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<GrpcEmptyResponse> RemoveUserLoginsAsync(RemoveUserLoginsRequest request)
    {
        var result = await UserManager.RemoveUserLoginsAsync(request.UserId, request.UserLoginIds);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail($"删除失败。{result.DescribeError}");
    }

    /// <summary>
    ///     查询用户令牌信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<UserTokensResponse> FetchUserTokensAsync(FetchUserTokensRequest request)
    {
        var filter = request.Filter;

        var result = await UserManager.FetchUserTokensAsync(
            filter.UserId,
            filter.LoginProviderSearch,
            filter.NameSearch,
            request.Page,
            request.Size);

        return new UserTokensResponse
        {
            Result = GrpcResponse.SuccessResult(),
            Page = GrpcResponse.PageResult(result),
            Data = result.Data
        };
    }

    /// <summary>
    ///     获取用户令牌信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<UserTokenResponse> GetUserTokenAsync(GetUserTokenRequest request)
    {
        var result = await UserManager.GetUserTokenAsync(request.UserId, request.UserTokenId);

        if (result is null)
            return new UserTokenResponse
            {
                Result = GrpcResponse.FailResult("用户令牌信息不存在"),
                Data = null
            };

        return new UserTokenResponse
        {
            Result = GrpcResponse.SuccessResult(),
            Data = result
        };
    }

    /// <summary>
    ///     添加用户令牌信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<GrpcEmptyResponse> AddUserTokenAsync(AddUserTokenRequest request)
    {
        var result = await UserManager.AddUserTokenAsync(request.UserId, request.UserTokenPackage);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail($"添加失败。{result.DescribeError}");
    }

    /// <summary>
    ///     替换用户令牌信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<GrpcEmptyResponse> ReplaceUserTokenAsync(ReplaceUserTokenRequest request)
    {
        var result = await UserManager.ReplaceUserTokenAsync(
            request.UserId,
            request.UserTokenId,
            request.UserTokenPackage);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail($"替换失败。{result.DescribeError}");
    }

    /// <summary>
    ///     删除用户令牌信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<GrpcEmptyResponse> RemoveUserTokenAsync(RemoveUserTokenRequest request)
    {
        var result = await UserManager.RemoveUserTokenAsync(request.UserId, request.UserTokenId);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail($"删除失败。{result.DescribeError}");
    }

    /// <summary>
    ///     删除用户令牌信息
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<GrpcEmptyResponse> RemoveUserTokensAsync(RemoveUserTokensRequest request)
    {
        var result = await UserManager.RemoveUserTokensAsync(request.UserId, request.UserTokenIds);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail($"删除失败。{result.DescribeError}");
    }

    #endregion
}