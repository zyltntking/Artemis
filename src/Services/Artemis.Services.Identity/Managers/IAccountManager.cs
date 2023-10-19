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

    /// <summary>
    /// 修改密码
    /// </summary>
    /// <param name="username">用户名</param>
    /// <param name="oldPassword">原密码</param>
    /// <param name="newPassword">新密码</param>
    /// <returns></returns>
    Task<StoreResult> ChangePasswordAsync(string username, string oldPassword, string newPassword);

    /// <summary>
    /// 重置密码
    /// </summary>
    /// <param name="userId">用户标识</param>
    /// <param name="password">新密码</param>
    /// <returns></returns>
    Task<StoreResult> ReSetPasswordAsync(Guid userId, string password);

    /// <summary>
    /// 批量修改密码
    /// </summary>
    /// <param name="userIds">用户标识列表</param>
    /// <param name="password">新密码</param>
    /// <returns></returns>
    Task<StoreResult> ReSetPasswordAsync(List<Guid> userIds, string password);
}