using System.ComponentModel;

namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
///     中华民族
/// </summary>
[Description("民族类型(英文标识)")]
public sealed class ChineseNationEn : Enumeration
{
    /// <summary>
    ///     未知
    /// </summary>
    [Description("未知")] public static ChineseNationEn Unknown = new(0, nameof(Unknown));

    ///<summary>阿昌族</summary>
    [Description("阿昌族")] public static ChineseNationEn Achangzu = new(1, nameof(Achangzu));

    ///<summary>白族</summary>
    [Description("白族")] public static ChineseNationEn Baizu = new(2, nameof(Baizu));

    ///<summary>保安族</summary>
    [Description("保安族")] public static ChineseNationEn Baoanzu = new(3, nameof(Baoanzu));

    ///<summary>布朗族</summary>
    [Description("布朗族")] public static ChineseNationEn Bulangzu = new(4, nameof(Bulangzu));

    ///<summary>布依族</summary>
    [Description("布依族")] public static ChineseNationEn Buyizu = new(5, nameof(Buyizu));

    ///<summary>朝鲜族</summary>
    [Description("朝鲜族")] public static ChineseNationEn Chaoxianzu = new(6, nameof(Chaoxianzu));

    ///<summary>达斡尔族</summary>
    [Description("达斡尔族")] public static ChineseNationEn Dawoerzu = new(7, nameof(Dawoerzu));

    ///<summary>傣族</summary>
    [Description("傣族")] public static ChineseNationEn Daizu = new(8, nameof(Daizu));

    ///<summary>德昂族</summary>
    [Description("德昂族")] public static ChineseNationEn Deangzu = new(9, nameof(Deangzu));

    ///<summary>东乡族</summary>
    [Description("东乡族")] public static ChineseNationEn Dongxiangzu = new(10, nameof(Dongxiangzu));

    ///<summary>侗族</summary>
    [Description("侗族")] public static ChineseNationEn Dongzu = new(11, nameof(Dongzu));

    ///<summary>独龙族</summary>
    [Description("独龙族")] public static ChineseNationEn Dulongzu = new(12, nameof(Dulongzu));

    ///<summary>鄂伦春族</summary>
    [Description("鄂伦春族")] public static ChineseNationEn Elunchunzu = new(13, nameof(Elunchunzu));

    ///<summary>俄罗斯族</summary>
    [Description("俄罗斯族")] public static ChineseNationEn Eluosizu = new(14, nameof(Eluosizu));

    ///<summary>鄂温克族</summary>
    [Description("鄂温克族")] public static ChineseNationEn Ewenkezu = new(15, nameof(Ewenkezu));

    ///<summary>高山族</summary>
    [Description("高山族")] public static ChineseNationEn Gaoshanzu = new(16, nameof(Gaoshanzu));

    ///<summary>仡佬族</summary>
    [Description("仡佬族")] public static ChineseNationEn Gelaozu = new(17, nameof(Gelaozu));

    ///<summary>汉族</summary>
    [Description("汉族")] public static ChineseNationEn Hanzu = new(18, nameof(Hanzu));

    ///<summary>哈尼族</summary>
    [Description("哈尼族")] public static ChineseNationEn Hanizu = new(19, nameof(Hanizu));

    ///<summary>哈萨克族</summary>
    [Description("哈萨克族")] public static ChineseNationEn Hasakezu = new(20, nameof(Hasakezu));

    ///<summary>赫哲族</summary>
    [Description("赫哲族")] public static ChineseNationEn Hezhezu = new(21, nameof(Hezhezu));

    ///<summary>回族</summary>
    [Description("回族")] public static ChineseNationEn Huizu = new(22, nameof(Huizu));

    ///<summary>基诺族</summary>
    [Description("基诺族")] public static ChineseNationEn Jinuozu = new(23, nameof(Jinuozu));

    ///<summary>景颇族</summary>
    [Description("景颇族")] public static ChineseNationEn Jingpozu = new(24, nameof(Jingpozu));

    ///<summary>京族</summary>
    [Description("京族")] public static ChineseNationEn Jingzu = new(25, nameof(Jingzu));

    ///<summary>柯尔克孜族</summary>
    [Description("柯尔克孜族")] public static ChineseNationEn Keerkezizu = new(26, nameof(Keerkezizu));

    ///<summary>拉祜族</summary>
    [Description("拉祜族")] public static ChineseNationEn Lahuzu = new(27, nameof(Lahuzu));

    ///<summary>珞巴族</summary>
    [Description("珞巴族")] public static ChineseNationEn Luobazu = new(28, nameof(Luobazu));

    ///<summary>僳僳族</summary>
    [Description("僳僳族")] public static ChineseNationEn Lisuzu = new(29, nameof(Lisuzu));

    ///<summary>黎族</summary>
    [Description("黎族")] public static ChineseNationEn Lizu = new(30, nameof(Lizu));

    ///<summary>满族</summary>
    [Description("满族")] public static ChineseNationEn Manzu = new(31, nameof(Manzu));

    ///<summary>毛南族</summary>
    [Description("毛南族")] public static ChineseNationEn Maonanzu = new(32, nameof(Maonanzu));

    ///<summary>门巴族</summary>
    [Description("门巴族")] public static ChineseNationEn Menbazu = new(33, nameof(Menbazu));

    ///<summary>蒙古族</summary>
    [Description("蒙古族")] public static ChineseNationEn Mengguzu = new(34, nameof(Mengguzu));

    ///<summary>苗族</summary>
    [Description("苗族")] public static ChineseNationEn Miaozu = new(35, nameof(Miaozu));

    ///<summary>仫佬族</summary>
    [Description("仫佬族")] public static ChineseNationEn Mulaozu = new(36, nameof(Mulaozu));

    ///<summary>纳西族</summary>
    [Description("纳西族")] public static ChineseNationEn Naxizu = new(37, nameof(Naxizu));

    ///<summary>怒族</summary>
    [Description("怒族")] public static ChineseNationEn Nuzu = new(38, nameof(Nuzu));

    ///<summary>普米族</summary>
    [Description("普米族")] public static ChineseNationEn Pumizu = new(39, nameof(Pumizu));

    ///<summary>羌族</summary>
    [Description("羌族")] public static ChineseNationEn Qiangzu = new(40, nameof(Qiangzu));

    ///<summary>撒拉族</summary>
    [Description("撒拉族")] public static ChineseNationEn Salazu = new(41, nameof(Salazu));

    ///<summary>畲族</summary>
    [Description("畲族")] public static ChineseNationEn Shezu = new(42, nameof(Shezu));

    ///<summary>水族</summary>
    [Description("水族")] public static ChineseNationEn Shuizu = new(43, nameof(Shuizu));

    ///<summary>塔吉克族</summary>
    [Description("塔吉克族")] public static ChineseNationEn Tajikezu = new(44, nameof(Tajikezu));

    ///<summary>塔塔尔族</summary>
    [Description("塔塔尔族")] public static ChineseNationEn Tataerzu = new(45, nameof(Tataerzu));

    ///<summary>土家族</summary>
    [Description("土家族")] public static ChineseNationEn Tujiazu = new(46, nameof(Tujiazu));

    ///<summary>土族</summary>
    [Description("土族")] public static ChineseNationEn Tuzu = new(47, nameof(Tuzu));

    ///<summary>佤族</summary>
    [Description("佤族")] public static ChineseNationEn Wazu = new(48, nameof(Wazu));

    ///<summary>维吾尔族</summary>
    [Description("维吾尔族")] public static ChineseNationEn Weiwuerzu = new(49, nameof(Weiwuerzu));

    ///<summary>乌孜别克族</summary>
    [Description("乌孜别克族")] public static ChineseNationEn Wuzibiekezu = new(50, nameof(Wuzibiekezu));

    ///<summary>锡伯族</summary>
    [Description("锡伯族")] public static ChineseNationEn Xibozu = new(51, nameof(Xibozu));

    ///<summary>瑶族</summary>
    [Description("瑶族")] public static ChineseNationEn Yaozu = new(52, nameof(Yaozu));

    ///<summary>彝族</summary>
    [Description("彝族")] public static ChineseNationEn Yizu = new(53, nameof(Yizu));

    ///<summary>裕固族</summary>
    [Description("裕固族")] public static ChineseNationEn Yuguzu = new(54, nameof(Yuguzu));

    ///<summary>藏族</summary>
    [Description("藏族")] public static ChineseNationEn Zangzu = new(55, nameof(Zangzu));

    ///<summary>壮族</summary>
    [Description("壮族")] public static ChineseNationEn Zhuangzu = new(56, nameof(Zhuangzu));


    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private ChineseNationEn(int id, string name) : base(id, name)
    {
    }
}