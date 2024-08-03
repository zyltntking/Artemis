using Artemis.Data.Core;

namespace Artemis.Service.Shared.Business.VisionScreen;

/// <summary>
///     验光仪接口
/// </summary>
public interface IOptometer : IOptometerInfo
{
}

/// <summary>
///     验光仪数据信息接口
/// </summary>
public interface IOptometerInfo : IOptometerPackage, IKeySlot
{
    /// <summary>
    ///     档案标识
    /// </summary>
    public Guid RecordId { get; set; }
}

/// <summary>
///     验光仪数据包接口
/// </summary>
public interface IOptometerPackage
{
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
    ///     筛查工作人员姓名
    /// </summary>
    string? OptometerScreeningStuffName { get; set; }

    /// <summary>
    ///     操作时间
    /// </summary>
    DateTime? OptometerOperationTime { get; set; }
}