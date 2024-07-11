using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;
using Artemis.Data.Store;
using Artemis.Service.Identity.Context;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.Identity.Stores;

#region Interface

/// <summary>
///     认证角色凭据存储接口
/// </summary>
public interface IIdentityRoleClaimStore : IStore<IdentityRoleClaim, int>;

#endregion

/// <summary>
///     认证角色凭据存储
/// </summary>
public sealed class IdentityRoleClaimStore : Store<IdentityRoleClaim, int>, IIdentityRoleClaimStore
{
    /// <summary>
    ///     基本存储实例构造
    /// </summary>
    /// <param name="context"></param>
    /// <param name="storeOptions"></param>
    /// <param name="handlerProxy"></param>
    /// <param name="cacheProxy"></param>
    /// <param name="logger"></param>
    /// <exception cref="StoreParameterNullException"></exception>
    public IdentityRoleClaimStore(
        IdentityContext context,
        IStoreOptions? storeOptions = null,
        IHandlerProxy? handlerProxy = null,
        ICacheProxy? cacheProxy = null,
        ILogger? logger = null) : base(context, storeOptions, handlerProxy, cacheProxy, logger)
    {
    }
}