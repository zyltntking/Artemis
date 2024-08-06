using Artemis.Data.Core;
using Artemis.Data.Core.Fundamental.Design;
using Artemis.Data.Core.Fundamental.Types;
using Artemis.Data.Store.Extensions;
using Artemis.Service.Protos;
using Artemis.Service.Protos.Business.VisionScreen;
using Artemis.Service.Task.Context;
using Grpc.Core;

namespace Artemis.Service.Business.VisionScreen.Services;

/// <summary>
/// 视力筛查核心服务实现
/// </summary>
public class VisionScreeningCoreServiceImplement : VisionScreeningCoreService.VisionScreeningCoreServiceBase
{
    #region Overrides of VisionScreeningCoreServiceBase

    /// <summary>
    /// 生成任务
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    public override Task<EmptyResponse> GeneratorTask(GeneratorTaskRequest request, ServerCallContext context)
    {
        var organization = request.OrgnizationTree;

        var task = BuildTaskTree(request.TaskName, null, 1,  organization, null);


        return base.GeneratorTask(request, context);
    }


    private ArtemisTask BuildTaskTree(string taskName, string? parentTaskCode, int serial, TaskBindOrgnizationTreePacket organization, ArtemisTask? taskNode)
    {
        if (organization.Type == OrganizationType.Management)
        {
            var task = Instance.CreateInstance<ArtemisTask>();
            task.TaskName = taskName;
            task.NormalizedTaskName = taskName.Normalize();
            task.TaskCode = DesignCode.Task(organization.Code, serial, parentTaskCode);
            task.DesignCode = organization.Code;
            task.TaskShip = string.IsNullOrWhiteSpace(parentTaskCode) ? TaskShip.Child : TaskShip.Root;
            task.TaskMode = TaskMode.Normal;
            task.TaskState = TaskState.Created;
            task.Description = organization.Name;
        }
        else
        {
            // Functional
        }

        return null;

    }

    #endregion
}