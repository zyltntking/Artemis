using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;
using Artemis.Data.Store;
using Artemis.Service.Identity.Context;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.Identity.Stores;

#region Interface

/// <summary>
///     认证用户登录存储接口
/// </summary>
public interface IIdentityUserLoginStore : IKeyLessStore<IdentityUserLogin>;

#endregion

/// <summary>
///     认证用户登录存储
/// </summary>
public sealed class IdentityUserLoginStore : KeyLessStore<IdentityUserLogin>, IIdentityUserLoginStore
{
    /// <summary>
    ///     无键模型基本存储实例构造
    /// </summary>
    /// <param name="context"></param>
    /// <param name="storeOptions"></param>
    /// <param name="handlerProxy"></param>
    /// <param name="cacheProxy"></param>
    /// <param name="logger"></param>
    /// <exception cref="StoreParameterNullException"></exception>
    public IdentityUserLoginStore(
        IdentityContext context,
        IStoreOptions? storeOptions = null,
        IHandlerProxy? handlerProxy = null,
        ICacheProxy? cacheProxy = null,
        ILogger? logger = null) : base(context, storeOptions, handlerProxy, cacheProxy, logger)
    {
    }

    /// <summary>
    ///     实体键生成委托
    /// </summary>
    protected override Func<IdentityUserLogin, string>? EntityKey { get; init; } =
        userLogin => $"{userLogin.LoginProvider}:{userLogin.ProviderKey}";
}