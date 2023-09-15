using Artemis.Services.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace Artemis.Services.Identity;

/// <summary>
///     认证服务接口
/// </summary>
public interface IIdentityService : IIdentityService<ArtemisUser>
{
}

/// <summary>
///     认证服务接口
/// </summary>
/// <typeparam name="TUser">用户类型</typeparam>
public interface IIdentityService<TUser> : IIdentityService<TUser, Guid> where TUser : IdentityUser<Guid>
{
}

/// <summary>
///     认证服务接口
/// </summary>
public interface IIdentityService<TUser, TKey> where TUser : IdentityUser<TKey> where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     注册
    /// </summary>
    /// <param name="username">用户名</param>
    /// <param name="password">密码</param>
    /// <param name="email">邮箱</param>
    /// <param name="phone">手机号码</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    Task<(IdentityResult result, TUser user)> SignUp(string username, string password, string? email = null,
        string? phone = null,
        CancellationToken cancellationToken = default);
}