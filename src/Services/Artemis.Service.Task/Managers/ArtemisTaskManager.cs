using Artemis.Data.Store;
using Artemis.Service.Task.Stores;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.Task.Managers;

/// <summary>
///     任务管理器
/// </summary>
public sealed class ArtemisTaskManager : Manager, IArtemisTaskManager
{
    /// <summary>
    ///     创建新的管理器实例
    /// </summary>
    /// <param name="taskStore">存储访问器依赖</param>
    /// <param name="taskAgentStores"></param>
    /// <param name="options">配置依赖</param>
    /// <param name="logger">日志依赖</param>
    /// <param name="agentStore"></param>
    /// <param name="taskUnitStore"></param>
    /// <param name="taskTargetStore"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public ArtemisTaskManager(
        IArtemisTaskStore taskStore,
        IArtemisAgentStore agentStore,
        IArtemisTaskUnitStore taskUnitStore,
        IArtemisTaskTargetStore taskTargetStore,
        IArtemisTaskAgentStores taskAgentStores,
        IManagerOptions? options = null,
        ILogger? logger = null) : base(options, logger)
    {
        TaskStore = taskStore ?? throw new ArgumentNullException(nameof(taskStore));
        AgentStore = agentStore ?? throw new ArgumentNullException(nameof(agentStore));
        TaskUnitStore = taskUnitStore ?? throw new ArgumentNullException(nameof(taskUnitStore));
        TaskTargetStore = taskTargetStore ?? throw new ArgumentNullException(nameof(taskTargetStore));
        TaskAgentStores = taskAgentStores ?? throw new ArgumentNullException(nameof(taskAgentStores));
    }

    #region Overrides of KeyLessManager<ArtemisTask,Guid>

    /// <summary>
    ///     释放托管的Store
    /// </summary>
    protected override void StoreDispose()
    {
        TaskStore.Dispose();
        AgentStore.Dispose();
        TaskUnitStore.Dispose();
        TaskTargetStore.Dispose();
        TaskAgentStores.Dispose();
    }

    #endregion

    #region StoreAccess

    private IArtemisTaskStore TaskStore { get; }

    private IArtemisAgentStore AgentStore { get; }

    private IArtemisTaskUnitStore TaskUnitStore { get; }

    private IArtemisTaskTargetStore TaskTargetStore { get; }

    private IArtemisTaskAgentStores TaskAgentStores { get; }

    #endregion
}