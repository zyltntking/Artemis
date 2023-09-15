using Artemis.Extensions.Web.Controller;
using Artemis.Services.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Artemis.App.Identity.Controllers;

/// <summary>
///     测试控制器
/// </summary>
public class TestController : ApiController
{
    /// <summary>
    ///     泛型API控制器
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="manager"></param>
    public TestController(
        ILogger<TestController> logger,
        IIdentityManager manager) : base(logger)
    {
        Manager = manager;
    }

    private IIdentityManager Manager { get; }

    /// <summary>
    ///     Get测试
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<string> TestGet()
    {
         return Manager.Test();
    }
}