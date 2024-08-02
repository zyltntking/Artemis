namespace Artemis.Service.Shared.Business.VisionScreen;

/// <summary>
///     视力筛查记录接口
/// </summary>
public interface IVisionScreenRecord
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
    ///     筛查工作人员姓名
    /// </summary>
    string? ScreeningStuffName { get; set; }

    /// <summary>
    ///     操作时间
    /// </summary>
    DateTime? OperationTime { get; set; }

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
    DateOnly? Birthday { get; set; }

    /// <summary>
    ///     年龄
    /// </summary>
    int? Age { get; set; }

    /// <summary>
    ///     左眼与视力表的距离
    /// </summary>
    double? LeftChartDistance { get; set; }

    /// <summary>
    ///     右眼与视力表的距离
    /// </summary>
    double? RightChartDistance { get; set; }

    /// <summary>
    ///     左眼裸眼视力
    /// </summary>
    double? LeftNakedEyeVision { get; set; }

    /// <summary>
    ///     右眼裸眼视力
    /// </summary>
    double? RightNakedEyeVision { get; set; }

    /// <summary>
    ///     是否佩戴眼镜
    /// </summary>
    bool IsWareGlasses { get; set; }

    /// <summary>
    ///     左眼矫正视力
    /// </summary>
    double? LeftCorrectedVision { get; set; }

    /// <summary>
    ///     右眼矫正视力
    /// </summary>
    double? RightCorrectedVision { get; set; }

    /// <summary>
    ///     左眼远视类型
    /// </summary>
    double? LeftEyeHyperopiaType { get; set; }

    /// <summary>
    ///     右眼远视类型
    /// </summary>
    double? RightEyeHyperopiaType { get; set; }

    /// <summary>
    ///     左眼是否佩戴角膜塑形镜
    /// </summary>
    bool? IsWareLeftOkLenses { get; set; }

    /// <summary>
    ///     右眼是否佩戴角膜塑形镜
    /// </summary>
    bool? IsWareRightOkLenses { get; set; }

    /// <summary>
    ///     是否已经过电子视力表筛查
    /// </summary>
    bool IsChartChecked { get; set; }

    /// <summary>
    ///     电子视力表筛查次数
    /// </summary>
    int ChartCheckedTimes { get; set; }

    /// <summary>
    ///     瞳距
    /// </summary>
    double? PupilDistance { get; set; }

    /// <summary>
    ///     左眼球镜
    /// </summary>
    double? LeftSphere { get; set; }

    /// <summary>
    ///     左眼等效球镜度数
    /// </summary>
    int? LeftEquivalentSphere { get; set; }

    /// <summary>
    ///     右眼球镜
    /// </summary>
    double? RightSphere { get; set; }

    /// <summary>
    ///     右眼等效球径度数
    /// </summary>
    int? RightEquivalentSphere { get; set; }

    /// <summary>
    ///     左眼柱镜
    /// </summary>
    double? LeftCylinder { get; set; }

    /// <summary>
    ///     右眼柱镜
    /// </summary>
    double? RightCylinder { get; set; }

    /// <summary>
    ///     左眼轴位
    /// </summary>
    double? LeftAxis { get; set; }

    /// <summary>
    ///     右眼轴位
    /// </summary>
    double? RightAxis { get; set; }

    /// <summary>
    ///     左眼散光度数
    /// </summary>
    double? LeftAstigmatism { get; set; }

    /// <summary>
    ///     右眼散光度数
    /// </summary>
    double? RightAstigmatism { get; set; }

    /// <summary>
    ///     左眼角膜曲率r1(mm)
    /// </summary>
    double? LeftCornealCurvatureR1 { get; set; }

    /// <summary>
    ///     右眼角膜曲率r1(mm)
    /// </summary>
    double? RightCornealCurvatureR1 { get; set; }

    /// <summary>
    ///     左眼角膜曲率r1(d)
    /// </summary>
    double? LeftCornealCurvatureD1 { get; set; }

    /// <summary>
    ///     右眼角膜曲率r1(d)
    /// </summary>
    double? RightCornealCurvatureD1 { get; set; }

    /// <summary>
    ///     左眼角膜曲率r1角度
    /// </summary>
    double? LeftCornealCurvatureAngle1 { get; set; }

    /// <summary>
    ///     右眼角膜曲率r1角度
    /// </summary>
    double? RightCornealCurvatureAngle1 { get; set; }

    /// <summary>
    ///     左眼角膜曲率r2(mm)
    /// </summary>
    double? LeftCornealCurvatureR2 { get; set; }

    /// <summary>
    ///     右眼角膜曲率r2(mm)
    /// </summary>
    double? RightCornealCurvatureR2 { get; set; }

    /// <summary>
    ///     左眼角膜曲率r2(d)
    /// </summary>
    double? LeftCornealCurvatureD2 { get; set; }

    /// <summary>
    ///     右眼角膜曲率r2(d)
    /// </summary>
    double? RightCornealCurvatureD2 { get; set; }

    /// <summary>
    ///     左眼角膜曲率r2角度
    /// </summary>
    double? LeftCornealCurvatureAngle2 { get; set; }

    /// <summary>
    ///     右眼角膜曲率r2角度
    /// </summary>
    double? RightCornealCurvatureAngle2 { get; set; }

    /// <summary>
    ///     左眼角膜曲率平均值(mm)
    /// </summary>
    double? LeftCornealCurvatureAverage { get; set; }

    /// <summary>
    ///     右眼角膜曲率平均值(mm)
    /// </summary>
    double? RightCornealCurvatureAverage { get; set; }

    /// <summary>
    ///     左眼角膜曲率平均值(d)
    /// </summary>
    double? LeftCornealCurvatureAverageD { get; set; }

    /// <summary>
    ///     右眼角膜曲率平均值(d)
    /// </summary>
    double? RightCornealCurvatureAverageD { get; set; }

    /// <summary>
    ///     左眼角膜曲率散光度
    /// </summary>
    double? LeftCornealCurvatureAstigmatism { get; set; }

    /// <summary>
    ///     右眼角膜曲率散光度
    /// </summary>
    double? RightCornealCurvatureAstigmatism { get; set; }

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