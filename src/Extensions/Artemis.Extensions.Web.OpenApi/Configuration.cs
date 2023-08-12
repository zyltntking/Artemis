using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace Artemis.Extensions.Web.OpenApi;

/// <summary>
///     OpenApi Document Configuration
/// </summary>
internal class Configuration
{
    /// <summary>
    ///     OpenAPI document info
    /// </summary>
    private readonly OpenApiDocumentInfo _info;

    /// <summary>
    ///     初始化一个<see cref="Configuration" />实例
    /// </summary>
    /// <param name="info">文档信息详情</param>
    /// <exception cref="ArgumentNullException"></exception>
    public Configuration(OpenApiDocumentInfo info)
    {
        _info = info ?? throw new ArgumentNullException(nameof(info));
    }

    /// <summary>
    ///     获取OpenApi文档信息
    /// </summary>
    public OpenApiInfo OpenApiInfo => new()
    {
        Title = _info.Title,
        Description = _info.Description,
        Version = _info.Version,
        TermsOfService = new Uri("https://github.com/zyltntking/Artemis"),
        Extensions =
        {
            ["x-ms-code-generation-settings"] = new OpenApiObject
            {
                ["name"] = new OpenApiString(_info.ClientName)
            }
        },
        Contact = new OpenApiContact
        {
            Name = "Artemis",
            Url = new Uri("https://github.com/zyltntking/Artemis"),
            Email = "zyltntking@live.cn"
        },
        License = new OpenApiLicense
        {
            Name = "MIT",
            Url = new Uri("https://github.com/zyltntking/Artemis")
        }
    };
}