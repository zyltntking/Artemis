using System.ComponentModel;

namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
///     乡级行政区划类型
/// </summary>
[Description("乡级行政区划类型")]
public sealed class TownshipType : Enumeration
{
    /// <summary>
    ///     未知类型
    /// </summary>
    [Description("未知类型")] public static TownshipType Unknown = new(-1, nameof(Unknown));

    /// <summary>
    ///     乡
    /// </summary>
    [Description("乡")] public static TownshipType Township = new(0, nameof(Township));

    /// <summary>
    ///     镇
    /// </summary>
    [Description("镇")] public static TownshipType Town = new(1, nameof(Town));

    /// <summary>
    ///     街道
    /// </summary>
    [Description("街道")] public static TownshipType SubDistrict = new(2, nameof(SubDistrict));

    /// <summary>
    ///     民族乡
    /// </summary>
    [Description("民族乡")] public static TownshipType EthnicTownship = new(3, nameof(EthnicTownship));

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private TownshipType(int id, string name) : base(id, name)
    {
    }
}