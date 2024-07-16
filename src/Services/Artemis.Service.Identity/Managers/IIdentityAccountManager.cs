using Artemis.Data.Store;
using Artemis.Extensions.Identity;
using Artemis.Service.Shared.Transfer.Identity;

namespace Artemis.Service.Identity.Managers;

/// <summary>
///     认证账号管理接口
/// </summary>
public interface IIdentityAccountManager : IManager
{
    /// <summary>
    ///     签到/登录
    /// </summary>
    /// <param name="userSign">用户标识</param>
    /// <param name="password">密码</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>登录结果和登录后的Token信息</returns>
    Task<(SignResult result, TokenRecord? token)> SignInAsync(
        string userSign,
        string password,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     报名/注册
    /// </summary>
    /// <param name="userSign">用户标识信息</param>
    /// <param name="password">密码</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>注册结果和登录后的Token信息</returns>
    Task<(SignResult result, TokenRecord? token)> SignUpAsync(
        UserSign userSign,
        string password,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     修改密码
    /// </summary>
    /// <param name="userId">用户标识</param>
    /// <param name="oldPassword">原密码</param>
    /// <param name="newPassword">新密码</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>修改结果</returns>
    Task<SignResult> ChangePasswordAsync(
        Guid userId,
        string oldPassword,
        string newPassword,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     重置密码
    /// </summary>
    /// <param name="userId">用户标识</param>
    /// <param name="password">新密码</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>重置结果</returns>
    Task<SignResult> ResetPasswordAsync(
        Guid userId,
        string password,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     批量修改密码
    /// </summary>
    /// <param name="dictionary">重置密码信息字典</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>批量重置结果</returns>
    Task<SignResult> ResetPasswordsAsync(
        IDictionary<Guid, string> dictionary,
        CancellationToken cancellationToken = default);
}