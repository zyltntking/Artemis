using Artemis.Extensions.Web.Filter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Artemis.Extensions.Web.Controller;

/// <summary>
///     泛型凭据认证Api控制器
/// </summary>
[ArtemisClaim]
[ApiController]
[Produces("application/json")]
[Route("api/[controller]/[action]")]
public abstract class ClaimedApiController : ApiController
{
    /// <summary>
    ///     泛型API控制器
    /// </summary>
    /// <param name="logger"></param>
    protected ClaimedApiController(ILogger logger) : base(logger)
    {
    }
}

/// <summary>
///     泛型Api控制器
/// </summary>
[ApiController]
[Produces("application/json")]
[Route("api/[controller]/[action]")]
public abstract class ApiController : ControllerBase
{
    /// <summary>
    ///     泛型API控制器
    /// </summary>
    /// <param name="logger"></param>
    protected ApiController(ILogger logger)
    {
        Logger = logger;
    }

    /// <summary>
    ///     日志访问器
    /// </summary>
    protected ILogger Logger { get; }
}