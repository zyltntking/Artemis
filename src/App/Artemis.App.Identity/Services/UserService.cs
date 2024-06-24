using Artemis.Protos.Identity;
using Artemis.Service.Identity.Managers;

namespace Artemis.App.Identity.Services;

/// <summary>
///     用户服务
/// </summary>
public class UserService : User.UserBase
{
    /// <summary>
    ///     用户管理器
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="logger"></param>
    public UserService(IIdentityUserManager userManager, ILogger<UserService> logger)
    {
        UserManager = userManager;
        UserManager.HandlerRegister = Guid.NewGuid;

        Logger = logger;
    }

    /// <summary>
    ///     用户管理器
    /// </summary>
    private IIdentityUserManager UserManager { get; }

    /// <summary>
    ///     日志
    /// </summary>
    private ILogger<UserService> Logger { get; }
}