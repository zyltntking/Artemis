using Artemis.Data.Core;

namespace Artemis.Service.Shared.Business.VisionScreen;

/// <summary>
///     视力表数据接口
/// </summary>
public interface IVisualChart : IVisualChartInfo
{
}

/// <summary>
/// 电子视力表信息
/// </summary>
public interface IVisualChartInfo : IVisualChartPackage, IKeySlot
{
    /// <summary>
    ///     档案标识
    /// </summary>
    public Guid RecordId { get; set; }
}

/// <summary>
/// 电子视力表数据包
/// </summary>
public interface IVisualChartPackage
{
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
    ///     筛查工作人员姓名
    /// </summary>
    string? ChartScreeningStuffName { get; set; }

    /// <summary>
    ///     操作时间
    /// </summary>
    DateTime? ChartOperationTime { get; set; }
}