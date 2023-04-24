using System.Reflection;
using Artemis.App.Swashbuckle.Attributes;
using Artemis.App.Swashbuckle.Filters.DocumentFilters;
using Artemis.App.Swashbuckle.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Artemis.App.Swashbuckle.Filters.SchemaFilters;

public class CustomSchemaInheritanceFilter : ISchemaFilter
{
    private readonly IGeneratorConfig _config;

    public CustomSchemaInheritanceFilter(IGeneratorConfig config)
    {
        _config = config;
    }
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var attr = context.Type.GetCustomAttribute<CustomSwaggerSchemaInheritanceAttribute>();
        if (attr != null && schema.Properties.Any())
        {
            var propsToRemove = schema.Properties
                .Where(p => !attr.NotInheritedPropertiesName
                    .Select(prop => prop.ToLower())
                    .Contains(p.Key.ToLower()))
                .ToList();

            foreach (var prop in propsToRemove)
            {
                var definationToRemove = prop.Value.AllOf?.FirstOrDefault()?.Reference?.Id;
                if (definationToRemove != null)
                {
                    // remove AttachedSchemas
                    context.SchemaRepository.Schemas.Remove(definationToRemove);
                }
                // remove property as it is inherited from common object
                schema.Properties.Remove(prop);
            }

            schema.AllOf = new List<OpenApiSchema>
                {
                    new OpenApiSchema
                    {
                        Reference = new OpenApiReference
                        {
                            ExternalResource = GetExternalLink(attr)
                        }
                    }
                };
        }
    }

    private string GetExternalLink(CustomSwaggerSchemaInheritanceAttribute attr)
    {
        switch (attr.DefinitionLevel)
        {
            case CommonObjectType.ResourceProviderCommonDefinition:
                return $"{_config.RPCommonFilePath}{UpdateCommonRefsDocumentFilter.DefinitionsPrefix}{attr.ExternalSchemaName}";
            case CommonObjectType.GlobalCommonDefinition:
                return $"{_config.GlobalCommonFilePath}{UpdateCommonRefsDocumentFilter.DefinitionsPrefix}{attr.ExternalSchemaName}";
            default:
                throw new NotSupportedException("Please add more support here...");
        }
    }
}