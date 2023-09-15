﻿using Artemis.Data.Core;
using Artemis.Services.Identity.Data;
using Artemis.Services.Identity.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Artemis.Services.Identity;

/// <summary>
///     账户服务实现
/// </summary>
public class AccountService : AccountService<ArtemisUser>, IAccountService
{
    /// <summary>
    ///     账户服务构造
    /// </summary>
    /// <param name="userManager">用户管理器</param>
    /// <param name="userStore">用户存储</param>
    /// <param name="signInManager">签入管理器</param>
    /// <param name="logger">日志</param>
    public AccountService(
        UserManager<ArtemisUser> userManager,
        IUserStore<ArtemisUser> userStore,
        SignInManager<ArtemisUser> signInManager,
        ILogger<AccountService<ArtemisUser>> logger) : base(userManager, userStore, signInManager,logger)
    {
    }
}

/// <summary>
///     账户服务实现
/// </summary>
/// <typeparam name="TUser">用户类型</typeparam>
public abstract class AccountService<TUser> : AccountService<TUser, Guid>, IAccountService<TUser>
    where TUser : IdentityUser<Guid>
{
    /// <summary>
    ///     账户服务构造
    /// </summary>
    /// <param name="userManager">用户管理器</param>
    /// <param name="userStore">用户存储</param>
    /// <param name="signInManager"></param>
    /// <param name="logger">日志</param>
    protected AccountService(
        UserManager<TUser> userManager,
        IUserStore<TUser> userStore,
        SignInManager<TUser> signInManager,
        ILogger logger) : base(userManager, userStore, signInManager, logger)
    {
    }
}

/// <summary>
///     账户服务实现
/// </summary>
public abstract class AccountService<TUser, TKey> : IAccountService<TUser, TKey> 
    where TUser : IdentityUser<TKey> 
    where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     账户服务构造
    /// </summary>
    /// <param name="userManager">用户管理器</param>
    /// <param name="userStore">用户存储</param>
    /// <param name="signInManager"></param>
    /// <param name="logger">日志</param>
    protected AccountService(
        UserManager<TUser> userManager,
        IUserStore<TUser> userStore,
        SignInManager<TUser> signInManager,
        ILogger logger)
    {
        UserManger = userManager;
        UserStore = userStore;
        SignInManager = signInManager;
        Logger = logger;
    }

    /// <summary>
    ///     用户管理器
    /// </summary>
    private UserManager<TUser> UserManger { get; }

    /// <summary>
    /// 签到管理器
    /// </summary>
    private SignInManager<TUser> SignInManager { get; }

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
            if (!UserManger.SupportsUserEmail) 
                throw new NotSupportEmailException();
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
            if (!UserManger.SupportsUserPhoneNumber) 
                throw new NotSupportPhoneNumberException();
            return (IUserPhoneNumberStore<TUser>)UserStore;
        }
    }

    /// <summary>
    ///     日志记录器
    /// </summary>
    private ILogger Logger { get; }

    #region Implementation of IAccountService<TUser>

    /// <summary>
    ///     注册
    /// </summary>
    /// <param name="username">用户名</param>
    /// <param name="password">密码</param>
    /// <param name="email">邮箱</param>
    /// <param name="phone">手机号码</param>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    public async Task<AttachIdentityResult<TUser>> SignUp(string username, string password, string? email = null,
        string? phone = null, CancellationToken cancellationToken = default)
    {
        Logger.LogInformation($"用户注册：{username}，邮箱：{email}，手机号码：{phone}");

        var user = Instance.CreateInstance<TUser>();

        await UserStore.SetUserNameAsync(user, username, cancellationToken);

        if (email != null) 
            await EmailStore.SetEmailAsync(user, email, cancellationToken);

        if (phone != null) 
            await PhoneNumberStore.SetPhoneNumberAsync(user, phone, cancellationToken);

        var result = await UserManger.CreateAsync(user, password);

        return result.Attach(user);
    }

    #endregion
}