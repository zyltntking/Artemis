using Artemis.Data.Core;
using Artemis.Data.Store;
using Artemis.Service.Resource.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.Resource.Stores;

#region Interface

/// <summary>
///     组织机构存储接口
/// </summary>
public interface IArtemisOrganizationStore : IStore<ArtemisOrganization>;

#endregion

/// <summary>
///  组织机构存储
/// </summary>
public class ArtemisOrganizationStore : Store<ArtemisOrganization>, IArtemisOrganizationStore
{
    /// <summary>
    ///     基本存储实例构造
    /// </summary>
    /// <param name="context"></param>
    /// <param name="storeOptions"></param>
    /// <param name="handlerProxy"></param>
    /// <param name="cache"></param>
    /// <param name="logger"></param>
    /// <param name="describer"></param>
    public ArtemisOrganizationStore(
        ResourceContext context, 
        IStoreOptions? storeOptions = null, 
        IHandlerProxy? handlerProxy = null, 
        IDistributedCache? cache = null, 
        ILogger? logger = null, 
        StoreErrorDescriber? describer = null) : base(context, storeOptions, handlerProxy, cache, logger, describer)
    {
    }
}