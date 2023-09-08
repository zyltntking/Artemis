using Artemis.Extensions.Web.Filter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Artemis.Extensions.Web.Controller;

/// <summary>
///     泛型凭据认证Api控制器
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
[ArtemisClaim]
public abstract class ClaimedGenericApiController : GenericApiController
{
    /// <summary>
    /// 泛型API控制器
    /// </summary>
    /// <param name="logger"></param>
    protected ClaimedGenericApiController(ILogger logger) : base(logger)
    {
    }
}

/// <summary>
///     泛型Api控制器
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
public abstract class GenericApiController : ControllerBase
{
    /// <summary>
    /// 泛型API控制器
    /// </summary>
    /// <param name="logger"></param>
    protected GenericApiController(ILogger logger)
    {
        Logger = logger;
    }

    /// <summary>
    /// 日志访问器
    /// </summary>
    protected ILogger Logger { get; }
}