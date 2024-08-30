using System.ComponentModel;
using System.Linq.Dynamic.Core;
using Artemis.Data.Core;
using Artemis.Data.Core.Fundamental;
using Artemis.Data.Core.Fundamental.Design;
using Artemis.Data.Core.Fundamental.Types;
using Artemis.Data.Store;
using Artemis.Data.Store.Extensions;
using Artemis.Extensions.Identity;
using Artemis.Service.Business.VisionScreen.Context;
using Artemis.Service.Business.VisionScreen.Stores;
using Artemis.Service.Identity.Stores;
using Artemis.Service.Protos;
using Artemis.Service.Protos.Business.VisionScreen;
using Artemis.Service.Resource.Stores;
using Artemis.Service.School.Stores;
using Artemis.Service.Shared.Business.VisionScreen.Transfer;
using Artemis.Service.Shared.Resource.Transfer;
using Artemis.Service.Shared.School.Transfer;
using Artemis.Service.Shared.Task.Transfer;
using Artemis.Service.Task.Context;
using Artemis.Service.Task.Stores;
using Grpc.Core;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Business.VisionScreen.Services;

/// <summary>
/// 视力筛查核心服务实现
/// </summary>
public class VisionScreeningCoreServiceImplement : VisionScreeningCoreService.VisionScreeningCoreServiceBase
{
    /// <summary>
    /// 视力筛查核心服务实现
    /// </summary>
    /// <param name="userStore"></param>
    /// <param name="taskStore"></param>
    /// <param name="taskUnitStore"></param>
    /// <param name="unitTargetStore"></param>
    /// <param name="schoolStore"></param>
    /// <param name="classStore"></param>
    /// <param name="studentStore"></param>
    /// <param name="teacherStore"></param>
    /// <param name="standardCatalogStore"></param>
    /// <param name="standardItemStore"></param>
    /// <param name="divisionStore"></param>
    /// <param name="organizationStore"></param>
    /// <param name="visionScreenRecordStore"></param>
    /// <param name="optometerStore"></param>
    /// <param name="visualChartStore"></param>
    /// <param name="studentRelationBindingStore"></param>
    /// <param name="teacherUserBindingStore"></param>
    /// <param name="organizationUsersBindingStore"></param>
    /// <param name="notificationMessageStore"></param>
    /// <param name="systemModuleStore"></param>
    public VisionScreeningCoreServiceImplement(
        IIdentityUserStore userStore,
        IArtemisTaskStore taskStore,
        IArtemisTaskUnitStore taskUnitStore,
        IArtemisTaskUnitTargetStore unitTargetStore,
        IArtemisSchoolStore schoolStore,
        IArtemisClassStore classStore,
        IArtemisStudentStore studentStore,
        IArtemisTeacherStore teacherStore,
        IArtemisStandardCatalogStore standardCatalogStore,
        IArtemisStandardItemStore standardItemStore,
        IArtemisDivisionStore divisionStore,
        IArtemisOrganizationStore organizationStore,
        IArtemisVisionScreenRecordStore visionScreenRecordStore,
        IArtemisOptometerStore optometerStore,
        IArtemisVisualChartStore visualChartStore,
        IArtemisStudentRelationBindingStore studentRelationBindingStore,
        IArtemisTeacherUserBindingStore teacherUserBindingStore,
        IArtemisOrganizationUsersBindingStore organizationUsersBindingStore,
        IArtemisNotificationMessageStore notificationMessageStore,
        IArtemisSystemModuleStore systemModuleStore)
    {
        UserStore = userStore;
        TaskStore = taskStore;
        TaskUnitStore = taskUnitStore;
        TaskUnitTargetStore = unitTargetStore;
        SchoolStore = schoolStore;
        ClassStore = classStore;
        StudentStore = studentStore;
        TeacherStore = teacherStore;
        StandardCatalogStore = standardCatalogStore;
        StandardItemStore = standardItemStore;
        DivisionStore = divisionStore;
        OrganizationStore = organizationStore;
        VisionScreenRecordStore = visionScreenRecordStore;
        OptometerStore = optometerStore;
        VisualChartStore = visualChartStore;
        StudentRelationBindingStore = studentRelationBindingStore;
        TeacherUserBindingStore = teacherUserBindingStore;
        OrganizationUsersBindingStore = organizationUsersBindingStore;
        NotificationMessageStore = notificationMessageStore;
        SystemModuleStore = systemModuleStore;
    }

    private IIdentityUserStore UserStore { get; }

    private IArtemisTaskStore TaskStore { get; }

    private IArtemisTaskUnitStore TaskUnitStore { get; }

    private IArtemisTaskUnitTargetStore TaskUnitTargetStore { get; }

    private IArtemisSchoolStore SchoolStore { get; }

    private IArtemisClassStore ClassStore { get; }

    private IArtemisStudentStore StudentStore { get; }

    private IArtemisTeacherStore TeacherStore { get; }

    private IArtemisStandardCatalogStore StandardCatalogStore { get; }

    private IArtemisStandardItemStore StandardItemStore { get; }

    private IArtemisDivisionStore DivisionStore { get; }

    private IArtemisOrganizationStore OrganizationStore { get; }

    private IArtemisVisionScreenRecordStore VisionScreenRecordStore { get; }

    private IArtemisOptometerStore OptometerStore { get; }

    private IArtemisVisualChartStore VisualChartStore { get; }

    private IArtemisStudentRelationBindingStore StudentRelationBindingStore { get; }

    private IArtemisTeacherUserBindingStore TeacherUserBindingStore { get; }

    private IArtemisOrganizationUsersBindingStore OrganizationUsersBindingStore { get; }

    private IArtemisNotificationMessageStore NotificationMessageStore { get; }

    private IArtemisSystemModuleStore SystemModuleStore { get; }

    #region Overrides of VisionScreeningCoreServiceBase

    /// <summary>
    /// 教师绑定用户
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("教师绑定用户")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> BindTeacherUser(BindTeacherUserRequest request, ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);

        var teacherId = Guid.Parse(request.TeacherId);

        var userExists = await UserStore.ExistsAsync(userId, context.CancellationToken);

        if (!userExists)
        {
            return ResultAdapter.AdaptEmptyFail<AffectedResponse>("用户不存在");
        }

        var teacherExists = await TeacherStore.ExistsAsync(teacherId, context.CancellationToken);

        if (!teacherExists)
        {
            return ResultAdapter.AdaptEmptyFail<AffectedResponse>("教师不存在");
        }

        var binding = await TeacherUserBindingStore.EntityQuery
            .Where(bind => bind.TeacherId == teacherId)
            .FirstOrDefaultAsync(context.CancellationToken);

        var userBindExists = await TeacherUserBindingStore.EntityQuery
            .Where(bind => bind.UserId == userId)
            .AnyAsync(context.CancellationToken);

        StoreResult result;

        if (binding == null)
        {
            if (userBindExists)
            {
                return ResultAdapter.AdaptEmptyFail<AffectedResponse>("用户已经绑定了其他教师");
            }

            binding = Instance.CreateInstance<ArtemisTeacherUserBinding>();
            binding.TeacherId = teacherId;
            binding.UserId = userId;
            result = await TeacherUserBindingStore.CreateAsync(binding, context.CancellationToken);
        }
        else
        {
            if (binding.UserId == userId)
            {
                return ResultAdapter.AdaptEmptyFail<AffectedResponse>("教师已经绑定了该用户");
            }

            if (userBindExists)
            {
                return ResultAdapter.AdaptEmptyFail<AffectedResponse>("用户已经绑定了其他教师");
            }

            binding.UserId = userId;
            binding.TeacherId = teacherId;
            result = await TeacherUserBindingStore.UpdateAsync(binding, context.CancellationToken);
        }

        return result.AffectedResponse();
    }

    /// <summary>
    /// 解绑教师用户
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("解绑教师用户")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> UnBindTeacherUser(UnBindTeacherUserRequest request, ServerCallContext context)
    {
        var teacherId = Guid.Parse(request.TeacherId);

        var teacherExists = await TeacherStore.ExistsAsync(teacherId, context.CancellationToken);

        if (!teacherExists)
        {
            return ResultAdapter.AdaptEmptyFail<AffectedResponse>("教师不存在");
        }

        var bindings = await TeacherUserBindingStore.EntityQuery
            .Where(bind => bind.TeacherId == teacherId)
            .ToListAsync(context.CancellationToken);

        var result = await TeacherUserBindingStore.DeleteAsync(bindings, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 机构添加用户
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("机构添加用户")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> BindOrganizationUsers(BindOrganizationUsersRequest request, ServerCallContext context)
    {
        var organizationId = Guid.Parse(request.OrganizationId);
        var userIds = request.UserIds.Select(Guid.Parse);

        var organizationExists = await OrganizationStore.ExistsAsync(organizationId, context.CancellationToken);

        if (!organizationExists)
        {
            return ResultAdapter.AdaptEmptyFail<AffectedResponse>("机构不存在");
        }

        var storedUserIds = await UserStore.EntityQuery
            .Where(item => userIds.Contains(item.Id))
            .Select(item => item.Id)
            .ToListAsync(context.CancellationToken);

        if (storedUserIds.Any())
        {
            var beenBindUserIds = await OrganizationUsersBindingStore.EntityQuery
                .Where(item => item.OrganizationId == organizationId)
                .Select(item => item.UserId)
                .ToListAsync(context.CancellationToken);

            var notBindUserIds = storedUserIds.Except(beenBindUserIds).ToList();

            if (notBindUserIds.Any())
            {
                var binds = notBindUserIds.Select(item =>
                {
                    var bind = Instance.CreateInstance<ArtemisOrganizationUsersBinding>();
                    bind.OrganizationId = organizationId;
                    bind.UserId = item;

                    return bind;
                });

                var result = await OrganizationUsersBindingStore.CreateAsync(binds);

                return result.AffectedResponse();
            }

            return ResultAdapter.AdaptEmptyFail<AffectedResponse>("用户均已是该机构所属");

        }

        return ResultAdapter.AdaptEmptyFail<AffectedResponse>("用户不存在");
    }

    /// <summary>
    /// 机构移除用户
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("机构移除用户")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> UnBindOrganizationUsers(UnBindOrganizationUsersRequest request, ServerCallContext context)
    {
        var organizationId = Guid.Parse(request.OrganizationId);
        var userIds = request.UserIds.Select(Guid.Parse);

        var organizationExists = await OrganizationStore.ExistsAsync(organizationId, context.CancellationToken);

        if (!organizationExists)
        {
            return ResultAdapter.AdaptEmptyFail<AffectedResponse>("机构不存在");
        }

        var binds = await OrganizationUsersBindingStore.EntityQuery
            .Where(item => item.OrganizationId == organizationId)
            .Where(item => userIds.Contains(item.UserId))
            .ToListAsync(context.CancellationToken);

        if (binds.Any())
        {
            var result = await OrganizationUsersBindingStore.DeleteAsync(binds, context.CancellationToken);

            return result.AffectedResponse();
        }

        return ResultAdapter.AdaptEmptyFail<AffectedResponse>("没有找到这些用户与该机构的关联信息");
    }

    /// <summary>
    /// 创建(子)任务
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("创建(子)任务")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> CreateOrAcceptTask(CreateOrAcceptTaskRequest request, ServerCallContext context)
    {
        var organizationId = Guid.Parse(request.OrganizationId);

        var organization = await OrganizationStore.FindMapEntityAsync<OrganizationInfo>(organizationId, context.CancellationToken);

        if (organization == null)
        {
            return ResultAdapter.AdaptEmptyFail<AffectedResponse>("本次任务关联的组织机构不存在");
        }

        Guid? taskId;

        string? taskCode;

        var startTime = DateTime.Parse(request.StartTime);

        var endTime = DateTime.Parse(request.EndTime);

        StoreResult taskResult;

        if (request.TaskId != null)
        {
            taskId = Guid.Parse(request.TaskId);

            var task = await TaskStore.FindEntityAsync(taskId.Value, context.CancellationToken);

            if (task == null)
            {
                return ResultAdapter.AdaptEmptyFail<AffectedResponse>("本次任务关联的上级任务不存在");
            }

            task.TaskState = TaskState.Waiting;
            task.TaskName = request.TaskName;
            task.TaskShip = TaskShip.Child;
            task.StartTime = startTime;
            task.EndTime = endTime;
            task.Description = organization.Name;

            taskResult = await TaskStore.UpdateAsync(task, context.CancellationToken);

            taskCode = task.TaskCode;
        }
        else
        {
            var task = Instance.CreateInstance<ArtemisTask>();
            task.TaskName = request.TaskName;
            task.ParentId = null;
            task.NormalizedTaskName = request.TaskName.Normalize();
            task.TaskCode = DesignCode.Task(organization.Code!, 1);
            task.DesignCode = organization.Code;
            task.TaskShip = TaskShip.Root;
            task.TaskMode = TaskMode.Normal;
            task.TaskState = TaskState.Waiting;
            task.StartTime = startTime;
            task.EndTime = endTime;
            task.Description = organization.Name;

            taskResult = await TaskStore.CreateAsync(task, context.CancellationToken);

            taskId = task.Id;
            taskCode = task.TaskCode;
        }

        var managementOrganizationIds = request.SubManagementOrganizationIds.Select(Guid.Parse).ToList();

        var functionalOrganizationIds = request.SubFunctionalOrganizationIds.Select(Guid.Parse).ToList();

        var organizationIds = managementOrganizationIds.Concat(functionalOrganizationIds);

        var organizations = await OrganizationStore.FindMapEntitiesAsync<OrganizationInfo>(organizationIds);

        var organizationList = organizations.ToList();

        var subTaskResult = StoreResult.Failed();

        var index = 1;

        if (managementOrganizationIds.Any())
        {
            var subTasks = new List<ArtemisTask>();

            foreach (var managementOrganizationId in managementOrganizationIds)
            {
                var managementOrganization = organizationList.FirstOrDefault(item => item.Id == managementOrganizationId);

                if (managementOrganization != null)
                {
                    var subTask = Instance.CreateInstance<ArtemisTask>();

                    subTask.TaskName = request.TaskName;
                    subTask.ParentId = taskId.Value;
                    subTask.NormalizedTaskName = subTask.TaskName.Normalize();
                    subTask.TaskCode = DesignCode.Task(organization.Code!, index, taskCode);
                    subTask.DesignCode = managementOrganization.Code;
                    subTask.TaskShip = TaskShip.Child;
                    subTask.TaskMode = TaskMode.Normal;
                    subTask.TaskState = TaskState.Created;
                    subTask.Description = managementOrganization.Name;
                    subTask.StartTime = startTime;
                    subTask.EndTime = endTime;

                    subTasks.Add(subTask);
                    index++;
                }
            }

            if (subTasks.Any())
            {
                subTaskResult = await TaskStore.CreateAsync(subTasks, context.CancellationToken);
            }
        }

        var taskUnitsResult = StoreResult.Failed();

        if (functionalOrganizationIds.Any())
        {
            var taskUnits = new List<ArtemisTaskUnit>();

            foreach (var functionalOrganizationId in functionalOrganizationIds)
            {
                var functionalOrganization = organizationList.FirstOrDefault(item => item.Id == functionalOrganizationId);

                if (functionalOrganization != null)
                {
                    var taskUnit = Instance.CreateInstance<ArtemisTaskUnit>();

                    taskUnit.TaskId = taskId.Value;
                    taskUnit.UnitName = request.TaskName;
                    taskUnit.NormalizedUnitName = taskUnit.UnitName.Normalize();
                    taskUnit.UnitCode = DesignCode.Task(organization.Code!, index, taskCode);
                    taskUnit.DesignCode = functionalOrganization.Code;
                    taskUnit.TaskUnitMode = TaskMode.Normal;
                    taskUnit.TaskUnitState = TaskState.Created;
                    taskUnit.Description = organization.Name;
                    taskUnit.StartTime = startTime;
                    taskUnit.EndTime = endTime;

                    taskUnits.Add(taskUnit);
                    index++;
                }
            }

            if (taskUnits.Any())
            {
                taskUnitsResult = await TaskUnitStore.CreateAsync(taskUnits, context.CancellationToken);
            }
        }

        var affectRows = taskResult.AffectRows + subTaskResult.AffectRows + taskUnitsResult.AffectRows;

        var result = StoreResult.Failed();

        if (affectRows > 0)
        {
            result = StoreResult.Success(affectRows);
        }

        return result.AffectedResponse();
    }

    /// <summary>
    /// 获取当前未毕业班级的列表
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("获取当前未毕业班级的列表")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<FetchSchoolNotGraduatedClassResponse> FetchSchoolNotGraduatedClass(FetchSchoolNotGraduatedClassRequest request, ServerCallContext context)
    {
        var schoolId = Guid.Parse(request.SchoolId);

        var schoolExists = await SchoolStore.ExistsAsync(schoolId, context.CancellationToken);

        if (!schoolExists)
        {
            return ResultAdapter.AdaptEmptyFail<FetchSchoolNotGraduatedClassResponse>("学校不存在");
        }

        var classInfos = await ClassStore.EntityQuery
            .Where(item => item.SchoolId == schoolId)
            .Where(item => item.GradeName != GradeName.FinishSchool)
            .ProjectToType<ClassInfo>()
            .ToListAsync(context.CancellationToken);

        if (!classInfos.Any())
        {
            return ResultAdapter.AdaptEmptyFail<FetchSchoolNotGraduatedClassResponse>("当前学校没有可用的未毕业班级");
        }

        var classIds = classInfos.Select(item => item.Id).ToList();

        var classCountGroupInfos = await StudentStore.EntityQuery
            .Where(item => item.SchoolId == schoolId)
            .Where(item => item.ClassId != null)
            .Where(item => classIds.Contains(item.ClassId!.Value))
            .GroupBy(item => item.ClassId)
            .Select(group => new
            {
                ClassId = group.Key,
                Count = group.Count()
            })
            .ToListAsync(context.CancellationToken);

        var packets = new List<SchoolNotGraduatedClassPacket>();

        foreach (var classInfo in classInfos)
        {
            var packet = Instance.CreateInstance<SchoolNotGraduatedClassPacket>();

            var classCount = classCountGroupInfos
                .FirstOrDefault(item => item.ClassId == classInfo.Id);

            packet.SchoolId = classInfo.SchoolId.GuidToString();
            packet.ClassId = classInfo.Id.GuidToString();
            packet.ClassName = classInfo.Name;
            packet.Count = classCount?.Count ?? 0;

            packets.Add(packet);
        }

        return packets.ReadInfoResponse<FetchSchoolNotGraduatedClassResponse, List<SchoolNotGraduatedClassPacket>>();
    }

    /// <summary>
    /// 查询记录任务字段
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查询记录任务字段")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<QueryRecordTaskFieldResponse> QueryRecordTaskField(QueryRecordFieldRequest request, ServerCallContext context)
    {
        var filedList = await RecordFieldQuery(request)
            .ProjectToType<RecordTaskFieldPacket>()
            .Distinct()
            .ToListAsync(context.CancellationToken);

        return filedList.ReadInfoResponse<QueryRecordTaskFieldResponse, IEnumerable<RecordTaskFieldPacket>>();
    }

    /// <summary>
    /// 查询记录区域字段
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查询记录区域字段")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<QueryRecordDivisionFieldResponse> QueryRecordDivisionField(QueryRecordFieldRequest request, ServerCallContext context)
    {
        var filedList = await RecordFieldQuery(request)
            .ProjectToType<RecordDivisionFieldPacket>()
            .Distinct()
            .ToListAsync(context.CancellationToken);

        return filedList.ReadInfoResponse<QueryRecordDivisionFieldResponse, IEnumerable<RecordDivisionFieldPacket>>();
    }

    /// <summary>
    /// 查询记录学校字段
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查询记录学校字段")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<QueryRecordSchoolFieldResponse> QueryRecordSchoolField(QueryRecordFieldRequest request, ServerCallContext context)
    {
        var filedList = await RecordFieldQuery(request)
            .ProjectToType<RecordSchoolFieldPacket>()
            .Distinct()
            .ToListAsync(context.CancellationToken);

        return filedList.ReadInfoResponse<QueryRecordSchoolFieldResponse, IEnumerable<RecordSchoolFieldPacket>>();
    }

    /// <summary>
    /// 查询记录年级字段
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查询记录年级字段")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<QueryRecordGradeFieldResponse> QueryRecordGradeField(QueryRecordFieldRequest request, ServerCallContext context)
    {
        var filedList = await RecordFieldQuery(request)
            .ProjectToType<RecordGradeFieldPacket>()
            .Distinct()
            .ToListAsync(context.CancellationToken);

        return filedList.ReadInfoResponse<QueryRecordGradeFieldResponse, IEnumerable<RecordGradeFieldPacket>>();
    }

    /// <summary>
    /// 查询记录班级字段
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查询记录班级字段")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<QueryRecordClassFieldResponse> QueryRecordClassField(QueryRecordFieldRequest request, ServerCallContext context)
    {
        var filedList = await RecordFieldQuery(request)
            .ProjectToType<RecordClassFieldPacket>()
            .Distinct()
            .ToListAsync(context.CancellationToken);

        return filedList.ReadInfoResponse<QueryRecordClassFieldResponse, IEnumerable<RecordClassFieldPacket>>();
    }

    /// <summary>
    /// 获取当前任务的学校列表
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("获取当前任务的学校列表")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<FetchTaskSchoolResponse> FetchTaskSchool(FetchTaskSchoolRequest request, ServerCallContext context)
    {
        var taskId = Guid.Parse(request.TaskId);

        var taskExists = await TaskStore.ExistsAsync(taskId, context.CancellationToken);

        if (!taskExists)
        {
            return ResultAdapter.AdaptEmptyFail<FetchTaskSchoolResponse>("任务不存在");
        }

        var schoolGroupInfos = await VisionScreenRecordStore.EntityQuery
            .Where(item => item.TaskId == taskId)
            .GroupBy(item => item.SchoolId)
            .Select(group => new
            {
                SchoolId = group.Key,
                group.FirstOrDefault()!.SchoolName,
                Count = group.Count()
            })
            .ToListAsync(context.CancellationToken);

        var packets = new List<TaskSchoolPacket>();

        foreach (var schoolInfo in schoolGroupInfos)
        {
            var packet = Instance.CreateInstance<TaskSchoolPacket>();

            packet.TaskId = request.TaskId;
            packet.SchoolId = schoolInfo.SchoolId.GuidToString();
            packet.SchoolName = schoolInfo.SchoolName;
            packet.Count = schoolInfo.Count;

            packets.Add(packet);
        }

        return packets.ReadInfoResponse<FetchTaskSchoolResponse, List<TaskSchoolPacket>>();
    }

    /// <summary>
    /// 获取当前任务的班级列表
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("获取当前任务的班级列表")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<FetchTaskClassResponse> FetchTaskClass(FetchTaskClassRequest request, ServerCallContext context)
    {
        var taskId = Guid.Parse(request.TaskId);

        var taskExists = await TaskStore.ExistsAsync(taskId, context.CancellationToken);

        if (!taskExists)
        {
            return ResultAdapter.AdaptEmptyFail<FetchTaskClassResponse>("任务不存在");
        }

        var schoolGroupInfos = await VisionScreenRecordStore.EntityQuery
            .Where(item => item.TaskId == taskId)
            .Where(item => item.ClassId !=  null)
            .GroupBy(item => item.ClassId)
            .Select(group => new
            {
                ClassId = group.Key,
                group.FirstOrDefault()!.SchoolName,
                group.FirstOrDefault()!.ClassName,
                Count = group.Count()
            })
            .ToListAsync(context.CancellationToken);

        var packets = new List<TaskClassPacket>();

        foreach (var schoolInfo in schoolGroupInfos)
        {
            var packet = Instance.CreateInstance<TaskClassPacket>();

            packet.TaskId = request.TaskId;
            packet.ClassId = schoolInfo.ClassId!.Value.GuidToString();
            packet.SchoolName = schoolInfo.SchoolName;
            packet.ClassName = schoolInfo.ClassName;
            packet.Count = schoolInfo.Count;

            packets.Add(packet);
        }

        return packets.ReadInfoResponse<FetchTaskClassResponse, List<TaskClassPacket>>();
    }

    /// <summary>
    /// 获取当前任务的学校班级列表
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("获取当前任务的学校班级列表")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<FetchTaskSchoolClassResponse> FetchTaskSchoolClass(FetchTaskSchoolClassRequest request, ServerCallContext context)
    {
        var taskId = Guid.Parse(request.TaskId);

        var taskExists = await TaskStore.ExistsAsync(taskId, context.CancellationToken);

        if (!taskExists)
        {
            return ResultAdapter.AdaptEmptyFail<FetchTaskSchoolClassResponse>("任务不存在");
        }

        var schoolId = Guid.Parse(request.SchoolId);

        var schoolExists = await SchoolStore.ExistsAsync(schoolId, context.CancellationToken);

        if (!schoolExists)
        {
            return ResultAdapter.AdaptEmptyFail<FetchTaskSchoolClassResponse>("学校不存在");
        }

        var schoolClassGroupInfos = await VisionScreenRecordStore.EntityQuery
            .Where(item => item.TaskId == taskId)
            .Where(item => item.SchoolId == schoolId)
            .Where(item => item.ClassId != null)
            .GroupBy(item => item.ClassId)
            .Select(group => new
            {
                ClassId = group.Key,
                group.FirstOrDefault()!.SchoolName,
                group.FirstOrDefault()!.ClassName,
                Count = group.Count()
            })
            .ToListAsync(context.CancellationToken);

        var packets = new List<TaskSchoolClassPacket>();

        foreach (var schoolClassInfo in schoolClassGroupInfos)
        {
            var packet = Instance.CreateInstance<TaskSchoolClassPacket>();

            packet.TaskId = request.TaskId;
            packet.SchoolId = request.SchoolId;
            packet.ClassId = schoolClassInfo.ClassId!.Value.GuidToString();
            packet.SchoolName = schoolClassInfo.SchoolName;
            packet.ClassName = schoolClassInfo.ClassName;
            packet.Count = schoolClassInfo.Count;

            packets.Add(packet);
        }

        return packets.ReadInfoResponse<FetchTaskSchoolClassResponse, List<TaskSchoolClassPacket>>();
    }

    /// <summary>
    /// 创建任务目标和筛查记录
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("创建任务目标和筛查记录")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> AcceptTaskUnit(AcceptTaskUnitRequest request, ServerCallContext context)
    {
        var taskUnitId = Guid.Parse(request.TaskUnitId);

        var taskUnitInfo = await TaskUnitStore.FindEntityAsync(taskUnitId, context.CancellationToken);

        if (taskUnitInfo == null)
        {
            return ResultAdapter.AdaptEmptyFail<AffectedResponse>("没有找到匹配的任务单元信息");
        }

        var taskCodePrefix = taskUnitInfo.UnitCode![..26];

        var taskInfo = await TaskStore.EntityQuery
            .Where(item => item.TaskCode!.StartsWith(taskCodePrefix))
            .Where(item => item.ParentId == null)
            .FirstOrDefaultAsync(context.CancellationToken);

        if (taskInfo == null)
        {
            return ResultAdapter.AdaptEmptyFail<AffectedResponse>("没有找到匹配的任务信息");
        }

        var schoolId = Guid.Parse(request.SchoolId);

        var schoolInfo = await SchoolStore.FindMapEntityAsync<SchoolInfo>(schoolId, context.CancellationToken);

        if (schoolInfo == null)
        {
            return ResultAdapter.AdaptEmptyFail<AffectedResponse>("没有找到匹配的学校信息");
        }

        var divisionInfo = await DivisionStore.EntityQuery
            .Where(division => division.Code == schoolInfo.DivisionCode)
            .ProjectToType<DivisionInfo>()
            .FirstOrDefaultAsync(context.CancellationToken);

        if (divisionInfo == null)
        {
            return ResultAdapter.AdaptEmptyFail<AffectedResponse>("没有找到匹配的行政区划信息");
        }

        var organizationCode = schoolInfo.OrganizationCode;

        var organizationInfo = await OrganizationStore.EntityQuery
            .Where(item => item.Code == organizationCode)
            .FirstOrDefaultAsync(context.CancellationToken);

        if (organizationInfo == null)
        {
            return ResultAdapter.AdaptEmptyFail<AffectedResponse>("没有找到匹配的机构信息");
        }

        var classIds = request.ClassIds.Select(Guid.Parse);

        var classInfos = await ClassStore
            .KeyMatchQuery(classIds)
            .Where(item => item.GradeName != GradeName.FinishSchool)
            .ToListAsync(context.CancellationToken);

        if (!classInfos.Any())
        {
            return ResultAdapter.AdaptEmptyFail<AffectedResponse>("没有找到匹配的班级信息");
        }

        var classInfoIds = classInfos.Select(item => item.Id).ToList();

        var studentInfos = await StudentStore.EntityQuery
            .Where(student => student.SchoolId == schoolId)
            .Where(student => student.ClassId != null && classInfoIds.Contains(student.ClassId.Value))
            .ProjectToType<StudentInfo>()
            .ToListAsync(context.CancellationToken);

        if (!studentInfos.Any())
        {
            return ResultAdapter.AdaptEmptyFail<AffectedResponse>("没有找到匹配学生信息");
        }

        var index = 1;

        var targets = new List<ArtemisTaskUnitTarget>();

        foreach (var studentInfo in studentInfos)
        {
            var target = Instance.CreateInstance<ArtemisTaskUnitTarget>();

            var studentClass = classInfos.First(iClass => iClass.Id == studentInfo.ClassId);

            target.TaskUnitId = taskUnitId;
            target.TargetName = studentInfo.Name;
            target.DesignCode = DesignCode.Task(schoolInfo.OrganizationCode!, index, taskUnitInfo.UnitCode);
            target.TargetCode = target.DesignCode;
            target.TargetType = "VisionScreening";
            target.BindingTag = studentInfo.Id.GuidToString();
            target.TargetState = TaskState.Created;
            target.Description = $"{schoolInfo.Name}{studentClass.Name}{studentInfo.Name}";

            targets.Add(target);

            index++;
        }

        var targetResult = await TaskUnitTargetStore.CreateAsync(targets, context.CancellationToken);

        var standard = await StandardCatalogStore.EntityQuery
            .ProjectToType<StandardCatalogInfo>()
            .FirstOrDefaultAsync(context.CancellationToken);

        var records = new List<ArtemisVisionScreenRecord>();

        foreach (var target in targets)
        {
            var record = Instance.CreateInstance<ArtemisVisionScreenRecord>();

            if (target.BindingTag != null)
            {
                var targetStudentId = Guid.Parse(target.BindingTag);
                var studentInfo = studentInfos.First(item => item.Id == targetStudentId);

                // taskInfo
                record.TaskId = taskInfo.Id;
                record.TaskName = taskInfo.TaskName;
                record.TaskCode = taskInfo.TaskCode;

                // taskUnitInfo
                record.TaskUnitId = taskUnitInfo.Id;
                record.TaskUnitName = taskUnitInfo.UnitName;
                record.TaskUnitCode = taskUnitInfo.UnitCode;

                // taskUnitTarget
                record.TaskUnitTargetId = target.Id;
                record.TaskUnitTargetCode = target.TargetCode;

                // standard
                record.VisualStandardId = standard?.Id ?? Guid.Empty;

                // school
                record.SchoolId = schoolInfo.Id;
                record.SchoolName = schoolInfo.Name;
                record.SchoolCode = schoolInfo.Code;
                record.SchoolType = schoolInfo.Type;

                // division
                record.DivisionId = divisionInfo.Id;
                record.DivisionName = divisionInfo.Name;
                record.DivisionCode = divisionInfo.Code;

                // organization
                record.OrganizationId = organizationInfo.Id;
                record.OrganizationName = organizationInfo.Name;
                record.OrganizationCode = organizationInfo.Code;
                record.OrganizationDesignCode = organizationInfo.DesignCode;


                var classInfo = classInfos.First(item => item.Id == studentInfo.ClassId);
                // class
                record.ClassId = classInfo.Id;
                record.ClassName = classInfo.Name;
                record.ClassCode = classInfo.Code;
                record.GradeName = classInfo.GradeName;
                record.ClassSerialNumber = classInfo.SerialNumber;
                record.StudyPhase = classInfo.StudyPhase;
                record.SchoolLength = classInfo.SchoolLength;
                record.SchoolLengthValue = classInfo.Length;
                record.HeadTeacherId = classInfo.HeadTeacherId;
                record.HeadTeacherName = classInfo.HeadTeacherName;

                // student
                record.StudentId = studentInfo.Id;
                record.StudentName = studentInfo.Name;
                record.StudentNumber = studentInfo.StudentNumber;
                record.Nation = studentInfo.Nation;
                record.StudentCode = studentInfo.Code;
                record.Birthday = studentInfo.Birthday;
                if (studentInfo.Birthday != null)
                {
                    var age = DateTime.Today.Year - studentInfo.Birthday.Value.Year;
                    if (DateTime.Today.Month < studentInfo.Birthday.Value.Month || (DateTime.Today.Month == studentInfo.Birthday.Value.Month && DateTime.Today.Day < studentInfo.Birthday.Value.Day))
                    {
                        age--;
                    }

                    record.Age = age;
                }
                record.Gender = studentInfo.Gender;

                // finish
                records.Add(record);
            }
        }

        var recordResult = await VisionScreenRecordStore.CreateAsync(records);

        if (recordResult.Succeeded)
        {
            taskUnitInfo.TaskUnitState = TaskState.Waiting;

            await TaskUnitStore.UpdateAsync(taskUnitInfo, context.CancellationToken);
        }

        var affectRows = targetResult.AffectRows + recordResult.AffectRows;

        var result = StoreResult.Failed();

        if (affectRows > 0)
        {
            result = StoreResult.Success(affectRows);
        }

        return result.AffectedResponse();
    }

    /// <summary>
    /// 查询根任务
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查询根任务")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<FetchRootTaskResponse> FetchRootTask(FetchRootTaskRequest request, ServerCallContext context)
    {
        var taskNameSearch = request.TaskNameSearch ?? string.Empty;
        var startSet = DateTime.TryParse(request.StartTime, out var startTime);
        var endSet = DateTime.TryParse(request.EndTime, out var endTime);
        var taskState = request.TaskState ?? string.Empty;
        var page = request.Page ?? 0;
        var size = request.Size ?? 0;

        var query = TaskStore.EntityQuery.Where(task => task.ParentId == null);

        var total = await query.LongCountAsync(context.CancellationToken);

        var normalizedTaskName = taskNameSearch.StringNormalize();

        query = query.WhereIf(
            normalizedTaskName != string.Empty,
            task => EF.Functions.Like(task.NormalizedTaskName, $"%{normalizedTaskName}%"));

        query = query.WhereIf(taskState != string.Empty, task => task.TaskState == taskState);

        query = query.WhereIf(startSet, task => task.CreatedAt >= startTime);

        query = query.WhereIf(endSet, task => task.CreatedAt <= endTime);

        var count = await query.LongCountAsync(context.CancellationToken);

        query = query.OrderByDescending(task => task.CreatedAt);

        if (page > 0 && size > 0) query = query.Page(page, size);
        
        var tasks = await query.ProjectToType<TaskInfo>().ToListAsync(context.CancellationToken);

        var infos = new PageResult<TaskInfo>
        {
            Total = total,
            Count = count,
            Page = page,
            Size = size,
            Items = tasks
        };

        return infos.PagedResponse<FetchRootTaskResponse, TaskInfo>();
    }

    /// <summary>
    /// 查询任务下级节点
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查询任务下级节点")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<FetchTaskSubNodeResponse> FetchTaskSubNode(FetchTaskSubNodeRequest request, ServerCallContext context)
    {
        var taskId = Guid.Parse(request.Id);

        var taskExists = await TaskStore.ExistsAsync(taskId, context.CancellationToken);

        if (taskExists)
        {
            var subTaskInfos = await TaskStore.EntityQuery
                .Where(item => item.ParentId == taskId)
                .ProjectToType<TaskInfo>()
                .ToListAsync(context.CancellationToken);

            var taskUnitInfos = await TaskUnitStore.EntityQuery
                .Where(item => item.TaskId == taskId)
                .ProjectToType<TaskUnitInfo>()
                .ToListAsync(context.CancellationToken);

            var taskSubNodePacket = new TaskSubNodePacket();
            taskSubNodePacket.Children.Add(subTaskInfos.Adapt<IEnumerable<TaskPacket>>());
            taskSubNodePacket.TaskUnits.Add(taskUnitInfos.Adapt<IEnumerable<TaskUnitPacket>>());

            return taskSubNodePacket.ReadInfoResponse<FetchTaskSubNodeResponse, TaskSubNodePacket>();
        }

        return ResultAdapter.AdaptEmptyFail<FetchTaskSubNodeResponse>("任务不存在");
    }

    /// <summary>
    /// 查询任务单元目标
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查询任务单元目标")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<FetchTaskUnitTargetResponse> FetchTaskUnitTarget(FetchTaskUnitTargetRequest request, ServerCallContext context)
    {
        var taskUnitId = Guid.Parse(request.Id);

        var taskUnitExists = await TaskUnitStore.ExistsAsync(taskUnitId, context.CancellationToken);

        if (taskUnitExists)
        {
            var taskTargets = await TaskUnitTargetStore.EntityQuery
                .Where(item => item.TaskUnitId == taskUnitId)
                .ProjectToType<TaskUnitTargetInfo>()
                .ToListAsync(context.CancellationToken);

            return taskTargets.ReadInfoResponse<FetchTaskUnitTargetResponse, List<TaskUnitTargetInfo>>();
        }

        return ResultAdapter.AdaptEmptyFail<FetchTaskUnitTargetResponse>("任务单元不存在");
    }

    /// <summary>
    /// 添加验光仪数据
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("添加验光仪数据")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> AddOptometerPacket(AddOptometerRequest request, ServerCallContext context)
    {
        var recordId = Guid.Parse(request.RecordId);

        var record = await VisionScreenRecordStore
            .KeyMatchQuery(recordId)
            .FirstOrDefaultAsync(context.CancellationToken);

        var response = new AffectedResponse();

        if (record is not null)
        {
            request.OptometerPacket.Adapt(record);
            record.OptometerCheckedTimes += 1;
            record.IsOptometerChecked = true;
            record.CheckTime = DateTime.Now;
            // 计算等效球镜
            record.LeftEquivalentSphere = (int?)(record.LeftSphere * -100);
            record.RightEquivalentSphere = (int?)(record.RightSphere * -100);
            var recordResult = await VisionScreenRecordStore.UpdateAsync(record, context.CancellationToken);

            var optometer = Instance.CreateInstance<ArtemisOptometer>();
            request.OptometerPacket.Adapt(optometer);
            optometer.RecordId = recordId;
            // 计算等效球镜
            optometer.LeftEquivalentSphere = (int?)(optometer.LeftSphere * -100);
            optometer.RightEquivalentSphere = (int?)(optometer.RightSphere * -100);

            var optometerResult = await OptometerStore.CreateAsync(optometer, context.CancellationToken);

            var taskTarget = await TaskUnitTargetStore.FindEntityAsync(record.TaskUnitTargetId, context.CancellationToken);
            if (taskTarget != null)
            {
                taskTarget.TargetState = TaskState.Completed;
                taskTarget.ExecuteTime = optometer.OptometerOperationTime;

                await TaskUnitTargetStore.UpdateAsync(taskTarget, context.CancellationToken);
            }

            response.Data = recordResult.AffectRows + optometerResult.AffectRows;

            return response;

        }

        return ResultAdapter.AdaptEmptyFail<AffectedResponse>("筛查记录不存在");
    }

    /// <summary>
    /// 添加电子视力表数据
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("添加电子视力表数据")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> AddVisualChartPacket(AddVisualChartRequest request, ServerCallContext context)
    {
        var recordId = Guid.Parse(request.RecordId);

        var record = await VisionScreenRecordStore
            .KeyMatchQuery(recordId)
            .FirstOrDefaultAsync(context.CancellationToken);

        var response = new AffectedResponse();

        if (record is not null)
        {
            request.VisualChartPacket.Adapt(record);
            record.ChartCheckedTimes += 1;
            record.IsChartChecked = true;
            record.CheckTime = DateTime.Now;
            var recordResult = await VisionScreenRecordStore.UpdateAsync(record, context.CancellationToken);

            var visualChart = Instance.CreateInstance<ArtemisVisualChart>();
            request.VisualChartPacket.Adapt(visualChart);
            visualChart.RecordId = recordId;

            var visualChartResult = await VisualChartStore.CreateAsync(visualChart, context.CancellationToken);

            var taskTarget = await TaskUnitTargetStore.FindEntityAsync(record.TaskUnitTargetId, context.CancellationToken);
            if (taskTarget != null)
            {
                taskTarget.TargetState = TaskState.Completed;
                taskTarget.ExecuteTime = visualChart.ChartOperationTime;

                await TaskUnitTargetStore.UpdateAsync(taskTarget, context.CancellationToken);
            }

            response.Data = recordResult.AffectRows + visualChartResult.AffectRows;

            return response;

        }

        return ResultAdapter.AdaptEmptyFail<AffectedResponse>("筛查记录不存在");
    }

    /// <summary>
    /// 获取大屏学生视力档案
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("获取大屏学生视力档案")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<GetLargeScreenRecordResponse> GetLargeScreenRecord(GetLargeScreenRecordRequest request, ServerCallContext context)
    {
        var taskId = Guid.Parse(request.TaskId);
        var studentId = Guid.Parse(request.StudentId);

        var record = await VisionScreenRecordStore
            .EntityQuery
            .Where(item => item.TaskId == taskId && item.StudentId == studentId)
            .Where(item => item.CheckTime != null)
            .OrderByDescending(item => item.CheckTime)
            .ProjectToType<LargeScreenRecordPacket>()
            .FirstOrDefaultAsync(context.CancellationToken);

        if (record is not null)
        {
            var studentBinding = await StudentRelationBindingStore
                .EntityQuery
                .Where(binding => binding.StudentId == studentId)
                .ProjectToType<StudentRelationBindingInfo>()
                .FirstOrDefaultAsync(context.CancellationToken);

            record.ParentRelation = studentBinding?.Relation;
            record.Address = "";
            record.ParentMobile = "";
            record.DoctorAdvice = "";

            return ResultAdapter.AdaptSuccess<GetLargeScreenRecordResponse, LargeScreenRecordPacket>(record);
        }

        return ResultAdapter.AdaptEmptyFail<GetLargeScreenRecordResponse>("未找到视力档案");
        
    }


    /// <summary>
    /// 获取通知消息(多端通用)
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("获取通知消息(多端通用)")]
    [Authorize(AuthorizePolicy.Anonymous)]
    public override async Task<FetchUserMessageResponse> FetchUserMessage(EmptyRequest request, ServerCallContext context)
    {
        var (valid, userId) = context.GetHttpContext().GetUserId();

        if (!valid)
        {
            return ResultAdapter.AdaptEmptyFail<FetchUserMessageResponse>("解析凭据中的用户标识失败");
        }

        var userExists = await UserStore.ExistsAsync(userId, context.CancellationToken);

        if (!userExists)
        {
            return ResultAdapter.AdaptEmptyFail<FetchUserMessageResponse>("用户不存在");
        }

        var endType = context.GetHttpContext().GetEndType();

        var messageInfos = await NotificationMessageStore
            .EntityQuery
            .Where(item => item.UserId == userId && item.EndType == endType)
            .ProjectToType<NotificationMessagePacket>()
            .ToListAsync(context.CancellationToken);

        var notRead = messageInfos.Count(item => item.IsRead == false);

        var result = new FetchUserMessagePacket
        {
            NotReadCount = notRead
        };
        result.Messages.Add(messageInfos);

        return result.ReadInfoResponse<FetchUserMessageResponse, FetchUserMessagePacket>();
    }

    /// <summary>
    /// 读取通知消息(多端通用)
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("读取通知消息(多端通用)")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<ReadUserMessageResponse> ReadUserMessage(ReadUserMessageRequest request, ServerCallContext context)
    {
        var messageId = Guid.Parse(request.MessageId);

        var message = await NotificationMessageStore
            .KeyMatchQuery(messageId)
            .FirstOrDefaultAsync(context.CancellationToken);

        if (message != null)
        {
            message.IsRead = true;
            message.ReadTime = DateTime.Now;

            await NotificationMessageStore.UpdateAsync(message, context.CancellationToken);

            var packet = message.Adapt<NotificationMessagePacket>();

            return packet.ReadInfoResponse<ReadUserMessageResponse, NotificationMessagePacket>();

        }

        return ResultAdapter.AdaptEmptyFail<ReadUserMessageResponse>("消息不存在");
    }

    /// <summary>
    /// 获取系统模块树
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("获取系统模块树")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<FetchSystemModuleTreeResponse> FetchSystemModuleTree(EmptyRequest request, ServerCallContext context)
    {
        var systemModuleInfos = await SystemModuleStore
            .EntityQuery
            .ProjectToType<SystemModuleTreePacket>()
            .ToListAsync(context.CancellationToken);

        var roots = systemModuleInfos.Where(item => item.ParentId == null).ToList();

        var trees = new List<SystemModuleTreePacket>();

        foreach (var root in roots)
        {
            var tree = GenerateTree(root.Id, systemModuleInfos);

            trees.Add(tree);
        }

        return trees.ReadInfoResponse<FetchSystemModuleTreeResponse, List<SystemModuleTreePacket>>();
    }

    /// <summary>
    /// 获取系统模块树(经凭据过滤)
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("获取系统模块树")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<FetchSystemModuleTreeResponse> FetchClaimedSystemModuleTree(EmptyRequest request, ServerCallContext context)
    {
        var systemModuleInfos = await SystemModuleStore
            .EntityQuery
            .ProjectToType<SystemModuleTreePacket>()
            .ToListAsync(context.CancellationToken);

        var claims = context.GetHttpContext().GetClaims();

        // todo claimed

        var roots = systemModuleInfos.Where(item => item.ParentId == null).ToList();

        var trees = new List<SystemModuleTreePacket>();

        foreach (var root in roots)
        {
            var tree = GenerateTree(root.Id, systemModuleInfos);

            trees.Add(tree);
        }

        return trees.ReadInfoResponse<FetchSystemModuleTreeResponse, List<SystemModuleTreePacket>>();
    }

    /// <summary>
    /// 搜索视力档案
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("搜索视力档案")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<SearchRecordInfoResponse> SearchRecordInfo(SearchRecordInfoRequest request, ServerCallContext context)
    {
        Guid? taskId = string.IsNullOrWhiteSpace(request.TaskId) ? null : Guid.Parse(request.TaskId);

        Guid? schoolId = string.IsNullOrWhiteSpace(request.SchoolId) ? null : Guid.Parse(request.SchoolId);

        Guid? classId = string.IsNullOrWhiteSpace(request.ClassId) ? null : Guid.Parse(request.ClassId);

        var studentName = request.StudentName ?? string.Empty;

        var gender = request.Gender ?? string.Empty;

        var isWareOkLenses = request.IsWareOkLenses;

        var nakedEyeVisionUpperLimit = request.NakedEyeVisionUpperLimit;

        var nakedEyeVisionLowerLimit = request.NakedEyeVisionLowerLimit;

        var correctedVisionUpperLimit = request.CorrectedVisionUpperLimit;

        var correctedVisionLowerLimit = request.CorrectedVisionLowerLimit;

        var sphereUpperLimit = request.SphereUpperLimit;

        var sphereLowerLimit = request.SphereLowerLimit;

        var cylinderUpperLimit = request.CylinderUpperLimit;

        var cylinderLowerLimit = request.CylinderLowerLimit;

        var axisUpperLimit = request.AxisUpperLimit;

        var axisLowerLimit = request.AxisLowerLimit;

        var pupilDistanceUpperLimit = request.PupilDistanceUpperLimit;

        var pupilDistanceLowerLimit = request.PupilDistanceLowerLimit;

        DateTime? birthdayStartTime = string.IsNullOrWhiteSpace(request.BirthdayStartTime) ? null : DateTime.Parse(request.BirthdayStartTime);

        DateTime? birthdayEndTime = string.IsNullOrWhiteSpace(request.BirthdayEndTime) ? null : DateTime.Parse(request.BirthdayEndTime);

        DateTime? checkDateStartTime = string.IsNullOrWhiteSpace(request.CheckDateStartTime) ? null : DateTime.Parse(request.CheckDateStartTime);

        DateTime? checkDateEndTime = string.IsNullOrWhiteSpace(request.CheckDateEndTime) ? null : DateTime.Parse(request.CheckDateEndTime);

        var page = request.Page ?? 0;

        var size = request.Size ?? 0;

        var query = VisionScreenRecordStore.EntityQuery;

        var total = await query.LongCountAsync(context.CancellationToken);

        query = query.WhereIf(taskId != null, record => record.TaskId == taskId);

        query = query.WhereIf(schoolId != null, record => record.SchoolId == schoolId);

        query = query.WhereIf(classId != null, record => record.ClassId == classId);

        query = query.WhereIf(!string.IsNullOrEmpty(studentName),
            record => EF.Functions.Like(record.StudentName, $"%{studentName}%"));

        query = query.WhereIf(!string.IsNullOrEmpty(gender), record => record.Gender == gender);

        query = query.WhereIf(isWareOkLenses != null,
            record => record.IsWareLeftOkLenses == isWareOkLenses || record.IsWareRightOkLenses == isWareOkLenses);

        query = query.WhereIf(nakedEyeVisionUpperLimit != null && nakedEyeVisionLowerLimit != null,
            record => (record.LeftNakedEyeVision < nakedEyeVisionUpperLimit && 
                      record.LeftNakedEyeVision >= nakedEyeVisionLowerLimit) || 
                      (record.RightNakedEyeVision < nakedEyeVisionUpperLimit &&
                       record.RightNakedEyeVision >= nakedEyeVisionLowerLimit));

        query = query.WhereIf(correctedVisionUpperLimit != null && correctedVisionLowerLimit != null,
            record => (record.LeftCorrectedVision < correctedVisionUpperLimit &&
                       record.LeftCorrectedVision >= correctedVisionLowerLimit) ||
                      (record.RightCorrectedVision < correctedVisionUpperLimit &&
                       record.RightCorrectedVision >= correctedVisionLowerLimit));

        query = query.WhereIf(sphereUpperLimit != null && sphereLowerLimit != null,
            record => (record.LeftSphere < sphereUpperLimit &&
                       record.LeftSphere >= sphereLowerLimit) ||
                      (record.RightSphere < sphereUpperLimit &&
                       record.RightSphere >= sphereLowerLimit));

        query = query.WhereIf(cylinderUpperLimit != null && cylinderLowerLimit != null,
            record => (record.LeftCylinder < cylinderUpperLimit &&
                       record.LeftCylinder >= cylinderLowerLimit) ||
                      (record.RightCylinder < cylinderUpperLimit &&
                       record.RightCylinder >= cylinderLowerLimit));

        query = query.WhereIf(axisUpperLimit != null && axisLowerLimit != null,
            record => (record.LeftAxis < axisUpperLimit &&
                       record.LeftAxis >= axisLowerLimit) ||
                      (record.RightAxis < axisUpperLimit &&
                       record.RightAxis >= axisLowerLimit));

        query = query.WhereIf(pupilDistanceUpperLimit != null, record => record.PupilDistance < pupilDistanceUpperLimit);

        query = query.WhereIf(pupilDistanceLowerLimit != null, record => record.PupilDistance >= pupilDistanceLowerLimit);

        query = query.WhereIf(birthdayStartTime != null, record => record.Birthday >= birthdayStartTime);

        query = query.WhereIf(birthdayEndTime != null, record => record.Birthday <= birthdayEndTime);

        query = query.WhereIf(checkDateStartTime != null, record => record.CheckTime >= checkDateStartTime);

        query = query.WhereIf(checkDateEndTime != null, record => record.CheckTime <= checkDateEndTime);

        var count = await query.LongCountAsync(context.CancellationToken);

        query = query.OrderBy(task => task.TaskUnitTargetCode);

        if (page > 0 && size > 0) query = query.Page(page, size);
        var tasks = await query
        .ProjectToType<RecordInfoPacket>()
        .ToListAsync(context.CancellationToken);

        var result = new PageResult<RecordInfoPacket>
        {
            Page = page,
            Size = size,
            Count = count,
            Total = total,
            Items = tasks
        };

        return result.PagedResponse<SearchRecordInfoResponse, RecordInfoPacket>();
    }

    /// <summary>
    /// 查询仪表盘人数分布
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查询仪表盘人数分布")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<DashboardDataResponse> DashboardData(DashboardDataRequerst request, ServerCallContext context)
    {
        var taskId = Guid.Parse(request.TaskId);

        var taskExists = await TaskStore.ExistsAsync(taskId, context.CancellationToken);

        if (!taskExists)
        {
            return ResultAdapter.AdaptEmptyFail<DashboardDataResponse>("任务不存在");
        }

        Guid? schoolId = string.IsNullOrWhiteSpace(request.SchoolId) ? null : Guid.Parse(request.SchoolId);

        Guid? standardId = string.IsNullOrWhiteSpace(request.StandardId) ? null : Guid.Parse(request.StandardId);

        if (standardId == null)
        {
            standardId = await StandardCatalogStore.EntityQuery
                .Where(item => item.Code == "ST001")
                .Select(item => item.Id)
                .FirstOrDefaultAsync(context.CancellationToken);
        }

        var standardItems = await StandardItemStore.EntityQuery
            .Where(item => item.StandardCatalogId == standardId)
            .Select(item => new
            {
                item.Name,
                item.Code,
                item.Minimum,
                item.Maximum
            })
            .ToListAsync(context.CancellationToken);

        if (!standardItems.Any())
        {
            return ResultAdapter.AdaptEmptyFail<DashboardDataResponse>("标准不存在");
        }

        var gradeName = request.GradeName ?? string.Empty;

        var baseQuery = VisionScreenRecordStore.EntityQuery
            .Where(record => record.TaskId == taskId)
            .WhereIf(schoolId != null, record => record.SchoolId == schoolId)
            .WhereIf(!string.IsNullOrWhiteSpace(gradeName), record => record.GradeName == gradeName);

        var total = await baseQuery.CountAsync(context.CancellationToken);

        // 筛查分布：筛查，只过验光仪，只过视力表，都没过

        var checkedCount = await baseQuery
            .Where(record => record.IsOptometerChecked && record.IsChartChecked)
            .CountAsync(context.CancellationToken);

        var onlyOptoimeterCheckedCount = await baseQuery
            .Where(record => record.IsOptometerChecked && !record.IsChartChecked)
            .CountAsync(context.CancellationToken);

        var onlyChartCheckedCount = await baseQuery
            .Where(record => !record.IsOptometerChecked && record.IsChartChecked)
            .CountAsync(context.CancellationToken);

        var notCheckedCount = await baseQuery
            .Where(record => !record.IsOptometerChecked && !record.IsChartChecked)
            .CountAsync(context.CancellationToken);

        var dashboardPopulationDistributionPacket = new DashboardPopulationDistributionPacket
        {
            Total = total,
            Checked = checkedCount,
            OnlyOptoimeterChecked = onlyOptoimeterCheckedCount,
            OnlyChartChecked = onlyChartCheckedCount,
            NotChecked = notCheckedCount
        };

        // 增长统计
        // todo
        var dashboardIncreasedStatisticsPacket = new DashboardIncreasedStatisticsPacket();

        // 标准分布
        var dashboardStandardDistributionPacket = new DashboardStandardDistributionPacket();

        var standardDistributionQuery = baseQuery
            .Where(record => record.IsOptometerChecked)
            .Select(record => Math.Max(record.LeftEquivalentSphere ?? 0, record.RightEquivalentSphere ?? 0));

        foreach (var standardItem in standardItems)
        {
            var standardCount = await standardDistributionQuery
                .Where(item => item >= standardItem.Minimum)
                .Where(item => item < standardItem.Maximum)
                .CountAsync(context.CancellationToken);

            var item = new DashboardStandardDistributionItemPacket
            {
                ItemName = standardItem.Name,
                ItemCount = standardCount,
                ItemRate = Math.Round(Convert.ToDouble(standardCount) / Convert.ToDouble(total), 3)
            };
            dashboardStandardDistributionPacket.Items.Add(item);
        }

        dashboardStandardDistributionPacket.Items.Add(new DashboardStandardDistributionItemPacket
        {
            ItemName = "验光仪数据缺失",
            ItemCount = onlyChartCheckedCount,
            ItemRate = Math.Round(Convert.ToDouble(onlyChartCheckedCount) / Convert.ToDouble(total), 3)
        });

        dashboardStandardDistributionPacket.Items.Add(new DashboardStandardDistributionItemPacket
        {
            ItemName = "未筛查",
            ItemCount = notCheckedCount,
            ItemRate = Math.Round(Convert.ToDouble(notCheckedCount) / Convert.ToDouble(total), 3)
        });

        // 未筛查原因分布
        var exceptionReasonQuery = baseQuery
            .Where(record => !record.IsOptometerChecked && !record.IsChartChecked)
            .Select(record => string.IsNullOrWhiteSpace(record.ExceptionReason) ? "原因不明" : record.ExceptionReason);

        var exceptionReasonItems = await exceptionReasonQuery
            .GroupBy(item => item)
            .Select(group => new DashBoardExceptionReasonItemPacket
            {
                ItemName = group.Key,
                ItemCount = group.Count()
            })
            .ToListAsync(context.CancellationToken);

        var dashBoardExceptionReasonDistributionPacket = new DashBoardExceptionReasonDistributionPacket();

        dashBoardExceptionReasonDistributionPacket.Items.Add(exceptionReasonItems);

        //班级年级分布
        var flag = standardItems.First(item => item.Code == "Normal").Maximum;

        var gradeOrClassTotalDistributionQuery = baseQuery.Select(record => new
        {
            record.SchoolName,
            record.GradeName,
            record.ClassName,
            Flag = Math.Max(record.LeftEquivalentSphere ?? 0, record.RightEquivalentSphere ?? 0)
        });

        var gradeOrClassCountDistributionQuery = gradeOrClassTotalDistributionQuery
            .Where(item => item.Flag >= flag);

        var dashboardGradeOrClassDistributionPacket = new DashboardGradeOrClassDistributionPacket();

        if (string.IsNullOrWhiteSpace(request.GradeName))
        {
            // 按年级
            var totalGradeGroup = await gradeOrClassTotalDistributionQuery
                .GroupBy(item => new { item.SchoolName, item.GradeName })
                .Select(group => new
                {
                    group.Key.SchoolName,
                    GradeName = Enumeration.FromName<GradeName>(group.Key.GradeName!),
                    Total = group.Count()
                }).ToListAsync(context.CancellationToken);

            var totalList = totalGradeGroup.Select(item => new
            {
                Name = $"{item.SchoolName}{Enumeration.TryGetDescription<GradeName>(item.GradeName)}",
                item.Total
            });

            var countGradeGroup = await gradeOrClassCountDistributionQuery
                .GroupBy(item => new { item.SchoolName, item.GradeName })
                .Select(group => new
                {
                    group.Key.SchoolName,
                    GradeName = Enumeration.FromName<GradeName>(group.Key.GradeName!),
                    Count = group.Count()
                }).ToListAsync(context.CancellationToken);

            var countList = countGradeGroup.Select(item => new
            {
                Name = $"{item.SchoolName}{Enumeration.TryGetDescription<GradeName>(item.GradeName)}",
                item.Count
            });

            var list = totalList.Join(countList, t => t.Name, c => c.Name,
                (t, c) => new DashboardGradeOrClassDistributionItemPacket
                {
                    ItemName = t.Name,
                    ItemTotal = t.Total,
                    ItemCount = c.Count
                }).ToList();

            dashboardGradeOrClassDistributionPacket.Items.Add(list);
        }
        else
        {
            // 按班级
            var classTotalDistributionQuery = gradeOrClassCountDistributionQuery
                    .Where(item => item.GradeName == request.GradeName);

            var classCountDistributionQuery = gradeOrClassCountDistributionQuery
                .Where(item => item.GradeName == request.GradeName);

            var totalClassGroup = await classTotalDistributionQuery
                .GroupBy(item => item.ClassName)
                .Select(group => new
                {
                    group.Key,
                    Total = group.Count()
                }).ToListAsync(context.CancellationToken);

            var countClassGroup = await classCountDistributionQuery
                .GroupBy(item => item.ClassName)
                .Select(group => new
                {
                    group.Key,
                    Count = group.Count()
                }).ToListAsync(context.CancellationToken);

            var list = totalClassGroup.Join(countClassGroup, t => t.Key, c => c.Key,
                (t, c) => new DashboardGradeOrClassDistributionItemPacket
                {
                    ItemName = t.Key,
                    ItemTotal = t.Total,
                    ItemCount = c.Count
                }).ToList();

            dashboardGradeOrClassDistributionPacket.Items.Add(list);
        }

        var packet = new DashboardDataPacket
        {
            PopulationDistribution = dashboardPopulationDistributionPacket,
            IncreasedStatistic = dashboardIncreasedStatisticsPacket,
            StandardDistribution = dashboardStandardDistributionPacket,
            ExceptionReasonDistribution = dashBoardExceptionReasonDistributionPacket,
            GradeOrClassDistribution = dashboardGradeOrClassDistributionPacket
        };

        return packet.ReadInfoResponse<DashboardDataResponse, DashboardDataPacket>();
    }

    /// <summary>
    /// 请求批量PDF导出示例
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="responseStream">Used for sending responses back to the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>A task indicating completion of the handler.</returns>
    public override async System.Threading.Tasks.Task MultiplePdfExportExample(ExportRequest request, IServerStreamWriter<ExportResponse> responseStream, ServerCallContext context)
    {

        var total = request.ExportCount;

        for (var index = 1; index <= total; index++)
        {
            var complete = total - index == 0;

            string? url = complete ? "下载地址" : null;

            var packet = new ExportPacket
            {
                Total = total,
                Count = index,
                Completet = complete,
                Url = url
            };

            var response = packet.ReadInfoResponse<ExportResponse, ExportPacket>();

            await responseStream.WriteAsync(response);
            await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1));
        }

    }

    /// <summary>
    ///     递归生成树
    /// </summary>
    /// <param name="key">根标识</param>
    /// <param name="nodeList">节点列表</param>
    /// <returns></returns>
    private SystemModuleTreePacket GenerateTree(string key, List<SystemModuleTreePacket> nodeList)
    {
        var tree = nodeList.First(item => item.Id == key); 

        var children = nodeList
            .Where(item => item.ParentId != null && item.ParentId == tree.Id)
            .Select(item => GenerateTree(item.Id, nodeList))
            .ToList();

        tree.Children.Add(children);

        return tree;
    }

    /// <summary>
    /// 记录字段查询
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    private IQueryable<ArtemisVisionScreenRecord> RecordFieldQuery(QueryRecordFieldRequest request)
    {
        Guid? taskId = string.IsNullOrWhiteSpace(request.TaskId) ? null : Guid.Parse(request.TaskId);
        Guid? divisionId = string.IsNullOrWhiteSpace(request.DivisionId) ? null : Guid.Parse(request.DivisionId);
        Guid? schoolId = string.IsNullOrWhiteSpace(request.SchoolId) ? null : Guid.Parse(request.SchoolId);
        Guid? classId = string.IsNullOrWhiteSpace(request.ClassId) ? null : Guid.Parse(request.ClassId);
        var gradeName = request.GradeName ?? string.Empty;

        var query = VisionScreenRecordStore.EntityQuery
            .WhereIf(taskId != null, record => record.TaskId == taskId)
            .WhereIf(divisionId != null, record => record.DivisionId == divisionId)
            .WhereIf(schoolId != null, record => record.SchoolId == schoolId)
            .WhereIf(classId != null, record => record.ClassId == classId)
            .WhereIf(gradeName != string.Empty, record => record.GradeName == gradeName);

        return query;
    }

    #endregion
}