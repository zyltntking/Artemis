using Artemis.Data.Store;
using Artemis.Services.Identity.Data;
using Artemis.Services.Identity.Stores;
using Artemis.Shared.Identity.Transfer;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Artemis.Services.Identity.Managers;

/// <summary>
///     账号管理器
/// </summary>
public class AccountManager : Manager<ArtemisUser>, IAccountManager
{
    /// <summary>
    ///     创建新的管理器实例
    /// </summary>
    /// <param name="store">存储访问器依赖</param>
    /// <param name="cache">缓存管理器</param>
    /// <param name="optionsAccessor">配置依赖</param>
    /// <param name="logger">日志依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    public AccountManager(
        IArtemisUserStore store,
        IDistributedCache? cache = null,
        IOptions<ArtemisStoreOptions>? optionsAccessor = null,
        ILogger? logger = null) : base(store, cache, optionsAccessor, logger)
    {
    }

    #region StoreAccess

    /// <summary>
    ///     用户存储访问器
    /// </summary>
    private IArtemisUserStore UserStore => (IArtemisUserStore)Store;

    #endregion

    #region Overrides of Manager<ArtemisUser,Guid>

    /// <summary>
    ///     释放托管的Store
    /// </summary>
    protected override void StoreDispose()
    {
        UserStore.Dispose();
    }

    #endregion

    #region Implementation of IAccountManager

    /// <summary>
    ///     签到
    /// </summary>
    /// <param name="username">用户名</param>
    /// <param name="password">密码</param>
    /// <returns></returns>
    public Task<TokenInfo> SignInAsync(string username, string password)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     报名/注册
    /// </summary>
    /// <param name="username">用户名</param>
    /// <param name="password">密码</param>
    /// <returns>登录后的Token信息</returns>
    public Task<TokenInfo> SignUpAsync(string username, string password)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     签退/登出
    /// </summary>
    /// <returns></returns>
    public Task SignOutAsync()
    {
        throw new NotImplementedException();
    }

    #endregion
}