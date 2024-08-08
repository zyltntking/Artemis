using System.ComponentModel;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Artemis.Data.Core.Fundamental;

#region Interface

/// <summary>
///     枚举接口
/// </summary>
/// <typeparam name="TEnum">枚举类</typeparam>
file interface IEnumeration<TEnum> : IComparable, IEquatable<TEnum>
{
    /// <summary>
    ///     枚举名称
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     枚举ID
    /// </summary>
    int Id { get; }
}

#endregion

/// <summary>
///     枚举类
/// </summary>
public abstract class Enumeration : IEnumeration<Enumeration>
{
    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    protected Enumeration(int id, string name)
    {
        (Id, Name) = (id, name);
    }

    /// <summary>
    ///     枚举名称
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     枚举ID
    /// </summary>
    public int Id { get; }

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

    #region Equality members

    /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    ///     <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise,
    ///     <see langword="false" />.
    /// </returns>
    public bool Equals(Enumeration? other)
    {
        if (other == null)
            return false;

        var typeMatches = GetType() == other.GetType();
        var valueMatches = Id.Equals(other);

        return typeMatches && valueMatches;
    }

    #endregion

    /// <summary>
    ///     从值获取枚举
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <param name="id">值</param>
    /// <returns>枚举成员</returns>
    public static T? FromValue<T>(int id) where T : Enumeration
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
    public static T? FromName<T>(string name) where T : Enumeration
    {
        var matchingItem = Parse<T, string>(name, "name", item => item.Name == name.Normalize());
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
        var absoluteDifference = Math.Abs(lhs - rhs);
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
    ///     获取字符串枚举成员
    /// </summary>
    /// <param name="type">枚举类</param>
    /// <returns></returns>
    public static IEnumerable<string> GetAllName(Type type)
    {
        if (type.BaseType != typeof(Enumeration)) return new List<string>();

        return type.GetFields(BindingFlags.Public |
                              BindingFlags.Static |
                              BindingFlags.DeclaredOnly)
            .Select(info => info.GetValue(null))
            .Cast<Enumeration>()
            .Select(item => item.ToString());
    }

    /// <summary>
    ///     获取枚举类的所有枚举成员
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerable<string> GetAllName<T>() where T : Enumeration
    {
        return GetAll<T>().Select(item => item.Name);
    }

    /// <summary>
    ///     获取id枚举成员
    /// </summary>
    /// <param name="type">枚举类</param>
    /// <returns></returns>
    public static IEnumerable<int> GetAllId(Type type)
    {
        if (type.BaseType != typeof(Enumeration))
            return new List<int>();

        return type.GetFields(BindingFlags.Public |
                              BindingFlags.Static |
                              BindingFlags.DeclaredOnly)
            .Select(info => info.GetValue(null))
            .Cast<Enumeration>()
            .Select(item => item.Id);
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
    private static T? Parse<T, TK>(TK value, string description, Func<T, bool> predicate) where T : Enumeration
    {
        var matchingItem = GetAll<T>().FirstOrDefault(predicate);

        return matchingItem;
    }

    /// <summary>
    ///     转为记录字典
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="valid"></param>
    /// <returns></returns>
    public static EnumerationRecord ToRecordDictionary<T>(bool valid = true) where T : Enumeration
    {
        var prefix = "Artemis";

        var description = typeof(T).GetCustomAttribute<DescriptionAttribute>()?
            .Description
            .TrimStart(prefix);

        var record = new EnumerationRecord
        {
            TypeName = typeof(T).Name.TrimStart(prefix),
            Description = description,
            Valid = valid
        };

        var fieldList = typeof(T)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Select(info =>
            {
                var enumeration = info.GetValue(null) as T;

                var item = Generator.CreateInstance<EnumerationItemRecord>();

                if (enumeration is not null)
                {
                    item.ItemKey = enumeration;
                    item.ItemValue = enumeration;
                    item.Description = info.GetCustomAttribute<DescriptionAttribute>()?.Description;
                }

                return item;
            });

        record.Records = fieldList.ToList();

        return record;
    }

    #region Operator

    /// <summary>
    ///     Enumeration转String
    /// </summary>
    /// <param name="enumeration"></param>
    public static implicit operator string(Enumeration enumeration)
    {
        return enumeration.Name;
    }

    /// <summary>
    ///     Enumeration转Int32
    /// </summary>
    /// <param name="enumeration"></param>
    public static implicit operator int(Enumeration enumeration)
    {
        return enumeration.Id;
    }

    #endregion

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
        var valueMatches = Id.Equals(otherValue);

        return typeMatches && valueMatches;
    }

    /// <summary>
    ///     获取哈希码
    /// </summary>
    /// <returns>哈希码</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Id);
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

/// <summary>
///     枚举类扩展
/// </summary>
public static class EnumerationExtensions
{
    /// <summary>
    ///     添加转换器
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <param name="options">Json序列化配置</param>
    public static void AddEnumerationConverter<T>(this JsonSerializerOptions options) where T : Enumeration
    {
        options.Converters.Add(new EnumerationJsonConverter<T>());
    }
}

/// <summary>
///     枚举记录
/// </summary>
public record struct EnumerationRecord
{
    /// <summary>
    ///     类型名
    /// </summary>
    public required string TypeName { get; init; }

    /// <summary>
    ///     描述
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    ///     是否有效
    /// </summary>
    public required bool Valid { get; init; }

    /// <summary>
    ///     记录项目集合
    /// </summary>
    public ICollection<EnumerationItemRecord> Records { get; set; }
}

/// <summary>
///     枚举项目记录
/// </summary>
public record struct EnumerationItemRecord
{
    /// <summary>
    ///     项目键
    /// </summary>
    public required string ItemKey { get; set; }

    /// <summary>
    ///     项目值
    /// </summary>
    public required string ItemValue { get; set; }

    /// <summary>
    ///     描述
    /// </summary>
    public string? Description { get; set; }
}