using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Artemis.Extensions.ServiceConnect.SwaggerFilters;

/// <summary>
///     添加认证令牌操作过滤器
/// </summary>
public class AddIdentityToken : IOperationFilter
{
    /// <summary>
    ///     构造
    /// </summary>
    /// <param name="options"></param>
    public AddIdentityToken(IOptions<SharedIdentityOptions> options)
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
            .Intersect(IdentityPolicy.TokenPolicies)
            .Any();

        if (authorizePolicy)
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = Options.HeaderIdentityTokenKey,
                In = ParameterLocation.Header,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "String"
                },
                Description = "认证令牌"
            });
    }

    #endregion

    /// <summary>
    ///     认证配置依赖
    /// </summary>
    private SharedIdentityOptions Options { get; }
}