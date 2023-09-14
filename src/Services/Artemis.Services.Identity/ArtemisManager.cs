using Artemis.Data.Store;
using Artemis.Services.Identity.Data;
using Artemis.Services.Identity.Stores;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Artemis.Services.Identity;

/// <summary>
/// Artemis用户管理器
/// </summary>
public class ArtemisManager : Manager<ArtemisUser>, IArtemisManager
{
    /// <summary>
    ///     创建新的管理器实例
    /// </summary>
    /// <param name="userStore">用户存储访问器</param>
    /// <param name="userClaimStore">用户凭据存储访问器</param>
    /// <param name="userTokenStore"></param>
    /// <param name="roleStore">角色存储访问器</param>
    /// <param name="roleClaimStore">角色凭据存储访问器</param>
    /// <param name="userRoleStore"></param>
    /// <param name="optionsAccessor">配置依赖</param>
    /// <param name="errors">错误依赖</param>
    /// <param name="logger">日志依赖</param>
    /// <param name="userLoginStore"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public ArtemisManager(
        IArtemisUserStore userStore,
        IArtemisUserClaimStore userClaimStore,
        IArtemisUserLoginStore userLoginStore,
        IArtemisUserTokenStore userTokenStore,
        IArtemisRoleStore roleStore,
        IArtemisRoleClaimStore roleClaimStore,
        IArtemisUserRoleStore userRoleStore,
        IOptions<IStoreOptions>? optionsAccessor = null, 
        IStoreErrorDescriber? errors = null, 
        ILogger<IManager<ArtemisUser>>? logger = null) : base(userStore, optionsAccessor, errors, logger)
    {
        UserStore = userStore;
        UserClaimStore = userClaimStore;
        UserLoginStore = userLoginStore;
        UserTokenStore = userTokenStore;
        RoleStore = roleStore;
        RoleClaimStore = roleClaimStore;
        UserRoleStore = userRoleStore;
    }

    /// <summary>
    ///  用户存储访问器
    /// </summary>
    private IStore<ArtemisUser> UserStore { get; }

    /// <summary>
    ///  用户凭据存储访问器
    /// </summary>
    private IStore<ArtemisUserClaim, int> UserClaimStore { get; }

    /// <summary>
    ///  用户登录存储访问器
    /// </summary>
    private IStore<ArtemisUserLogin, int> UserLoginStore { get; }

    /// <summary>
    /// 用户令牌存储访问器
    /// </summary>
    private IStore<ArtemisUserToken, int> UserTokenStore { get; }

    /// <summary>
    /// 角色存储访问器
    /// </summary>
    private IStore<ArtemisRole> RoleStore { get; }

    /// <summary>
    /// 角色凭据存储访问器
    /// </summary>
    private IStore<ArtemisRoleClaim, int> RoleClaimStore { get; }

    /// <summary>
    /// 用户角色映射存储访问器
    /// </summary>
    private IStore<ArtemisUserRole, int> UserRoleStore { get; }

    #region Overrides of Manager<ArtemisUser,Guid>

    /// <summary>
    ///     释放托管的Store
    /// </summary>
    protected override void StoreDispose()
    {
        UserStore.Dispose();
        UserClaimStore.Dispose();
        UserLoginStore.Dispose();
        UserTokenStore.Dispose();
        RoleStore.Dispose();
        RoleClaimStore.Dispose();
        UserRoleStore.Dispose();
        base.StoreDispose();
    }

    #endregion
}