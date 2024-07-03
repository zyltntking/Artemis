using Artemis.Data.Core;
using Artemis.Data.Store;
using Artemis.Service.Task.Context;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.Task.Stores;

#region Interface

/// <summary>
///     任务存储接口
/// </summary>
public interface IArtemisTaskStore : IStore<ArtemisTask>;

#endregion

/// <summary>
///     任务存储
/// </summary>
public sealed class ArtemisTaskStore : Store<ArtemisTask>, IArtemisTaskStore
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
    public ArtemisTaskStore(
        TaskContext context,
        IStoreOptions? storeOptions = null,
        IHandlerProxy? handlerProxy = null,
        IDistributedCache? cache = null,
        ILogger? logger = null,
        StoreErrorDescriber? describer = null) : base(context, storeOptions, handlerProxy, cache, logger, describer)
    {
    }
}