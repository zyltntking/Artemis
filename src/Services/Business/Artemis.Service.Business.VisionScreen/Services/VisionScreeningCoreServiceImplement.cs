using System.ComponentModel;
using System.Linq.Dynamic.Core;
using Artemis.Data.Core;
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

    private IIdentityUserStore UserStore { get; set; }

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
    /// 生成任务
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("生成任务")]
    [Authorize(AuthorizePolicy.Token)]
    [Obsolete]
    public override async Task<AffectedResponse> GeneratorTask(GeneratorTaskRequest request, ServerCallContext context)
    {
        var organization = request.OrgnizationTree;

        var startTime = request.StartTime.Adapt<DateTime>();
        var endTime = request.StartTime.Adapt<DateTime>();

        var task = BuildTaskTree(
            request.TaskName, 
            organization.Code, 
            null, 
            1,  
            organization, 
            null, 
            startTime, 
            endTime);

        if (task is not null)
        {
            var result = await TaskStore.CreateAsync(task);

            return result.AffectedResponse();
        }

        return ResultAdapter.AdaptEmptyFail<AffectedResponse>("生成任务失败");
    }

    /// <summary>
    /// 生成任务目标
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("生成任务目标")]
    [Authorize(AuthorizePolicy.Token)]
    [Obsolete]
    public override async Task<AffectedResponse> GenerateTaskTarget(GenerateTaskTargetRequest request, ServerCallContext context)
    {
        var taskId = Guid.Parse(request.TaskId);

        var task = await TaskStore.FindMapEntityAsync<TaskInfo>(taskId, context.CancellationToken);

        if (task is not null && !string.IsNullOrWhiteSpace(task.TaskCode))
        {
            var targetExists = await TaskUnitTargetStore.EntityQuery
                .AnyAsync(target => target.TargetCode!.StartsWith(task.TaskCode), context.CancellationToken);

            if (targetExists)
            {
                return ResultAdapter.AdaptEmptyFail<AffectedResponse>("任务目标已存在, 请清理后再执行生成操作");
            }

            //var taskFeature = task.TaskCode[..26];

            //var rootTask = await TaskStore.EntityQuery
            //    .Where(iTask => iTask.TaskCode!.StartsWith(taskFeature) && iTask.ParentId == null)
            //    .ProjectToType<TaskInfo>()
            //    .FirstOrDefaultAsync(context.CancellationToken);

            var taskUnits = await TaskUnitStore.EntityQuery
                .Where(unit => !string.IsNullOrWhiteSpace(unit.UnitCode) &&
                               unit.UnitCode.StartsWith(task.TaskCode))
                .ProjectToType<TaskUnitInfo>()
                .ToListAsync(context.CancellationToken);

            var schoolOrganizationCodes = taskUnits.Select(unit => unit.DesignCode).ToList();

            var schools = await SchoolStore.EntityQuery
                .Where(school => schoolOrganizationCodes.Contains(school.OrganizationCode))
                .ProjectToType<SchoolInfo>()
                .ToListAsync(context.CancellationToken);

            var schoolIds = schools.Select(school => school.Id).ToList();

            var students = await StudentStore.EntityQuery
                .Where(student => student.SchoolId != null && 
                                  schoolIds.Contains((Guid)student.SchoolId))
                .Where(student => student.ClassId != null)
                .ProjectToType<StudentInfo>()
                .ToListAsync(context.CancellationToken);

            var classIds = students.Select(student => student.ClassId).ToList();

            var classes = await ClassStore.EntityQuery
                .Where(iClass => classIds.Contains(iClass.Id))
                .ProjectToType<ClassInfo>()
                .ToListAsync(context.CancellationToken);

            var targets = new List<ArtemisTaskUnitTarget>();

            var index = 1;

            foreach (var student in students)
            {
                var target = Instance.CreateInstance<ArtemisTaskUnitTarget>();
                var studentSchoolId = student.SchoolId;
                var studentSchool = schools.First(school => school.Id == studentSchoolId);
                var studentTaskUnit = taskUnits.First(unit => unit.DesignCode == studentSchool.OrganizationCode);
                var studentClass = classes.First(iClass => iClass.Id == student.ClassId);

                target.TaskUnitId = studentTaskUnit.Id;
                target.TargetName = student.Name;
                target.DesignCode =
                    DesignCode.Task(studentSchool.OrganizationCode!, index, studentTaskUnit.UnitCode);
                target.TargetCode = target.DesignCode;
                target.TargetType = "VisionScreening";
                target.BindingTag = student.Id.ToString();
                target.TargetState = TaskState.Created;
                targets.Add(target);
                target.Description = $"{studentSchool.Name}{studentClass.Name}{student.Name}";
                index++;
            }

            var result = await TaskUnitTargetStore.CreateAsync(targets);

            return result.AffectedResponse();
        }

        return ResultAdapter.AdaptEmptyFail<AffectedResponse>("任务不存在");
    }

    /// <summary>
    /// 生成筛查记录
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("生成筛查记录")]
    [Authorize(AuthorizePolicy.Token)]
    [Obsolete]
    public override async Task<AffectedResponse> GenerateRecord(GenerateRecordRequest request, ServerCallContext context)
    {
        var taskId = Guid.Parse(request.TaskId);

        var task = await TaskStore.FindMapEntityAsync<TaskInfo>(taskId, context.CancellationToken);

        if (task is not null && !string.IsNullOrWhiteSpace(task.TaskCode))
        {
            var targetExists = await TaskUnitTargetStore.EntityQuery
                .AnyAsync(target => target.TargetCode!.StartsWith(task.TaskCode), context.CancellationToken);

            if (!targetExists)
            {
                return ResultAdapter.AdaptEmptyFail<AffectedResponse>("任务目标不存在, 请先生成任务目标");
            }

            var recordExists = await VisionScreenRecordStore.EntityQuery
                .AnyAsync(record => record.TaskId == taskId, context.CancellationToken);

            if (recordExists)
            {
                return ResultAdapter.AdaptEmptyFail<AffectedResponse>("筛查记录已存在, 请清理后再执行生成操作");
            }

            var taskUnits = await TaskUnitStore.EntityQuery
                .Where(unit => !string.IsNullOrWhiteSpace(unit.UnitCode) &&
                               unit.UnitCode.StartsWith(task.TaskCode))
                .ProjectToType<TaskUnitInfo>()
                .ToListAsync(context.CancellationToken);

            var schoolOrganizationCodes = taskUnits.Select(unit => unit.DesignCode).Distinct().ToList();

            var schools = await SchoolStore.EntityQuery
                .Where(school => schoolOrganizationCodes.Contains(school.OrganizationCode))
                .ProjectToType<SchoolInfo>()
                .ToListAsync(context.CancellationToken);

            var schoolIds = schools.Select(school => school.Id).ToList();

            var students = await StudentStore.EntityQuery
                .Where(student => student.SchoolId != null &&
                                  schoolIds.Contains((Guid)student.SchoolId))
                .Where(student => student.ClassId != null)
                .ProjectToType<StudentInfo>()
                .ToListAsync(context.CancellationToken);

            var classIds = students.Select(student => student.ClassId).Distinct().ToList();

            var classes = await ClassStore.EntityQuery
                .Where(iClass => classIds.Contains(iClass.Id))
                .ProjectToType<ClassInfo>()
                .ToListAsync(context.CancellationToken);

            var targets = await TaskUnitTargetStore.EntityQuery
                .Where(target => !string.IsNullOrWhiteSpace(target.TargetCode) &&
                                 target.TargetCode.StartsWith(task.TaskCode))
                .ProjectToType<TaskUnitTargetInfo>()
                .ToListAsync(context.CancellationToken);

            var standard = await StandardCatalogStore.EntityQuery
                .ProjectToType<StandardCatalogInfo>()
                .FirstOrDefaultAsync(context.CancellationToken);

            var divisionCodes = schools.Select(school => school.DivisionCode).Distinct().ToList();

            var divisions = await DivisionStore.EntityQuery
                .Where(division => divisionCodes.Contains(division.Code))
                .ProjectToType<DivisionInfo>()
                .ToListAsync(context.CancellationToken);

            var organizations = await OrganizationStore.EntityQuery
                .Where(organization => schoolOrganizationCodes.Contains(organization.Code))
                .ProjectToType<OrganizationInfo>()
                .ToListAsync(context.CancellationToken);

            var records = new List<ArtemisVisionScreenRecord>();

            foreach (var target in targets)
            {
                var record = Instance.CreateInstance<ArtemisVisionScreenRecord>();

                if (target.BindingTag != null)
                {
                    var studentId = Guid.Parse(target.BindingTag);
                    var student = students.First(student => student.Id == studentId);

                    // taskInfo
                    record.TaskId = taskId;
                    record.TaskName = task.TaskName;
                    record.TaskCode = task.TaskCode;

                    var taskUnit = taskUnits.First(unit => unit.Id == target.TaskUnitId);
                    // taskUnitInfo
                    record.TaskUnitId = taskUnit.Id;
                    record.TaskUnitName = taskUnit.UnitName;
                    record.TaskUnitCode = taskUnit.UnitCode;

                    // taskUnitTargetInfo
                    record.TaskUnitTargetId = target.Id;
                    record.TaskUnitTargetCode = target.TargetCode;

                    // todo task agent
                    record.TaskAgentId = null;

                    // standard
                    record.VisualStandardId = standard?.Id ?? Guid.Empty;

                    var school = schools.First(school => school.Id == student.SchoolId);
                    // school
                    record.SchoolId = school.Id;
                    record.SchoolName = school.Name;
                    record.SchoolCode = school.Code;
                    record.SchoolType = school.Type;

                    var division = divisions.First(division => division.Code == school.DivisionCode);
                    // division
                    record.DivisionId = division.Id;
                    record.DivisionName = division.Name;
                    record.DivisionCode = division.Code;
                    
                    var organization = organizations.First(organization => organization.Code == school.OrganizationCode);
                    // organization
                    record.OrganizationId = organization.Id;
                    record.OrganizationName = organization.Name;
                    record.OrganizationCode = organization.Code;
                    record.OrganizationDesignCode = organization.DesignCode;

                    var iClass = classes.First(iClass => iClass.Id == student.ClassId);
                    // class
                    record.ClassId = iClass.Id;
                    record.ClassName = iClass.Name;
                    record.ClassCode = iClass.Code;
                    record.GradeName = iClass.GradeName;
                    record.ClassSerialNumber = iClass.SerialNumber;
                    record.StudyPhase = iClass.StudyPhase;
                    record.SchoolLength = iClass.SchoolLength;
                    record.SchoolLengthValue = iClass.Length;
                    record.HeadTeacherId = iClass.HeadTeacherId;
                    record.HeadTeacherName = iClass.HeadTeacherName;

                    // student
                    record.StudentId = student.Id;
                    record.StudentName = student.Name;
                    record.StudentNumber = student.StudentNumber;
                    record.Nation = student.Nation;
                    record.StudentCode = student.Code;
                    record.Birthday = student.Birthday;
                    if (student.Birthday != null)
                    {
                        var age = DateTime.Today.Year - student.Birthday.Value.Year;
                        if (DateTime.Today.Month < student.Birthday.Value.Month || (DateTime.Today.Month == student.Birthday.Value.Month && DateTime.Today.Day < student.Birthday.Value.Day))
                        {
                            age--;
                        }

                        record.Age = age;
                    }
                    record.Gender = student.Gender;

                    // finish
                    records.Add(record);
                }
            }

            var result = await VisionScreenRecordStore.CreateAsync(records);

            return result.AffectedResponse();
        }

        return ResultAdapter.AdaptEmptyFail<AffectedResponse>("任务不存在");
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
    [Authorize(AuthorizePolicy.Token)]
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
    /// 构建任务树
    /// </summary>
    /// <param name="taskName"></param>
    /// <param name="organizationCode"></param>
    /// <param name="parentTaskCode"></param>
    /// <param name="serial"></param>
    /// <param name="organization"></param>
    /// <param name="taskNode"></param>
    /// <param name="startTime"></param>
    /// <param name="endTime"></param>
    /// <returns></returns>
    private ArtemisTask? BuildTaskTree(
        string taskName, 
        string organizationCode, 
        string? parentTaskCode, 
        int serial, 
        TaskBindOrgnizationTreePacket organization, 
        ArtemisTask? taskNode, 
        DateTime startTime, 
        DateTime endTime)
    {
        if (organization.Type == OrganizationType.Management)
        {
            var task = Instance.CreateInstance<ArtemisTask>();
            task.TaskName = taskName;
            task.ParentId = taskNode?.Id;
            task.NormalizedTaskName = taskName.Normalize();
            task.TaskCode = DesignCode.Task(organizationCode, serial, parentTaskCode);
            task.DesignCode = organization.Code;
            task.TaskShip = string.IsNullOrWhiteSpace(parentTaskCode) ? TaskShip.Root : TaskShip.Child;
            task.TaskMode = TaskMode.Normal;
            task.TaskState = TaskState.Created;
            task.Description = organization.Name;
            task.StartTime = startTime;
            task.EndTime = endTime;
            task.Children ??= new List<ArtemisTask>();
            var index = 1;
            foreach (var childOrganization in organization.Children)
            {
                var childTask = BuildTaskTree(
                    taskName, 
                    organizationCode, 
                    task.TaskCode, 
                    index, 
                    childOrganization, 
                    task, 
                    startTime, 
                    endTime);

                if (childTask != null)
                {
                    task.Children.Add(childTask);
                }

                index++;
            }

            return task;
        }

        if (taskNode != null)
        {
            taskNode.TaskUnits ??= new List<ArtemisTaskUnit>();

            var taskUnit = Instance.CreateInstance<ArtemisTaskUnit>();
            taskUnit.TaskId = taskNode.Id;
            taskUnit.UnitName = taskName;
            taskUnit.NormalizedUnitName = taskName.Normalize();
            taskUnit.UnitCode = DesignCode.Task(organizationCode, serial, parentTaskCode);
            taskUnit.DesignCode = organization.Code;
            taskUnit.TaskUnitMode = TaskMode.Normal;
            taskUnit.TaskUnitState = TaskState.Created;
            taskUnit.Description = organization.Name;
            taskUnit.StartTime = startTime;
            taskUnit.EndTime = endTime;
            taskNode.TaskUnits.Add(taskUnit);
        }

        return null;

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

    #endregion
}