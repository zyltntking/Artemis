using System.ComponentModel;

namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
///     地区级行政区划类型
/// </summary>
[Description("地区级行政区划类型")]
public class PrefectureType : Enumeration
{
    /// <summary>
    ///     未知类型
    /// </summary>
    [Description("未知类型")] public static PrefectureType Unknown = new(-1, nameof(Unknown));

    /// <summary>
    ///     地区
    /// </summary>
    [Description("地区")] public static PrefectureType Prefecture = new(0, nameof(Prefecture));

    /// <summary>
    ///     自治州
    /// </summary>
    [Description("自治州")] public static PrefectureType AutonomousPrefecture = new(1, nameof(AutonomousPrefecture));

    /// <summary>
    ///     市
    /// </summary>
    [Description("市")] public static PrefectureType Municipality = new(2, nameof(Municipality));

    /// <summary>
    ///     盟
    /// </summary>
    [Description("盟")] public static PrefectureType League = new(1, nameof(League));

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    protected PrefectureType(int id, string name) : base(id, name)
    {
    }
}