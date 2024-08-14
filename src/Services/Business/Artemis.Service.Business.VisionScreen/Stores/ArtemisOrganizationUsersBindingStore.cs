using Artemis.Data.Core;
using Artemis.Data.Store;
using Artemis.Service.Business.VisionScreen.Context;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.Business.VisionScreen.Stores;

#region Interface

/// <summary>
///     机构用户关系绑定存储接口
/// </summary>
public interface IArtemisOrganizationUsersBindingStore : IStore<ArtemisOrganizationUsersBinding>;

#endregion

/// <summary>
///    机构用户关系绑定存储
/// </summary>
public class ArtemisOrganizationUsersBindingStore : Store<ArtemisOrganizationUsersBinding>, IArtemisOrganizationUsersBindingStore
{
    /// <summary>
    ///     基本存储实例构造
    /// </summary>
    /// <param name="context"></param>
    /// <param name="storeOptions"></param>
    /// <param name="handlerProxy"></param>
    /// <param name="cacheProxy"></param>
    /// <param name="logger"></param>
    /// <param name="describer"></param>
    public ArtemisOrganizationUsersBindingStore(
        BusinessContext context,
        IStoreOptions? storeOptions = null,
        IHandlerProxy? handlerProxy = null,
        ICacheProxy? cacheProxy = null,
        ILogger? logger = null,
        StoreErrorDescriber? describer = null) : base(context, storeOptions, handlerProxy, cacheProxy, logger,
        describer)
    {
    }
}