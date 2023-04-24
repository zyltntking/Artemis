namespace Artemis.App.Swashbuckle.Attributes;

/// <summary>
///  通用对象类型
/// </summary>
public enum CommonObjectType
{
    /// <summary>
    ///  This is the default value. It means that the schema is defined in the resource provider common definition file.
    /// </summary>
    ResourceProviderCommonDefinition,
    /// <summary>
    ///  This means that the schema is defined in the global common definition file.
    /// </summary>
    GlobalCommonDefinition
}

/// <summary>
///   This attribute is used to define the inheritance of a schema from an external schema.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class CustomSwaggerSchemaInheritanceAttribute : Attribute
{
    /// <summary>
    ///  This attribute is used to define the inheritance of a schema from an external schema.
    /// </summary>
    /// <param name="externalSchemaName"></param>
    /// <param name="notInheritedPropertiesName"></param>
    /// <param name="definitionLevel"></param>
    public CustomSwaggerSchemaInheritanceAttribute(string externalSchemaName, string[] notInheritedPropertiesName, CommonObjectType definitionLevel = CommonObjectType.ResourceProviderCommonDefinition)
    {
        ExternalSchemaName = externalSchemaName;
        DefinitionLevel = definitionLevel;
        NotInheritedPropertiesName = notInheritedPropertiesName;
    }

    /// <summary>
    ///  外部架构名称
    /// </summary>
    public string ExternalSchemaName { get; private set; }
    /// <summary>
    ///  通用对象类型
    /// </summary>
    public CommonObjectType DefinitionLevel { get; private set; }
    /// <summary>
    ///  未继承的属性名称
    /// </summary>
    public string[] NotInheritedPropertiesName { get; }
    /// <summary>
    ///  模式描述格式
    /// </summary>
    public string SchemaDescriptionFormat { get; private set; }
}