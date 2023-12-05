using System.ComponentModel;

namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
///     省级行政区划类型
/// </summary>
[Description("省级行政区划类型")]
public class ProvinceType : Enumeration
{
    /// <summary>
    ///     未知类型
    /// </summary>
    [Description("未知类型")] public static ProvinceType Unknown = new(-1, nameof(Unknown));

    /// <summary>
    ///     省
    /// </summary>
    [Description("省")] public static ProvinceType Province = new(0, nameof(Province));

    /// <summary>
    ///     自治区
    /// </summary>
    [Description("自治区")] public static ProvinceType AutonomousRegion = new(1, nameof(AutonomousRegion));

    /// <summary>
    ///     直辖市
    /// </summary>
    [Description("直辖市")] public static ProvinceType Municipality = new(2, nameof(Municipality));

    /// <summary>
    ///     特别行政区
    /// </summary>
    [Description("特别行政区")]
    public static ProvinceType SpecialAdministrativeRegion = new(3, nameof(SpecialAdministrativeRegion));


    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    protected ProvinceType(int id, string name) : base(id, name)
    {
    }
}