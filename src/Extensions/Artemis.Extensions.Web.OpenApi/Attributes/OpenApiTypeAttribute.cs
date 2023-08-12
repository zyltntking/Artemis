namespace Artemis.Extensions.Web.OpenApi.Attributes;

/// <summary>
///     OpenApi文档类型
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class OpenApiTypeAttribute : Attribute
{
    /// <summary>
    ///     构造
    /// </summary>
    /// <param name="typeName"></param>
    public OpenApiTypeAttribute(string typeName)
    {
        TypeName = typeName;
    }

    /// <summary>
    ///     类型名称
    /// </summary>
    public string TypeName { get; }
}