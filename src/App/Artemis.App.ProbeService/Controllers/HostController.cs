using Artemis.Core.Monitor;
using Artemis.Core.Monitor.Fundamental.Interface;
using Artemis.Data.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Artemis.App.ProbeService.Controllers;

/// <summary>
///     主机控制器
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class HostController : ControllerBase
{
    private readonly HostConfig _hostConfig;

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="hostOptions">主机配置</param>
    public HostController(IOptions<HostConfig> hostOptions)
    {
        _hostConfig = hostOptions.Value;
    }

    /// <summary>
    ///     主机信息
    /// </summary>
    /// <returns>主机信息</returns>
    [HttpPost]
    public DataResult<IHostInfo> HostInfo()
    {
        var probe = new Probe(_hostConfig.HostType, _hostConfig.InstanceType);

        var info = probe.HostInfo;

        return DataResult.Success(info);
    }
}