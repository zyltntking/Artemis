using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Service.Shared.Business.VisionScreen;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Business.VisionScreen.Models;

/// <summary>
///     视力表数据模型
/// </summary>
public class VisualChart : ConcurrencyPartition, IVisualChart
{
    /// <summary>
    ///     档案标识
    /// </summary>
    [Comment("档案标识")]
    public Guid RecordId { get; set; }

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
}