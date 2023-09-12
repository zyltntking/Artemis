using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Extensions.Web;
using Artemis.Extensions.Web.Controller;
using Artemis.Services.Identity;
using Artemis.Services.Identity.Data;
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


        ///// <summary>
        ///// 注册
        ///// </summary>
        ///// <param name="request">注册请求</param>
        ///// <returns></returns>
        //[HttpPost, HttpHead]
        //[Description("注册")]
        //public async Task<DataResult<SignInResponse>> SignUp([FromBody] [Required] SignUpRequest request)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //var user = Utility.CreateInstance<ArtemisIdentityUser>();

        //        //await UserStore.SetUserNameAsync(user, request.Username, CancellationToken.None);

        //        //var result = await UserManger.CreateAsync(user, request.Password);

        //        //if (result.Succeeded)
        //        //{
        //        //    await SignInManager.SignInAsync(user, false);


        //        //}

        //        var resultAttach = await IdentityService.SignUp(request.Username, request.Password);

        //        foreach (var error in resultAttach.Result.Errors)
        //        {
        //            ModelState.AddModelError(error.Code, error.Description);
        //        }

        //        var response = new SignInResponse
        //        {
        //            Expire = DateTime.Now.AddDays(30),
        //            Token = resultAttach.Attach.Id.ToString()
        //        };

        //        return DataResult.Success(response);
        //    }

        //    return ModelState.Fail<SignInResponse>();
        //}
    }
}
