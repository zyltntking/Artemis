using Artemis.Data.Core;
using Artemis.Services.Identity.Managers;
using Artemis.Shared.Identity.Services;
using Artemis.Shared.Identity.Transfer;
using Grpc.Core;

namespace Artemis.App.Identity.Services
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public class UserService : IUserService
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
        /// 用户管理器
        /// </summary>
        private IUserManager UserManager { get; }

        #region Implementation of IUserService

        /// <summary>
        ///     搜索用户
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public async Task<DataResult<PageResult<UserInfo>>> FetchUsersAsync(
            PageRequest<FetchUsersFilter> request, 
            ServerCallContext? context = default)
        {
            var filter = request.Filter;

            var result = await UserManager.FetchUserAsync(
                filter.NameSearch,
                filter.EmailSearch,
                filter.PhoneNumberSearch,
                request.Page,
                request.Size,
                context?.CancellationToken ?? default);

            return DataResult.Success(result);
        }

        /// <summary>
        ///     获取用户
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="context">上下文</param>
        /// <returns>角色信息<see cref="UserInfo" /></returns>
        public async Task<DataResult<UserInfo>> GetUserAsync(
            GetUserRequest request, 
            ServerCallContext? context = default)
        {
            var result = await UserManager.GetUserAsync(
                request.UserId,
                context?.CancellationToken ?? default);

            return result is not null
                ? DataResult.Success(result)
                : DataResult.Fail<UserInfo>("未查询到匹配的用户");
        }

        /// <summary>
        ///     创建用户
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public async Task<DataResult<UserInfo>> CreateUserAsync(
            CreateUserRequest request, 
            ServerCallContext? context = default)
        {
            var (result, user) = await UserManager.CreateUserAsync(
                request,
                request.Password,
                context?.CancellationToken ?? default);

            return result.Succeeded
                ? DataResult.Success(user)!
                : DataResult.Fail<UserInfo>($"创建失败。{result.DescribeError}");
        }

        /// <summary>
        ///     创建用户
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public async Task<DataResult<EmptyRecord>> CreateUsersAsync(
            CreateUsersRequest request, 
            ServerCallContext? context = default)
        {
            var packages = request.UserPackages.ToList();

            var result = await UserManager.CreateUsersAsync(
                packages,
                context?.CancellationToken ?? default);

            return result.Succeeded
                ? DataResult.Success(new EmptyRecord())
                : DataResult.Fail<EmptyRecord>($"创建失败。{result.DescribeError}");
        }

        /// <summary>
        ///     更新用户
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public async Task<DataResult<UserInfo>> UpdateUserAsync(
            UpdateUserRequest request, 
            ServerCallContext? context = default)
        {
            var (result, user) = await UserManager.UpdateUserAsync(
                request.UserId,
                request.UserPackage,
                context?.CancellationToken ?? default);

            return result.Succeeded
                ? DataResult.Success(user)!
                : DataResult.Fail<UserInfo>($"更新失败。{result.DescribeError}");
        }

        /// <summary>
        ///     更新用户
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public async Task<DataResult<EmptyRecord>> UpdateUsersAsync(
            UpdateUsersRequest request, 
            ServerCallContext? context = default)
        {
            var result = await UserManager.UpdateUsersAsync(
                request.UserPackages,
                context?.CancellationToken ?? default);

            return result.Succeeded
                ? DataResult.Success(new EmptyRecord())
                : DataResult.Fail<EmptyRecord>($"更新失败。{result.DescribeError}");
        }

        /// <summary>
        ///     删除用户
        /// </summary>
        /// <param name="request">删除用户请求</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public async Task<DataResult<EmptyRecord>> DeleteUserAsync(
            DeleteUserRequest request, 
            ServerCallContext? context = default)
        {
            var result = await UserManager.DeleteUserAsync(
                request.UserId,
                context?.CancellationToken ?? default);

            return result.Succeeded
                ? DataResult.Success(new EmptyRecord())
                : DataResult.Fail<EmptyRecord>($"删除失败。{result.DescribeError}");
        }

        /// <summary>
        ///     删除用户
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public async Task<DataResult<EmptyRecord>> DeleteUsersAsync(
            DeleteUsersRequest request, 
            ServerCallContext? context = default)
        {
            var result = await UserManager.DeleteUsersAsync(
                request.UserIds,
                context?.CancellationToken ?? default);

            return result.Succeeded
                ? DataResult.Success(new EmptyRecord())
                : DataResult.Fail<EmptyRecord>($"删除失败。{result.DescribeError}");
        }

        /// <summary>
        ///     查询用户角色
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public async Task<DataResult<PageResult<RoleInfo>>> FetchUserRolesAsync(
            PageRequest<FetchUserRolesFilter> request, 
            ServerCallContext? context = default)
        {
            var filter = request.Filter;

            var result = await UserManager.FetchUserRolesAsync(
                filter.UserId,
                filter.RoleNameSearch,
                request.Page,
                request.Size,
                context?.CancellationToken ?? default);

            return DataResult.Success(result);
        }

        /// <summary>
        ///     获取用户角色
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public async Task<DataResult<RoleInfo>> GetUserRoleAsync(
            GetUserRoleRequest request, 
            ServerCallContext? context = default)
        {
            var result = await UserManager.GetUserRoleAsync(
                request.UserId,
                request.RoleId,
                context?.CancellationToken ?? default);

            return result is not null
                ? DataResult.Success(result)
                : DataResult.Fail<RoleInfo>("未查询到匹配的角色");
        }

        /// <summary>
        ///     添加用户角色
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public async Task<DataResult<EmptyRecord>> AddUserRoleAsync(
            AddUserRoleRequest request, 
            ServerCallContext? context = default)
        {
            var result = await UserManager.AddUserRoleAsync(
                request.UserId,
                request.RoleId,
                context?.CancellationToken ?? default);

            return result.Succeeded
                ? DataResult.Success(new EmptyRecord())
                : DataResult.Fail<EmptyRecord>($"添加失败。{result.DescribeError}");
        }

        /// <summary>
        ///     添加用户角色
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public async Task<DataResult<EmptyRecord>> AddUserRolesAsync(
            AddUserRolesRequest request, 
            ServerCallContext? context = default)
        {
            var result = await UserManager.AddUserRolesAsync(
                request.UserId,
                request.RoleIds,
                context?.CancellationToken ?? default);

            return result.Succeeded
                ? DataResult.Success(new EmptyRecord())
                : DataResult.Fail<EmptyRecord>($"添加失败。{result.DescribeError}");
        }

        /// <summary>
        ///     删除用户
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public async Task<DataResult<EmptyRecord>> RemoveUserRoleAsync(
            RemoveUserRoleRequest request, 
            ServerCallContext? context = default)
        {
            var result = await UserManager.RemoveUserRoleAsync(
                request.UserId,
                request.RoleId,
                context?.CancellationToken ?? default);

            return result.Succeeded
                ? DataResult.Success(new EmptyRecord())
                : DataResult.Fail<EmptyRecord>($"删除失败。{result.DescribeError}");
        }

        /// <summary>
        ///     删除用户
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public async Task<DataResult<EmptyRecord>> RemoveUserRolesAsync(
            RemoveUserRolesRequest request, 
            ServerCallContext? context = default)
        {
            var result = await UserManager.RemoveUserRolesAsync(
                request.UserId,
                request.RoleIds,
                context?.CancellationToken ?? default);

            return result.Succeeded
                ? DataResult.Success(new EmptyRecord())
                : DataResult.Fail<EmptyRecord>($"删除失败。{result.DescribeError}");
        }

        /// <summary>
        ///     查询用户凭据信息
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public async Task<DataResult<PageResult<UserClaimInfo>>> FetchUserClaimsAsync(
            PageRequest<FetchUserClaimsFilter> request, 
            ServerCallContext? context = default)
        {
            var filter = request.Filter;

            var result = await UserManager.FetchUserClaimsAsync(
                filter.UserId,
                filter.ClaimTypeSearch,
                request.Page,
                request.Size,
                context?.CancellationToken ?? default);

            return DataResult.Success(result);
        }

        /// <summary>
        ///     获取用户凭据信息
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public async Task<DataResult<UserClaimInfo>> GetUserClaimAsync(
            GetUserClaimRequest request, 
            ServerCallContext? context = default)
        {
            var result = await UserManager.GetUserClaimAsync(
                request.UserId,
                request.UserClaimId,
                context?.CancellationToken ?? default);

            return result is not null
                ? DataResult.Success(result)
                : DataResult.Fail<UserClaimInfo>("未查询到对应的凭据");
        }

        /// <summary>
        ///     添加用户凭据信息
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public async Task<DataResult<EmptyRecord>> AddUserClaimAsync(
            AddUserClaimRequest request, 
            ServerCallContext? context = default)
        {
            var result = await UserManager.AddUserClaimAsync(
                request.UserId,
                request.ClaimPackage,
                context?.CancellationToken ?? default);

            return result.Succeeded
                ? DataResult.Success(new EmptyRecord())
                : DataResult.Fail<EmptyRecord>($"添加失败。{result.DescribeError}");
        }

        /// <summary>
        ///     添加用户凭据信息
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public async Task<DataResult<EmptyRecord>> AddUserClaimsAsync(
            AddUserClaimsRequest request, 
            ServerCallContext? context = default)
        {
            var result = await UserManager.AddUserClaimsAsync(
                request.UserId,
                request.ClaimPackages,
                context?.CancellationToken ?? default);

            return result.Succeeded
                ? DataResult.Success(new EmptyRecord())
                : DataResult.Fail<EmptyRecord>($"添加失败。{result.DescribeError}");
        }

        /// <summary>
        ///     删除用户凭据信息
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public async Task<DataResult<EmptyRecord>> RemoveUserClaimAsync(
            RemoveUserClaimRequest request, 
            ServerCallContext? context = default)
        {
            var result = await UserManager.RemoveUserClaimAsync(
                request.UserId,
                request.UserClaimId,
                context?.CancellationToken ?? default);

            return result.Succeeded
                ? DataResult.Success(new EmptyRecord())
                : DataResult.Fail<EmptyRecord>($"删除失败。{result.DescribeError}");
        }

        /// <summary>
        ///     删除用户凭据信息
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public async Task<DataResult<EmptyRecord>> RemoveUsersClaimAsync(
            RemoveUserClaimsRequest request, 
            ServerCallContext? context = default)
        {
            var result = await UserManager.RemoveUserClaimsAsync(
                request.UserId,
                request.UserClaimIds,
                context?.CancellationToken ?? default);

            return result.Succeeded
                ? DataResult.Success(new EmptyRecord())
                : DataResult.Fail<EmptyRecord>($"删除失败。{result.DescribeError}");
        }

        /// <summary>
        ///     查询用户登录信息
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public async Task<DataResult<PageResult<UserLoginInfo>>> FetchUserLoginsAsync(
            PageRequest<FetchUserLoginsFilter> request, 
            ServerCallContext? context = default)
        {
            var filter = request.Filter;

            var result = await UserManager.FetchUserLoginsAsync(
                filter.UserId,
                filter.LoginProviderSearch,
                request.Page,
                request.Size,
                context?.CancellationToken ?? default);

            return DataResult.Success(result);
        }

        /// <summary>
        ///     获取用户登录信息
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public async Task<DataResult<UserLoginInfo>> GetUserLoginAsync(
            GetUserLoginRequest request, 
            ServerCallContext? context = default)
        {
            var result = await UserManager.GetUserLoginAsync(
                request.UserId,
                request.UserLoginId,
                context?.CancellationToken ?? default);

            return result is not null
                ? DataResult.Success(result)
                : DataResult.Fail<UserLoginInfo>("未查询到匹配的用户登录信息");
        }

        /// <summary>
        ///     添加用户登录信息
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public async Task<DataResult<EmptyRecord>> AddUserLoginAsync(
            AddUserLoginRequest request, 
            ServerCallContext? context = default)
        {
            var result = await UserManager.AddUserLoginAsync(
                request.UserId,
                request.UserLoginPackage,
                context?.CancellationToken ?? default);

            return result.Succeeded
                ? DataResult.Success(new EmptyRecord())
                : DataResult.Fail<EmptyRecord>($"添加失败。{result.DescribeError}");
        }

        /// <summary>
        ///     替换用户登录信息
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public async Task<DataResult<EmptyRecord>> ReplaceUserLoginAsync(
            ReplaceUserLoginRequest request, 
            ServerCallContext? context = default)
        {
            var result = await UserManager.ReplaceUserLoginAsync(
                request.UserId,
                request.UserLoginId,
                request.UserLoginPackage,
                context?.CancellationToken ?? default);

            return result.Succeeded
                ? DataResult.Success(new EmptyRecord())
                : DataResult.Fail<EmptyRecord>($"替换失败。{result.DescribeError}");
        }

        /// <summary>
        ///     删除用户登录信息
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public async Task<DataResult<EmptyRecord>> RemoveUserLoginAsync(
            RemoveUserLoginRequest request, 
            ServerCallContext? context = default)
        {
            var result = await UserManager.RemoveUserLoginAsync(
                request.UserId,
                request.UserLoginId,
                context?.CancellationToken ?? default);

            return result.Succeeded
                ? DataResult.Success(new EmptyRecord())
                : DataResult.Fail<EmptyRecord>($"删除失败。{result.DescribeError}");
        }

        /// <summary>
        ///     删除用户登录信息
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public async Task<DataResult<EmptyRecord>> RemoveUserLoginsAsync(
            RemoveUserLoginsRequest request, 
            ServerCallContext? context = default)
        {
            var result = await UserManager.RemoveUserLoginsAsync(
                request.UserId,
                request.UserLoginIds,
                context?.CancellationToken ?? default);

            return result.Succeeded
                ? DataResult.Success(new EmptyRecord())
                : DataResult.Fail<EmptyRecord>($"删除失败。{result.DescribeError}");
        }

        /// <summary>
        ///     查询用户令牌信息
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public async Task<DataResult<PageResult<UserTokenInfo>>> FetchUserTokensAsync(
            PageRequest<FetchUserTokensFilter> request, 
            ServerCallContext? context = default)
        {
            var filter = request.Filter;

            var result = await UserManager.FetchUserTokensAsync(
                filter.UserId,
                filter.LoginProviderSearch,
                filter.NameSearch,
                request.Page,
                request.Size,
                context?.CancellationToken ?? default);

            return DataResult.Success(result);
        }

        /// <summary>
        ///     获取用户令牌信息
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public async Task<DataResult<UserTokenInfo>> GetUserTokenAsync(
            GetUserTokenRequest request, 
            ServerCallContext? context = default)
        {
            var result = await UserManager.GetUserTokenAsync(
                request.UserId,
                request.UserTokenId,
                context?.CancellationToken ?? default);

            return result is not null
                ? DataResult.Success(result)
                : DataResult.Fail<UserTokenInfo>("未查询到匹配的用户令牌信息");
        }

        /// <summary>
        ///     添加用户令牌信息
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public async Task<DataResult<EmptyRecord>> AddUserTokenAsync(
            AddUserTokenRequest request, 
            ServerCallContext? context = default)
        {
            var result = await UserManager.AddUserTokenAsync(
                request.UserId,
                request.UserTokenPackage,
                context?.CancellationToken ?? default);

            return result.Succeeded
                ? DataResult.Success(new EmptyRecord())
                : DataResult.Fail<EmptyRecord>($"添加失败。{result.DescribeError}");
        }

        /// <summary>
        ///     替换用户令牌信息
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public async Task<DataResult<EmptyRecord>> ReplaceUserTokenAsync(
            ReplaceUserTokenRequest request, 
            ServerCallContext? context = default)
        {
            var result = await UserManager.ReplaceUserTokenAsync(
                request.UserId,
                request.UserTokenId,
                request.UserTokenPackage,
                context?.CancellationToken ?? default);

            return result.Succeeded
                ? DataResult.Success(new EmptyRecord())
                : DataResult.Fail<EmptyRecord>($"替换失败。{result.DescribeError}");
        }

        /// <summary>
        ///     删除用户令牌信息
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public async Task<DataResult<EmptyRecord>> RemoveUserTokenAsync(
            RemoveUserTokenRequest request, 
            ServerCallContext? context = default)
        {
            var result = await UserManager.RemoveUserTokenAsync(
                request.UserId,
                request.UserTokenId,
                context?.CancellationToken ?? default);

            return result.Succeeded
                ? DataResult.Success(new EmptyRecord())
                : DataResult.Fail<EmptyRecord>($"删除失败。{result.DescribeError}");
        }

        /// <summary>
        ///     删除用户令牌信息
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public async Task<DataResult<EmptyRecord>> RemoveUserTokensAsync(
            RemoveUserTokensRequest request, 
            ServerCallContext? context = default)
        {
            var result = await UserManager.RemoveUserTokensAsync(
                request.UserId,
                request.UserTokenIds,
                context?.CancellationToken ?? default);

            return result.Succeeded
                ? DataResult.Success(new EmptyRecord())
                : DataResult.Fail<EmptyRecord>($"删除失败。{result.DescribeError}");
        }

        #endregion
    }
}
