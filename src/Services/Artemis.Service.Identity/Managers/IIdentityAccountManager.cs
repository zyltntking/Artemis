using Artemis.Data.Shared.Transfer;
using Artemis.Data.Store;
using Artemis.Service.Identity.Context;

namespace Artemis.Service.Identity.Managers;

/// <summary>
///     认证账号管理接口
/// </summary>
public interface IIdentityAccountManager : IManager<IdentityUser, Guid, Guid>
{
    /// <summary>
    ///     签到/登录
    /// </summary>
    /// <param name="userSign">用户签名</param>
    /// <param name="password">密码</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>登录结果和登录后的Token信息</returns>
    Task<(SignResult result, TokenDocument? token)> SignInAsync(
        string userSign,
        string password,
        CancellationToken cancellationToken = default);
}