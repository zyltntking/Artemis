using Artemis.App.Swashbuckle.Filters.DocumentFilters;
using Artemis.App.Swashbuckle.Filters.OperationFilters;
using Artemis.App.Swashbuckle.Filters.SchemaFilters;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Artemis.App.Swashbuckle;

/// <summary>
/// 扩展方法
/// </summary>
public static class SwashbuckleExtensions
{
    /// <summary>
    /// 生成Swagger
    /// </summary>
    /// <param name="options">生成配置</param>
    /// <param name="config">配置</param>
    public static void GenerateSwagger(this SwaggerGenOptions options, IGeneratorConfig config)
    {
        options.SchemaFilter<EnumerationSchemaFilter>();

        foreach (var (type, schema) in config.MappingTypeToSchema)
        {
            options.MapType(type, () => schema);
        }

        options.CustomOperationIds(description => (description.ActionDescriptor as ControllerActionDescriptor)?.ActionName);

        if (config.UseXmlCommentFiles)
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;

            var fileList = new List<string>();

            foreach (var pattern in config.XmlCommentFiles)
            {
                var files = Directory.GetFiles(directory, pattern);
                fileList.AddRange(files);
            }

            fileList = fileList.Distinct().ToList();

            foreach (var file in fileList)
            {
                options.IncludeXmlComments(file, true);
            }
        }

        options.CustomSchemaIds(DefaultSchemaIdSelector);

        options.EnableAnnotations();

        options.IgnoreObsoleteActions();

        options.IgnoreObsoleteProperties();

        if (config.UseAllOfToExtendReferenceSchemas)
        {
            options.UseAllOfToExtendReferenceSchemas(); // we prefer $ref over AllOf (and set up description ourselves)
        }

        if (config.GenerateExternal)
        {
            options.DocumentFilter<AddHostFilter>(config.HostName);
            options.DocumentFilter<AddSchemesFilter>();
        }
        options.DocumentFilter<AddProducesFilter>();
        options.DocumentFilter<AddConsumesFilter>();

        //options.OperationFilter<AddVersionParameterWithExactValueInQuery>();
        options.OperationFilter<ODataParametersSwaggerOperationFilter>();

        options.SchemaFilter<SchemaPropertiesTypesFilter>();
        //// Set the body name correctly
        //options.RequestBodyFilter<SetBodyNameExtensionFilter>();

        //// This is used to remove duplicated api-version query parameter
        //options.OperationFilter<RemoveDuplicateApiVersionParameterFilter>();

        //// This is used to add the x-ms-long-running-operation attribute and the options
        //options.OperationFilter<LongRunningOperationFilter>();
    }

    /// <summary>
    ///  默认SchemaId选择器
    /// </summary>
    /// <param name="type">类型</param>
    /// <returns></returns>
    private static string DefaultSchemaIdSelector(Type type)
    {
        if (!type.IsConstructedGenericType)
        {
            return type.Name;
        }

        var prefix = type.GetGenericArguments()
            .Select(DefaultSchemaIdSelector)
            .Aggregate((previous, current) => previous + current);

        return prefix + type.Name.Split('`').First();
    }

}