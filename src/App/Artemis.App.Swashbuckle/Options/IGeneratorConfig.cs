using Microsoft.OpenApi.Models;

namespace Artemis.App.Swashbuckle.Options;

/// <summary>
///     生成配置接口
/// </summary>
public interface IGeneratorConfig
{
    /// <summary>
    ///     支持的API版本
    /// </summary>
    IEnumerable<string> SupportedApiVersions { get; init; }

    /// <summary>
    ///     默认API版本
    /// </summary>
    string DefaultApiVersion { get; init; }

    /// <see cref="OpenApiDocumentInfo.Title" />
    public string Title { get; set; }

    /// <see cref="OpenApiDocumentInfo.Description" />
    public string Description { get; set; }

    /// <see cref="OpenApiDocumentInfo.ClientName" />
    public string ClientName { get; set; }

    /// <summary>
    ///     主机名
    /// </summary>
    string? HostName { get; init; }

    /// <summary>
    ///     映射类型到架构
    /// </summary>
    Dictionary<Type, OpenApiSchema> MappingTypeToSchema { get; }

    /// <summary>
    ///     使用xml注释文档
    /// </summary>
    bool UseXmlCommentFiles { get; init; }

    /// <summary>
    ///     xml注释文档
    /// </summary>
    IEnumerable<string> XmlCommentFiles { get; }

    /// <summary>
    ///     使用AllOf扩展引用架构
    /// </summary>
    bool UseAllOfToExtendReferenceSchemas { get; init; }

    /// <summary>
    ///     是否为第三方客户生成
    /// </summary>
    bool GenerateExternal { get; init; }

    /// <summary>
    ///     隐藏参数路径
    /// </summary>
    bool HideParameters { get; init; }

    /// <summary>
    ///     以字符串形式使用枚举
    /// </summary>
    bool EnumsAsString { get; init; }
}