using Artemis.Data.Core;
using Artemis.Extensions.Web.Controller;
using Artemis.Shared.Identity.Services;
using Microsoft.AspNetCore.Mvc;

namespace Artemis.App.Identity.Services
{
    /// <summary>
    /// 账户服务
    /// </summary>
    public class AccountService : ApiController, IAccountService
    {
        /// <summary>
        ///     泛型API控制器
        /// </summary>
        /// <param name="logger"></param>
        public AccountService(ILogger<AccountService> logger) : base(logger)
        {
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<DataResult<SignInReply>> SignIn(SignInRequest request)
        {
            return SignInAsync(request);
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<DataResult<SignInReply>> Test()
        {
            var replay = new SignInReply
            {
                Token = "i can fly",
                Expire = DateTime.Today
            };

            return Task.FromResult(DataResult.Success(replay));
        }

        #region Implementation of IAccountService

        /// <summary>
        ///     登录
        /// </summary>
        /// <param name="request">登录请求</param>
        /// <returns>登录响应<see cref="SignInReply" /></returns>
        [NonAction]
        public Task<DataResult<SignInReply>> SignInAsync(SignInRequest request)
        {
            var replay = new SignInReply
            {
                Expire = DateTime.Now,
                Token = "i can fly"
            };

            var result = DataResult.Success(replay);

            return Task.FromResult(result);
        }

        #endregion
    }
}
