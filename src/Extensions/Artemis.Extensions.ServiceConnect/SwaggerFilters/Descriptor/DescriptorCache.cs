using Artemis.Data.Core;
using Artemis.Data.Core.Fundamental;

namespace Artemis.Extensions.ServiceConnect.SwaggerFilters.Descriptor;

/// <summary>
///     描述器缓存
/// </summary>
internal static class DescriptorCache
{
    #region RequiredFieldMap

    /// <summary>
    ///     必要属性映射缓存
    /// </summary>
    private static IDictionary<string, ISet<string>>? _requiredFieldsMap;

    /// <summary>
    ///     必要属性映射访问器
    /// </summary>
    internal static IDictionary<string, ISet<string>> RequiredFieldsMap =>
        _requiredFieldsMap ??= new Dictionary<string, ISet<string>>();

    #endregion

    #region FormatMap

    #region Formatctions

    /// <summary>
    ///     格式化日期时间
    /// </summary>
    /// <param name="schema"></param>
    private static void FormatDateTime(SchemaInfo schema)
    {
        schema.Format ??= "date-time";
    }

    /// <summary>
    ///     格式化日期
    /// </summary>
    /// <param name="schema"></param>
    private static void FormatDate(SchemaInfo schema)
    {
        schema.Format ??= "date";
    }

    /// <summary>
    ///     格式化时间
    /// </summary>
    /// <param name="schema"></param>
    private static void FormatTime(SchemaInfo schema)
    {
        schema.Format ??= "time";
        schema.Pattern ??= Matcher.TimePattern();
    }

    /// <summary>
    ///     格式化时间戳
    /// </summary>
    /// <param name="schema"></param>
    private static void FormatTimestamp(SchemaInfo schema)
    {
        schema.Format ??= "timestamp";
        schema.Minimum ??= 0;
        schema.Example ??= DateTime.Now.ToUnixTimeStamp().ToString("D");
        // todo more pattern
    }

    /// <summary>
    ///     格式化邮箱
    /// </summary>
    /// <param name="schema"></param>
    private static void FormatEmail(SchemaInfo schema)
    {
        schema.Format ??= "email";
    }

    /// <summary>
    ///     格式化手机号码
    /// </summary>
    /// <param name="schema"></param>
    private static void FormatPhone(SchemaInfo schema)
    {
        schema.Format ??= "phone";
        schema.MinLength ??= 11;
        schema.MaxLength ??= 16;
        schema.Pattern ??= Matcher.PhonePattern(PhonePatternMode.Lax);
    }

    /// <summary>
    ///     格式化主机
    /// </summary>
    /// <param name="schema"></param>
    private static void FormatHostName(SchemaInfo schema)
    {
        schema.Format ??= "hostname";
    }

    /// <summary>
    ///     格式化ipv4
    /// </summary>
    /// <param name="schema"></param>
    private static void FormatIpv4(SchemaInfo schema)
    {
        schema.Format ??= "ipv4";
    }

    /// <summary>
    ///     格式化ipv6
    /// </summary>
    /// <param name="schema"></param>
    private static void FormatIpv6(SchemaInfo schema)
    {
        schema.Format ??= "ipv6";
    }

    /// <summary>
    ///     格式化uri
    /// </summary>
    /// <param name="schema"></param>
    private static void FormatUri(SchemaInfo schema)
    {
        schema.Format ??= "uri";
        // todo custom pattern and example
    }

    /// <summary>
    ///     格式化md5
    /// </summary>
    /// <param name="schema"></param>
    private static void FormatMd5(SchemaInfo schema)
    {
        schema.Format ??= "md5";
        schema.MinLength ??= 32;
        schema.MaxLength ??= 32;
        schema.Pattern ??= Matcher.Md5Pattern();
    }

    /// <summary>
    ///     格式化guid
    /// </summary>
    /// <param name="schema"></param>
    private static void FormatGuid(SchemaInfo schema)
    {
        schema.Format ??= "guid";
        schema.MinLength ??= 32;
        schema.MaxLength ??= 38;
        schema.Pattern ??= Matcher.GuidPattern();
    }

    /// <summary>
    ///     格式化车牌号
    /// </summary>
    /// <param name="schema"></param>
    private static void FormatCar(SchemaInfo schema)
    {
        schema.Format ??= "car";
        //schema.Example ??= "粤A12345";
        //schema.Pattern ??= Matcher.CarPattern();
    }

    /// <summary>
    ///     格式化密码
    /// </summary>
    /// <param name="schema"></param>
    private static void FormatPassword(SchemaInfo schema)
    {
        schema.Format ??= "password";
        schema.IsPassword = true;
    }

    /// <summary>
    ///     格式化身份证号码
    /// </summary>
    /// <param name="schema"></param>
    private static void FormatIdCard(SchemaInfo schema)
    {
        schema.Format ??= "idcard";
        schema.MinLength ??= 15;
        schema.MaxLength ??= 18;
        schema.Pattern ??= Matcher.IdentityCardNumberPattern();
    }

    /// <summary>
    ///     格式化电话号码
    /// </summary>
    /// <param name="schema"></param>
    private static void FormatTelephone(SchemaInfo schema)
    {
        schema.Format ??= "telephone";
        schema.MinLength ??= 7;
        schema.MaxLength ??= 16;
        schema.Pattern ??= Matcher.TelephonePattern;
    }

    /// <summary>
    ///     格式化视频链接
    /// </summary>
    /// <param name="schema"></param>
    private static void FormatVideoUrl(SchemaInfo schema)
    {
        schema.Format ??= "video-url";
        schema.Pattern ??= Matcher.VideoUrlPattern;
    }

    /// <summary>
    ///     格式化图片链接
    /// </summary>
    /// <param name="schema"></param>
    private static void FormatImageUrl(SchemaInfo schema)
    {
        schema.Format ??= "image-url";
        schema.Pattern ??= Matcher.ImageUrlPattern;
    }

    /// <summary>
    ///     格式化异常
    /// </summary>
    /// <param name="schema"></param>
    private static void FormatException(SchemaInfo schema)
    {
        schema.Format ??= "exception";
        // todo more pattern
    }

    #endregion

    /// <summary>
    ///     格式化映射缓存
    /// </summary>
    private static IDictionary<string, Action<SchemaInfo>>? _formatMap;

    /// <summary>
    ///     格式化映射访问器
    /// </summary>
    internal static IDictionary<string, Action<SchemaInfo>> FormatMap => _formatMap ??=
        new Dictionary<string, Action<SchemaInfo>>
        {
            { "datetime", FormatDateTime },
            { "date", FormatDate },
            { "time", FormatTime },
            { "timestamp", FormatTimestamp },
            { "email", FormatEmail },
            { "phone", FormatPhone },
            { "hostname", FormatHostName },
            { "ipv4", FormatIpv4 },
            { "ipv6", FormatIpv6 },
            { "uri", FormatUri },
            { "md5", FormatMd5 },
            { "guid", FormatGuid },
            { "car", FormatCar },
            { "password", FormatPassword },
            { "idcard", FormatIdCard },
            { "tel", FormatTelephone },
            { "video-url", FormatVideoUrl },
            { "image-url", FormatImageUrl },
            { "exception", FormatException }
        };

    #endregion
}