﻿using Artemis.Data.Core.Fundamental.Types;

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
    public static string KeyValuePairStampNormalize(string key, string value)
    {
        var flag = KeyValuePairFlagNormalize(key, value);

        return Hash.HashData(flag, HashType.Md5).StringNormalize();
    }

    /// <summary>
    ///     键值对戳标准化
    /// </summary>
    /// <param name="pair">键值对</param>
    /// <returns>戳</returns>
    public static string KeyValuePairStampNormalize(this KeyValuePair<string, string> pair)
    {
        return KeyValuePairStampNormalize(pair.Key, pair.Value);
    }

    /// <summary>
    ///     键值对标识标准化
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    /// <returns></returns>
    public static string KeyValuePairFlagNormalize(string key, string value)
    {
        return $"{key}:{value}";
    }

    /// <summary>
    ///     键值对标识标准化
    /// </summary>
    /// <param name="pair">键值对</param>
    /// <returns></returns>
    public static string KeyValuePairFlagNormalize(this KeyValuePair<string, string> pair)
    {
        return KeyValuePairFlagNormalize(pair.Key, pair.Value);
    }
}