using System.Net;
using Artemis.Extensions.Web.Controller;
using Artemis.Extensions.Web.Filter;
using Microsoft.AspNetCore.Mvc;

namespace Artemis.App.IdentityApplication.Controllers
{
    /// <summary>
    /// 认证接口组
    /// </summary>
    [ArtemisClaim]
    public class AuthenticationController : GenericApiController
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpPost, HttpHead]
        public string Login()
        {
            return "success";
        }
    }
}
