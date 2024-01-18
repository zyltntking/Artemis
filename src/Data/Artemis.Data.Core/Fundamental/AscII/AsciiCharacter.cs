using System.Security.Cryptography;
using System.Text;

namespace Artemis.Data.Core.Fundamental.AscII;

/// <summary>
///     Ascii显示字符
/// </summary>
public static class AsciiCharacter
{
    #region CharacterRange

    /// <summary>
    ///     字符范围
    /// </summary>
    /// <remarks>0x20 space</remarks>
    private static Tuple<byte, byte> CharacterRange => _characterRange ??= new Tuple<byte, byte>(0x20, 0x7E);

    private static Tuple<byte, byte>? _characterRange;

    /// <summary>
    ///     数字范围
    /// </summary>
    private static Tuple<byte, byte> DigitRange => _digitRange ??= new Tuple<byte, byte>(0x30, 0x39);

    private static Tuple<byte, byte>? _digitRange;

    /// <summary>
    ///     大写字母范围
    /// </summary>
    private static Tuple<byte, byte> UpperCaseRange => _upperCaseRange ??= new Tuple<byte, byte>(0x41, 0x5A);

    private static Tuple<byte, byte>? _upperCaseRange;

    /// <summary>
    ///     小写字母范围
    /// </summary>
    private static Tuple<byte, byte> LowerCaseRange => _lowerCaseRange ??= new Tuple<byte, byte>(0x61, 0x7A);

    private static Tuple<byte, byte>? _lowerCaseRange;

    #endregion

    #region CodeVerify

    /// <summary>
    ///     编码是否为可打印字符
    /// </summary>
    /// <param name="code">字节码</param>
    /// <param name="includeSpace">是否包括空格</param>
    /// <returns></returns>
    private static bool IsCharacter(byte code, bool includeSpace = false)
    {
        var (min, max) = CharacterRange;

        return (includeSpace ? code >= min : code > min) && code <= max;
    }

    /// <summary>
    ///     字符是否是可打印字符
    /// </summary>
    /// <param name="character">字符</param>
    /// <param name="includeSpace">是否包括空格</param>
    /// <returns></returns>
    public static bool IsCharacter(char character, bool includeSpace = false)
    {
        return IsCharacter((byte)character, includeSpace);
    }

    /// <summary>
    ///     编码是否是数字
    /// </summary>
    /// <param name="code">字节码</param>
    /// <returns></returns>
    private static bool IsDigit(byte code)
    {
        var (min, max) = DigitRange;

        return code >= min && code <= max;
    }

    /// <summary>
    ///     字符是否是数字
    /// </summary>
    /// <param name="character">字符</param>
    /// <returns></returns>
    public static bool IsDigit(char character)
    {
        return IsDigit((byte)character);
    }

    /// <summary>
    ///     编码是否是大写字母
    /// </summary>
    /// <param name="code">字节码</param>
    /// <returns></returns>
    private static bool IsUpperCase(byte code)
    {
        var (min, max) = UpperCaseRange;

        return code >= min && code <= max;
    }

    /// <summary>
    ///     字符是否是大写字母
    /// </summary>
    /// <param name="character">字符</param>
    /// <returns></returns>
    public static bool IsUpperCase(char character)
    {
        return IsUpperCase((byte)character);
    }

    /// <summary>
    ///     编码是否是小写字母
    /// </summary>
    /// <param name="code">字节码</param>
    /// <returns></returns>
    private static bool IsLowerCase(byte code)
    {
        var (min, max) = LowerCaseRange;

        return code >= min && code <= max;
    }

    /// <summary>
    ///     字符是否是小写字母
    /// </summary>
    /// <param name="character">字符</param>
    /// <returns></returns>
    public static bool IsLowerCase(char character)
    {
        return IsLowerCase((byte)character);
    }

    /// <summary>
    ///     编码是否是字母
    /// </summary>
    /// <param name="code">字节码</param>
    /// <returns></returns>
    private static bool IsLetter(byte code)
    {
        return IsUpperCase(code) || IsLowerCase(code);
    }

    /// <summary>
    ///     字符是否是字母
    /// </summary>
    /// <param name="character">字符</param>
    /// <returns></returns>
    public static bool IsLetter(char character)
    {
        return IsLetter((byte)character);
    }

    /// <summary>
    ///     是否是字母和数字
    /// </summary>
    /// <param name="code">字节码</param>
    /// <returns></returns>
    private static bool IsLetterOrDigit(byte code)
    {
        return IsLetter(code) || IsDigit(code);
    }

    /// <summary>
    ///     字符是否是字母和数字
    /// </summary>
    /// <param name="character">字符</param>
    /// <returns></returns>
    public static bool IsLetterOrDigit(char character)
    {
        return IsLetterOrDigit((byte)character);
    }

    /// <summary>
    ///     字符是否是特殊字符
    /// </summary>
    /// <param name="character"></param>
    /// <param name="includeSpace"></param>
    /// <returns></returns>
    public static bool IsNonAlphanumeric(char character, bool includeSpace = false)
    {
        return IsNonAlphanumeric((byte)character, includeSpace);
    }

    /// <summary>
    ///     是否是特殊字符
    /// </summary>
    /// <param name="code">字节码</param>
    /// <param name="includeSpace">是否包括空格</param>
    /// <returns></returns>
    private static bool IsNonAlphanumeric(byte code, bool includeSpace = false)
    {
        return IsCharacter(code, includeSpace) && !IsLetterOrDigit(code);
    }

    #endregion

    #region RandomCharacters

    /// <summary>
    ///     随机可打印字符串
    /// </summary>
    /// <param name="length">长度</param>
    /// <returns></returns>
    public static string RandomCharacters(int length)
    {
        var builder = new StringBuilder(length);

        var (min, max) = CharacterRange;

        for (var i = 0; i < length; i++)
        {
            var code = RandomNumberGenerator.GetInt32(min + 1, max + 1);

            builder.Append((char)code);
        }

        return builder.ToString();
    }

    /// <summary>
    ///     随机数字字符串
    /// </summary>
    /// <param name="length">长度</param>
    /// <returns></returns>
    public static string RandomDigits(int length)
    {
        var builder = new StringBuilder(length);

        var (min, max) = DigitRange;

        for (var i = 0; i < length; i++)
        {
            var code = RandomNumberGenerator.GetInt32(min, max + 1);

            builder.Append((char)code);
        }

        return builder.ToString();
    }

    /// <summary>
    ///     随机大写字符串
    /// </summary>
    /// <param name="length">长度</param>
    /// <returns></returns>
    public static string RandomUpperCases(int length)
    {
        var builder = new StringBuilder(length);

        var (min, max) = UpperCaseRange;

        for (var i = 0; i < length; i++)
        {
            var code = RandomNumberGenerator.GetInt32(min, max + 1);

            builder.Append((char)code);
        }

        return builder.ToString();
    }

    /// <summary>
    ///     随机小写字符串
    /// </summary>
    /// <param name="length">长度</param>
    /// <returns></returns>
    public static string RandomLowerCases(int length)
    {
        var builder = new StringBuilder(length);

        var (min, max) = LowerCaseRange;

        for (var i = 0; i < length; i++)
        {
            var code = RandomNumberGenerator.GetInt32(min, max + 1);

            builder.Append((char)code);
        }

        return builder.ToString();
    }

    /// <summary>
    ///     随机字母字符串
    /// </summary>
    /// <param name="length">长度</param>
    /// <returns></returns>
    public static string RandomLetters(int length)
    {
        var builder = new StringBuilder(length);

        var (upperMin, _) = UpperCaseRange;

        var (_, lowerMax) = LowerCaseRange;

        for (var i = 0; i < length; i++)
        {
            var code = RandomNumberGenerator.GetInt32(upperMin, lowerMax + 1);

            while (!IsLetter((byte)code)) code = RandomNumberGenerator.GetInt32(upperMin, lowerMax + 1);

            builder.Append((char)code);
        }

        return builder.ToString();
    }

    /// <summary>
    ///     随机字母和数字字符串
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string RandomLetterOrDigits(int length)
    {
        var builder = new StringBuilder(length);

        var (digitMin, _) = DigitRange;

        var (_, lowerMax) = LowerCaseRange;

        for (var i = 0; i < length; i++)
        {
            var code = RandomNumberGenerator.GetInt32(digitMin, lowerMax + 1);

            while (!IsLetterOrDigit((byte)code)) code = RandomNumberGenerator.GetInt32(digitMin, lowerMax + 1);

            builder.Append((char)code);
        }

        return builder.ToString();
    }

    /// <summary>
    ///     随机特殊字符字符串
    /// </summary>
    /// <param name="length">长度</param>
    /// <returns></returns>
    public static string RandomNonAlphanumeric(int length)
    {
        var builder = new StringBuilder(length);

        var (min, max) = CharacterRange;

        for (var i = 0; i < length; i++)
        {
            var code = RandomNumberGenerator.GetInt32(min + 1, max + 1);

            while (!IsNonAlphanumeric((byte)code)) code = RandomNumberGenerator.GetInt32(min + 1, max + 1);

            builder.Append((char)code);
        }

        return builder.ToString();
    }

    #endregion
}