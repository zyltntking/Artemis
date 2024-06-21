using Artemis.Data.Store;
using Artemis.Service.Identity.Context;
using Artemis.Service.Identity.Stores;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.Identity.Managers;

#region interface

/// <summary>
///     认证账号管理接口
/// </summary>
public interface IIdentityAccountManager : IManager<IdentityUser, Guid, Guid>
{
}

#endregion

/// <summary>
///     认证账号管理
/// </summary>
public class IdentityAccountManager : Manager<IdentityUser, Guid, Guid>, IIdentityAccountManager
{
    /// <summary>
    ///     创建新的管理器实例
    /// </summary>
    /// <param name="userStore">存储访问器依赖</param>
    /// <param name="roleClaimStore">角色凭据依赖</param>
    /// <param name="options">配置依赖</param>
    /// <param name="logger">日志依赖</param>
    /// <param name="userClaimStore">用户凭据依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    public IdentityAccountManager(
        IIdentityUserStore userStore,
        IIdentityUserClaimStore userClaimStore,
        IIdentityRoleClaimStore roleClaimStore,
        IManagerOptions? options = null,
        ILogger? logger = null) : base(userStore, options, logger)
    {
        UserStore = userStore;
        UserClaimStore = userClaimStore;
        RoleClaimStore = roleClaimStore;
    }

    #region Dispose

    /// <summary>
    ///     释放托管的Store
    /// </summary>
    protected override void StoreDispose()
    {
        UserStore.Dispose();
        UserClaimStore.Dispose();
        RoleClaimStore.Dispose();
    }

    #endregion

    #region StoreAccess

    /// <summary>
    ///     用户存储访问器
    /// </summary>
    private IIdentityUserStore UserStore { get; }

    /// <summary>
    ///     用户凭据存储访问器
    /// </summary>
    private IIdentityUserClaimStore UserClaimStore { get; }

    /// <summary>
    ///     用户凭据存储访问器
    /// </summary>
    private IIdentityRoleClaimStore RoleClaimStore { get; }

    #endregion
}