using Artemis.App.Swashbuckle.Filters;
using Artemis.App.Swashbuckle.Filters.DocumentFilters;
using Artemis.App.Swashbuckle.Filters.OperationFilters;
using Artemis.App.Swashbuckle.Filters.SchemaFilters;
using Artemis.App.Swashbuckle.Options;
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

        //var actualDocumentsToGenerate = config.SupportedApiVersions;

        //var documentToGenerateList = actualDocumentsToGenerate.ToList();

        //if (!documentToGenerateList.Any())
        //{
        //    documentToGenerateList = new List<string>
        //    {
        //        config.DefaultApiVersion
        //    };
        //}

        //documentToGenerateList.ToList().ForEach(v => options.SwaggerDoc(v, OpenApiOptions.GetConfiguration(config, v).GetInfo()));

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

        // todo DocInclusionPredicate

        if (config.GenerateExternal)
        {
            // options.UseInlineDefinitionsForEnums(); // we prefer commonize enums
            options.DocumentFilter<AddHostFilter>(config.HostName);
            options.DocumentFilter<AddSchemesFilter>();
        }
        options.DocumentFilter<AddProducesFilter>();
        options.DocumentFilter<AddConsumesFilter>();

        // todo AddVersionParameterWithExactValueInQuery
        //options.OperationFilter<AddVersionParameterWithExactValueInQuery>();
        options.OperationFilter<ODataParametersSwaggerOperationFilter>();

        options.SchemaFilter<SchemaPropertiesTypesFilter>();
        // Set the body name correctly
        options.RequestBodyFilter<SetBodyNameExtensionFilter>();

        // todo RemoveDuplicateApiVersionParameterFilter
        // This is used to remove duplicated api-version query parameter
        //options.OperationFilter<RemoveDuplicateApiVersionParameterFilter>();

        // This is used to add the x-ms-long-running-operation attribute and the options
        options.OperationFilter<LongRunningOperationFilter>();

        //todo PolymorphismDocumentFilter

        // Schema level filters
        // Works in conjunction with PolymorphismDocumentFilter and PolymorphismSchemaFilter for GeoJsonObject like requirement.
        options.SchemaFilter<SubTypeOfFilter>();

        // Adds 'x-ms-azure-resource' extension to a class marked by Microsoft.Azure.OpenApiExtensions.Attributes.AzureResourceAttribute.
        options.SchemaFilter<AddAzureResourceExtensionFilter>();

        // Manipulates Descriptions of schema mostly used on common generic types
        options.SchemaFilter<CustomSchemaPropertiesFilter>();

        // todo AllReusableParametersFilter

        // Adds x-ms-mutability to the property marked by Microsoft.Azure.OpenApiExtensions.Attributes.MutabilityAttribute
        options.SchemaFilter<AddMutabilityExtensionFilter>();

        // Marked class will be flattened in client library by AutoRest to make it more user friendly.
        options.SchemaFilter<AddClientFlattenExtensionFilter>();

        // Adds "readOnly": true to the property marked by Microsoft.Azure.Global.Services.Common.Service.OpenApi.ValidationAttribute.ReadOnlyPropertyAttribute
        options.SchemaFilter<AddReadOnlyPropertyFilter>();

        ////Handle bug that Swashbuckle has: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/1488 , https://github.com/Azure/autorest/issues/3417
        //c.SchemaFilter<ReverseAllOfPropertiesFilter>();
        // Operation level filters
        // Set Description field using the XMLDoc summary if absent and clear summary. By
        // default Swashbuckle uses remarks tag to populate operation description instead
        // of using summary tag.
        options.OperationFilter<MoveSummaryToDescriptionFilter>();

        // Adds x-ms-pageable extension to operation marked with Page-able attribute.
        options.OperationFilter<AddPageableExtensionFilter>();

        // Adds x-ms-long-running-operation extension to operation marked with Microsoft.Azure.OpenApiExtensions.Attributes.LongRunningOperationAttribute.
        options.OperationFilter<AddLongRunningOperationExtensionFilter>();

        // todo DefaultResponseOperationFilter
        //options.OperationFilter<DefaultResponseOperationFilter>(config);

        // Clear all the supported mime type from response object. Supported mime type is
        // added at document level, with hard coded value of application/json.
        options.OperationFilter<SetProducesContentTypeFilter>();

        // Clear all consumed types except application/json.
        options.OperationFilter<SetConsumesContentTypeFilter>();

        // Removes parameters that shouldn't be on swagger (if process specifies--externalswagger-gen in command line)
        options.OperationFilter<HideParamInDocsFilter>(config.HideParameters);

        // This is applied if swagger is generated using open api 3.0 spec, helps to fix bug in autorest tool.
        // No impact for swagger generated using 2.0 spec.
        options.OperationFilter<ArrayInQueryParametersFilter>();

        // This is used to set default values, specifically to denote the api version as required parameter.
        options.OperationFilter<SwaggerDefaultValuesFilter>();

        // Adds x-ms-enum to a property enum type. Adds extension attributes to indicate
        // AutoRest to model enum as string. This is as per OpenAPI specifications.
        options.SchemaFilter<AddEnumExtensionFilter>(config.EnumsAsString);

        if (config.GenerateExternal)
        {
            //options.SchemaFilter<CustomSchemaInheritanceFilter>(config);

            // This is used to add x-ms-examples field to each operation. We use our own filter in order to allow for customizing the destination path.
            options.DocumentFilter<ExampleFilter>(config);

            //options.DocumentFilter<UpdateCommonRefsDocumentFilter>(config);
        }
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

/// <summary>
///  OpenApi配置
/// </summary>
internal static class OpenApiOptions
{
    /// <summary>
    ///  获取配置
    /// </summary>
    /// <param name="config"></param>
    /// <param name="version"></param>
    /// <returns></returns>
    public static Configuration GetConfiguration(IGeneratorConfig config, string? version = null)
    {
        var info = new OpenApiDocumentInfo(config.Title, config.Description, version ?? config.DefaultApiVersion, config.ClientName);
        return new Configuration(info);
    }
}