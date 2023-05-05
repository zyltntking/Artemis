﻿using Artemis.Core.Monitor;
using Artemis.Core.Monitor.Fundamental.Components.Interface;
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
    private HostConfig HostConfig { get; }

    private IProbe Probe { get; }

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="hostOptions">主机配置</param>
    public HostController(IOptions<HostConfig> hostOptions)
    {
        HostConfig = hostOptions.Value;
        Probe = new Probe(HostConfig.HostType, HostConfig.InstanceType);

    }

    /// <summary>
    ///     主机信息
    /// </summary>
    /// <returns>主机信息</returns>
    [HttpGet]
    public DataResult<IHostInfo> HostInfo()
    {
        var info = Probe.HostInfo;

        return DataResult.Success(info);
    }

    /// <summary>
    ///   内存状态
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public DataResult<IMemoryStatus> MemoryStatus()
    {
        var status = Probe.MemoryStatus;
        return DataResult.Success(status);
    }
}