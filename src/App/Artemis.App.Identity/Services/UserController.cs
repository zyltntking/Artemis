using Artemis.Extensions.Web.Controller;
using Artemis.Services.Identity.Managers;

namespace Artemis.App.Identity.Services;

/// <summary>
///     用户控制器
/// </summary>
public class UserController : ApiController
{
    /// <summary>
    ///     泛型API控制器
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="logger"></param>
    public UserController(IIdentityManager manager, ILogger logger) : base(logger)
    {
        Manager = manager;
    }

    /// <summary>
    ///     管理器
    /// </summary>
    private IIdentityManager Manager { get; }
}