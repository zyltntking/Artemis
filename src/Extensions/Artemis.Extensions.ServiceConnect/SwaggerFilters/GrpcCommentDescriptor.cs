using Artemis.Extensions.ServiceConnect.SwaggerFilters.Descriptor;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Artemis.Extensions.ServiceConnect.SwaggerFilters;

/// <summary>
///     Grpc注释描述器
/// </summary>
internal sealed class GrpcCommentDescriptor : ISchemaFilter
{
    /// <summary>
    ///     描述器构造
    /// </summary>
    /// <param name="options"></param>
    public GrpcCommentDescriptor(
        IOptions<IdentityOptions> options)
    {
        Options = options.Value;
    }

    #region Implementation of ISchemaFilter

    /// <summary>
    ///     实现
    /// </summary>
    /// <param name="schema"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var description = schema.Description;

        if (!string.IsNullOrWhiteSpace(description))
        {
            var comments = description.Split("\r\n");

            var schemaInfo = CommentDescriptor.Describe(comments, context.MemberInfo);

            schema.Minimum = schemaInfo.Minimum;
            schema.Maximum = schemaInfo.Maximum;
            schema.MinLength = schemaInfo.MinLength;
            schema.MaxLength = schemaInfo.MaxLength;
            schema.Format = schemaInfo.Format;
            schema.Pattern = schemaInfo.Pattern;
            schema.Example = schemaInfo.Example is not null ? new OpenApiString(schemaInfo.Example) : null;

            schema.Description = schemaInfo.Description ?? string.Empty;

            if (schemaInfo.IsPassword) schema.MinLength = Options.Password.RequiredLength;
            // todo password schema logic
        }

        if (DescriptorCache.RequiredFieldsMap.ContainsKey(context.Type.Name))
        {
            schema.Required = DescriptorCache.RequiredFieldsMap[context.Type.Name];
            DescriptorCache.RequiredFieldsMap.Remove(context.Type.Name);
        }
    }

    #endregion

    /// <summary>
    ///     认证配置依赖
    /// </summary>
    private IdentityOptions Options { get; }
}