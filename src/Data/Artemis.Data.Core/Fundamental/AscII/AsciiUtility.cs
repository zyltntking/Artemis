namespace Artemis.Data.Core.Fundamental.AscII;

/// <summary>
///     Ascii工具
/// </summary>
public static class AsciiUtility
{
    /// <summary>
    ///     字符种类(唯一字符数)
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static int UniqueCharsCount(string input)
    {
        return input.Distinct().Count();
    }

    /// <summary>
    ///     是否包含数字
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsContainNumber(string input)
    {
        return input.Any(AsciiCharacter.IsDigit);
    }

    /// <summary>
    ///     是否包含大写字母
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsContainUpperCase(string input)
    {
        return input.Any(AsciiCharacter.IsUpperCase);
    }

    /// <summary>
    ///     是否包含小写字母
    /// </summary>
    /// <param name="input">输入字符串</param>
    /// <returns></returns>
    public static bool IsContainLowerCase(string input)
    {
        return input.Any(AsciiCharacter.IsLowerCase);
    }

    /// <summary>
    ///     是否包含非字母数字字符(可打印的特殊字符)
    /// </summary>
    /// <param name="input"></param>
    /// <param name="includeSpace">是否包括空格字符</param>
    /// <returns></returns>
    public static bool IsContainNonAlphanumeric(string input, bool includeSpace = true)
    {
        return input.Any(character => AsciiCharacter.IsNonAlphanumeric(character, includeSpace));
    }
}