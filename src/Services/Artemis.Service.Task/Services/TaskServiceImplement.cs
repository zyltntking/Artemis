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
    /// <param name="taskTreeManager"></param>
    /// <param name="logger">日志记录器</param>
    public TaskServiceImplement(
        ITaskTreeManager taskTreeManager,
        ILogger<TaskServiceImplement> logger)
    {
        TaskTreeManager = taskTreeManager;
        Logger = logger;
    }

    /// <summary>
    ///     任务树管理器
    /// </summary>
    private ITaskTreeManager TaskTreeManager { get; }

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
        var taskInfos = await TaskTreeManager.FetchTasksAsync(
            request.TaskName,
            request.TaskCode,
            request.DesignCode,
            request.TaskShip,
            request.TaskMode,
            request.TaskState,
            request.Page ?? 0,
            request.Size ?? 0,
            context.CancellationToken);

        var response = taskInfos.PagedResponse<SearchTaskInfoResponse, TaskInfo>();

        var taskReplies = taskInfos.Items!.Select(item => item.Adapt<TaskInfoPacket>());

        response.Data.Items.Add(taskReplies);

        return response;
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

        var taskInfo = await TaskTreeManager.ReadEntityInfoAsync(taskId, context.CancellationToken);

        return taskInfo.ReadInfoResponse<ReadTaskInfoResponse, TaskInfo>();
    }

    /// <summary>
    ///     读取任务树信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("读取任务树信息")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<ReadTaskInfoTreeResponse> ReadTaskInfoTree(ReadTaskInfoRequest request,
        ServerCallContext context)
    {
        var taskId = Guid.Parse(request.TaskId);

        var taskInfoTree = await TaskTreeManager.GetEntityInfoTreeAsync(taskId, context.CancellationToken);

        return taskInfoTree.ReadInfoResponse<ReadTaskInfoTreeResponse, TaskInfoTree>();
    }

    /// <summary>
    ///     创建任务
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("创建任务")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> CreateTask(CreateTaskRequest request, ServerCallContext context)
    {
        var package = request.Adapt<TaskPackage>();

        var result = await TaskTreeManager.CreateEntityAsync(package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量创建任务
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量创建任务")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchCreateTask(BatchCreateTaskRequest request,
        ServerCallContext context)
    {
        var tasks = request.Batch.Adapt<IEnumerable<TaskPackage>>();

        var result = await TaskTreeManager.BatchCreateEntityAsync(tasks, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     更新任务
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

        var result = await TaskTreeManager.UpdateEntityAsync(taskId, package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量更新任务
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量更新任务")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchUpdateTask(BatchUpdateTaskRequest request,
        ServerCallContext context)
    {
        var dictionary = request.Batch.ToDictionary(
            item => Guid.Parse(item.TaskId),
            item => item.Adapt<TaskPackage>());

        var result = await TaskTreeManager.BatchUpdateEntityAsync(dictionary, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     删除任务
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("删除任务")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> DeleteTask(DeleteTaskRequest request, ServerCallContext context)
    {
        var taskId = Guid.Parse(request.TaskId);

        var result = await TaskTreeManager.DeleteEntityAsync(taskId, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量删除任务
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量删除任务")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchDeleteTask(BatchDeleteTaskRequest request,
        ServerCallContext context)
    {
        var taskIds = request.TaskIds.Select(Guid.Parse);

        var result = await TaskTreeManager.BatchDeleteEntityAsync(taskIds, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     创建子任务
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("创建子任务")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> CreateChildTask(CreateChildTaskRequest request,
        ServerCallContext context)
    {
        var taskId = Guid.Parse(request.TaskId);

        var package = request.Adapt<TaskPackage>();

        var result = await TaskTreeManager.CreateChildEntityAsync(taskId, package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量创建子任务
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量创建子任务")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchCreateChildTask(BatchCreateChildTaskRequest request,
        ServerCallContext context)
    {
        var taskId = Guid.Parse(request.TaskId);

        var packages = request.Batch.Adapt<IEnumerable<TaskPackage>>();

        var result = await TaskTreeManager.BatchCreateChildEntityAsync(taskId, packages, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     添加子任务
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("添加子任务")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> AddChildTask(AddChildTaskRequest request, ServerCallContext context)
    {
        var taskId = Guid.Parse(request.TaskId);

        var childId = Guid.Parse(request.ChildTask.ChildTaskId);

        var result = await TaskTreeManager.AddChildEntityAsync(taskId, childId, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量添加子任务
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量添加子任务")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchAddChildTask(BatchAddChildTaskRequest request,
        ServerCallContext context)
    {
        var taskId = Guid.Parse(request.TaskId);

        var childIds = request.ChildTaskIds.Select(Guid.Parse);

        var result = await TaskTreeManager.BatchAddChildEntityAsync(taskId, childIds, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     删除子任务
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("删除子任务")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> DeleteChildTask(DeleteChildTaskRequest request,
        ServerCallContext context)
    {
        var taskId = Guid.Parse(request.TaskId);

        var childTaskId = Guid.Parse(request.ChildTaskId);

        var result = await TaskTreeManager.DeleteChildEntityAsync(taskId, childTaskId, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量删除子任务
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量删除子任务")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchDeleteChildTask(BatchDeleteChildTaskRequest request,
        ServerCallContext context)
    {
        var taskId = Guid.Parse(request.TaskId);

        var childTaskIds = request.ChildTaskIds.Select(Guid.Parse);

        var result = await TaskTreeManager.BatchDeleteChildEntityAsync(taskId, childTaskIds, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     移除子任务
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("移除子任务")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> RemoveChildTask(RemoveChildTaskRequest request,
        ServerCallContext context)
    {
        var taskId = Guid.Parse(request.TaskId);

        var childTaskId = Guid.Parse(request.ChildTask.ChildTaskId);

        var result = await TaskTreeManager.RemoveChildEntityAsync(taskId, childTaskId, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量移除子任务
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量移除子任务")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchRemoveChildTask(BatchRemoveChildTaskRequest request,
        ServerCallContext context)
    {
        var taskId = Guid.Parse(request.TaskId);

        var childTaskIds = request.ChildTaskIds.Select(Guid.Parse);

        var result = await TaskTreeManager.BatchRemoveChildEntityAsync(taskId, childTaskIds, context.CancellationToken);

        return result.AffectedResponse();
    }

    #endregion
}