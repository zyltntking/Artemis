using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Artemis.App.Logic.IdentityLogic;
using Artemis.App.Logic.IdentityLogic.Data;
using Artemis.Data.Core;
using Artemis.Extensions.Web;
using Artemis.Extensions.Web.Controller;
using Microsoft.AspNetCore.Mvc;

namespace Artemis.App.IdentityApplication.Controllers
{
    /// <summary>
    /// 认证接口组
    /// </summary>
    public class AuthenticationController : GenericApiController
    {
        /// <summary>
        /// 认证控制器
        /// </summary>
        /// <param name="identityService">认证服务逻辑</param>
        /// <param name="logger">日志依赖</param>
        public AuthenticationController(
            IIdentityService<ArtemisIdentityUser> identityService,
            ILogger<AuthenticationController> logger) : base(logger)
        {
            IdentityService = identityService;
        }

        /// <summary>
        /// 用户管理器
        /// </summary>
        private IIdentityService<ArtemisIdentityUser> IdentityService { get; }


        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="request">注册请求</param>
        /// <returns></returns>
        [HttpPost, HttpHead]
        [Description("注册")]
        public async Task<DataResult<SignInResponse>> SignUp([FromBody] [Required] SignUpRequest request)
        {
            if (ModelState.IsValid)
            {
                //var user = Utility.CreateInstance<ArtemisIdentityUser>();

                //await UserStore.SetUserNameAsync(user, request.Username, CancellationToken.None);

                //var result = await UserManger.CreateAsync(user, request.Password);

                //if (result.Succeeded)
                //{
                //    await SignInManager.SignInAsync(user, false);


                //}

                var resultAttach = await IdentityService.SignUp(request.Username, request.Password);

                foreach (var error in resultAttach.Result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }

            return ModelState.Fail<SignInResponse>();
        }
    }

    /// <summary>
    /// 注册请求
    /// </summary>
    public class SignUpRequest
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        public string Username { get; set; } = null!;

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }

    /// <summary>
    /// 登录响应
    /// </summary>
    public class SignInResponse
    {
        /// <summary>
        /// 令牌
        /// </summary>
        public string Token { get; set; } = null!;

        /// <summary>
        /// 释放时间
        /// </summary>
        public DateTime Expire { get; set; } = DateTime.Now;
    }
}
