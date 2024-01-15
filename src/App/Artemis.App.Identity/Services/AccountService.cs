using Artemis.Protos.Identity;
using Artemis.Services.Identity.Managers;
using Microsoft.Extensions.Caching.Distributed;

namespace Artemis.App.Identity.Services;

/// <summary>
/// 账户服务
/// </summary>
public class AccountService : Account.AccountBase
{
    /// <summary>
    /// 账户服务
    /// </summary>
    /// <param name="accountManager">账户管理器依赖</param>
    /// <param name="cache">缓存依赖</param>
    /// <param name="logger">日志依赖</param>
    public AccountService(
        IAccountManager accountManager,
        IDistributedCache cache,
        ILogger<AccountService> logger)
    {
        AccountManager = accountManager;
        Cache = cache;
        Logger = logger;
    }

    /// <summary>
    ///     角色管理器
    /// </summary>
    private IAccountManager AccountManager { get; }

    /// <summary>
    ///     分布式缓存依赖
    /// </summary>
    private IDistributedCache Cache { get; }

    /// <summary>
    /// 日志依赖
    /// </summary>
    private ILogger<AccountService> Logger { get; }

}