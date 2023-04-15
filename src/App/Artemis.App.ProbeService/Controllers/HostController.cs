using Artemis.Core.Monitor.Fundamental.Model;
using Artemis.Data.Core;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Artemis.App.ProbeService.Controllers
{
    /// <summary>
    /// 主机控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HostController : ControllerBase
    {
        /// <summary>
        /// 主机信息
        /// </summary>
        /// <returns>主机信息</returns>
        [HttpGet]
        public DataResult<HostInfo> HostInfo()
        {
            return DataResult.Success(new HostInfo());
        }
    }
}
