using Microsoft.OpenApi.Models;

namespace Artemis.App.Swashbuckle;

/// <summary>
/// 生成配置接口
/// </summary>
public interface IGeneratorConfig
{
    /// <summary>
    /// 主机名
    /// </summary>
    string? HostName { get; set; }

    /// <summary>
    /// 映射类型到架构
    /// </summary>
    Dictionary<Type, OpenApiSchema> MappingTypeToSchema { get; }

    /// <summary>
    /// 使用xml注释文档
    /// </summary>
    bool UseXmlCommentFiles { get; set; }

    /// <summary>
    /// xml注释文档
    /// </summary>
    IEnumerable<string> XmlCommentFiles { get; }

    /// <summary>
    /// 使用AllOf扩展引用架构
    /// </summary>
    bool UseAllOfToExtendReferenceSchemas { get; set; }

    /// <summary>
    /// 是否为第三方客户生成
    /// </summary>
    bool GenerateExternal { get; set; }
}