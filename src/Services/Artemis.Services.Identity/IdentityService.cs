using Artemis.Data.Core;
using Artemis.Services.Identity.Data;
using Artemis.Services.Identity.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Artemis.Services.Identity;

/// <summary>
///     认证服务实现
/// </summary>
public class IdentityService : IdentityService<ArtemisIdentityUser>, IIdentityService
{
    /// <summary>
    ///     认证服务构造
    /// </summary>
    /// <param name="userManager">用户管理器</param>
    /// <param name="userStore">用户存储</param>
    /// <param name="logger">日志</param>
    public IdentityService(
        UserManager<ArtemisIdentityUser> userManager,
        IUserStore<ArtemisIdentityUser> userStore,
        ILogger<IdentityService<ArtemisIdentityUser>> logger) : base(userManager, userStore, logger)
    {
    }
}

/// <summary>
///     认证服务实现
/// </summary>
/// <typeparam name="TUser">用户类型</typeparam>
public class IdentityService<TUser> : IdentityService<TUser, Guid>, IIdentityService<TUser>
    where TUser : IdentityUser<Guid>
{
    /// <summary>
    ///     认证服务构造
    /// </summary>
    /// <param name="userManager">用户管理器</param>
    /// <param name="userStore">用户存储</param>
    /// <param name="logger">日志</param>
    public IdentityService(
        UserManager<TUser> userManager,
        IUserStore<TUser> userStore,
        ILogger<IdentityService<TUser>> logger) : base(userManager, userStore, logger)
    {
    }
}

/// <summary>
///     认证服务实现
/// </summary>
public abstract class IdentityService<TUser, TKey> where TUser : IdentityUser<TKey> where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     认证服务构造
    /// </summary>
    /// <param name="userManager">用户管理器</param>
    /// <param name="userStore">用户存储</param>
    /// <param name="logger">日志</param>
    protected IdentityService(
        UserManager<TUser> userManager,
        IUserStore<TUser> userStore,
        ILogger<IdentityService<TUser, TKey>> logger)
    {
        UserManger = userManager;
        UserStore = userStore;
        Logger = logger;
    }

    /// <summary>
    ///     用户管理器
    /// </summary>
    private UserManager<TUser> UserManger { get; }

    /// <summary>
    ///     用户存储器
    /// </summary>
    private IUserStore<TUser> UserStore { get; }

    /// <summary>
    ///     Email存储器
    /// </summary>
    private IUserEmailStore<TUser> EmailStore
    {
        get
        {
            if (!UserManger.SupportsUserEmail) throw new NotSupportEmailException();
            return (IUserEmailStore<TUser>)UserStore;
        }
    }

    /// <summary>
    ///     PhoneNumber存储器
    /// </summary>
    private IUserPhoneNumberStore<TUser> PhoneNumberStore
    {
        get
        {
            if (!UserManger.SupportsUserPhoneNumber) throw new NotSupportPhoneNumberException();
            return (IUserPhoneNumberStore<TUser>)UserStore;
        }
    }

    /// <summary>
    ///     日志记录器
    /// </summary>
    private ILogger Logger { get; }

    #region Implementation of IIdentityService<TUser>

    /// <summary>
    ///     签入
    /// </summary>
    /// <param name="username">用户名</param>
    /// <param name="password">密码</param>
    /// <param name="email">邮箱</param>
    /// <param name="phone">手机号码</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<AttachResult<TUser>> SignUp(string username, string password, string? email = null,
        string? phone = null, CancellationToken cancellationToken = default)
    {
        var user = Instance.CreateInstance<TUser>();

        await UserStore.SetUserNameAsync(user, username, cancellationToken);

        if (email != null) await EmailStore.SetEmailAsync(user, email, cancellationToken);

        if (phone != null) await PhoneNumberStore.SetPhoneNumberAsync(user, phone, cancellationToken);

        var result = await UserManger.CreateAsync(user, password);

        return result.Attach(user);
    }

    #endregion
}