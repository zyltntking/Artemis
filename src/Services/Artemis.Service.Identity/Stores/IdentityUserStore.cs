using Artemis.Data.Core;
using Artemis.Data.Store;
using Artemis.Service.Identity.Context;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.Identity.Stores;

#region Interface

/// <summary>
///     认证用户存储接口
/// </summary>
public interface IIdentityUserStore : IStore<IdentityUser, Guid, Guid>;

#endregion

/// <summary>
///     认证用户存储
/// </summary>
public sealed class IdentityUserStore : Store<IdentityUser, Guid, Guid>, IIdentityUserStore
{
    /// <summary>
    ///     基本存储实例构造
    /// </summary>
    /// <param name="context"></param>
    /// <param name="storeOptions"></param>
    /// <param name="handlerProxy"></param>
    /// <param name="cache"></param>
    /// <param name="logger"></param>
    public IdentityUserStore(
        IdentityContext context,
        IStoreOptions? storeOptions = null,
        IHandlerProxy? handlerProxy = null,
        IDistributedCache? cache = null,
        ILogger? logger = null) : base(context, storeOptions, handlerProxy, cache, logger)
    {
    }
}