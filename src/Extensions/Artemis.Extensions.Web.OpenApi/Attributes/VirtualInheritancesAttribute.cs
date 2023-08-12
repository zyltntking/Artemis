namespace Artemis.Extensions.Web.OpenApi.Attributes;

/// <summary>
///     虚继承属性
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class VirtualInheritancesAttribute : Attribute
{
}

/// <summary>
///     虚继承提供程序
/// </summary>
public interface IVirtualInheritancesProvider
{
    /// <summary>
    ///     虚继承提供程序映射
    /// </summary>
    Dictionary<string, VirtuallyInheritedObjectProperties> GetVirtualInheritances(string documentVersion);
}

/// <summary>
///     虚继承对象属性
/// </summary>
public sealed class VirtuallyInheritedObjectProperties
{
    /// <summary>
    ///     构造
    /// </summary>
    /// <param name="inheritesClassType">继承类类型</param>
    public VirtuallyInheritedObjectProperties(Type inheritesClassType)
    {
        InheritesClassType = inheritesClassType;
    }

    /// <summary>
    ///     构造
    /// </summary>
    /// <param name="inheritesClassName"></param>
    /// <param name="childClassName"></param>
    /// <param name="inheritesClassDescription"></param>
    /// <param name="innerPropertyClassType"></param>
    /// <param name="innerPropertyName"></param>
    /// <exception cref="Exception"></exception>
    public VirtuallyInheritedObjectProperties(
        string inheritesClassName,
        string childClassName,
        string inheritesClassDescription,
        Type innerPropertyClassType,
        string innerPropertyName = "properties")
    {
        InnerPropertyClassType = innerPropertyClassType;
        InnerPropertyName = innerPropertyName;
        InheritesClassName = inheritesClassName;
        InnerPropertyClassName = childClassName;
        InheritesClassDescription = inheritesClassDescription;
        InnerPropertyName = innerPropertyName;
        if (InheritesClassName == InnerPropertyClassName)
            throw new Exception($"Circular reference detected when generating swagger schema {InheritesClassName}");
    }

    /// <summary>
    ///     继承类类型
    /// </summary>
    public Type InheritesClassType { get; }

    /// <summary>
    ///     继承类名
    /// </summary>
    public string InheritesClassName { get; }

    /// <summary>
    ///     内部属性类型名
    /// </summary>
    public string InnerPropertyClassName { get; }

    /// <summary>
    ///     继承类描述
    /// </summary>
    public string InheritesClassDescription { get; }

    /// <summary>
    ///     内部类类型
    /// </summary>
    public Type InnerPropertyClassType { get; }

    /// <summary>
    ///     内部属性名
    /// </summary>
    public string InnerPropertyName { get; }
}