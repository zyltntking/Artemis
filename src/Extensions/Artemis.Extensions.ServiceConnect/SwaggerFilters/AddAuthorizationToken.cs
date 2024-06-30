using Artemis.Data.Core.Fundamental;
using Artemis.Extensions.ServiceConnect.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Artemis.Extensions.ServiceConnect.SwaggerFilters;

/// <summary>
///     添加认证令牌操作过滤器
/// </summary>
internal sealed class AddAuthorizationToken : IOperationFilter
{
    /// <summary>
    ///     构造
    /// </summary>
    /// <param name="options"></param>
    public AddAuthorizationToken(
        IOptions<ArtemisAuthorizationConfig> options)
    {
        Options = options.Value;
    }

    #region Implementation of IOperationFilter

    /// <summary>
    ///     实现
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="context"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var authorizePolicy = context.ApiDescription
            .ActionDescriptor
            .EndpointMetadata
            .OfType<AuthorizeAttribute>()
            .Select(item => item.Policy)
            .Intersect(Options.Policies)
            .Any();

        if (authorizePolicy)
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = Options.RequestHeaderTokenKey,
                In = ParameterLocation.Header,
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    MinLength = 32,
                    MaxLength = 32,
                    Format = "md5",
                    Pattern = Matcher.Md5Pattern(),
                    Example = new OpenApiString("6BB0A25E549A723D3323F21E54570488")
                },
                Description = "认证令牌"
            });
    }

    #endregion

    /// <summary>
    ///     认证配置依赖
    /// </summary>
    private ArtemisAuthorizationConfig Options { get; }
}