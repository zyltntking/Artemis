using System.Reflection;
using Artemis.Extensions.Web.OpenApi.Attributes;
using Artemis.Extensions.Web.OpenApi.Filters;
using Artemis.Extensions.Web.OpenApi.Filters.DocumentFilters;
using Artemis.Extensions.Web.OpenApi.Filters.OperationFilters;
using Artemis.Extensions.Web.OpenApi.Filters.SchemaFilters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Artemis.Extensions.Web.OpenApi;

/// <summary>
///     OpenApi扩展
/// </summary>
public static class OpenApiExtensions
{
    /// <summary>
    ///     添加OpenApi文档
    /// </summary>
    /// <param name="builder">WebApp Builder</param>
    /// <param name="config">配置</param>
    public static IServiceCollection AddOpenApiDoc(this WebApplicationBuilder builder, DocumentConfig config)
    {
        builder.Services.AddEndpointsApiExplorer();

        config.EnsureValidity();

        builder.Services.AddSwaggerGen(options =>
        {
            var actualDocumentsToGenerate = config.ApiVersions;

            actualDocumentsToGenerate.ToList().ForEach(version =>
                options.SwaggerDoc(version, GetConfiguration(config, version).OpenApiInfo));

            // 设置 IFormFile 类型为 "file"
            options.MapType(typeof(IFormFile), () => new OpenApiSchema { Type = "file", Format = "binary" });
            options.MapType(typeof(Stream), () => new OpenApiSchema { Type = "file", Format = "binary" });

            foreach (var kvp in config.TypeSchemaMapping) options.MapType(kvp.Key, () => kvp.Value);

            options.CustomOperationIds(d => (d.ActionDescriptor as ControllerActionDescriptor)?.ActionName);

            if (config.UseXmlCommentFiles)
            {
                var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly)
                    .ToList();
                xmlFiles.ForEach(xmlFile => options.IncludeXmlComments(xmlFile));
            }

            options.CustomSchemaIds(SchemaIdSelector);

            options.EnableAnnotations();

            if (!config.GenerateExternalSwagger)
            {
                // options.UseInlineDefinitionsForEnums();
            }

            if (config.UseAllOfToExtendReferenceSchemas) options.UseAllOfToExtendReferenceSchemas();

            options.DocInclusionPredicate((docName, apiDesc) => DocumentApiInclusion(config, docName, apiDesc));

            if (config.GenerateExternalSwagger)
            {
                options.DocumentFilter<AddHostFilter>(config);
                options.DocumentFilter<AddSchemesFilter>();
            }

            options.DocumentFilter<AddProducesFilter>();
            options.DocumentFilter<AddConsumesFilter>();

            options.OperationFilter<AddVersionParameterWithExactValueInQuery>();
            options.OperationFilter<ODataParametersSwaggerOperationFilter>();

            options.SchemaFilter<SchemaPropertiesTypesFilter>();
            // 设置正确的body名称
            options.RequestBodyFilter<SetBodyNameFilter>();

            // 删除重复的api-version参数
            options.OperationFilter<RemoveDuplicateApiVersionParameter>();

            // 添加慢查询接口属性x-ms-long-running-operation
            options.OperationFilter<LongRunningOperationFilter>();
        });

        return builder.Services;
    }

    /// <summary>
    ///     使用OpenApi文档
    /// </summary>
    /// <param name="app"></param>
    /// <param name="config">配置</param>
    public static WebApplication UseOpenApiDoc(this WebApplication app, DocumentConfig config)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger(options => { options.RouteTemplate = "swagger/{documentName}/swagger.json"; });

            app.UseSwaggerUI(options =>
            {
                config.ApiVersions.ToList().ForEach(version =>
                {
                    options.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{config.Title} {version}");
                });
            });

            app.UseReDoc();
        }

        return app;
    }

    /// <summary>
    ///     获取配置
    /// </summary>
    /// <param name="config">文档配置</param>
    /// <param name="version">API版本号</param>
    /// <returns></returns>
    private static Configuration GetConfiguration(DocumentConfig config, string? version = null)
    {
        var info = new OpenApiDocumentInfo(config.Title, config.Description, version ?? config.DefaultApiVersion,
            config.ClientName);

        return new Configuration(info);
    }

    /// <summary>
    ///     模式ID选择器
    /// </summary>
    /// <param name="modelType"></param>
    /// <returns></returns>
    private static string SchemaIdSelector(Type modelType)
    {
        // 检查泛型参数是否有自定义名称
        if (modelType.IsConstructedGenericType)
        {
            var firstGenericArgType = modelType.GetGenericArguments()[0];
            var applyToParentGenericParamNaming = firstGenericArgType.GetCustomAttributes<SchemaNameStrategyAttribute>()
                .SingleOrDefault(at => at.NamingStrategy == NamingStrategy.ApplyToParentWrapper);
            if (applyToParentGenericParamNaming != null)
                return applyToParentGenericParamNaming.CustomNameProvider.GetName(modelType);
        }

        var customNamingAttribute = modelType.GetCustomAttributes<SchemaNameStrategyAttribute>()
            .SingleOrDefault(at => at.NamingStrategy == NamingStrategy.Custom);
        if (customNamingAttribute != null) return customNamingAttribute.CustomNameProvider.GetName(modelType);

        if (!modelType.IsConstructedGenericType) return modelType.Name;

        var prefix = modelType.GetGenericArguments()
            .Select(SchemaIdSelector)
            .Aggregate((previous, current) => previous + current);

        return prefix + modelType.Name.Split('`').First();
    }

    /// <summary>
    ///     文档API包含指示
    /// </summary>
    /// <param name="config">文档配置</param>
    /// <param name="docName">文档名称</param>
    /// <param name="apiDesc">Api描述</param>
    /// <returns></returns>
    private static bool DocumentApiInclusion(DocumentConfig config, string docName, ApiDescription apiDesc)
    {
        return ApiVersionInclusion(config, docName, apiDesc) && VisibilityInclusion(config, apiDesc);
    }

    /// <summary>
    ///     文档可见性包含
    /// </summary>
    /// <param name="config">文档配置</param>
    /// <param name="apiDesc">Api描述</param>
    /// <returns></returns>
    private static bool VisibilityInclusion(DocumentConfig config, ApiDescription apiDesc)
    {
        if (config.GenerateExternalSwagger)
        {
            var apiHideInDocsAttributes = apiDesc.ActionDescriptor.EndpointMetadata
                .Where(x => x is HideInDocsAttribute).Cast<HideInDocsAttribute>();

            return !apiHideInDocsAttributes.Any();
        }

        return true;
    }

    /// <summary>
    ///     API版本包含
    /// </summary>
    /// <param name="config">文档配置</param>
    /// <param name="docName">文档名称</param>
    /// <param name="apiDesc">Api描述</param>
    /// <returns></returns>
    private static bool ApiVersionInclusion(DocumentConfig config, string docName, ApiDescription apiDesc)
    {
        var supportedApiVersions = config.SupportedApiVersions;

        if ((supportedApiVersions == null || !supportedApiVersions.Any()) && !config.GenerateExternalSwagger)
            // 生成一个版本
            return true;

        // 过滤每个Api版本的api端点
        var metadata = apiDesc.ActionDescriptor.EndpointMetadata;
        var apiVersionAttributes = metadata.Where(x => x.GetType() == typeof(ApiVersionAttribute))
            .Cast<ApiVersionAttribute>().ToList();
        var apiVersionRangeAttributes =
            metadata.Where(x => x is ApiVersionRangeAttribute).Cast<ApiVersionRangeAttribute>().ToList();

        var endpointMappedApiVersionsAttributes = metadata.Where(x => x.GetType() == typeof(MapToApiVersionAttribute))
            .Cast<MapToApiVersionAttribute>().ToList();

        if (!apiVersionAttributes.Any() &&
            !endpointMappedApiVersionsAttributes.Any() &&
            !apiVersionRangeAttributes.Any())
            // 端与版本无关，将包含在所有版本中
            return true;

        var currentDocApiVersion = ApiVersion.Parse(docName);
        if (apiVersionRangeAttributes.Any(range =>
                currentDocApiVersion >= range.FromVersion &&
                (range.ToVersion == null || currentDocApiVersion < range.ToVersion)))
            return true;

        var apiVersions = apiVersionAttributes.SelectMany(x => x.Versions.Select(y => y.ToString())).ToList();

        var endpointVersions = endpointMappedApiVersionsAttributes.SelectMany(x => x.Versions.Select(y => y.ToString()))
            .ToList();

        if (endpointVersions.Any()) return endpointVersions.Any(v => v == docName);
        return apiVersions.Any(v => v == docName);
    }
}