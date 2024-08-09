using Artemis.Data.Core;
using Artemis.Service.Shared.Business.VisionScreen;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Business.VisionScreen.Models;

/// <summary>
///     筛查仪数据
/// </summary>
public class Optometer : ConcurrencyPartition, IOptometer
{
    /// <summary>
    ///     档案标识
    /// </summary>
    [Comment("档案标识")]
    public Guid RecordId { get; set; }

    /// <summary>
    ///     瞳距
    /// </summary>
    [Comment("瞳距")]
    public double? PupilDistance { get; set; }

    /// <summary>
    /// 左眼瞳孔半径
    /// </summary>
    [Comment("左眼瞳孔半径")]
    public double? LeftPupilRadius { get; set; }

    /// <summary>
    /// 右眼瞳孔半径
    /// </summary>
    [Comment("右眼瞳孔半径")]
    public double? RightPupilRadius { get; set; }

    /// <summary>
    /// 左眼垂直方向斜视度数
    /// </summary>
    [Comment("左眼垂直方向斜视度数")]
    public double? LeftVerticalAxis { get; set; }

    /// <summary>
    /// 右眼垂直方向斜视度数
    /// </summary>
    [Comment("右眼垂直方向斜视度数")]
    public double? RightVerticalAxis { get; set; }

    /// <summary>
    ///  左眼水平方向斜视度数
    /// </summary>
    [Comment("左眼水平方向斜视度数")]
    public double? LeftHorizontalAxis { get; set; }

    /// <summary>
    /// 右眼水平方向斜视度数
    /// </summary>
    [Comment("右眼水平方向斜视度数")]
    public double? RightHorizontalAxis { get; set; }

    /// <summary>
    ///  左眼红光反射
    /// </summary>
    [Comment("左眼红光反射")]
    public double? LeftRedReflect { get; set; }

    /// <summary>
    /// 右眼红光反射
    /// </summary>
    [Comment("右眼红光反射")]
    public double? RightRedReflect { get; set; }

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
}