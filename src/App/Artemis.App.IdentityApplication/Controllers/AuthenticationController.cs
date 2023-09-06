using Artemis.Extensions.Web.Controller;
using Artemis.Extensions.Web.Filter;
using Microsoft.AspNetCore.Mvc;

namespace Artemis.App.IdentityApplication.Controllers
{
    /// <summary>
    /// 认证接口组
    /// </summary>
    public class AuthenticationController : ClaimedGenericApiController
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpPost, HttpHead]
        [ArtemisClaim]
        public string Login()
        {
            return "success";
        }
    }
}
