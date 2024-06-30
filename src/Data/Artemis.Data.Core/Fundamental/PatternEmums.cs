namespace Artemis.Data.Core.Fundamental;

/// <summary>
///     摘要长度
/// </summary>
public enum Md5DigestMode
{
    /// <summary>
    ///     16位
    /// </summary>
    Digest16,

    /// <summary>
    ///     32位
    /// </summary>
    Digest32
}

/// <summary>
///     Guid格式
/// </summary>
public enum GuidFormat
{
    /// <summary>
    ///     无字符分割
    /// </summary>
    N,

    /// <summary>
    ///     有字符分割
    /// </summary>
    D,

    /// <summary>
    ///     有字符分割并加上大括号
    /// </summary>
    B,

    /// <summary>
    ///     有字符分割并加上括号
    /// </summary>
    P,

    /// <summary>
    ///     混合模式(匹配用)
    /// </summary>
    Mix
}

/// <summary>
///     日期匹配模式
/// </summary>
public enum DatePatternMode
{
    /// <summary>
    ///     严格模式
    /// </summary>
    /// <remarks>支持闰年判断</remarks>
    Strict,

    /// <summary>
    ///     宽松模式
    /// </summary>
    Lax
}

/// <summary>
///     时间匹配模式
/// </summary>
public enum TimePatternMode
{
    /// <summary>
    ///     24小时制
    /// </summary>
    Time24,

    /// <summary>
    ///     12小时制
    /// </summary>
    Time12
}

/// <summary>
///     内容匹配模式
/// </summary>
public enum ContentPatternMode
{
    /// <summary>
    ///     小括号内内容
    /// </summary>
    Parentheses,

    /// <summary>
    ///     中括号内内容
    /// </summary>
    Bracket,

    /// <summary>
    ///     大括号内内容
    /// </summary>
    Braces
}

/// <summary>
///     手机号匹配模式
/// </summary>
public enum PhonePatternMode
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
public enum LicensePlateNumberPatternMode
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

/// <summary>
///     字符拼写模式
/// </summary>
public enum CharacterCaseMode
{
    /// <summary>
    ///     大写
    /// </summary>
    UpperCase,

    /// <summary>
    ///     小写
    /// </summary>
    LowerCase,

    /// <summary>
    ///     混合
    /// </summary>
    Mix
}

/// <summary>
///     身份证代数模式
/// </summary>
public enum IdCardGenerationMode
{
    /// <summary>
    ///     第一代
    /// </summary>
    /// <remarks>15位数字</remarks>
    First,

    /// <summary>
    ///     第二代
    /// </summary>
    /// <remarks>17位数字+校验</remarks>
    Second,

    /// <summary>
    ///     混合
    /// </summary>
    Mix
}