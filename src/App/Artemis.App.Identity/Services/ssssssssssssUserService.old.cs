//using System.ComponentModel.DataAnnotations;
//using Artemis.Data.Core;
//using Artemis.Extensions.Web.Controller;
//using Artemis.Services.Identity.Managers;
//using Artemis.Shared.Identity.Services;
//using Artemis.Shared.Identity.Transfer;
//using Grpc.Core;
//using Microsoft.AspNetCore.Mvc;

//namespace Artemis.App.Identity.Services;

///// <summary>
/////     用户服务
///// </summary>
//[Route("api/Users")]
//public class UserService : ApiController, IUserService
//{
//    /// <summary>
//    ///     泛型API控制器
//    /// </summary>
//    /// <param name="manager"></param>
//    /// <param name="logger"></param>
//    public UserService(
//        IUserManager manager,
//        ILogger<UserService> logger) : base(logger)
//    {
//        UserManager = manager;
//    }

//    /// <summary>
//    ///     认证管理器
//    /// </summary>
//    private IUserManager UserManager { get; }

//    /// <summary>
//    ///     查询用户
//    /// </summary>
//    /// <param name="nameSearch">用户名搜索值</param>
//    /// <param name="emailSearch">用户邮箱搜索值</param>
//    /// <param name="phoneNumberSearch">用户电话号码搜索值</param>
//    /// <param name="page">页码</param>
//    /// <param name="size">条目数</param>
//    /// <returns>Roles PagedResult</returns>
//    /// <remark>GET api/Roles</remark>
//    [HttpGet]
//    public Task<DataResult<PageResult<UserInfo>>> Fetch(
//        string? nameSearch,
//        string? emailSearch,
//        string phoneNumberSearch,
//        int page = 1,
//        int size = 20)
//    {
//        var request = new PageRequest<FetchUsersFilter>
//        {
//            Filter = new FetchUsersFilter
//            {
//                NameSearch = nameSearch,
//                EmailSearch = emailSearch,
//                PhoneNumberSearch = phoneNumberSearch
//            },
//            Page = page,
//            Size = size
//        };

//        return FetchUsersAsync(request);
//    }

//    /// <summary>
//    ///     获取用户
//    /// </summary>
//    /// <param name="userId">用户标识</param>
//    /// <returns>User Result</returns>
//    /// <remark>GET api/Users/{roleId}</remark>
//    [HttpGet("{userId}")]
//    public Task<DataResult<UserInfo>> GetRole(Guid userId)
//    {
//        var request = new GetUserRequest
//        {
//            UserId = userId
//        };

//        return GetUserAsync(request);
//    }

//    /// <summary>
//    ///     创建用户
//    /// </summary>
//    /// <param name="request">创建用户请求</param>
//    /// <returns>Create Status</returns>
//    /// <remark>POST api/Users</remark>
//    [HttpPost]
//    [ProducesResponseType(StatusCodes.Status201Created)]
//    public Task<DataResult<UserInfo>> PostRole([FromBody] [Required] CreateUserRequest request)
//    {
//        return CreateUserAsync(request);
//    }

//    #region Implementation of IUserService

//    /// <summary>
//    ///     搜索用户
//    /// </summary>
//    /// <param name="request">请求</param>
//    /// <param name="context">上下文</param>
//    /// <returns></returns>
//    [NonAction]
//    public async Task<DataResult<PageResult<UserInfo>>> FetchUsersAsync(
//        PageRequest<FetchUsersFilter> request,
//        ServerCallContext? context = default)
//    {
//        var filter = request.Filter;

//        var result = await UserManager.FetchUserAsync(
//            filter.NameSearch,
//            filter.EmailSearch,
//            filter.PhoneNumberSearch,
//            request.Page,
//            request.Size,
//            context?.CancellationToken ?? default);

//        return DataResult.Success(result);
//    }

//    /// <summary>
//    ///     获取用户
//    /// </summary>
//    /// <param name="request">请求</param>
//    /// <param name="context">上下文</param>
//    /// <returns>角色信息<see cref="UserInfo" /></returns>
//    [NonAction]
//    public async Task<DataResult<UserInfo>> GetUserAsync(
//        GetUserRequest request,
//        ServerCallContext? context = default)
//    {
//        var result = await UserManager.GetUserAsync(
//            request.UserId,
//            context?.CancellationToken ?? default);

//        return result is not null
//            ? DataResult.Success(result)
//            : DataResult.Fail<UserInfo>("未查询到匹配的角色");
//    }

//    /// <summary>
//    ///     创建用户
//    /// </summary>
//    /// <param name="request">请求</param>
//    /// <param name="context">上下文</param>
//    /// <returns></returns>
//    /// <exception cref="NotImplementedException"></exception>
//    [NonAction]
//    public async Task<DataResult<UserInfo>> CreateUserAsync(
//        CreateUserRequest request,
//        ServerCallContext? context = default)
//    {
//        var (result, user) = await UserManager.CreateUserAsync(
//            request,
//            request.Password,
//            context?.CancellationToken ?? default);

//        return result.Succeeded
//            ? DataResult.Success(user)!
//            : DataResult.Fail<UserInfo>($"创建失败。{result.DescribeError}");
//    }

//    /// <summary>
//    ///     创建或更新用户
//    /// </summary>
//    /// <param name="request">请求</param>
//    /// <param name="context">上下文</param>
//    /// <returns></returns>
//    [NonAction]
//    public async Task<DataResult<UserInfo>> CreateOrUpdateUserAsync(UpdateUserRequest request,
//        ServerCallContext? context = default)
//    {
//        var (result, user) = await UserManager.CreateOrUpdateUserAsync(
//            request.UserId,
//            request.UserPack,
//            request.Password,
//            context?.CancellationToken ?? default);

//        return result.Succeeded
//            ? DataResult.Success(user)!
//            : DataResult.Fail<UserInfo>($"创建或更新失败。{result.DescribeError}");
//    }

//    /// <summary>
//    ///     删除用户
//    /// </summary>
//    /// <param name="request">删除用户请求</param>
//    /// <param name="context">上下文</param>
//    /// <returns></returns>
//    [NonAction]
//    public async Task<DataResult<EmptyRecord>> DeleteUserAsync(DeleteUserRequest request,
//        ServerCallContext? context = default)
//    {
//        var result = await UserManager.DeleteUserAsync(
//            request.UserId,
//            context?.CancellationToken ?? default);

//        return result.Succeeded
//            ? DataResult.Success(new EmptyRecord())
//            : DataResult.Fail<EmptyRecord>($"删除失败。{result.DescribeError}");
//    }

//    #endregion
//}

