namespace Artemis.Data.Core.Fundamental.Model;

#region IPosition

/// <summary>
///     位置接口
/// </summary>
file interface IPosition
{
    /// <summary>
    ///     社区
    /// </summary>
    string? Community { get; set; }

    /// <summary>
    ///     建筑区(居住区、小区)
    /// </summary>
    string? BuildingZone { get; set; }

    /// <summary>
    ///     建筑物(楼栋)
    /// </summary>
    string? Building { get; set; }

    /// <summary>
    ///     单元(入口号)
    /// </summary>
    string? Unit { get; set; }

    /// <summary>
    ///     楼层数
    /// </summary>
    string? Floor { get; set; }

    /// <summary>
    ///     区域
    /// </summary>
    string? Area { get; set; }

    /// <summary>
    ///     室(门号)
    /// </summary>
    string? Room { get; set; }

    /// <summary>
    ///     侧
    /// </summary>
    string? Side { get; set; }

    /// <summary>
    ///     补充信息(详细描述)
    /// </summary>
    string? Supplement { get; set; }
}

#endregion

/// <summary>
///     位置信息
/// </summary>
public abstract class PositionInfo : IPosition
{
    #region Overrides of Object

    /// <summary>
    ///     Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString()
    {
        var segments = new List<string>();

        if (!string.IsNullOrWhiteSpace(Community))
            segments.Add(Community);

        if (!string.IsNullOrWhiteSpace(BuildingZone))
            segments.Add(BuildingZone);

        if (!string.IsNullOrWhiteSpace(Building))
            segments.Add(Building);

        if (!string.IsNullOrWhiteSpace(Unit))
            segments.Add(Unit);

        if (!string.IsNullOrWhiteSpace(Floor))
            segments.Add(Floor);

        if (!string.IsNullOrWhiteSpace(Area))
            segments.Add(Area);

        if (!string.IsNullOrWhiteSpace(Room))
            segments.Add(Room);

        if (!string.IsNullOrWhiteSpace(Side))
            segments.Add(Side);

        if (!string.IsNullOrWhiteSpace(Supplement))
            segments.Add(Supplement);

        var position = string.Join(" ", segments);

        return position;
    }

    #endregion

    #region Implementation of IPosition

    /// <summary>
    ///     社区
    /// </summary>
    public string? Community { get; set; }

    /// <summary>
    ///     建筑区(居住区、小区)
    /// </summary>
    public string? BuildingZone { get; set; }

    /// <summary>
    ///     建筑物(楼栋)
    /// </summary>
    public string? Building { get; set; }

    /// <summary>
    ///     单元(入口号)
    /// </summary>
    public string? Unit { get; set; }

    /// <summary>
    ///     楼层数
    /// </summary>
    public string? Floor { get; set; }

    /// <summary>
    ///     区域
    /// </summary>
    public string? Area { get; set; }

    /// <summary>
    ///     室(门号)
    /// </summary>
    public string? Room { get; set; }

    /// <summary>
    ///     侧
    /// </summary>
    public string? Side { get; set; }

    /// <summary>
    ///     补充信息(详细描述)
    /// </summary>
    public string? Supplement { get; set; }

    #endregion
}