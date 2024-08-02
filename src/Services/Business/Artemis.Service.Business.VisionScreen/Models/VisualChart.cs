using Artemis.Data.Core;
using Artemis.Service.Shared.Business.VisionScreen;

namespace Artemis.Service.Business.VisionScreen.Models;

/// <summary>
///     视力表数据模型
/// </summary>
public class VisualChart : ConcurrencyPartition, IVisualChart
{
    #region Implementation of IVisualChart

    /// <summary>
    ///     档案标识
    /// </summary>
    public Guid RecordId { get; set; }

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
    public double? LeftEyeHyperopiaType { get; set; }

    /// <summary>
    ///     右眼远视类型
    /// </summary>
    public double? RightEyeHyperopiaType { get; set; }

    /// <summary>
    ///     左眼是否佩戴角膜塑形镜
    /// </summary>
    public bool? IsWareLeftOkLenses { get; set; }

    /// <summary>
    ///     右眼是否佩戴角膜塑形镜
    /// </summary>
    public bool? IsWareRightOkLenses { get; set; }

    /// <summary>
    ///     是否已经过电子视力表筛查
    /// </summary>
    public bool IsChartChecked { get; set; }

    #endregion
}