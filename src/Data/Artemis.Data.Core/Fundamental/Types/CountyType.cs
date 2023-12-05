using System.ComponentModel;

namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
///     县级行政区划类型
/// </summary>
[Description("县级行政区划类型")]
public class CountyType : Enumeration
{
    /// <summary>
    ///     未知类型
    /// </summary>
    [Description("未知类型")] public static CountyType Unknown = new(-1, nameof(Unknown));

    /// <summary>
    ///     县
    /// </summary>
    [Description("县")] public static CountyType County = new(0, nameof(County));

    /// <summary>
    ///     自治县
    /// </summary>
    [Description("自治县")] public static CountyType AutonomousCounty = new(1, nameof(AutonomousCounty));

    /// <summary>
    ///     县级市
    /// </summary>
    [Description("县级市")] public static CountyType City = new(2, nameof(City));

    /// <summary>
    ///     市辖区
    /// </summary>
    [Description("市辖区区")] public static CountyType District = new(3, nameof(District));

    /// <summary>
    ///     旗
    /// </summary>
    [Description("旗")] public static CountyType Banner = new(4, nameof(Banner));

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    protected CountyType(int id, string name) : base(id, name)
    {
    }
}