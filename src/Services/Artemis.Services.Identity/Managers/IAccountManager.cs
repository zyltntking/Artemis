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
    ///     签到/登录
    /// </summary>
    /// <param name="username">用户名</param>
    /// <param name="password">密码</param>
    /// <returns>登录后的Token信息</returns>
    Task<TokenInfo> SignInAsync(string username, string password);

    /// <summary>
    ///     报名/注册
    /// </summary>
    /// <param name="username">用户名</param>
    /// <param name="password">密码</param>
    /// <returns>登录后的Token信息</returns>
    Task<TokenInfo> SignUpAsync(string username, string password);

    /// <summary>
    ///     签退/登出
    /// </summary>
    /// <returns></returns>
    Task SignOutAsync();
}