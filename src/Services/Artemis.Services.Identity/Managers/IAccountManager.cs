using Artemis.Data.Store;
using Artemis.Services.Identity.Data;
using Artemis.Shared.Identity.Transfer;

namespace Artemis.Services.Identity.Managers;

/// <summary>
///     账号管理器接口
/// </summary>
public interface IAccountManager : IManager<ArtemisUser>
{
    /// <summary>
    ///     签到
    /// </summary>
    /// <param name="username">用户名</param>
    /// <param name="password">密码</param>
    /// <returns></returns>
    Task<TokenInfo> SignInAsync(string username, string password);
}