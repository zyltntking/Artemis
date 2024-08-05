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
    /// <param name="taskUnitManager"></param>
    /// <param name="taskUnitTargetManager"></param>
    /// <param name="logger">日志记录器</param>
    public TaskServiceImplement(
        ITaskTreeManager taskTreeManager,
        ITaskUnitManager taskUnitManager,
        ITaskUnitTargetManager taskUnitTargetManager,
        ILogger<TaskServiceImplement> logger)
    {
        TaskTreeManager = taskTreeManager;
        TaskUnitManager = taskUnitManager;
        TaskUnitTargetManager = taskUnitTargetManager;
        
        Logger = logger;
    }

    /// <summary>
    ///     任务树管理器
    /// </summary>
    private ITaskTreeManager TaskTreeManager { get; }

    /// <summary>
    /// 任务单元管理器
    /// </summary>
    private ITaskUnitManager TaskUnitManager { get; }

    /// <summary>
    /// 任务单元目标管理器
    /// </summary>
    private ITaskUnitTargetManager TaskUnitTargetManager { get; }

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
    [Authorize(AuthorizePolicy.Token)]
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

        return taskInfos.PagedResponse<SearchTaskInfoResponse, TaskInfo>();
    }

    /// <summary>
    ///     读取任务信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("读取任务信息")]
    [Authorize(AuthorizePolicy.Token)]
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
    [Authorize(AuthorizePolicy.Token)]
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
    [Authorize(AuthorizePolicy.Token)]
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
    [Authorize(AuthorizePolicy.Token)]
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
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> UpdateTask(UpdateTaskRequest request, ServerCallContext context)
    {
        var taskId = Guid.Parse(request.TaskId);

        var package = request.Task.Adapt<TaskPackage>();

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
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> BatchUpdateTask(BatchUpdateTaskRequest request,
        ServerCallContext context)
    {
        var dictionary = request.Batch.ToDictionary(
            item => Guid.Parse(item.TaskId),
            item => item.Task.Adapt<TaskPackage>());

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
    [Authorize(AuthorizePolicy.Token)]
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
    [Authorize(AuthorizePolicy.Token)]
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
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> CreateChildTask(CreateChildTaskRequest request,
        ServerCallContext context)
    {
        var taskId = Guid.Parse(request.TaskId);

        var package = request.Task.Adapt<TaskPackage>();

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
    [Authorize(AuthorizePolicy.Token)]
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
    [Authorize(AuthorizePolicy.Token)]
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
    [Authorize(AuthorizePolicy.Token)]
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
    [Authorize(AuthorizePolicy.Token)]
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
    [Authorize(AuthorizePolicy.Token)]
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
    [Authorize(AuthorizePolicy.Token)]
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
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> BatchRemoveChildTask(BatchRemoveChildTaskRequest request,
        ServerCallContext context)
    {
        var taskId = Guid.Parse(request.TaskId);

        var childTaskIds = request.ChildTaskIds.Select(Guid.Parse);

        var result = await TaskTreeManager.BatchRemoveChildEntityAsync(taskId, childTaskIds, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 查询任务单元信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查询任务单元信息")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<SearchTaskUnitInfoResponse> SearchTaskUnitInfo(SearchTaskUnitInfoRequest request, ServerCallContext context)
    {
        var taskId = Guid.Parse(request.TaskId);

        var info = await TaskUnitManager.FetchTaskUnitsAsync(
            taskId,
            request.UnitName,
            request.UnitCode,
            request.DesignCode,
            request.TaskUnitState,
            request.TaskUnitMode,
            request.Page ?? 0,
            request.Size ?? 0,
            context.CancellationToken);

        return info.PagedResponse<SearchTaskUnitInfoResponse, TaskUnitInfo>();
    }

    /// <summary>
    /// 获取任务单元
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("获取任务单元")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<ReadTaskUnitResponse> ReadTaskUnitInfo(ReadTaskUnitRequest request, ServerCallContext context)
    {
        var taskId = Guid.Parse(request.TaskId);
        var unitId = Guid.Parse(request.TaskUnitId);

        var info = await TaskUnitManager.ReadSubEntityInfoAsync(taskId, unitId, context.CancellationToken);

        return info.ReadInfoResponse<ReadTaskUnitResponse, TaskUnitInfo>();
    }

    /// <summary>
    /// 创建任务单元
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("创建任务单元")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> CreateTaskUnit(CreateTaskUnitRequest request, ServerCallContext context)
    {
        var taskId = Guid.Parse(request.TaskId);
        var package = request.TaskUnit.Adapt<TaskUnitPackage>();

        var result = await TaskUnitManager.CreateSubEntityAsync(taskId, package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 批量创建任务单元
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量创建任务单元")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> BatchCreateTaskUnit(BatchCreateTaskUnitRequest request, ServerCallContext context)
    {
        var taskId = Guid.Parse(request.TaskId);

        var packages = request.Batch.Adapt<IEnumerable<TaskUnitPackage>>();

        var result = await TaskUnitManager.BatchCreateSubEntityAsync(taskId, packages, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 更新任务单元
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("更新任务单元")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> UpdateTaskUnit(UpdateTaskUnitRequest request, ServerCallContext context)
    {
        var taskId = Guid.Parse(request.TaskId);

        var unitId = Guid.Parse(request.TaskUnitId);

        var package = request.TaskUnit.Adapt<TaskUnitPackage>();

        var result = await TaskUnitManager.UpdateSubEntityAsync(taskId, unitId, package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 批量更新任务单元
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量更新任务单元")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> BatchUpdateTaskUnit(BatchUpdateTaskUnitRequest request, ServerCallContext context)
    {
        var taskId = Guid.Parse(request.TaskId);

        var dictionary = request.Batch.ToDictionary(
            item => Guid.Parse(item.TaskUnitId),
            item => item.TaskUnit.Adapt<TaskUnitPackage>());

        var result = await TaskUnitManager.BatchUpdateSubEntityAsync(taskId, dictionary, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///删除任务单元
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("删除任务单元")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> DeleteTaskUnit(DeleteTaskUnitRequest request, ServerCallContext context)
    {
        var taskId = Guid.Parse(request.TaskId);

        var unitId = Guid.Parse(request.TaskUnitId);

        var result = await TaskUnitManager.DeleteSubEntityAsync(taskId, unitId, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 批量删除任务单元
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量删除任务单元")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> BatchDeleteTaskUnit(BatchDeleteTaskUnitRequest request, ServerCallContext context)
    {
        var taskId = Guid.Parse(request.TaskId);

        var unitIds = request.TaskUnitIds.Select(Guid.Parse);

        var result = await TaskUnitManager.BatchDeleteSubEntityAsync(taskId, unitIds, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 查询任务单元目标
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查询任务单元目标")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<SearchTaskUnitTargetResponse> SearchTaskUnitTarget(SearchTaskUnitTargetRequest request, ServerCallContext context)
    {
        var id = Guid.Parse(request.TaskUnitId);

        var info = await TaskUnitTargetManager.FetchTaskUnitTargetsAsync(
            id,
            request.TargetName,
            request.TargetCode,
            request.DesignCode,
            request.TargetType,
            request.TargetState,
            request.Page ?? 0,
            request.Size ?? 0,
            context.CancellationToken);

        return info.PagedResponse<SearchTaskUnitTargetResponse, TaskUnitTargetInfo>();
    }

    /// <summary>
    /// 获取任务单元目标
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("获取任务单元目标")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<ReadTaskUnitTargetResponse> ReadTaskUnitTarget(ReadTaskUnitTargetRequest request, ServerCallContext context)
    {
        var unitId = Guid.Parse(request.TaskUnitId);
        var targetId = Guid.Parse(request.TaskUnitTargetId);

        var info = await TaskUnitTargetManager.ReadSubEntityInfoAsync(unitId, targetId, context.CancellationToken);

        return info.ReadInfoResponse<ReadTaskUnitTargetResponse, TaskUnitTargetInfo>();
    }

    /// <summary>
    /// 创建任务单元目标
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("创建任务单元目标")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> CreateTaskUnitTarget(CreateTaskUnitTargetRequest request, ServerCallContext context)
    {
        var unitId = Guid.Parse(request.TaskUnitId);

        var package = request.TaskUnitTarget.Adapt<TaskUnitTargetPackage>();

        var result = await TaskUnitTargetManager.CreateSubEntityAsync(unitId, package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 批量创建任务单元目标
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量创建任务单元目标")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> BatchCreateTaskUnitTarget(BatchCreateTaskUnitTargetRequest request, ServerCallContext context)
    {
        var unitId = Guid.Parse(request.TaskUnitId);

        var packages = request.Batch.Adapt<IEnumerable<TaskUnitTargetPackage>>();

        var result = await TaskUnitTargetManager.BatchCreateSubEntityAsync(unitId, packages, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 更新任务单元目标
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("更新任务单元目标")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> UpdateTaskUnitTarget(UpdateTaskUnitTargetRequest request, ServerCallContext context)
    {
        var unitId = Guid.Parse(request.TaskUnitId);

        var targetId = Guid.Parse(request.TaskUnitTargetId);

        var package = request.TaskUnitTarget.Adapt<TaskUnitTargetPackage>();

        var result = await TaskUnitTargetManager.UpdateSubEntityAsync(unitId, targetId, package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 批量更新任务单元目标
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量更新任务单元目标")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> BatchUpdateTaskUnitTarget(BatchUpdateTaskUnitTargetRequest request, ServerCallContext context)
    {
        var unitId = Guid.Parse(request.TaskUnitId);

        var dictionary = request.Batch.ToDictionary(
            item => Guid.Parse(item.Id),
            item => item.TaskUnitTarget.Adapt<TaskUnitTargetPackage>());

        var result = await TaskUnitTargetManager.BatchUpdateSubEntityAsync(unitId, dictionary, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 删除任务单元目标
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("删除任务单元目标")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> DeleteTaskUnitTarget(DeleteTaskUnitTargetRequest request, ServerCallContext context)
    {
        var unitId = Guid.Parse(request.TaskUnitId);
        var targetId = Guid.Parse(request.TaskUnitTargetId);

        var result = await TaskUnitTargetManager.DeleteSubEntityAsync(unitId, targetId, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 批量删除任务单元目标
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量删除任务单元目标")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> BatchDeleteTaskUnitTarget(BatchDeleteTaskUnitTargetRequest request, ServerCallContext context)
    {
        var unitId = Guid.Parse(request.TaskUnitId);

        var targetIds = request.TaskUnitTargetIds.Select(Guid.Parse);

        var result = await TaskUnitTargetManager.BatchDeleteSubEntityAsync(unitId, targetIds, context.CancellationToken);

        return result.AffectedResponse();
    }

    #endregion
}