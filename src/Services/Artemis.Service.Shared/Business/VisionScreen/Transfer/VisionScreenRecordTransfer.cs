namespace Artemis.Service.Shared.Business.VisionScreen.Transfer;

/// <summary>
/// 视力筛查记录信息
/// </summary>
public record VisionScreenRecordInfo : VisionScreenRecordPackage, IVisionScreenRecordInfo
{
    #region Implementation of IKeySlot<Guid>

    /// <summary>
    ///     存储标识
    /// </summary>
    public Guid Id { get; set; }

    #endregion
}

/// <summary>
/// 视力筛查记录数据包
/// </summary>
public record VisionScreenRecordPackage : IVisionScreenRecordPackage
{
    #region Implementation of IVisualChartPackage

    /// <summary>
    ///     左眼与视力表的距离
    /// </summary>
    public double? LeftChartDistance { get; set; }

    /// <summary>
    ///     右眼与视力表的距离
    /// </summary>
    public double? RightChartDistance { get; set; }

    /// <summary>
    ///     左眼裸眼视力
    /// </summary>
    public double? LeftNakedEyeVision { get; set; }

    /// <summary>
    ///     右眼裸眼视力
    /// </summary>
    public double? RightNakedEyeVision { get; set; }

    /// <summary>
    ///     是否佩戴眼镜
    /// </summary>
    public bool IsWareGlasses { get; set; }

    /// <summary>
    ///     左眼矫正视力
    /// </summary>
    public double? LeftCorrectedVision { get; set; }

    /// <summary>
    ///     右眼矫正视力
    /// </summary>
    public double? RightCorrectedVision { get; set; }

    /// <summary>
    ///     左眼远视类型
    /// </summary>
    public string? LeftEyeHyperopiaType { get; set; }

    /// <summary>
    ///     右眼远视类型
    /// </summary>
    public string? RightEyeHyperopiaType { get; set; }

    /// <summary>
    ///     左眼是否佩戴角膜塑形镜
    /// </summary>
    public bool? IsWareLeftOkLenses { get; set; }

    /// <summary>
    ///     右眼是否佩戴角膜塑形镜
    /// </summary>
    public bool? IsWareRightOkLenses { get; set; }

    /// <summary>
    ///     筛查工作人员姓名
    /// </summary>
    public string? ChartScreeningStuffName { get; set; }

    /// <summary>
    ///     操作时间
    /// </summary>
    public DateTime? ChartOperationTime { get; set; }

    #endregion

    #region Implementation of IOptometerPackage

    /// <summary>
    ///     瞳距
    /// </summary>
    public double? PupilDistance { get; set; }

    /// <summary>
    /// 左眼瞳孔半径
    /// </summary>
    public double? LeftPupilRadius { get; set; }

    /// <summary>
    /// 右眼瞳孔半径
    /// </summary>
    public double? RightPupilRadius { get; set; }

    /// <summary>
    /// 左眼垂直方向斜视度数
    /// </summary>
    public double? LeftVerticalAxis { get; set; }

    /// <summary>
    /// 右眼垂直方向斜视度数
    /// </summary>
    public double? RightVerticalAxis { get; set; }

    /// <summary>
    ///  左眼水平方向斜视度数
    /// </summary>
    public double? LeftHorizontalAxis { get; set; }

    /// <summary>
    /// 右眼水平方向斜视度数
    /// </summary>
    public double? RightHorizontalAxis { get; set; }

    /// <summary>
    ///  左眼红光反射
    /// </summary>
    public double? LeftRedReflect { get; set; }

    /// <summary>
    /// 右眼红光反射
    /// </summary>
    public double? RightRedReflect { get; set; }

    /// <summary>
    ///     左眼球镜
    /// </summary>
    public double? LeftSphere { get; set; }

    /// <summary>
    ///     左眼等效球镜度数
    /// </summary>
    public int? LeftEquivalentSphere { get; set; }

    /// <summary>
    ///     右眼球镜
    /// </summary>
    public double? RightSphere { get; set; }

    /// <summary>
    ///     右眼等效球径度数
    /// </summary>
    public int? RightEquivalentSphere { get; set; }

    /// <summary>
    ///     左眼柱镜
    /// </summary>
    public double? LeftCylinder { get; set; }

    /// <summary>
    ///     右眼柱镜
    /// </summary>
    public double? RightCylinder { get; set; }

    /// <summary>
    ///     左眼轴位
    /// </summary>
    public double? LeftAxis { get; set; }

    /// <summary>
    ///     右眼轴位
    /// </summary>
    public double? RightAxis { get; set; }

    /// <summary>
    ///     左眼散光度数
    /// </summary>
    public double? LeftAstigmatism { get; set; }

    /// <summary>
    ///     右眼散光度数
    /// </summary>
    public double? RightAstigmatism { get; set; }

    /// <summary>
    ///     左眼角膜曲率r1(mm)
    /// </summary>
    public double? LeftCornealCurvatureR1 { get; set; }

    /// <summary>
    ///     右眼角膜曲率r1(mm)
    /// </summary>
    public double? RightCornealCurvatureR1 { get; set; }

    /// <summary>
    ///     左眼角膜曲率r1(d)
    /// </summary>
    public double? LeftCornealCurvatureD1 { get; set; }

    /// <summary>
    ///     右眼角膜曲率r1(d)
    /// </summary>
    public double? RightCornealCurvatureD1 { get; set; }

    /// <summary>
    ///     左眼角膜曲率r1角度
    /// </summary>
    public double? LeftCornealCurvatureAngle1 { get; set; }

    /// <summary>
    ///     右眼角膜曲率r1角度
    /// </summary>
    public double? RightCornealCurvatureAngle1 { get; set; }

    /// <summary>
    ///     左眼角膜曲率r2(mm)
    /// </summary>
    public double? LeftCornealCurvatureR2 { get; set; }

    /// <summary>
    ///     右眼角膜曲率r2(mm)
    /// </summary>
    public double? RightCornealCurvatureR2 { get; set; }

    /// <summary>
    ///     左眼角膜曲率r2(d)
    /// </summary>
    public double? LeftCornealCurvatureD2 { get; set; }

    /// <summary>
    ///     右眼角膜曲率r2(d)
    /// </summary>
    public double? RightCornealCurvatureD2 { get; set; }

    /// <summary>
    ///     左眼角膜曲率r2角度
    /// </summary>
    public double? LeftCornealCurvatureAngle2 { get; set; }

    /// <summary>
    ///     右眼角膜曲率r2角度
    /// </summary>
    public double? RightCornealCurvatureAngle2 { get; set; }

    /// <summary>
    ///     左眼角膜曲率平均值(mm)
    /// </summary>
    public double? LeftCornealCurvatureAverage { get; set; }

    /// <summary>
    ///     右眼角膜曲率平均值(mm)
    /// </summary>
    public double? RightCornealCurvatureAverage { get; set; }

    /// <summary>
    ///     左眼角膜曲率平均值(d)
    /// </summary>
    public double? LeftCornealCurvatureAverageD { get; set; }

    /// <summary>
    ///     右眼角膜曲率平均值(d)
    /// </summary>
    public double? RightCornealCurvatureAverageD { get; set; }

    /// <summary>
    ///     左眼角膜曲率散光度
    /// </summary>
    public double? LeftCornealCurvatureAstigmatism { get; set; }

    /// <summary>
    ///     右眼角膜曲率散光度
    /// </summary>
    public double? RightCornealCurvatureAstigmatism { get; set; }

    /// <summary>
    ///     筛查工作人员姓名
    /// </summary>
    public string? OptometerScreeningStuffName { get; set; }

    /// <summary>
    ///     操作时间
    /// </summary>
    public DateTime? OptometerOperationTime { get; set; }

    #endregion

    #region Implementation of IVisionScreenRecordPackage

    /// <summary>
    ///     任务标识
    /// </summary>
    public Guid TaskId { get; set; }

    /// <summary>
    ///     任务名称
    /// </summary>
    public string? TaskName { get; set; }

    /// <summary>
    ///     任务编码
    /// </summary>
    public string? TaskCode { get; set; }

    /// <summary>
    ///     任务单元标识
    /// </summary>
    public Guid TaskUnitId { get; set; }

    /// <summary>
    ///     任务单元名称
    /// </summary>
    public string? TaskUnitName { get; set; }

    /// <summary>
    ///     任务单元编码
    /// </summary>
    public string? TaskUnitCode { get; set; }

    /// <summary>
    ///     任务目标标识
    /// </summary>
    public Guid TaskUnitTargetId { get; set; }

    /// <summary>
    ///     任务目标编码
    /// </summary>
    public string? TaskUnitTargetCode { get; set; }

    /// <summary>
    ///     任务代理标识
    /// </summary>
    public Guid? TaskAgentId { get; set; }

    /// <summary>
    ///     任务代理名称
    /// </summary>
    public string? TaskAgentName { get; set; }

    /// <summary>
    ///     任务代理编码
    /// </summary>
    public string? TaskAgentCode { get; set; }

    /// <summary>
    ///     任务代理类型
    /// </summary>
    public string? TaskAgentType { get; set; }

    /// <summary>
    ///     医师姓名
    /// </summary>
    public string? DoctorName { get; set; }

    /// <summary>
    ///     应用的视力标准标识
    /// </summary>
    public Guid VisualStandardId { get; set; }

    /// <summary>
    ///     医嘱
    /// </summary>
    public string? DoctorAdvice { get; set; }

    /// <summary>
    ///     医嘱时间
    /// </summary>
    public DateTime? PrescribedTime { get; set; }

    /// <summary>
    ///     学校标识
    /// </summary>
    public Guid SchoolId { get; set; }

    /// <summary>
    ///     学校名称
    /// </summary>
    public required string SchoolName { get; set; }

    /// <summary>
    ///     学校编码
    /// </summary>
    public string? SchoolCode { get; set; }

    /// <summary>
    ///     学校类型
    /// </summary>
    public string? SchoolType { get; set; }

    /// <summary>
    ///     行政区划标识
    /// </summary>
    public Guid DivisionId { get; set; }

    /// <summary>
    ///     行政区划名称
    /// </summary>
    public string? DivisionName { get; set; }

    /// <summary>
    ///     行政区划编码
    /// </summary>
    public string? DivisionCode { get; set; }

    /// <summary>
    ///     组织机构标识
    /// </summary>
    public Guid OrganizationId { get; set; }

    /// <summary>
    ///     组织机构名称
    /// </summary>
    public required string OrganizationName { get; set; }

    /// <summary>
    ///     组织机构编码
    /// </summary>
    public string? OrganizationCode { get; set; }

    /// <summary>
    ///     组织机构设计编码
    /// </summary>
    public required string OrganizationDesignCode { get; set; }

    /// <summary>
    ///     班级标识
    /// </summary>
    public Guid? ClassId { get; set; }

    /// <summary>
    ///     班级名称
    /// </summary>
    public string? ClassName { get; set; }

    /// <summary>
    ///     班级编码
    /// </summary>
    public string? ClassCode { get; set; }

    /// <summary>
    ///     年级名称
    /// </summary>
    public string? GradeName { get; set; }

    /// <summary>
    ///     班级序列号
    /// </summary>
    public int? ClassSerialNumber { get; set; }

    /// <summary>
    ///     学段
    /// </summary>
    public string? StudyPhase { get; set; }

    /// <summary>
    ///     学制
    /// </summary>
    public string? SchoolLength { get; set; }

    /// <summary>
    ///    学制值
    /// </summary>
    public int? SchoolLengthValue { get; set; }

    /// <summary>
    ///    班主任标识
    /// </summary>
    public Guid? HeadTeacherId { get; set; }

    /// <summary>
    /// 班主任名称
    /// </summary>
    public string? HeadTeacherName { get; set; }

    /// <summary>
    ///     学生标识
    /// </summary>
    public Guid StudentId { get; set; }

    /// <summary>
    ///     学生名称
    /// </summary>
    public string? StudentName { get; set; }

    /// <summary>
    ///     学生编号
    /// </summary>
    public string? StudentCode { get; set; }

    /// <summary>
    ///     生日
    /// </summary>
    public DateTime? Birthday { get; set; }

    /// <summary>
    ///     年龄
    /// </summary>
    public int? Age { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    public string? Gender { get; set; }

    /// <summary>
    ///    民族
    /// </summary>
    public string? Nation { get; set; }

    /// <summary>
    ///     是否已经过电子视力表筛查
    /// </summary>
    public bool IsChartChecked { get; set; }

    /// <summary>
    ///     电子视力表筛查次数
    /// </summary>
    public int ChartCheckedTimes { get; set; }

    /// <summary>
    ///     是否已经过验光仪筛查
    /// </summary>
    public bool IsOptometerChecked { get; set; }

    /// <summary>
    ///     验光仪筛查次数
    /// </summary>
    public int OptometerCheckedTimes { get; set; }

    /// <summary>
    ///     筛查时间
    /// </summary>
    public DateTime? CheckTime { get; set; }

    /// <summary>
    ///     筛查结果
    /// </summary>
    public string? ExceptionReason { get; set; }

    /// <summary>
    ///     报告发送人
    /// </summary>
    public string? ReportSender { get; set; }

    /// <summary>
    ///     报告发送时间
    /// </summary>
    public DateTime? ReportSendTime { get; set; }

    /// <summary>
    ///     报告签收人
    /// </summary>
    public string? ReportReceiver { get; set; }

    /// <summary>
    ///     报告签收时间
    /// </summary>
    public DateTime? ReportReceiveTime { get; set; }

    /// <summary>
    /// 是否反馈
    /// </summary>
    public bool IsFeedBack { get; set; }

    /// <summary>
    /// 是否签名
    /// </summary>
    public bool IsSign { get; set; }

    /// <summary>
    /// 用户签名
    /// </summary>
    public string? UserSign { get; set; }

    /// <summary>
    /// 用户签名时间
    /// </summary>
    public DateTime? UserSignTime { get; set; }

    /// <summary>
    /// 取消筛查标识
    /// </summary>
    public string? CancelFlag { get; set; }

    #endregion
}