using System.Text.RegularExpressions;

namespace Artemis.Data.Core.Fundamental;

/// <summary>
///     匹配器
/// </summary>
public static class Matcher
{
    /// <summary>
    ///     严格模式
    /// </summary>
    private const string StrictPhonePattern =
        @"^(?:(?:\+|00)86)?1(?:(?:3[\d])|(?:4[5-79])|(?:5[0-35-9])|(?:6[5-7])|(?:7[0-8])|(?:8[\d])|(?:9[01256789]))\d{8}$";

    /// <summary>
    ///     正常模式
    /// </summary>
    private const string NormalPhonePattern = @"^(?:(?:\+|00)86)?1[3-9]\d{9}$";

    /// <summary>
    ///     宽松模式
    /// </summary>
    private const string LaxPhonePattern = @"^(?:(?:\+|00)86)?1\d{10}$";

    /// <summary>
    ///     新能源车牌号匹配模式
    /// </summary>
    private const string NewEnergyCarNumberPattern =
        "^[京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领][A-HJ-NP-Z](([DF]((?![IO])[a-zA-Z0-9](?![IO]))[0-9]{4})|([0-9]{5}[DF]))$";

    /// <summary>
    ///     非新能源车牌号匹配模式
    /// </summary>
    private const string NonNewEnergyCarNumberPattern =
        "^[京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领][A-HJ-NP-Z][A-HJ-NP-Z0-9]{4}[A-HJ-NP-Z0-9挂学警港澳]$";

    /// <summary>
    ///     混合车牌号匹配模式
    /// </summary>
    private const string MixCarNumberPattern =
        "^[京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领][A-HJ-NP-Z][A-HJ-NP-Z0-9]{4,5}[A-HJ-NP-Z0-9挂学警港澳]";

    /// <summary>
    ///     电子邮件匹配模式
    /// </summary>
    private const string EmailPattern =
        @"^(([^<>()[\]\\.,;:\s@""]+(\.[^<>()[\]\\.,;:\s@""]+)*)|("".+""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$";

    /// <summary>
    /// 中括号内内容匹配模式
    /// </summary>
    private const string BracketContentPattern = @"\[(.*?)\]";

    /// <summary>
    /// 中括号内内容匹配器
    /// </summary>
    /// <param name="mode">匹配模式</param>
    /// <returns></returns>
    public static Regex ContentMatcher(ContentMatcherMode mode = ContentMatcherMode.Bracket)
    {
        return mode switch
        {
            ContentMatcherMode.Bracket => new Regex(BracketContentPattern),
            _ => new Regex(BracketContentPattern)
        };
    }

    /// <summary>
    ///     匹配电子邮件
    /// </summary>
    /// <returns></returns>
    public static Regex EmailMatcher => new(EmailPattern);

    /// <summary>
    ///     匹配手机号码
    /// </summary>
    /// <param name="mode">匹配模式</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static Regex PhoneMatcher(PhoneMatcherMode mode = PhoneMatcherMode.Normal)
    {
        return mode switch
        {
            PhoneMatcherMode.Strict => new Regex(StrictPhonePattern),
            PhoneMatcherMode.Normal => new Regex(NormalPhonePattern),
            PhoneMatcherMode.Lax => new Regex(LaxPhonePattern),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };
    }

    /// <summary>
    ///     匹配车牌号
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static Regex CarNumberMatcher(CarNumberMatchMode mode = CarNumberMatchMode.Mix)
    {
        return mode switch
        {
            CarNumberMatchMode.NewEnergy => new Regex(NewEnergyCarNumberPattern),
            CarNumberMatchMode.NonNewEnergy => new Regex(NonNewEnergyCarNumberPattern),
            CarNumberMatchMode.Mix => new Regex(MixCarNumberPattern),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };
    }
}

/// <summary>
/// 内容匹配模式
/// </summary>
public enum ContentMatcherMode
{
    /// <summary>
    /// 小括号内内容
    /// </summary>
    Parentheses,
    /// <summary>
    /// 中括号内内容
    /// </summary>
    Bracket,
    /// <summary>
    /// 大括号内内容
    /// </summary>
    Braces
}

/// <summary>
///     手机号匹配模式
/// </summary>
public enum PhoneMatcherMode
{
    /// <summary>
    ///     严格模式
    /// </summary>
    /// <remarks>根据工信部2019年最新公布的手机号段</remarks>
    Strict,

    /// <summary>
    ///     正常模式
    /// </summary>
    /// <remarks>只要是13,14,15,16,17,18,19开头即可</remarks>
    Normal,

    /// <summary>
    ///     宽松模式
    /// </summary>
    /// <remarks>只要是1开头即可</remarks>
    Lax
}

/// <summary>
///     车牌号匹配模式
/// </summary>
public enum CarNumberMatchMode
{
    /// <summary>
    ///     新能源车牌
    /// </summary>
    NewEnergy,

    /// <summary>
    ///     非新能源车牌
    /// </summary>
    NonNewEnergy,

    /// <summary>
    ///     混合模式
    /// </summary>
    Mix
}