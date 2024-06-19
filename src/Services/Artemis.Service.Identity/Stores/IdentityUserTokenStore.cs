using Artemis.Data.Core.Exceptions;
using Artemis.Data.Store;
using Artemis.Service.Identity.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
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
public class IdentityUserTokenStore : KeyLessStore<IdentityUserToken>, IIdentityUserTokenStore
{
    /// <summary>
    ///     无键模型基本存储实例构造
    /// </summary>
    /// <param name="context"></param>
    /// <param name="storeOptions"></param>
    /// <param name="cache"></param>
    /// <param name="logger"></param>
    /// <exception cref="StoreParameterNullException"></exception>
    public IdentityUserTokenStore(
        DbContext context,
        IStoreOptions? storeOptions = null,
        IDistributedCache? cache = null,
        ILogger? logger = null) : base(context, storeOptions, cache, logger)
    {
    }

    /// <summary>
    ///     实体键生成委托
    /// </summary>
    protected override Func<IdentityUserToken, string>? EntityKey { get; init; } = userToken =>
        $"{userToken.UserId}:{userToken.LoginProvider}:{userToken.Name}";
}