using System.ComponentModel;
using Artemis.Extensions.Identity;
using Artemis.Service.Shared.Task.Transfer;
using Artemis.Service.Task.Managers;
using Artemis.Service.Task.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Artemis.Service.Task.Services;

/// <summary>
///     任务服务
/// </summary>
public class TaskServiceImplement : TaskService.TaskServiceBase
{
    /// <summary>
    ///     任务服务
    /// </summary>
    /// <param name="taskManager">任务管理器</param>
    /// <param name="cache"></param>
    /// <param name="options"></param>
    /// <param name="logger">日志记录器</param>
    public TaskServiceImplement(
        IArtemisTaskManager taskManager,
        IDistributedCache cache,
        IOptions<ArtemisIdentityOptions> options,
        ILogger<TaskServiceImplement> logger)
    {
        TaskManager = taskManager;
        Cache = cache;
        Options = options.Value;
        Logger = logger;
    }

    /// <summary>
    ///     任务管理器
    /// </summary>
    private IArtemisTaskManager TaskManager { get; }

    /// <summary>
    ///     分布式缓存依赖
    /// </summary>
    private IDistributedCache Cache { get; }

    /// <summary>
    ///     授权配置项
    /// </summary>
    private ArtemisIdentityOptions Options { get; }

    /// <summary>
    ///     日志依赖
    /// </summary>
    private ILogger<TaskServiceImplement> Logger { get; }

    #region Overrides of TaskServiceBase

    /// <summary>
    ///     查询任务信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查询任务信息")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<SearchTaskInfoResponse> SearchTaskInfo(
        SearchTaskInfoRequest request,
        ServerCallContext context)
    {
        var taskInfos = await TaskManager.FetchTasksAsync(
            request.TaskName,
            request.TaskShip,
            request.TaskMode,
            request.TaskStatus,
            request.Page ?? 0,
            request.Size ?? 0,
            context.CancellationToken);

        return taskInfos.PagedResponse<SearchTaskInfoResponse, TaskInfo>();
    }

    /// <summary>
    ///     读取任务信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("读取任务信息")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<ReadTaskInfoResponse> ReadTaskInfo(ReadTaskInfoRequest request,
        ServerCallContext context)
    {
        var taskId = Guid.Parse(request.TaskId);

        var taskInfo = await TaskManager.GetTaskAsync(taskId, context.CancellationToken);

        return taskInfo.ReadInfoResponse<ReadTaskInfoResponse, TaskInfoTree>();
    }

    #endregion
}