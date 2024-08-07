using Artemis.Data.Core;

namespace Artemis.Service.Shared.Business.VisionScreen;

/// <summary>
///     视力筛查记录接口
/// </summary>
public interface IVisionScreenRecord : IVisionScreenRecordInfo
{
}

/// <summary>
///     视力筛查记录信息接口
/// </summary>
public interface IVisionScreenRecordInfo : IVisionScreenRecordPackage, IKeySlot
{
}

/// <summary>
///     视力筛查记录数据包接口
/// </summary>
public interface IVisionScreenRecordPackage : IVisualChartPackage, IOptometerPackage
{
    /// <summary>
    ///     任务标识
    /// </summary>
    Guid TaskId { get; set; }

    /// <summary>
    ///     任务名称
    /// </summary>
    string? TaskName { get; set; }

    /// <summary>
    ///     任务编码
    /// </summary>
    string? TaskCode { get; set; }

    /// <summary>
    ///     任务单元标识
    /// </summary>
    Guid TaskUnitId { get; set; }

    /// <summary>
    ///     任务单元名称
    /// </summary>
    string? TaskUnitName { get; set; }

    /// <summary>
    ///     任务单元编码
    /// </summary>
    string? TaskUnitCode { get; set; }

    /// <summary>
    ///     任务目标标识
    /// </summary>
    Guid TaskUnitTargetId { get; set; }

    /// <summary>
    ///     任务目标编码
    /// </summary>
    string? TaskUnitTargetCode { get; set; }

    /// <summary>
    ///     任务代理标识
    /// </summary>
    Guid? TaskAgentId { get; set; }

    /// <summary>
    ///     任务代理名称
    /// </summary>
    string? TaskAgentName { get; set; }

    /// <summary>
    ///     任务代理编码
    /// </summary>
    string? TaskAgentCode { get; set; }

    /// <summary>
    ///     任务代理类型
    /// </summary>
    string? TaskAgentType { get; set; }

    /// <summary>
    ///     医师姓名
    /// </summary>
    string? DoctorName { get; set; }

    /// <summary>
    ///     应用的视力标准标识
    /// </summary>
    Guid VisualStandardId { get; set; }

    /// <summary>
    ///     医嘱
    /// </summary>
    string? DoctorAdvice { get; set; }

    /// <summary>
    ///     医嘱时间
    /// </summary>
    DateTime? PrescribedTime { get; set; }

    /// <summary>
    ///     学校标识
    /// </summary>
    Guid SchoolId { get; set; }

    /// <summary>
    ///     学校名称
    /// </summary>
    string SchoolName { get; set; }

    /// <summary>
    ///     学校编码
    /// </summary>
    string? SchoolCode { get; set; }

    /// <summary>
    ///     学校类型
    /// </summary>
    string? SchoolType { get; set; }

    /// <summary>
    ///     行政区划标识
    /// </summary>
    Guid DivisionId { get; set; }

    /// <summary>
    ///     行政区划名称
    /// </summary>
    string? DivisionName { get; set; }

    /// <summary>
    ///     行政区划编码
    /// </summary>
    string? DivisionCode { get; set; }

    /// <summary>
    ///     组织机构标识
    /// </summary>
    Guid OrganizationId { get; set; }

    /// <summary>
    ///     组织机构名称
    /// </summary>
    string OrganizationName { get; set; }

    /// <summary>
    ///     组织机构编码
    /// </summary>
    string? OrganizationCode { get; set; }

    /// <summary>
    ///     组织机构设计编码
    /// </summary>
    string OrganizationDesignCode { get; set; }

    /// <summary>
    ///     班级标识
    /// </summary>
    Guid? ClassId { get; set; }

    /// <summary>
    ///     班级名称
    /// </summary>
    string? ClassName { get; set; }

    /// <summary>
    ///     班级编码
    /// </summary>
    string? ClassCode { get; set; }

    /// <summary>
    ///     年级名称
    /// </summary>
    string? GradeName { get; set; }

    /// <summary>
    ///     班级序列号
    /// </summary>
    int? ClassSerialNumber { get; set; }

    /// <summary>
    ///     学段
    /// </summary>
    string? StudyPhase { get; set; }

    /// <summary>
    ///     学制
    /// </summary>
    string? SchoolLength { get; set; }

    /// <summary>
    ///    学制值
    /// </summary>
    int? SchoolLengthValue { get; set; }

    /// <summary>
    ///    班主任标识
    /// </summary>
    Guid? HeadTeacherId { get; set; }

    /// <summary>
    /// 班主任名称
    /// </summary>
    string? HeadTeacherName { get; set; }

    /// <summary>
    ///     学生标识
    /// </summary>
    Guid StudentId { get; set; }

    /// <summary>
    ///     学生名称
    /// </summary>
    string? StudentName { get; set; }

    /// <summary>
    ///     学生编号
    /// </summary>
    string? StudentCode { get; set; }

    /// <summary>
    ///     生日
    /// </summary>
    DateTime? Birthday { get; set; }

    /// <summary>
    ///     年龄
    /// </summary>
    int? Age { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    string? Gender { get; set; }

    /// <summary>
    ///     是否已经过电子视力表筛查
    /// </summary>
    bool IsChartChecked { get; set; }

    /// <summary>
    ///     电子视力表筛查次数
    /// </summary>
    int ChartCheckedTimes { get; set; }

    /// <summary>
    ///     是否已经过验光仪筛查
    /// </summary>
    bool IsOptometerChecked { get; set; }

    /// <summary>
    ///     验光仪筛查次数
    /// </summary>
    int OptometerCheckedTimes { get; set; }

    /// <summary>
    ///     筛查时间
    /// </summary>
    DateTime? CheckTime { get; set; }

    /// <summary>
    ///     筛查结果
    /// </summary>
    string? ExceptionReason { get; set; }

    /// <summary>
    ///     报告发送人
    /// </summary>
    string? ReportSender { get; set; }

    /// <summary>
    ///     报告发送时间
    /// </summary>
    DateTime? ReportSendTime { get; set; }

    /// <summary>
    ///     报告签收人
    /// </summary>
    string? ReportReceiver { get; set; }

    /// <summary>
    ///     报告签收时间
    /// </summary>
    DateTime? ReportReceiveTime { get; set; }
}