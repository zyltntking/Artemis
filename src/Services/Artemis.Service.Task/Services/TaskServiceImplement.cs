using System.ComponentModel;
using Artemis.Extensions.Identity;
using Artemis.Service.Protos;
using Artemis.Service.Protos.Task;
using Artemis.Service.Shared.Task.Transfer;
using Artemis.Service.Task.Managers;
using Grpc.Core;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

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
    /// <param name="logger">日志记录器</param>
    public TaskServiceImplement(
        IArtemisTaskManager taskManager,
        ILogger<TaskServiceImplement> logger)
    {
        TaskManager = taskManager;
        Logger = logger;
    }

    /// <summary>
    ///     任务管理器
    /// </summary>
    private IArtemisTaskManager TaskManager { get; }

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
            request.TaskState,
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

    /// <summary>
    /// 创建任务
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("创建任务")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> CreateTask(CreateTaskRequest request, ServerCallContext context)
    {
        var package = request.Adapt<TaskPackage>();

        var result = await TaskManager.CreateTaskAsync(package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 批量创建任务
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量创建任务")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchCreateTask(BatchCreateTaskRequest request, ServerCallContext context)
    {
        var tasks = request.Batch.Adapt<IEnumerable<TaskPackage>>();

        var result = await TaskManager.CreateTasksAsync(tasks, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 更新任务
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("更新任务")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> UpdateTask(UpdateTaskRequest request, ServerCallContext context)
    {
        var taskId = Guid.Parse(request.TaskId);

        var package = request.Adapt<TaskPackage>();

        var result = await TaskManager.UpdateTaskAsync(taskId, package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 批量更新任务
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量更新任务")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchUpdateTask(BatchUpdateTaskRequest request, ServerCallContext context)
    {
        var dictionary = request.Batch.ToDictionary(
            item => Guid.Parse(item.TaskId),
            item => item.Adapt<TaskPackage>());

        var result = await TaskManager.UpdateTasksAsync(dictionary, context.CancellationToken);

        return result.AffectedResponse();
    }

    #endregion
}