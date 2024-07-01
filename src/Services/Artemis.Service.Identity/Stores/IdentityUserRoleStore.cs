using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;
using Artemis.Data.Store;
using Artemis.Service.Identity.Context;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.Identity.Stores;

#region Interface

/// <summary>
///     认证用户角色关系存储接口
/// </summary>
public interface IIdentityUserRoleStore : IKeyLessStore<IdentityUserRole>;

#endregion

/// <summary>
///     认证用户角色关系存储
/// </summary>
public sealed class IdentityUserRoleStore : KeyLessStore<IdentityUserRole>, IIdentityUserRoleStore
{
    /// <summary>
    ///     无键模型基本存储实例构造
    /// </summary>
    /// <param name="context"></param>
    /// <param name="storeOptions"></param>
    /// <param name="handlerProxy"></param>
    /// <param name="cache"></param>
    /// <param name="logger"></param>
    /// <exception cref="StoreParameterNullException"></exception>
    public IdentityUserRoleStore(
        IdentityContext context,
        IStoreOptions? storeOptions = null,
        IHandlerProxy? handlerProxy = null,
        IDistributedCache? cache = null,
        ILogger? logger = null) : base(context, storeOptions, handlerProxy, cache, logger)
    {
    }
}