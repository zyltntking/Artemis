using Artemis.Data.Core;
using Artemis.Data.Store;
using Artemis.Service.Business.VisionScreen.Context;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.Business.VisionScreen.Stores;

#region Interface

/// <summary>
///     通知消息存储接口
/// </summary>
public interface IArtemisNotificationMessageStore : IStore<ArtemisNotificationMessage>;

#endregion

/// <summary>
///     通知消息存储
/// </summary>
public class ArtemisNotificationMessageStore : Store<ArtemisNotificationMessage>, IArtemisNotificationMessageStore
{
    /// <summary>
    ///     通知消息存储实例构造
    /// </summary>
    /// <param name="context"></param>
    /// <param name="storeOptions"></param>
    /// <param name="handlerProxy"></param>
    /// <param name="cacheProxy"></param>
    /// <param name="logger"></param>
    /// <param name="describer"></param>
    public ArtemisNotificationMessageStore(
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