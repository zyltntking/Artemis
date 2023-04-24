using Artemis.Core.Monitor.Fundamental.Model;
using Artemis.Core.Monitor.Fundamental.Types;
using Artemis.Data.Core;
using Microsoft.AspNetCore.Mvc;

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
        [HttpPost]
        public DataResult<HostInfo> HostInfo()
        {
            var hostInfo = new HostInfo
            {
                HostName = Environment.MachineName,
                HostType = HostType.Service,
            };

            return DataResult.Success(hostInfo);
        }
    }
}
