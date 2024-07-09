using System.Text.RegularExpressions;

namespace Artemis.Data.Core.Fundamental;

/// <summary>
///     匹配器
/// </summary>
public static class Matcher
{
    #region Email

    /// <summary>
    ///     电子邮件匹配模式
    /// </summary>
    public static string EmailPattern =>
        @"^(([^<>()[\]\\.,;:\s@""]+(\.[^<>()[\]\\.,;:\s@""]+)*)|("".+""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$";

    /// <summary>
    ///     匹配电子邮件
    /// </summary>
    /// <returns></returns>
    public static Regex EmailMatcher => new(EmailPattern);

    #endregion

    #region IdentityCardNumber

    /// <summary>
    ///     身份证号码匹配模式缓存
    /// </summary>
    private static IDictionary<IdCardGenerationMode, string>? _identityCardNumberPatternCache;

    /// <summary>
    ///     身份证号码匹配模式缓存
    /// </summary>
    private static IDictionary<IdCardGenerationMode, string> IdentityCardNumberPatternCache =>
        _identityCardNumberPatternCache ??= new Dictionary<IdCardGenerationMode, string>
        {
            { IdCardGenerationMode.First, @"^[1-9]\d{7}(?:0\d|10|11|12)(?:0[1-9]|[1-2][\d]|30|31)\d{3}$" },
            {
                IdCardGenerationMode.Second,
                @"^[1-9]\d{5}(?:18|19|20)\d{2}(?:0[1-9]|10|11|12)(?:0[1-9]|[1-2]\d|30|31)\d{3}[\dXx]$"
            },
            {
                IdCardGenerationMode.Mix,
                @"^\d{6}((((((19|20)\d{2})(0[13-9]|1[012])(0[1-9]|[12]\d|30))|(((19|20)\d{2})(0[13578]|1[02])31)|((19|20)\d{2})02(0[1-9]|1\d|2[0-8])|((((19|20)([13579][26]|[2468][048]|0[48]))|(2000))0229))\d{3})|((((\d{2})(0[13-9]|1[012])(0[1-9]|[12]\d|30))|((\d{2})(0[13578]|1[02])31)|((\d{2})02(0[1-9]|1\d|2[0-8]))|(([13579][26]|[2468][048]|0[048])0229))\d{2}))(\d|X|x)$"
            }
        };

    /// <summary>
    ///     身份证号码匹配模式
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static string IdentityCardNumberPattern(IdCardGenerationMode mode = IdCardGenerationMode.Mix)
    {
        if (IdentityCardNumberPatternCache.ContainsKey(mode)) return IdentityCardNumberPatternCache[mode];
        throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
    }

    /// <summary>
    ///     身份证号码匹配器
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    public static Regex IdentityCardNumberMatcher(IdCardGenerationMode mode = IdCardGenerationMode.Mix)
    {
        return new Regex(IdentityCardNumberPattern(mode));
    }

    #endregion

    #region Md5

    /// <summary>
    ///     Md5匹配模式缓存
    /// </summary>
    private static IDictionary<(Md5DigestMode, CharacterCaseMode), string>? _md5PatternCache;

    /// <summary>
    ///     Md5匹配模式缓存
    /// </summary>
    private static IDictionary<(Md5DigestMode, CharacterCaseMode), string> Md5PatternCache =>
        _md5PatternCache ??= new Dictionary<(Md5DigestMode, CharacterCaseMode), string>
        {
            { (Md5DigestMode.Digest16, CharacterCaseMode.UpperCase), "^[A-F0-9]{16}$" },
            { (Md5DigestMode.Digest16, CharacterCaseMode.LowerCase), "^[a-f0-9]{16}$" },
            { (Md5DigestMode.Digest16, CharacterCaseMode.Mix), "^([A-F0-9]{16})|([a-f0-9]{16})$" },
            { (Md5DigestMode.Digest32, CharacterCaseMode.UpperCase), "^[A-F0-9]{32}$" },
            { (Md5DigestMode.Digest32, CharacterCaseMode.LowerCase), "^[a-f0-9]{32}$" },
            { (Md5DigestMode.Digest32, CharacterCaseMode.Mix), "^([A-F0-9]{32})|([a-f0-9]{32})$" }
        };

    /// <summary>
    ///     Md5匹配模式
    /// </summary>
    /// <param name="digestMode"></param>
    /// <param name="caseMode"></param>
    /// <returns></returns>
    public static string Md5Pattern(
        Md5DigestMode digestMode = Md5DigestMode.Digest32,
        CharacterCaseMode caseMode = CharacterCaseMode.Mix)
    {
        if (Md5PatternCache.ContainsKey((digestMode, caseMode))) return Md5PatternCache[(digestMode, caseMode)];

        throw new ArgumentOutOfRangeException($"{nameof(digestMode)},{nameof(caseMode)}", $"{digestMode},{caseMode}");
    }

    /// <summary>
    ///     Md5哈希码匹配器
    /// </summary>
    /// <param name="digestMode"></param>
    /// <param name="caseMode"></param>
    /// <returns></returns>
    public static Regex Md5Matcher(
        Md5DigestMode digestMode = Md5DigestMode.Digest32,
        CharacterCaseMode caseMode = CharacterCaseMode.Mix)
    {
        return new Regex(Md5Pattern(digestMode, caseMode));
    }

    #endregion

    #region Guid

    /// <summary>
    ///     Guid匹配模式缓存
    /// </summary>
    private static IDictionary<(GuidFormat, CharacterCaseMode), string>? _guidPatternCache;

    /// <summary>
    ///     Guid匹配模式缓存
    /// </summary>
    private static IDictionary<(GuidFormat, CharacterCaseMode), string> GuidPatternCache =>
        _guidPatternCache ??= new Dictionary<(GuidFormat, CharacterCaseMode), string>
        {
            { (GuidFormat.N, CharacterCaseMode.UpperCase), "^([A-F0-9]{32})$" },
            { (GuidFormat.N, CharacterCaseMode.LowerCase), "^([a-f0-9]{32})$" },
            { (GuidFormat.N, CharacterCaseMode.Mix), "^([(A-F|a-f)0-9]{32})$" },
            { (GuidFormat.D, CharacterCaseMode.UpperCase), "^([A-F0-9]{8}-([A-F0-9]{4}-){3}[A-F0-9]{12})$" },
            { (GuidFormat.D, CharacterCaseMode.LowerCase), "^([a-f0-9]{8}-([a-f0-9]{4}-){3}[a-f0-9]{12})$" },
            {
                (GuidFormat.D, CharacterCaseMode.Mix),
                "^([A-F0-9]{8}-([A-F0-9]{4}-){3}[A-F0-9]{12})|([a-f0-9]{8}-([a-f0-9]{4}-){3}[a-f0-9]{12})$"
            },
            { (GuidFormat.B, CharacterCaseMode.UpperCase), @"^(\{[A-F0-9]{8}-([A-F0-9]{4}-){3}[A-F0-9]{12}\})$" },
            { (GuidFormat.B, CharacterCaseMode.LowerCase), @"^(\{[a-f0-9]{8}-([a-f0-9]{4}-){3}[a-f0-9]{12}\})$" },
            {
                (GuidFormat.B, CharacterCaseMode.Mix),
                @"^(\{[A-F0-9]{8}-([A-F0-9]{4}-){3}[A-F0-9]{12}\})|(\{[a-f0-9]{8}-([a-f0-9]{4}-){3}[a-f0-9]{12}\})$"
            },
            { (GuidFormat.P, CharacterCaseMode.UpperCase), @"^(\([A-F0-9]{8}-([A-F0-9]{4}-){3}[A-F0-9]{12}\))$" },
            { (GuidFormat.P, CharacterCaseMode.LowerCase), @"^(\([a-f0-9]{8}-([a-f0-9]{4}-){3}[a-f0-9]{12}\))$" },
            {
                (GuidFormat.P, CharacterCaseMode.Mix),
                @"^(\([A-F0-9]{8}-([A-F0-9]{4}-){3}[A-F0-9]{12}\))|(\([a-f0-9]{8}-([a-f0-9]{4}-){3}[a-f0-9]{12}\))$"
            },
            {
                (GuidFormat.Mix, CharacterCaseMode.UpperCase),
                "^([A-F0-9]{32})|([A-F0-9]{8}-([A-F0-9]{4}-){3}[A-F0-9]{12})$"
            },
            {
                (GuidFormat.Mix, CharacterCaseMode.LowerCase),
                "^([a-f0-9]{32})|([a-f0-9]{8}-([a-f0-9]{4}-){3}[a-f0-9]{12})$"
            },
            {
                (GuidFormat.Mix, CharacterCaseMode.Mix),
                "^([A-F0-9]{32})|([A-F0-9]{8}-([A-F0-9]{4}-){3}[A-F0-9]{12})|([a-f0-9]{32})|([a-f0-9]{8}-([a-f0-9]{4}-){3}[a-f0-9]{12})$"
            }
        };

    /// <summary>
    ///     Guid匹配模式
    /// </summary>
    /// <param name="guidMode"></param>
    /// <param name="caseMode"></param>
    /// <returns></returns>
    public static string GuidPattern(
        GuidFormat guidMode = GuidFormat.D,
        CharacterCaseMode caseMode = CharacterCaseMode.Mix)
    {
        if (GuidPatternCache.ContainsKey((guidMode, caseMode))) return GuidPatternCache[(guidMode, caseMode)];

        throw new ArgumentOutOfRangeException($"{nameof(guidMode)},{nameof(caseMode)}", $"{guidMode},{caseMode}");
    }

    /// <summary>
    ///     Guid匹配器
    /// </summary>
    /// <param name="guidMode"></param>
    /// <param name="caseMode"></param>
    /// <returns></returns>
    public static Regex GuidMatcher(
        GuidFormat guidMode = GuidFormat.D,
        CharacterCaseMode caseMode = CharacterCaseMode.Mix)
    {
        return new Regex(GuidPattern(guidMode, caseMode));
    }

    #endregion

    #region Date

    /// <summary>
    ///     严格模式日期匹配模式
    /// </summary>
    private const string StrictDatePattern =
        @"^(([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})-(((0[13578]|1[02])-(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)-(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8]))))|((([0-9]{2})(0[48]|[2468][048]|[13579][26])|((0[48]|[2468][048]|[3579][26])00))-02-29)$";

    /// <summary>
    ///     宽松模式日期匹配模式
    /// </summary>
    private const string LaxDatePattern = @"^\d{1,4}(-)(1[0-2]|0?[1-9])\1(0?[1-9]|[1-2]\d|30|31)$";

    /// <summary>
    ///     日期匹配模式缓存
    /// </summary>
    private static IDictionary<DatePatternMode, string>? _datePatternCache;

    /// <summary>
    ///     日期匹配模式缓存
    /// </summary>
    private static IDictionary<DatePatternMode, string> DatePatternCache =>
        _datePatternCache ??= new Dictionary<DatePatternMode, string>
        {
            { DatePatternMode.Strict, StrictDatePattern },
            { DatePatternMode.Lax, LaxDatePattern }
        };

    /// <summary>
    ///     日期匹配模式
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    public static string DatePattern(DatePatternMode mode = DatePatternMode.Strict)
    {
        if (DatePatternCache.ContainsKey(mode)) return DatePatternCache[mode];
        throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
    }

    /// <summary>
    ///     日期匹配器
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    public static Regex DateMatcher(DatePatternMode mode = DatePatternMode.Strict)
    {
        return new Regex(DatePattern(mode));
    }

    #endregion

    #region Time

    /// <summary>
    ///     时间匹配模式缓存
    /// </summary>
    private static IDictionary<TimePatternMode, string>? _timePatternCache;

    /// <summary>
    ///     时间匹配模式缓存
    /// </summary>
    private static IDictionary<TimePatternMode, string> TimePatternCache =>
        _timePatternCache ??= new Dictionary<TimePatternMode, string>
        {
            { TimePatternMode.Time24, @"^(?:[01]\d|2[0-3]):[0-5]\d:[0-5]\d$" },
            { TimePatternMode.Time12, @"^(?:1[0-2]|0?[1-9]):[0-5]\d:[0-5]\d$" }
        };

    /// <summary>
    ///     时间匹配模式
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    public static string TimePattern(TimePatternMode mode = TimePatternMode.Time24)
    {
        if (TimePatternCache.ContainsKey(mode)) return TimePatternCache[mode];
        throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
    }

    /// <summary>
    ///     时间匹配器
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    public static Regex TimeMatcher(TimePatternMode mode = TimePatternMode.Time24)
    {
        return new Regex(TimePattern(mode));
    }

    #endregion

    #region Content

    /// <summary>
    ///     内容匹配模式缓存
    /// </summary>
    private static IDictionary<ContentPatternMode, string>? _contentPatternCache;

    /// <summary>
    ///     内容匹配模式缓存
    /// </summary>
    private static IDictionary<ContentPatternMode, string> ContentPatternCache =>
        _contentPatternCache ??= new Dictionary<ContentPatternMode, string>
        {
            { ContentPatternMode.Parentheses, @"\((.*?)\)" },
            { ContentPatternMode.Bracket, @"\[(.*?)\]" },
            { ContentPatternMode.Braces, @"\{(.*?)\}" }
        };

    /// <summary>
    ///     内容匹配模式
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    public static string ContentPattern(ContentPatternMode mode = ContentPatternMode.Bracket)
    {
        if (ContentPatternCache.ContainsKey(mode)) return ContentPatternCache[mode];
        throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
    }

    /// <summary>
    ///     中括号内内容匹配器
    /// </summary>
    /// <param name="mode">匹配模式</param>
    /// <returns></returns>
    public static Regex ContentMatcher(ContentPatternMode mode = ContentPatternMode.Bracket)
    {
        return new Regex(ContentPattern(mode));
    }

    #endregion

    #region Phone

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
    ///     内容匹配模式缓存
    /// </summary>
    private static IDictionary<PhonePatternMode, string>? _phonePatternCache;

    /// <summary>
    ///     内容匹配模式缓存
    /// </summary>
    private static IDictionary<PhonePatternMode, string> PhonePatternCache =>
        _phonePatternCache ??= new Dictionary<PhonePatternMode, string>
        {
            { PhonePatternMode.Strict, StrictPhonePattern },
            { PhonePatternMode.Normal, NormalPhonePattern },
            { PhonePatternMode.Lax, LaxPhonePattern }
        };

    /// <summary>
    ///     电话号码匹配模式(国内)
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    public static string PhonePattern(PhonePatternMode mode = PhonePatternMode.Normal)
    {
        if (PhonePatternCache.ContainsKey(mode)) return PhonePatternCache[mode];
        throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
    }

    /// <summary>
    ///     匹配手机号码
    /// </summary>
    /// <param name="mode">匹配模式</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static Regex PhoneMatcher(PhonePatternMode mode = PhonePatternMode.Normal)
    {
        return new Regex(PhonePattern(mode));
    }

    #endregion

    #region Telephone

    /// <summary>
    ///     固定电话匹配模式
    /// </summary>
    public static string TelephonePattern => @"^(?:(?:\d{3}-)?\d{8}|^(?:\d{4}-)?\d{7,8})(?:-\d+)?$";

    /// <summary>
    ///     固定电话匹配模式
    /// </summary>
    /// <returns></returns>
    public static Regex TelephoneMatcher => new(TelephonePattern);

    #endregion

    #region VideoUrlPattern

    /// <summary>
    ///     视频地址匹配模式
    /// </summary>
    public static string VideoUrlPattern =>
        @"^https?:\/\/(.+\/)+.+(\.(swf|avi|flv|mpg|rm|mov|wav|asf|3gp|mkv|rmvb|mp4))$";

    /// <summary>
    ///     视频地址匹配器
    /// </summary>
    public static Regex VideoUrlMatcher => new(VideoUrlPattern);

    #endregion

    #region ImageUrlPattern

    /// <summary>
    ///     图片地址匹配模式
    /// </summary>
    public static string ImageUrlPattern => @"^https?:\/\/(.+\/)+.+(\.(gif|png|jpg|jpeg|webp|bmp))$";

    /// <summary>
    ///     图片地址匹配器
    /// </summary>
    public static Regex ImageUrlMatcher => new(ImageUrlPattern);

    #endregion

    #region LicencePlateNumber

    /// <summary>
    ///     新能源车牌号匹配模式
    /// </summary>
    private const string NewEnergyLicencePlateNumberPattern =
        "^[京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领][A-HJ-NP-Z](([DF]((?![IO])[a-zA-Z0-9](?![IO]))[0-9]{4})|([0-9]{5}[DF]))$";

    /// <summary>
    ///     非新能源车牌号匹配模式
    /// </summary>
    private const string NonNewEnergyLicencePlateNumberPattern =
        "^[京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领][A-HJ-NP-Z][A-HJ-NP-Z0-9]{4}[A-HJ-NP-Z0-9挂学警港澳]$";

    /// <summary>
    ///     混合车牌号匹配模式
    /// </summary>
    private const string MixLicencePlateNumberPattern =
        "^[京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领][A-HJ-NP-Z][A-HJ-NP-Z0-9]{4,5}[A-HJ-NP-Z0-9挂学警港澳]";

    /// <summary>
    ///     车牌号匹配模式缓存
    /// </summary>
    private static IDictionary<LicensePlateNumberPatternMode, string>? _licencePlateNumberPatternNumberPatternCache;

    /// <summary>
    ///     车牌号匹配模式缓存
    /// </summary>
    private static IDictionary<LicensePlateNumberPatternMode, string> LicencePlateNumberPatternCache =>
        _licencePlateNumberPatternNumberPatternCache ??= new Dictionary<LicensePlateNumberPatternMode, string>
        {
            { LicensePlateNumberPatternMode.NewEnergy, NewEnergyLicencePlateNumberPattern },
            { LicensePlateNumberPatternMode.NonNewEnergy, NonNewEnergyLicencePlateNumberPattern },
            { LicensePlateNumberPatternMode.Mix, MixLicencePlateNumberPattern }
        };

    /// <summary>
    ///     车牌号匹配模式
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    public static string CarPattern(LicensePlateNumberPatternMode mode = LicensePlateNumberPatternMode.Mix)
    {
        if (LicencePlateNumberPatternCache.ContainsKey(mode)) return LicencePlateNumberPatternCache[mode];
        throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
    }

    /// <summary>
    ///     匹配车牌号
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static Regex CarMatcher(LicensePlateNumberPatternMode mode = LicensePlateNumberPatternMode.Mix)
    {
        return new Regex(CarPattern(mode));
    }

    #endregion
}