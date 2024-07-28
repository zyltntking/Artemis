using System.ComponentModel;

namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
///     行政区划等级
/// </summary>
[Description("行政区划等级")]
public sealed class RegionLevel : Enumeration
{
    /// <summary>
    ///     未知等级
    /// </summary>
    [Description("未知等级")] public static RegionLevel Unknown = new(-1, nameof(Unknown));

    /// <summary>
    ///     国家级
    /// </summary>
    [Description("国家级")] public static RegionLevel State = new(0, nameof(State));

    /// <summary>
    ///     省级
    /// </summary>
    [Description("省级")] public static RegionLevel Province = new(1, nameof(Province));

    /// <summary>
    ///     市级
    /// </summary>
    [Description("市级")] public static RegionLevel Prefecture = new(2, nameof(Prefecture));

    /// <summary>
    ///     县级
    /// </summary>
    [Description("县级")] public static RegionLevel County = new(3, nameof(County));

    /// <summary>
    ///     乡级
    /// </summary>
    [Description("乡级")] public static RegionLevel Township = new(4, nameof(Township));

    /// <summary>
    /// 街道级
    /// </summary>
    [Description("街道级")]
    public static RegionLevel Street = new(5, nameof(Street));


    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private RegionLevel(int id, string name) : base(id, name)
    {
    }
}