using System.ComponentModel;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

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
    public static TKey? IdFromString<TKey>(this string? id) where TKey : IEquatable<TKey>
    {
        if (id == null) return default;
        return (TKey?)TypeDescriptor.GetConverter(typeof(TKey)).ConvertFromInvariantString(id);
    }

    /// <summary>
    ///     id转string
    /// </summary>
    /// <param name="id">The id to convert.</param>
    /// <returns>A <see cref="string" /> representation of the provided <paramref name="id" />.</returns>
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
        return id.ToString("D");
    }

    #region Serialize && Deserialize

    /// <summary>
    ///     序列化
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <param name="model">实体模型</param>
    /// <param name="preserveReference">序列化循环引用</param>
    /// <param name="writeIndented">是否启用格式化</param>
    /// <returns></returns>
    public static string Serialize<T>(this T model, bool preserveReference = false, bool writeIndented = false)
        where T : class
    {
        var options = new JsonSerializerOptions
        {
            IgnoreReadOnlyProperties = false,
            ReferenceHandler = preserveReference ? ReferenceHandler.Preserve : ReferenceHandler.IgnoreCycles,
            WriteIndented = writeIndented,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        return JsonSerializer.Serialize(model, options);
    }

    /// <summary>
    ///     序列化为字节码
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="model"></param>
    /// <param name="preserveReference"></param>
    /// <param name="writeIndented"></param>
    /// <returns></returns>
    public static byte[] SerializeToBytes<T>(this T model, bool preserveReference = false, bool writeIndented = false)
    {
        var options = new JsonSerializerOptions
        {
            IgnoreReadOnlyProperties = false,
            ReferenceHandler = preserveReference ? ReferenceHandler.Preserve : ReferenceHandler.IgnoreCycles,
            WriteIndented = writeIndented,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        return JsonSerializer.SerializeToUtf8Bytes(model, options);
    }

    /// <summary>
    ///     反序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <param name="preserveReference"></param>
    /// <returns></returns>
    public static T? Deserialize<T>(this string json, bool preserveReference = false) where T : class
    {
        var options = new JsonSerializerOptions
        {
            IgnoreReadOnlyProperties = true,
            ReferenceHandler = preserveReference ? ReferenceHandler.Preserve : ReferenceHandler.IgnoreCycles
        };

        return JsonSerializer.Deserialize<T>(json, options);
    }

    /// <summary>
    ///     自字节码反序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="bytes"></param>
    /// <param name="preserveReference"></param>
    /// <returns></returns>
    public static T? Deserialize<T>(this byte[] bytes, bool preserveReference = false) where T : class
    {
        var options = new JsonSerializerOptions
        {
            IgnoreReadOnlyProperties = true,
            ReferenceHandler = preserveReference ? ReferenceHandler.Preserve : ReferenceHandler.IgnoreCycles
        };

        return JsonSerializer.Deserialize<T>(bytes, options);
    }

    #endregion
}