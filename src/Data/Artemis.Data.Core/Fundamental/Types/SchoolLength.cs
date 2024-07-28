using System.ComponentModel;

namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
/// 学制
/// </summary>
public sealed class SchoolLength : Enumeration
{
    /// <summary>
    /// 一年制
    /// </summary>
    [Description("一年制")]
    public static readonly SchoolLength OneYear = new(1, nameof(OneYear));

    /// <summary>
    /// 二年制
    /// </summary>
    [Description("二年制")]
    public static readonly SchoolLength TwoYears = new(2, nameof(TwoYears));

    /// <summary>
    /// 三年制
    /// </summary>
    [Description("三年制")]
    public static readonly SchoolLength ThreeYears = new(3, nameof(ThreeYears));

    /// <summary>
    /// 四年制
    /// </summary>
    [Description("四年制")]
    public static readonly SchoolLength FourYears = new(4, nameof(FourYears));

    /// <summary>
    /// 五年制
    /// </summary>
    [Description("五年制")]
    public static readonly SchoolLength FiveYears = new(5, nameof(FiveYears));

    /// <summary>
    /// 六年制
    /// </summary>
    [Description("六年制")]
    public static readonly SchoolLength SixYears = new(6, nameof(SixYears));

    /// <summary>
    /// 七年制
    /// </summary>
    [Description("七年制")]
    public static readonly SchoolLength SevenYears = new(7, nameof(SevenYears));

    /// <summary>
    /// 八年制
    /// </summary>
    [Description("八年制")]
    public static readonly SchoolLength EightYears = new(8, nameof(EightYears));

    /// <summary>
    /// 九年制
    /// </summary>
    [Description("九年制")]
    public static readonly SchoolLength NineYears = new(9, nameof(NineYears));

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private SchoolLength(int id, string name) : base(id, name)
    {
    }
}