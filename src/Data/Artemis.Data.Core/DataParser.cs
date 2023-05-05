using System.ComponentModel;

namespace Artemis.Data.Core;

/// <summary>
///     数据解析器
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
    ///     string转id
    /// </summary>
    /// <param name="id">The id to convert.</param>
    /// <returns>An instance of <typeparamref name="TKey" /> representing the provided <paramref name="id" />.</returns>
    public static TKey? IdFromString<TKey>(this string? id)
    {
        if (id == null) return default;
        return (TKey?)TypeDescriptor.GetConverter(typeof(TKey)).ConvertFromInvariantString(id);
    }

    /// <summary>
    ///     id转string
    /// </summary>
    /// <param name="id">The id to convert.</param>
    /// <returns>An <see cref="string" /> representation of the provided <paramref name="id" />.</returns>
    public static string? IdToString<TKey>(this TKey id) where TKey : IEquatable<TKey>
    {
        return Equals(id, default(TKey)) ? null : id.ToString();
    }

    /// <summary>
    ///     string装guid
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Guid GuidFromString(this string? id)
    {
        if (id == null) return default;
        return Guid.Parse(id);
    }

    /// <summary>
    ///     Guid转String
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static string GuidToString(this Guid id)
    {
        return id.ToString();
    }
}

/// <summary>
///     字节码转换器
/// </summary>
public static class ByteParser
{
    /// <summary>
    ///     扩展Bytes数组
    /// </summary>
    /// <param name="sourceArray">源数组</param>
    /// <param name="extendLength">扩展长度</param>
    /// <returns>扩展结果</returns>
    public static byte[] ExtendBytes(byte[] sourceArray, long extendLength)
    {
        if (sourceArray.Length == extendLength)
            return sourceArray;

        var length = extendLength > sourceArray.Length ? sourceArray.Length : extendLength;

        var destinationArray = new byte[extendLength];

        Array.Copy(sourceArray, 0, destinationArray, 0, length);

        return destinationArray;
    }

    /// <summary>
    ///     固定字节码
    /// </summary>
    /// <param name="bytes">字节码</param>
    /// <returns>输入的字节码</returns>
    public static byte[] FixedBytes(byte[] bytes)
    {
        return bytes;
    }

    #region Long

    /// <summary>
    ///     Long转Bytes(高位优先)
    /// </summary>
    /// <param name="number">long</param>
    /// <returns>bytes</returns>
    /// <code><![CDATA[var bytes = new byte[8];]]></code>
    /// <code><![CDATA[bytes[0] = (byte)((number >> 56) & 0xFF);]]></code>
    /// <code><![CDATA[bytes[1] = (byte)((number >> 48) & 0xFF);]]></code>
    /// <code><![CDATA[bytes[2] = (byte)((number >> 40) & 0xFF);]]></code>
    /// <code><![CDATA[bytes[3] = (byte)((number >> 32) & 0xFF);]]></code>
    /// <code><![CDATA[bytes[4] = (byte)((number >> 24) & 0xFF);]]></code>
    /// <code><![CDATA[bytes[5] = (byte)((number >> 16) & 0xFF);]]></code>
    /// <code><![CDATA[bytes[6] = (byte)((number >> 8) & 0xFF);]]></code>
    /// <code><![CDATA[bytes[7] = (byte)(number & 0xFF);]]></code>
    public static byte[] LongToBytes(long number)
    {
        var bytes = BitConverter.GetBytes(number);
        return BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes;
    }

    /// <summary>
    ///     Bytes转Long
    /// </summary>
    /// <param name="bytes">bytes</param>
    /// <param name="index">转换索引</param>
    /// <returns>long</returns>
    /// <code><![CDATA[((long)bytes[offset] &lt;&lt; 56) |]]></code>
    /// <code><![CDATA[((long)bytes[offset + 1] &lt;&lt; 48) |]]></code>
    /// <code><![CDATA[((long)bytes[offset + 2] &lt;&lt; 40) |]]></code>
    /// <code><![CDATA[((long)bytes[offset + 3] &lt;&lt; 32) |]]></code>
    /// <code><![CDATA[((long)bytes[offset + 4] &lt;&lt; 24) |]]></code>
    /// <code><![CDATA[((long)bytes[offset + 5] &lt;&lt; 16) |]]></code>
    /// <code><![CDATA[((long)bytes[offset + 6] &lt;&lt; 8) |]]></code>
    /// <code><![CDATA[bytes[offset + 7];]]></code>
    public static long BytesToLong(byte[] bytes, int index = 0)
    {
        return BitConverter.ToInt64(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, index);
    }

    #endregion
}