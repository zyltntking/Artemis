using Artemis.Data.Core;
using Artemis.Extensions.Web.Controller;
using Artemis.Shared.Identity.Services;
using Microsoft.AspNetCore.Mvc;

namespace Artemis.App.Identity.Services
{
    /// <summary>
    /// 账户服务
    /// </summary>
    public class AccountService : IAccountService
    {

        #region Implementation of IAccountService

        /// <summary>
        ///     登录
        /// </summary>
        /// <param name="request">登录请求</param>
        /// <returns>登录响应<see cref="SignInReply" /></returns>
        public Task<DataResult<SignInReply>> SignIn(SignInRequest request)
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
