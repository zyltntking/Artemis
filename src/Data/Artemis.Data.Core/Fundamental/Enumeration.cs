using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Artemis.Data.Core.Fundamental;

/// <summary>
///     枚举类
/// </summary>
public abstract class Enumeration : IComparable
{
    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    protected Enumeration(int id, string name)
    {
        (Id, Name) = (id, name.ToUpper());
    }

    /// <summary>
    ///     枚举名称
    /// </summary>
    private string Name { get; }

    /// <summary>
    ///     枚举ID
    /// </summary>
    private int Id { get; }

    #region Implementation of IComparable

    /// <summary>
    ///     比较
    /// </summary>
    /// <param name="other">其他</param>
    /// <returns>比较结果</returns>
    public int CompareTo(object? other)
    {
        return Id.CompareTo(((Enumeration)other!).Id);
    }

    #endregion

    /// <summary>
    ///     从值获取枚举
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <param name="id">值</param>
    /// <returns>枚举成员</returns>
    public static T FromValue<T>(int id) where T : Enumeration
    {
        var matchingItem = Parse<T, int>(id, "id", item => item.Id == id);
        return matchingItem;
    }

    /// <summary>
    ///     从名称获取枚举
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <param name="name">枚举成员名称</param>
    /// <returns>枚举成员</returns>
    public static T FromName<T>(string name) where T : Enumeration
    {
        var matchingItem = Parse<T, string>(name, "name", item => item.Name == name.ToUpper());
        return matchingItem;
    }

    /// <summary>
    ///     绝对值差
    /// </summary>
    /// <param name="lhs">左手资源</param>
    /// <param name="rhs">右手资源</param>
    /// <returns>差值</returns>
    public static int AbsoluteDifference(Enumeration lhs, Enumeration rhs)
    {
        var absoluteDifference = Math.Abs(lhs.Id - rhs.Id);
        return absoluteDifference;
    }

    /// <summary>
    ///     获取枚举类的所有枚举成员
    /// </summary>
    /// <typeparam name="T">枚举类</typeparam>
    /// <returns>枚举成员</returns>
    private static IEnumerable<T> GetAll<T>() where T : Enumeration
    {
        return typeof(T).GetFields(BindingFlags.Public |
                                   BindingFlags.Static |
                                   BindingFlags.DeclaredOnly)
            .Select(info => info.GetValue(null))
            .Cast<T>();
    }

    /// <summary>
    ///     转换函数
    /// </summary>
    /// <typeparam name="T">枚举类</typeparam>
    /// <typeparam name="TK">源类</typeparam>
    /// <param name="value">源值</param>
    /// <param name="description">描述</param>
    /// <param name="predicate">匹配表达式</param>
    /// <returns>枚举成员</returns>
    /// <exception cref="InvalidOperationException">无效操作异常</exception>
    private static T Parse<T, TK>(TK value, string description, Func<T, bool> predicate) where T : Enumeration
    {
        var matchingItem = GetAll<T>().FirstOrDefault(predicate);

        return matchingItem ??
               throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T)}");
    }

    #region Overrides of Object

    /// <summary>
    ///     转为字符串
    /// </summary>
    /// <returns>字符串</returns>
    public override string ToString()
    {
        return Name;
    }

    /// <summary>
    ///     等于
    /// </summary>
    /// <param name="obj">对象</param>
    /// <returns>是否相等</returns>
    public override bool Equals(object? obj)
    {
        if (obj is not Enumeration otherValue) return false;

        var typeMatches = GetType() == obj.GetType();
        var valueMatches = Id.Equals(otherValue.Id);

        return typeMatches && valueMatches;
    }

    /// <summary>
    ///     获取哈希码
    /// </summary>
    /// <returns>哈希码</returns>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    #endregion
}

/// <summary>
///     枚举类Json转换器
/// </summary>
/// <typeparam name="T"></typeparam>
public class EnumerationJsonConverter<T> : JsonConverter<T> where T : Enumeration
{
    #region Overrides of JsonConverter<T>

    /// <summary>Reads and converts the JSON to type <typeparamref name="T" />.</summary>
    /// <param name="reader">The reader.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    /// <returns>The converted value.</returns>
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return Enumeration.FromName<T>(reader.GetString()!);
    }

    /// <summary>Writes a specified value as JSON.</summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The value to convert to JSON.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }

    #endregion
}