using Artemis.Data.Core.Fundamental.Kit;
using Artemis.Data.Core.Fundamental.Types;

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

    /// <summary>
    ///     键值对戳标准化
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    /// <returns>戳</returns>
    public static string KeyValuePairStamp(string key, string value)
    {
        var flag = KeyValuePairSummary(key, value);

        return flag.CheckStamp();
    }

    /// <summary>
    ///     键值对戳标准化
    /// </summary>
    /// <param name="pair">键值对</param>
    /// <returns>戳</returns>
    public static string KeyValuePairStamp(this KeyValuePair<string, string> pair)
    {
        return KeyValuePairStamp(pair.Key, pair.Value);
    }

    /// <summary>
    ///     键值对标识标准化
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    /// <returns></returns>
    public static string KeyValuePairSummary(string key, string value)
    {
        return $"{key}:{value}";
    }

    /// <summary>
    ///     键值对标识标准化
    /// </summary>
    /// <param name="pair">键值对</param>
    /// <returns></returns>
    public static string KeyValuePairSummary(this KeyValuePair<string, string> pair)
    {
        return KeyValuePairSummary(pair.Key, pair.Value);
    }

    /// <summary>
    ///     生成检查戳
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string CheckStamp(this string key)
    {
        return Hash.HashData(key.StringNormalize(), HashType.Md5).Normalize();
    }

    /// <summary>
    ///     生成并发戳
    /// </summary>
    public static string ConcurrencyStamp => Guid.NewGuid().ToString("D");

    /// <summary>
    ///     生成加密戳
    /// </summary>
    public static string SecurityStamp => Base32.GenerateBase32();

    /// <summary>
    ///     生成签名
    /// </summary>
    public static string Signature => Guid.NewGuid().ToString("N");
}