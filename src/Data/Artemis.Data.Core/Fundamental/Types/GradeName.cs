using System.ComponentModel;

namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
/// 年级名
/// </summary>
[Description("年级名")]
public sealed class GradeName : Enumeration
{
    /// <summary>
    /// 已毕业
    /// </summary>
    [Description("已毕业")]
    public static GradeName FinishSchool = new(-1, nameof(FinishSchool));

    /// <summary>
    /// 一年级
    /// </summary>
    [Description("一年级")]
    public static GradeName FirstGrade = new(1, nameof(FirstGrade));

    /// <summary>
    /// 二年级
    /// </summary>
    [Description("二年级")]
    public static GradeName SecondGrade = new(2, nameof(SecondGrade));

    /// <summary>
    /// 三年级
    /// </summary>
    [Description("三年级")]
    public static GradeName ThirdGrade = new(3, nameof(ThirdGrade));

    /// <summary>
    /// 四年级
    /// </summary>
    [Description("四年级")]
    public static GradeName ForthGrade = new(4, nameof(ForthGrade));

    /// <summary>
    /// 五年级
    /// </summary>
    [Description("五年级")]
    public static GradeName FifthGrade = new(5, nameof(FifthGrade));

    /// <summary>
    /// 六年级
    /// </summary>
    [Description("六年级")]
    public static GradeName SixthGrade = new(6, nameof(SixthGrade));

    /// <summary>
    /// 七年级
    /// </summary>
    [Description("七年级")]
    public static GradeName SeventhGrade = new(7, nameof(SeventhGrade));

    /// <summary>
    /// 八年级
    /// </summary>
    [Description("八年级")]
    public static GradeName EighthGrade = new(8, nameof(EighthGrade));

    /// <summary>
    /// 九年级
    /// </summary>
    [Description("九年级")]
    public static GradeName NinthGrade = new(9, nameof(NinthGrade));

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private GradeName(int id, string name) : base(id, name)
    {
    }
}