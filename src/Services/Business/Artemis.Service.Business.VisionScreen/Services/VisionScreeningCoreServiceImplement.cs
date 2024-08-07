using System.ComponentModel;
using Artemis.Data.Core;
using Artemis.Data.Core.Fundamental.Design;
using Artemis.Data.Core.Fundamental.Types;
using Artemis.Data.Store.Extensions;
using Artemis.Extensions.Identity;
using Artemis.Service.Business.VisionScreen.Context;
using Artemis.Service.Business.VisionScreen.Stores;
using Artemis.Service.Protos;
using Artemis.Service.Protos.Business.VisionScreen;
using Artemis.Service.Resource.Stores;
using Artemis.Service.School.Stores;
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
    public VisionScreeningCoreServiceImplement(
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
        IArtemisVisualChartStore visualChartStore)
    {
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
    }

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

    #region Overrides of VisionScreeningCoreServiceBase

    /// <summary>
    /// 生成任务
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("生成任务")]
    [Authorize(AuthorizePolicy.Token)]
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
                    record.TaskUnitId = target.TaskUnitId;
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
            var recordResult = await VisionScreenRecordStore.UpdateAsync(record, context.CancellationToken);

            var optometer = Instance.CreateInstance<ArtemisOptometer>();
            request.OptometerPacket.Adapt(optometer);
            optometer.RecordId = recordId;

            var optometerResult = await OptometerStore.CreateAsync(optometer, context.CancellationToken);

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

            response.Data = recordResult.AffectRows + visualChartResult.AffectRows;

            return response;

        }

        return ResultAdapter.AdaptEmptyFail<AffectedResponse>("筛查记录不存在");
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

    #endregion
}