using Artemis.Core.Monitor.Fundamental.Model;
using Artemis.Data.Core;
using Microsoft.AspNetCore.Mvc;

namespace Artemis.App.ProbeService.Controllers
{
    /// <summary>
    /// 主机控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    public class HostController : ControllerBase
    {
        /// <summary>
        /// 主机信息
        /// </summary>
        /// <returns>主机信息</returns>
        [HttpPost]
        //[Consumes("application/json")]
        public DataResult<HostInfo> HostInfo()
        {
            return DataResult.Success(new HostInfo());
        }
    }
}
