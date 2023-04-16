using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;

namespace Artemis.App.Swashbuckle;

/// <summary>
/// 生成配置
/// </summary>
public sealed class GeneratorConfig : IGeneratorConfig
{
    #region Implementation of IGeneratorOptions

    /// <summary>
    /// 主机名
    /// </summary>
    public string? HostName { get; set; }

    /// <summary>
    /// 映射类型到架构
    /// </summary>
    public Dictionary<Type, OpenApiSchema> MappingTypeToSchema { get; } = new()
    {
        { typeof(IFormFile), new OpenApiSchema { Type = "file", Format = "binary" } },
        { typeof(Stream), new OpenApiSchema { Type = "file", Format = "binary" } }
    };

    /// <summary>
    /// 使用xml注释文档
    /// </summary>
    public bool UseXmlCommentFiles { get; set; } = true;

    /// <summary>
    /// xml注释文档
    /// </summary>
    public IEnumerable<string> XmlCommentFiles { get; } = new List<string>
    {
        "Artemis*.xml"
    };

    /// <summary>
    /// 使用AllOf扩展引用架构
    /// </summary>
    public bool UseAllOfToExtendReferenceSchemas { get; set; }

    /// <summary>
    /// 是否为第三方客户生成
    /// </summary>
    public bool GenerateExternal { get; set; } = false;

    #endregion
}