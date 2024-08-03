namespace Artemis.Data.Core.Fundamental;

/// <summary>
///     字符串扩展
/// </summary>
public static class StringExtensions
{
    /// <summary>
    ///     移除开头字符串
    /// </summary>
    /// <param name="input"></param>
    /// <param name="trim"></param>
    /// <returns></returns>
    public static string TrimStart(this string input, string trim)
    {
        if (input.StartsWith(trim))
            input = input[trim.Length..];

        return input;
    }

    /// <summary>
    ///     移除结尾字符串
    /// </summary>
    /// <param name="input"></param>
    /// <param name="trim"></param>
    /// <returns></returns>
    public static string TrimEnd(this string input, string trim)
    {
        if (input.EndsWith(trim)) input = input[..^trim.Length];

        return input;
    }

    /// <summary>
    ///     移除开头和结尾的字符串
    /// </summary>
    /// <param name="input"></param>
    /// <param name="trim"></param>
    /// <returns></returns>
    public static string Trim(this string input, string trim)
    {
        return input.TrimStart(trim).TrimEnd(trim);
    }
}