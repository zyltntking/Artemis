using Artemis.Data.Store;
using Artemis.Services.Identity.Data;
using Artemis.Services.Identity.Stores;
using Artemis.Shared.Identity.Models;
using Microsoft.Extensions.Logging;

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
        ILogger<IManager<ArtemisUser>> logger) : base(userStore, null, null, logger)
    {
        UserStore = userStore;
        UserClaimStore = userClaimStore;
        UserLoginStore = userLoginStore;
        UserTokenStore = userTokenStore;
        RoleStore = roleStore;
        RoleClaimStore = roleClaimStore;
        UserRoleStore = userRoleStore;
    }

    #region StoreAccess

    /// <summary>
    ///  用户存储访问器
    /// </summary>
    private IArtemisUserStore UserStore { get; }

    /// <summary>
    ///  用户凭据存储访问器
    /// </summary>
    private IArtemisUserClaimStore UserClaimStore { get; }

    /// <summary>
    ///  用户登录存储访问器
    /// </summary>
    private IArtemisUserLoginStore UserLoginStore { get; }

    /// <summary>
    /// 用户令牌存储访问器
    /// </summary>
    private IArtemisUserTokenStore UserTokenStore { get; }

    /// <summary>
    /// 角色存储访问器
    /// </summary>
    private IArtemisRoleStore RoleStore { get; }

    /// <summary>
    /// 角色凭据存储访问器
    /// </summary>
    private IArtemisRoleClaimStore RoleClaimStore { get; }

    /// <summary>
    /// 用户角色映射存储访问器
    /// </summary>
    private IArtemisUserRoleStore UserRoleStore { get; }

    #endregion

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

    #region Implementation of IArtemisManager

    /// <summary>
    /// 测试
    /// </summary>
    public async Task<AttachIdentityResult<Role>> Test()
    {
        var role = new ArtemisRole
        {
            Name = "Test",
            NormalizedName = "TEST"
        };

        var result = await RoleStore.CreateAsync(role);

        throw new NotImplementedException();
    }

    #endregion
}