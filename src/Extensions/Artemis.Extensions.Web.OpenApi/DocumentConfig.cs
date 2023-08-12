using Microsoft.AspNetCore.OData.Query;
using Microsoft.OpenApi.Models;

namespace Artemis.Extensions.Web.OpenApi;

/// <summary>
///     OpenApi文档配置
/// </summary>
public sealed class DocumentConfig
{
    /// <summary>
    ///     <see cref="OpenApiDocumentInfo.Title" />
    /// </summary>
    public string Title { get; init; } = "Artemis";

    /// <summary>
    ///     <see cref="OpenApiDocumentInfo.Description" />
    /// </summary>
    public string Description { get; init; } = "Artemis OpenApi";

    /// <summary>
    ///     <see cref="OpenApiDocumentInfo.ClientName" />
    /// </summary>
    public string ClientName { get; init; } = "Artemis Client";

    /// <summary>
    ///     默认Api版本
    /// </summary>
    public string DefaultApiVersion => "version1";

    /// <summary>
    ///     受支持的Api版本
    /// </summary>
    public IEnumerable<string>? SupportedApiVersions { get; init; }

    /// <summary>
    ///     Api版本
    /// </summary>
    public IEnumerable<string> ApiVersions
    {
        get
        {
            var actualApiVersions = new List<string>();

            if (SupportedApiVersions == null || !SupportedApiVersions.Any())
                actualApiVersions = new List<string> { DefaultApiVersion };
            else
                actualApiVersions.AddRange(SupportedApiVersions);

            return actualApiVersions;
        }
    }

    /// <summary>
    ///     类型架构映射
    /// </summary>
    public Dictionary<Type, OpenApiSchema> TypeSchemaMapping { get; init; } = new()
    {
        { typeof(ODataQueryOptions<>), new OpenApiSchema() }
    };

    /// <summary>
    ///     是否使用Xml注释文件
    /// </summary>
    public bool UseXmlCommentFiles { get; init; } = true;

    /// <summary>
    ///     指示两种生成模式，一种用于向客户扩展API，另一种用于内部开发
    /// </summary>
    /// <value>true用于扩展，false用于本地开发</value>
    public bool GenerateExternalSwagger { get; init; }

    /// <summary>
    ///     在OpenApi规范中，当不使用$ref表征兄弟关系时，使用AllOf来表征兄弟关系
    ///     https://swagger.io/docs/specification/data-models/inheritance-and-polymorphism/#allOf
    ///     默认为false，即使用$ref表征兄弟关系
    /// </summary>
    public bool UseAllOfToExtendReferenceSchemas { get; init; }

    /// <summary>
    ///     默认主机名
    /// </summary>
    public string? DefaultHostName { get; init; }

    /// <summary>
    ///     验证配置合法性，若非法则抛出异常
    /// </summary>
    /// <exception cref="ArgumentNullException">参数空异常</exception>
    /// <exception cref="InvalidOperationException">非法操作异常</exception>
    public void EnsureValidity()
    {
        if (string.IsNullOrWhiteSpace(Title)) throw new ArgumentNullException(nameof(Title));

        if (string.IsNullOrWhiteSpace(Description)) throw new ArgumentNullException(nameof(Description));

        if (string.IsNullOrWhiteSpace(ClientName)) throw new ArgumentNullException(nameof(ClientName));

        //throw new ArgumentNullException();
        //throw new InvalidOperationException();
    }
}