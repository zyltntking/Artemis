using Artemis.Data.Core;
using Artemis.Extensions.Web.Controller;
using Artemis.Services.Identity.Managers;
using Artemis.Shared.Identity.Services;
using Artemis.Shared.Identity.Transfer;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

namespace Artemis.App.Identity.Services
{
    /// <summary>
    /// 用户服务
    /// </summary>
    [Route("api/Users")]
    public class UserService : ApiController, IUserService
    {
        /// <summary>
        ///     泛型API控制器
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="logger"></param>
        public UserService(
            IUserManager manager,
            ILogger<UserService> logger) : base(logger)
        {
            UserManager = manager;
        }

        /// <summary>
        ///     认证管理器
        /// </summary>
        private IUserManager UserManager { get; }

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="nameSearch">用户名搜索值</param>
        /// <param name="emailSearch">用户邮箱搜索值</param>
        /// <param name="phoneNumberSearch">用户电话号码搜索值</param>
        /// <param name="page">页码</param>
        /// <param name="size">条目数</param>
        /// <param name="cancellationToken">操作取消信号</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet]
        public Task<DataResult<PageResult<UserInfo>>> Fetch(
            string? nameSearch, 
            string? emailSearch, 
            string phoneNumberSearch, 
            int page = 1, 
            int size = 20,
            CancellationToken cancellationToken = default)
        {
            var request = new PageRequest<FetchUsersFilter>
            {
                Filter = new FetchUsersFilter
                {
                    NameSearch = nameSearch,
                    EmailSearch = emailSearch,
                    PhoneNumberSearch = phoneNumberSearch
                },
                Page = page,
                Size = size
            };

            return FetchUsersAsync(request, cancellationToken: cancellationToken);
        }

        #region Implementation of IUserService

        /// <summary>
        /// 搜索用户
        /// </summary>
        /// <param name="request">查询用户请求</param>
        /// <param name="context">服务请求上下文</param>
        /// <param name="cancellationToken">操作取消信号</param>
        /// <returns></returns>
        [NonAction]
        public async Task<DataResult<PageResult<UserInfo>>> FetchUsersAsync(
            PageRequest<FetchUsersFilter> request, 
            ServerCallContext? context = default,
            CancellationToken cancellationToken = default)
        {
            if (context is not null)
            {
                cancellationToken = context.CancellationToken;
            }

            var filter = request.Filter;

            var result = await UserManager.Fetch(
                filter.NameSearch,
                filter.EmailSearch,
                filter.PhoneNumberSearch,
                request.Page,
                request.Size,
                cancellationToken);

            return DataResult.Success(result);
        }

        #endregion
    }
}
