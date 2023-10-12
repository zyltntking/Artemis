using Artemis.Data.Store;
using Artemis.Services.Identity.Data;
using Artemis.Services.Identity.Stores;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Artemis.Services.Identity.Managers;

/// <summary>
/// 账号管理器
/// </summary>
public class AccountManager : Manager<ArtemisUser> , IAccountManager
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
}