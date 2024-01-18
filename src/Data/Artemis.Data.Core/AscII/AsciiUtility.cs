namespace Artemis.Data.Core.AscII;

/// <summary>
///     Ascii工具
/// </summary>
public static class AsciiUtility
{
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
    ///     是否包含大写字母
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsContainUpperCase(string input)
    {
        return input.Any(AsciiCharacter.IsUpperCase);
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
}