using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Artemis.Extensions.Web.OpenApi.Filters.DocumentFilters;

/// <summary>
///     多态性文档
/// </summary>
public sealed class PolymorphismDocumentFilter : IDocumentFilter
{
    private readonly IList<Type> _baseTypes;

    /// <summary>
    ///     初始化<see cref="PolymorphismDocumentFilter" />实例
    /// </summary>
    /// <param name="baseTypes">多态性基类</param>
    public PolymorphismDocumentFilter(IList<Type> baseTypes)
    {
        _baseTypes = baseTypes;
    }

    #region Implementation of IDocumentFilter

    /// <summary>
    ///     Applies filter.
    /// </summary>
    /// <param name="openApiDoc">OpenApiDocument.</param>
    /// <param name="context">DocumentFilterContext.</param>
    public void Apply(OpenApiDocument openApiDoc, DocumentFilterContext context)
    {
        foreach (var abstractType in _baseTypes)
            RegisterSubClasses(openApiDoc, context.SchemaRepository, context.SchemaGenerator, abstractType);
    }

    /// <summary>
    ///     注册子类
    /// </summary>
    /// <param name="openApiDoc"></param>
    /// <param name="schemaRegistry"></param>
    /// <param name="schemaGenerator"></param>
    /// <param name="abstractType"></param>
    private void RegisterSubClasses(OpenApiDocument openApiDoc, SchemaRepository schemaRegistry,
        ISchemaGenerator schemaGenerator, Type abstractType)
    {
        var converterAttribute = abstractType.GetCustomAttribute<JsonConverterAttribute>();
    }

    #endregion
}