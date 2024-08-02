using Artemis.Data.Core;
using Artemis.Data.Store;
using Artemis.Service.Business.VisionScreen.Context;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.Business.VisionScreen.Stores;

#region Interface

/// <summary>
///     验光仪存储接口
/// </summary>
public interface IArtemisOptometerStore : IStore<ArtemisOptometer>;

#endregion

/// <summary>
///     验光仪存储
/// </summary>
public class ArtemisOptometerStore : Store<ArtemisOptometer>, IArtemisOptometerStore
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
    public ArtemisOptometerStore(
        RawDataContext context,
        IStoreOptions? storeOptions = null,
        IHandlerProxy? handlerProxy = null,
        ICacheProxy? cacheProxy = null,
        ILogger? logger = null,
        StoreErrorDescriber? describer = null) : base(context, storeOptions, handlerProxy, cacheProxy, logger,
        describer)
    {
    }
}