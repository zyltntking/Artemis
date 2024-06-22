using Artemis.Data.Core;
using Artemis.Data.Shared.Transfer.Identity;
using Artemis.Data.Store;
using Artemis.Data.Store.Extensions;
using Artemis.Service.Identity.Context;
using Artemis.Service.Identity.Stores;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.Identity.Managers;

/// <summary>
///     认证用户管理
/// </summary>
public class IdentityUserManager : Manager<IdentityUser, Guid, Guid>, IIdentityUserManager
{
    /// <summary>
    ///     创建新的管理器实例
    /// </summary>
    /// <param name="userStore">存储访问器依赖</param>
    /// <param name="userTokenStore"></param>
    /// <param name="options">配置依赖</param>
    /// <param name="logger">日志依赖</param>
    /// <param name="roleStore"></param>
    /// <param name="userClaimStore"></param>
    /// <param name="userLoginStore"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public IdentityUserManager(
        IIdentityUserStore userStore,
        IIdentityRoleStore roleStore,
        IIdentityUserClaimStore userClaimStore,
        IIdentityUserLoginStore userLoginStore,
        IIdentityUserTokenStore userTokenStore,
        IManagerOptions? options = null,
        ILogger? logger = null) : base(userStore, options, logger)
    {
        UserStore = userStore;
        UserStore.HandlerRegister = HandlerRegister;
        RoleStore = roleStore;
        UserClaimStore = userClaimStore;
        UserLoginStore = userLoginStore;
        UserTokenStore = userTokenStore;
    }

    #region Dispose

    /// <summary>
    ///     释放托管的Store
    /// </summary>
    protected override void StoreDispose()
    {
        UserStore.Dispose();
        RoleStore.Dispose();
        UserClaimStore.Dispose();
        UserLoginStore.Dispose();
        UserTokenStore.Dispose();
    }

    #endregion

    #region StoreAccess

    /// <summary>
    ///     用户存储访问器
    /// </summary>
    private IIdentityUserStore UserStore { get; }

    /// <summary>
    ///     角色存储访问器
    /// </summary>
    private IIdentityRoleStore RoleStore { get; }

    /// <summary>
    ///     用户凭据存储访问器
    /// </summary>
    private IIdentityUserClaimStore UserClaimStore { get; }

    /// <summary>
    ///     用户登录存储访问器
    /// </summary>
    private IIdentityUserLoginStore UserLoginStore { get; }

    /// <summary>
    ///     用户令牌存储访问器
    /// </summary>
    private IIdentityUserTokenStore UserTokenStore { get; }

    #endregion
}