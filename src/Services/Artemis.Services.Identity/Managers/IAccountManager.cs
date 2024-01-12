using Artemis.Data.Store;
using Artemis.Services.Identity.Data;
using Artemis.Shared.Identity.Transfer;

namespace Artemis.Services.Identity.Managers;

/// <summary>
///     账号管理器接口
/// </summary>
public interface IAccountManager : IKeyWithManager<ArtemisUser>
{
    /// <summary>
    ///     签到/登录
    /// </summary>
    /// <param name="package">认证信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>登录后的Token信息</returns>
    Task<(SignResult result, TokenDocument? token)> SignInAsync(
        SignInPackage package,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     报名/注册
    /// </summary>
    /// <param name="package">用户信息</param>
    /// <param name="password">密码</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>登录后的Token信息</returns>
    Task<(SignResult result, TokenDocument? token)> SignUpAsync(
        UserPackage package,
        string password,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     修改密码
    /// </summary>
    /// <param name="userSign">用户名</param>
    /// <param name="oldPassword">原密码</param>
    /// <param name="newPassword">新密码</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<SignResult> ChangePasswordAsync(
        string userSign,
        string oldPassword,
        string newPassword,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     重置密码
    /// </summary>
    /// <param name="userId">用户标识</param>
    /// <param name="password">新密码</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<SignResult> ResetPasswordAsync(
        Guid userId,
        string password,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     批量修改密码
    /// </summary>
    /// <param name="userIds">用户标识列表</param>
    /// <param name="password">新密码</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<SignResult> ResetPasswordsAsync(
        IEnumerable<Guid> userIds,
        string password,
        CancellationToken cancellationToken = default);
}