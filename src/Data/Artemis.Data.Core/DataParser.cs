using System.ComponentModel;

namespace Artemis.Data.Core;

/// <summary>
/// 数据解析器
/// </summary>
public static class DataParser
{
    /// <summary>
    ///     获取Unix时间戳
    /// </summary>
    /// <param name="time">时间</param>
    /// <returns>时间戳</returns>
    public static long ToUnixTimeStamp(this DateTime time)
    {
        var span = time.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(span.TotalSeconds);
    }

    /// <summary>
    ///     获取本地时间
    /// </summary>
    /// <param name="timestamp">时间戳</param>
    /// <returns>Utc时间</returns>
    public static DateTime ToDateTime(this long timestamp)
    {
        return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(timestamp).ToLocalTime();
    }

    /// <summary>
    /// string转id
    /// </summary>
    /// <param name="id">The id to convert.</param>
    /// <returns>An instance of <typeparamref name="TKey"/> representing the provided <paramref name="id"/>.</returns>
    public static TKey? IdFromString<TKey>(this string? id)
    {
        if (id == null)
        {
            return default;
        }
        return (TKey?)TypeDescriptor.GetConverter(typeof(TKey)).ConvertFromInvariantString(id);
    }

    /// <summary>
    /// id转string
    /// </summary>
    /// <param name="id">The id to convert.</param>
    /// <returns>An <see cref="string"/> representation of the provided <paramref name="id"/>.</returns>
    public static string? IdToString<TKey>(this TKey id) where TKey : IEquatable<TKey>
    {
        return Equals(id, default(TKey)) ? null : id.ToString();
    }

    /// <summary>
    /// string装guid
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Guid GuidFromString(this string? id)
    {
        if (id == null)
        {
            return default;
        }
        return Guid.Parse(id);
    }

    /// <summary>
    /// Guid转String
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static string GuidToString(this Guid id)
    {
        return id.ToString();
    }
}