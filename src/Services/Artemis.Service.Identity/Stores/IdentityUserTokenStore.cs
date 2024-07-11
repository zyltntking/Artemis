using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;
using Artemis.Data.Store;
using Artemis.Service.Identity.Context;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.Identity.Stores;

#region Interface

/// <summary>
///     认证用户令牌存储接口
/// </summary>
public interface IIdentityUserTokenStore : IKeyLessStore<IdentityUserToken>;

#endregion

/// <summary>
///     认证用户令牌存储
/// </summary>
public sealed class IdentityUserTokenStore : KeyLessStore<IdentityUserToken>, IIdentityUserTokenStore
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
    public IdentityUserTokenStore(
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
    protected override Func<IdentityUserToken, string>? EntityKey { get; init; } = userToken =>
        $"{userToken.UserId}:{userToken.LoginProvider}:{userToken.Name}";
}