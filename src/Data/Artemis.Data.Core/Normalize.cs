namespace Artemis.Data.Core;

/// <summary>
///     标准化
/// </summary>
public static class Normalize
{
    /// <summary>
    ///     输入字符串标准化
    /// </summary>
    /// <param name="input">输入字符串</param>
    /// <returns>标准化字符串</returns>
    public static string StringNormalize(this string input)
    {
        return input.Normalize().ToUpperInvariant();
    }
}