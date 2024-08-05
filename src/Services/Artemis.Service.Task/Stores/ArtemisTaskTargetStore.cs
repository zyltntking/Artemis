using Artemis.Data.Core;
using Artemis.Data.Store;
using Artemis.Service.Task.Context;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.Task.Stores;

#region Interface

/// <summary>
///     任务目标存储接口
/// </summary>
public interface IArtemisTaskTargetStore : IStore<ArtemisTaskUnitTarget>;

#endregion

/// <summary>
///     任务目标存储
/// </summary>
public sealed class ArtemisTaskTargetStore : Store<ArtemisTaskUnitTarget>, IArtemisTaskTargetStore
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
    public ArtemisTaskTargetStore(
        TaskContext context,
        IStoreOptions? storeOptions = null,
        IHandlerProxy? handlerProxy = null,
        ICacheProxy? cacheProxy = null,
        ILogger? logger = null,
        StoreErrorDescriber? describer = null) : base(context, storeOptions, handlerProxy, cacheProxy, logger,
        describer)
    {
    }
}