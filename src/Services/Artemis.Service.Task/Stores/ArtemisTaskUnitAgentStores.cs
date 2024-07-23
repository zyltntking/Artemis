using Artemis.Data.Core;
using Artemis.Data.Store;
using Artemis.Service.Task.Context;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.Task.Stores;

#region Interface

/// <summary>
///     任务代理存储接口
/// </summary>
public interface IArtemisTaskUnitAgentStores : IKeyLessStore<ArtemisTaskUnitAgent>;

#endregion

/// <summary>
///     任务代理存储
/// </summary>
public sealed class ArtemisTaskUnitAgentStores : KeyLessStore<ArtemisTaskUnitAgent>, IArtemisTaskUnitAgentStores
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
    public ArtemisTaskUnitAgentStores(
        TaskContext context,
        IStoreOptions? storeOptions = null,
        IHandlerProxy? handlerProxy = null,
        ICacheProxy? cacheProxy = null,
        ILogger? logger = null,
        StoreErrorDescriber? describer = null) : base(context, storeOptions, handlerProxy, cacheProxy, logger,
        describer)
    {
    }

    /// <summary>
    ///     实体键生成委托
    /// </summary>
    protected override Func<ArtemisTaskUnitAgent, string>? EntityKey { get; init; } =
        taskAgent => $"{taskAgent.TaskUnitId}:{taskAgent.AgentId}";
}