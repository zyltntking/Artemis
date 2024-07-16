using System.ComponentModel;

namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
///     中华民族
/// </summary>
[Description("民族类型")]
public sealed class ChineseNation : Enumeration
{
    /// <summary>
    ///     未知
    /// </summary>
    [Description(nameof(未知))] public static ChineseNation 未知 = new(0, nameof(未知));

    /// <summary>
    ///     阿昌族
    /// </summary>
    [Description(nameof(阿昌族))] public static ChineseNation 阿昌族 = new(1, nameof(阿昌族));

    /// <summary>
    ///     白族
    /// </summary>
    [Description(nameof(白族))] public static ChineseNation 白族 = new(2, nameof(白族));

    /// <summary>
    ///     保安族
    /// </summary>
    [Description(nameof(保安族))] public static ChineseNation 保安族 = new(3, nameof(保安族));

    /// <summary>
    ///     布朗族
    /// </summary>
    [Description(nameof(布朗族))] public static ChineseNation 布朗族 = new(4, nameof(布朗族));

    /// <summary>
    ///     布依族
    /// </summary>
    [Description(nameof(布依族))] public static ChineseNation 布依族 = new(5, nameof(布依族));

    /// <summary>
    ///     朝鲜族
    /// </summary>
    [Description(nameof(朝鲜族))] public static ChineseNation 朝鲜族 = new(6, nameof(朝鲜族));

    /// <summary>
    ///     达斡尔族
    /// </summary>
    [Description(nameof(达斡尔族))] public static ChineseNation 达斡尔族 = new(7, nameof(达斡尔族));

    /// <summary>
    ///     傣族
    /// </summary>
    [Description(nameof(傣族))] public static ChineseNation 傣族 = new(8, nameof(傣族));

    /// <summary>
    ///     德昂族
    /// </summary>
    [Description(nameof(德昂族))] public static ChineseNation 德昂族 = new(9, nameof(德昂族));

    /// <summary>
    ///     东乡族
    /// </summary>
    [Description(nameof(东乡族))] public static ChineseNation 东乡族 = new(10, nameof(东乡族));

    /// <summary>
    ///     侗族
    /// </summary>
    [Description(nameof(侗族))] public static ChineseNation 侗族 = new(11, nameof(侗族));

    /// <summary>
    ///     独龙族
    /// </summary>
    [Description(nameof(独龙族))] public static ChineseNation 独龙族 = new(12, nameof(独龙族));

    /// <summary>
    ///     鄂伦春族
    /// </summary>
    [Description(nameof(鄂伦春族))] public static ChineseNation 鄂伦春族 = new(13, nameof(鄂伦春族));

    /// <summary>
    ///     俄罗斯族
    /// </summary>
    [Description(nameof(俄罗斯族))] public static ChineseNation 俄罗斯族 = new(14, nameof(俄罗斯族));

    /// <summary>
    ///     鄂温克族
    /// </summary>
    [Description(nameof(鄂温克族))] public static ChineseNation 鄂温克族 = new(15, nameof(鄂温克族));

    /// <summary>
    ///     高山族
    /// </summary>
    [Description(nameof(高山族))] public static ChineseNation 高山族 = new(16, nameof(高山族));

    /// <summary>
    ///     仡佬族
    /// </summary>
    [Description(nameof(仡佬族))] public static ChineseNation 仡佬族 = new(17, nameof(仡佬族));

    /// <summary>
    ///     汉族
    /// </summary>
    [Description(nameof(汉族))] public static ChineseNation 汉族 = new(18, nameof(汉族));

    /// <summary>
    ///     哈尼族
    /// </summary>
    [Description(nameof(哈尼族))] public static ChineseNation 哈尼族 = new(19, nameof(哈尼族));

    /// <summary>
    ///     哈萨克族
    /// </summary>
    [Description(nameof(哈萨克族))] public static ChineseNation 哈萨克族 = new(20, nameof(哈萨克族));

    /// <summary>
    ///     赫哲族
    /// </summary>
    [Description(nameof(赫哲族))] public static ChineseNation 赫哲族 = new(21, nameof(赫哲族));

    /// <summary>
    ///     回族
    /// </summary>
    [Description(nameof(回族))] public static ChineseNation 回族 = new(22, nameof(回族));

    /// <summary>
    ///     基诺族
    /// </summary>
    [Description(nameof(基诺族))] public static ChineseNation 基诺族 = new(23, nameof(基诺族));

    /// <summary>
    ///     景颇族
    /// </summary>
    [Description(nameof(景颇族))] public static ChineseNation 景颇族 = new(24, nameof(景颇族));

    /// <summary>
    ///     京族
    /// </summary>
    [Description(nameof(京族))] public static ChineseNation 京族 = new(25, nameof(京族));

    /// <summary>
    ///     柯尔克孜族
    /// </summary>
    [Description(nameof(柯尔克孜族))] public static ChineseNation 柯尔克孜族 = new(26, nameof(柯尔克孜族));

    /// <summary>
    ///     拉祜族
    /// </summary>
    [Description(nameof(拉祜族))] public static ChineseNation 拉祜族 = new(27, nameof(拉祜族));

    /// <summary>
    ///     珞巴族
    /// </summary>
    [Description(nameof(珞巴族))] public static ChineseNation 珞巴族 = new(28, nameof(珞巴族));

    /// <summary>
    ///     僳僳族
    /// </summary>
    [Description(nameof(僳僳族))] public static ChineseNation 僳僳族 = new(29, nameof(僳僳族));

    /// <summary>
    ///     黎族
    /// </summary>
    [Description(nameof(黎族))] public static ChineseNation 黎族 = new(30, nameof(黎族));

    /// <summary>
    ///     满族
    /// </summary>
    [Description(nameof(满族))] public static ChineseNation 满族 = new(31, nameof(满族));

    /// <summary>
    ///     毛南族
    /// </summary>
    [Description(nameof(毛南族))] public static ChineseNation 毛南族 = new(32, nameof(毛南族));

    /// <summary>
    ///     门巴族
    /// </summary>
    [Description(nameof(门巴族))] public static ChineseNation 门巴族 = new(33, nameof(门巴族));

    /// <summary>
    ///     蒙古族
    /// </summary>
    [Description(nameof(蒙古族))] public static ChineseNation 蒙古族 = new(34, nameof(蒙古族));

    /// <summary>
    ///     苗族
    /// </summary>
    [Description(nameof(苗族))] public static ChineseNation 苗族 = new(35, nameof(苗族));

    /// <summary>
    ///     仫佬族
    /// </summary>
    [Description(nameof(仫佬族))] public static ChineseNation 仫佬族 = new(36, nameof(仫佬族));

    /// <summary>
    ///     纳西族
    /// </summary>
    [Description(nameof(纳西族))] public static ChineseNation 纳西族 = new(37, nameof(纳西族));

    /// <summary>
    ///     怒族
    /// </summary>
    [Description(nameof(怒族))] public static ChineseNation 怒族 = new(38, nameof(怒族));

    /// <summary>
    ///     普米族
    /// </summary>
    [Description(nameof(普米族))] public static ChineseNation 普米族 = new(39, nameof(普米族));

    /// <summary>
    ///     羌族
    /// </summary>
    [Description(nameof(羌族))] public static ChineseNation 羌族 = new(40, nameof(羌族));

    /// <summary>
    ///     撒拉族
    /// </summary>
    [Description(nameof(撒拉族))] public static ChineseNation 撒拉族 = new(41, nameof(撒拉族));

    /// <summary>
    ///     畲族
    /// </summary>
    [Description(nameof(畲族))] public static ChineseNation 畲族 = new(42, nameof(畲族));

    /// <summary>
    ///     水族
    /// </summary>
    [Description(nameof(水族))] public static ChineseNation 水族 = new(43, nameof(水族));

    /// <summary>
    ///     塔吉克族
    /// </summary>
    [Description(nameof(塔吉克族))] public static ChineseNation 塔吉克族 = new(44, nameof(塔吉克族));

    /// <summary>
    ///     塔塔尔族
    /// </summary>
    [Description(nameof(塔塔尔族))] public static ChineseNation 塔塔尔族 = new(45, nameof(塔塔尔族));

    /// <summary>
    ///     土家族
    /// </summary>
    [Description(nameof(土家族))] public static ChineseNation 土家族 = new(46, nameof(土家族));

    /// <summary>
    ///     土族
    /// </summary>
    [Description(nameof(土族))] public static ChineseNation 土族 = new(47, nameof(土族));

    /// <summary>
    ///     佤族
    /// </summary>
    [Description(nameof(佤族))] public static ChineseNation 佤族 = new(48, nameof(佤族));

    /// <summary>
    ///     维吾尔族
    /// </summary>
    [Description(nameof(维吾尔族))] public static ChineseNation 维吾尔族 = new(49, nameof(维吾尔族));

    /// <summary>
    ///     乌孜别克族
    /// </summary>
    [Description(nameof(乌孜别克族))] public static ChineseNation 乌孜别克族 = new(50, nameof(乌孜别克族));

    /// <summary>
    ///     锡伯族
    /// </summary>
    [Description(nameof(锡伯族))] public static ChineseNation 锡伯族 = new(51, nameof(锡伯族));

    /// <summary>
    ///     瑶族
    /// </summary>
    [Description(nameof(瑶族))] public static ChineseNation 瑶族 = new(52, nameof(瑶族));

    /// <summary>
    ///     彝族
    /// </summary>
    [Description(nameof(彝族))] public static ChineseNation 彝族 = new(53, nameof(彝族));

    /// <summary>
    ///     裕固族
    /// </summary>
    [Description(nameof(裕固族))] public static ChineseNation 裕固族 = new(54, nameof(裕固族));

    /// <summary>
    ///     藏族
    /// </summary>
    [Description(nameof(藏族))] public static ChineseNation 藏族 = new(55, nameof(藏族));

    /// <summary>
    ///     壮族
    /// </summary>
    [Description(nameof(壮族))] public static ChineseNation 壮族 = new(56, nameof(壮族));


    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private ChineseNation(int id, string name) : base(id, name)
    {
    }
}