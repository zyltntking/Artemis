namespace Artemis.App.Swashbuckle.Attributes;

/// <summary>
///     Swagger类型特性
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class SwaggerTypeAttribute : Attribute
{
    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="typeName"></param>
    public SwaggerTypeAttribute(string typeName)
    {
        TypeName = typeName;
    }

    /// <summary>
    ///     类型名称
    /// </summary>
    public string TypeName { get; }
}