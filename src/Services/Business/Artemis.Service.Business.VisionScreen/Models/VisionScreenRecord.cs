using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Service.Shared.Business.VisionScreen;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Business.VisionScreen.Models;

/// <summary>
///     视力筛查记录
/// </summary>
public class VisionScreenRecord : ConcurrencyPartition, IVisionScreenRecord
{
    /// <summary>
    ///     任务标识
    /// </summary>
    [Comment("任务标识")]
    public Guid TaskId { get; set; }

    /// <summary>
    ///     任务名称
    /// </summary>
    [Comment("任务名称")]
    [MaxLength(128)]
    public string? TaskName { get; set; }

    /// <summary>
    ///     任务编码
    /// </summary>
    [Comment("任务编码")]
    [MaxLength(128)]
    public string? TaskCode { get; set; }

    /// <summary>
    ///     任务单元标识
    /// </summary>
    [Comment("任务单元标识")]
    public Guid TaskUnitId { get; set; }

    /// <summary>
    ///     任务单元名称
    /// </summary>
    [Comment("任务单元名称")]
    [MaxLength(128)]
    public string? TaskUnitName { get; set; }

    /// <summary>
    ///     任务单元编码
    /// </summary>
    [Comment("任务单元编码")]
    [MaxLength(128)]
    public string? TaskUnitCode { get; set; }

    /// <summary>
    ///     任务目标标识
    /// </summary>
    [Comment("任务目标标识")]
    public Guid TaskUnitTargetId { get; set; }

    /// <summary>
    ///     任务目标编码
    /// </summary>
    [Comment("任务目标编码")]
    [MaxLength(128)]
    public string? TaskUnitTargetCode { get; set; }

    /// <summary>
    ///     任务代理标识
    /// </summary>
    [Comment("任务代理标识")]
    public Guid? TaskAgentId { get; set; }

    /// <summary>
    ///     任务代理名称
    /// </summary>
    [Comment("任务代理名称")]
    [MaxLength(128)]
    public string? TaskAgentName { get; set; }

    /// <summary>
    ///     任务代理编码
    /// </summary>
    [Comment("任务代理编码")]
    [MaxLength(128)]
    public string? TaskAgentCode { get; set; }

    /// <summary>
    ///     任务代理类型
    /// </summary>
    [Comment("任务代理类型")]
    [MaxLength(128)]
    public string? TaskAgentType { get; set; }

    /// <summary>
    ///     医师姓名
    /// </summary>
    [Comment("医师姓名")]
    [MaxLength(128)]
    public string? DoctorName { get; set; }

    /// <summary>
    ///     应用的视力标准标识
    /// </summary>
    [Comment("应用的视力标准标识")]
    public Guid VisualStandardId { get; set; }

    /// <summary>
    ///     医嘱
    /// </summary>
    [Comment("医嘱")]
    public string? DoctorAdvice { get; set; }

    /// <summary>
    ///     医嘱时间
    /// </summary>
    [Comment("医嘱时间")]
    public DateTime? PrescribedTime { get; set; }

    /// <summary>
    ///     学校标识
    /// </summary>
    [Comment("学校标识")]
    public Guid SchoolId { get; set; }

    /// <summary>
    ///     学校名称
    /// </summary>
    [Comment("学校名称")]
    [MaxLength(128)]
    public required string SchoolName { get; set; }

    /// <summary>
    ///     学校编码
    /// </summary>
    [Comment("学校编码")]
    [MaxLength(128)]
    public string? SchoolCode { get; set; }

    /// <summary>
    ///     学校类型
    /// </summary>
    [Comment("学校类型")]
    [MaxLength(32)]
    public string? SchoolType { get; set; }

    /// <summary>
    ///     行政区划标识
    /// </summary>
    [Comment("行政区划标识")]
    public Guid DivisionId { get; set; }

    /// <summary>
    ///     行政区划名称
    /// </summary>
    [Comment("行政区划名称")]
    [MaxLength(128)]
    public string? DivisionName { get; set; }

    /// <summary>
    ///     行政区划编码
    /// </summary>
    [Comment("行政区划编码")]
    [MaxLength(32)]
    public string? DivisionCode { get; set; }

    /// <summary>
    ///     组织机构标识
    /// </summary>
    [Comment("组织机构标识")]
    public Guid OrganizationId { get; set; }

    /// <summary>
    ///     组织机构名称
    /// </summary>
    [Comment("组织机构名称")]
    [MaxLength(128)]
    public required string OrganizationName { get; set; }

    /// <summary>
    ///     组织机构编码
    /// </summary>
    [Comment("组织机构编码")]
    [MaxLength(128)]
    public string? OrganizationCode { get; set; }

    /// <summary>
    ///     组织机构设计编码
    /// </summary>
    [Comment("组织机构设计编码")]
    [MaxLength(128)]
    public required string OrganizationDesignCode { get; set; }

    /// <summary>
    ///     班级标识
    /// </summary>
    [Comment("班级标识")]
    public Guid? ClassId { get; set; }

    /// <summary>
    ///     班级名称
    /// </summary>
    [Comment("班级名称")]
    [MaxLength(128)]
    public string? ClassName { get; set; }

    /// <summary>
    ///     班级编码
    /// </summary>
    [Comment("班级编码")]
    [MaxLength(128)]
    public string? ClassCode { get; set; }

    /// <summary>
    ///     年级名称
    /// </summary>
    [Comment("年级名称")]
    [MaxLength(128)]
    public string? GradeName { get; set; }

    /// <summary>
    ///     班级序列号
    /// </summary>
    [Comment("班级序列号")]
    [MaxLength(128)]
    public int? ClassSerialNumber { get; set; }

    /// <summary>
    ///     学段
    /// </summary>
    [Comment("学段")]
    [MaxLength(32)]
    public string? StudyPhase { get; set; }

    /// <summary>
    ///     学制
    /// </summary>
    [Comment("学制")]
    [MaxLength(32)]
    public string? SchoolLength { get; set; }

    /// <summary>
    /// 学制值
    /// </summary>
    [Comment("学制值")]
    public int? SchoolLengthValue { get; set; }

    /// <summary>
    ///    班主任标识
    /// </summary>
    [Comment("班主任标识")]
    public Guid? HeadTeacherId { get; set; }

    /// <summary>
    /// 班主任名称
    /// </summary>
    [Comment("班主任名称")]
    [MaxLength(128)]
    public string? HeadTeacherName { get; set; }

    /// <summary>
    ///     学生标识
    /// </summary>
    [Comment("学生标识")]
    public Guid StudentId { get; set; }

    /// <summary>
    ///     学生名称
    /// </summary>
    [Comment("学生名称")]
    [MaxLength(128)]
    public string? StudentName { get; set; }

    /// <summary>
    ///     学生编号
    /// </summary>
    [Comment("学生编号")]
    [MaxLength(128)]
    public string? StudentCode { get; set; }

    /// <summary>
    ///     生日
    /// </summary>
    [Comment("生日")]
    public DateTime? Birthday { get; set; }

    /// <summary>
    ///     年龄
    /// </summary>
    [Comment("年龄")]
    public int? Age { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    [Comment("性别")]
    [MaxLength(32)]
    public string? Gender { get; set; }

    /// <summary>
    ///     左眼与视力表的距离
    /// </summary>
    [Comment("左眼与视力表的距离")]
    public double? LeftChartDistance { get; set; }

    /// <summary>
    ///     右眼与视力表的距离
    /// </summary>
    [Comment("右眼与视力表的距离")]
    public double? RightChartDistance { get; set; }

    /// <summary>
    ///     左眼裸眼视力
    /// </summary>
    [Comment("左眼裸眼视力")]
    public double? LeftNakedEyeVision { get; set; }

    /// <summary>
    ///     右眼裸眼视力
    /// </summary>
    [Comment("右眼裸眼视力")]
    public double? RightNakedEyeVision { get; set; }

    /// <summary>
    ///     是否佩戴眼镜
    /// </summary>
    [Comment("是否佩戴眼镜")]
    public bool IsWareGlasses { get; set; }

    /// <summary>
    ///     左眼矫正视力
    /// </summary>
    [Comment("左眼矫正视力")]
    public double? LeftCorrectedVision { get; set; }

    /// <summary>
    ///     右眼矫正视力
    /// </summary>
    [Comment("右眼矫正视力")]
    public double? RightCorrectedVision { get; set; }

    /// <summary>
    ///     左眼远视类型
    /// </summary>
    [Comment("左眼远视类型")]
    [MaxLength(32)]
    public string? LeftEyeHyperopiaType { get; set; }

    /// <summary>
    ///     右眼远视类型
    /// </summary>
    [Comment("右眼远视类型")]
    [MaxLength(32)]
    public string? RightEyeHyperopiaType { get; set; }

    /// <summary>
    ///     左眼是否佩戴角膜塑形镜
    /// </summary>
    [Comment("左眼是否佩戴角膜塑形镜")]
    public bool? IsWareLeftOkLenses { get; set; }

    /// <summary>
    ///     右眼是否佩戴角膜塑形镜
    /// </summary>
    [Comment("右眼是否佩戴角膜塑形镜")]
    public bool? IsWareRightOkLenses { get; set; }

    /// <summary>
    ///     筛查工作人员姓名
    /// </summary>
    [Comment("筛查工作人员姓名")]
    [MaxLength(128)]
    public string? ChartScreeningStuffName { get; set; }

    /// <summary>
    ///     操作时间
    /// </summary>
    [Comment("操作时间")]
    public DateTime? ChartOperationTime { get; set; }

    /// <summary>
    ///     是否已经过电子视力表筛查
    /// </summary>
    [Comment("是否已经过电子视力表筛查")]
    public bool IsChartChecked { get; set; }

    /// <summary>
    ///     电子视力表筛查次数
    /// </summary>
    [Comment("电子视力表筛查次数")]
    public int ChartCheckedTimes { get; set; }

    /// <summary>
    ///     瞳距
    /// </summary>
    [Comment("瞳距")]
    public double? PupilDistance { get; set; }

    /// <summary>
    ///     左眼球镜
    /// </summary>
    [Comment("左眼球镜")]
    public double? LeftSphere { get; set; }

    /// <summary>
    ///     左眼等效球镜度数
    /// </summary>
    [Comment("左眼等效球镜度数")]
    public int? LeftEquivalentSphere { get; set; }

    /// <summary>
    ///     右眼球镜
    /// </summary>
    [Comment("右眼球镜")]
    public double? RightSphere { get; set; }

    /// <summary>
    ///     右眼等效球径度数
    /// </summary>
    [Comment("右眼等效球径度数")]
    public int? RightEquivalentSphere { get; set; }

    /// <summary>
    ///     左眼柱镜
    /// </summary>
    [Comment("左眼柱镜")]
    public double? LeftCylinder { get; set; }

    /// <summary>
    ///     右眼柱镜
    /// </summary>
    [Comment("右眼柱镜")]
    public double? RightCylinder { get; set; }

    /// <summary>
    ///     左眼轴位
    /// </summary>
    [Comment("左眼轴位")]
    public double? LeftAxis { get; set; }

    /// <summary>
    ///     右眼轴位
    /// </summary>
    [Comment("右眼轴位")]
    public double? RightAxis { get; set; }

    /// <summary>
    ///     左眼散光度数
    /// </summary>
    [Comment("左眼散光度数")]
    public double? LeftAstigmatism { get; set; }

    /// <summary>
    ///     右眼散光度数
    /// </summary>
    [Comment("右眼散光度数")]
    public double? RightAstigmatism { get; set; }

    /// <summary>
    ///     左眼角膜曲率r1(mm)
    /// </summary>
    [Comment("左眼角膜曲率r1(mm)")]
    public double? LeftCornealCurvatureR1 { get; set; }

    /// <summary>
    ///     右眼角膜曲率r1(mm)
    /// </summary>
    [Comment("右眼角膜曲率r1(mm)")]
    public double? RightCornealCurvatureR1 { get; set; }

    /// <summary>
    ///     左眼角膜曲率r1(d)
    /// </summary>
    [Comment("左眼角膜曲率r1(d)")]
    public double? LeftCornealCurvatureD1 { get; set; }

    /// <summary>
    ///     右眼角膜曲率r1(d)
    /// </summary>
    [Comment("右眼角膜曲率r1(d)")]
    public double? RightCornealCurvatureD1 { get; set; }

    /// <summary>
    ///     左眼角膜曲率r1角度
    /// </summary>
    [Comment("左眼角膜曲率r1角度")]
    public double? LeftCornealCurvatureAngle1 { get; set; }

    /// <summary>
    ///     右眼角膜曲率r1角度
    /// </summary>
    [Comment("右眼角膜曲率r1角度")]
    public double? RightCornealCurvatureAngle1 { get; set; }

    /// <summary>
    ///     左眼角膜曲率r2(mm)
    /// </summary>
    [Comment("左眼角膜曲率r2(mm)")]
    public double? LeftCornealCurvatureR2 { get; set; }

    /// <summary>
    ///     右眼角膜曲率r2(mm)
    /// </summary>
    [Comment("右眼角膜曲率r2(mm)")]
    public double? RightCornealCurvatureR2 { get; set; }

    /// <summary>
    ///     左眼角膜曲率r2(d)
    /// </summary>
    [Comment("左眼角膜曲率r2(d)")]
    public double? LeftCornealCurvatureD2 { get; set; }

    /// <summary>
    ///     右眼角膜曲率r2(d)
    /// </summary>
    [Comment("右眼角膜曲率r2(d)")]
    public double? RightCornealCurvatureD2 { get; set; }

    /// <summary>
    ///     左眼角膜曲率r2角度
    /// </summary>
    [Comment("左眼角膜曲率r2角度")]
    public double? LeftCornealCurvatureAngle2 { get; set; }

    /// <summary>
    ///     右眼角膜曲率r2角度
    /// </summary>
    [Comment("右眼角膜曲率r2角度")]
    public double? RightCornealCurvatureAngle2 { get; set; }

    /// <summary>
    ///     左眼角膜曲率平均值(mm)
    /// </summary>
    [Comment("左眼角膜曲率平均值(mm)")]
    public double? LeftCornealCurvatureAverage { get; set; }

    /// <summary>
    ///     右眼角膜曲率平均值(mm)
    /// </summary>
    [Comment("右眼角膜曲率平均值(mm)")]
    public double? RightCornealCurvatureAverage { get; set; }

    /// <summary>
    ///     左眼角膜曲率平均值(d)
    /// </summary>
    [Comment("左眼角膜曲率平均值(d)")]
    public double? LeftCornealCurvatureAverageD { get; set; }

    /// <summary>
    ///     右眼角膜曲率平均值(d)
    /// </summary>
    [Comment("右眼角膜曲率平均值(d)")]
    public double? RightCornealCurvatureAverageD { get; set; }

    /// <summary>
    ///     左眼角膜曲率散光度
    /// </summary>
    [Comment("左眼角膜曲率散光度")]
    public double? LeftCornealCurvatureAstigmatism { get; set; }

    /// <summary>
    ///     右眼角膜曲率散光度
    /// </summary>
    [Comment("右眼角膜曲率散光度")]
    public double? RightCornealCurvatureAstigmatism { get; set; }

    /// <summary>
    ///     筛查工作人员姓名
    /// </summary>
    [Comment("筛查工作人员姓名")]
    public string? OptometerScreeningStuffName { get; set; }

    /// <summary>
    ///     操作时间
    /// </summary>
    [Comment("操作时间")]
    public DateTime? OptometerOperationTime { get; set; }

    /// <summary>
    ///     是否已经过验光仪筛查
    /// </summary>
    [Comment("是否已经过验光仪筛查")]
    public bool IsOptometerChecked { get; set; }

    /// <summary>
    ///     验光仪筛查次数
    /// </summary>
    [Comment("验光仪筛查次数")]
    public int OptometerCheckedTimes { get; set; }

    /// <summary>
    ///     筛查时间
    /// </summary>
    [Comment("筛查时间")]
    public DateTime? CheckTime { get; set; }

    /// <summary>
    ///     筛查结果
    /// </summary>
    [Comment("筛查结果")]
    public string? ExceptionReason { get; set; }

    /// <summary>
    ///     报告发送人
    /// </summary>
    [Comment("报告发送人")]
    public string? ReportSender { get; set; }

    /// <summary>
    ///     报告发送时间
    /// </summary>
    [Comment("报告发送时间")]
    public DateTime? ReportSendTime { get; set; }

    /// <summary>
    ///     报告签收人
    /// </summary>
    [Comment("报告签收人")]
    public string? ReportReceiver { get; set; }

    /// <summary>
    ///     报告签收时间
    /// </summary>
    [Comment("报告签收时间")]
    public DateTime? ReportReceiveTime { get; set; }

    /// <summary>
    /// 筛查报告反馈
    /// </summary>
    [Comment("筛查报告反馈")]
    [MaxLength(256)]
    public string? RecordFeedBack { get; set; }
}