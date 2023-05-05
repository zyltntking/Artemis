﻿using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Artemis.App.Swashbuckle.Filters.OperationFilters;

/// <summary>
///     By default MVC API versioning library adding "api-version" query parameter to each API.
///     And it is getting duplicated when there is an additional model binding with
///     same "api-version" parameter in any API while actually using this parameter.
///     This operation filter removes the duplicated "api-version" query parameter.
/// </summary>
public class RemoveDuplicateApiVersionParameterFilter : IOperationFilter
{
    private const string HttpHeaderNamesApiVersion = "api-version";

    /// <inheritdoc />
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation is null) throw new ArgumentNullException(nameof(operation));

        if (context is null) throw new ArgumentNullException(nameof(context));

        var apiDescription = context.ApiDescription;

        if (operation.Parameters == null) return;

        var isApiVersionParameterDuplicated =
            operation.Parameters.Count(parameter => parameter.Name == HttpHeaderNamesApiVersion) > 1;
        if (!isApiVersionParameterDuplicated) return;

        var description = apiDescription.ParameterDescriptions.FirstOrDefault(p =>
            p is { Name: HttpHeaderNamesApiVersion, DefaultValue: not null });

        // Creating copy to avoid Error "Collection was modified; enumeration operation may not execute"
        var parameterAddedSoFar = false;
        foreach (var parameter in operation.Parameters.OfType<OpenApiParameter>()
                     .Where(p => p.Name == HttpHeaderNamesApiVersion).ToList())
        {
            // remove duplicated "api-version" query parameters
            // any element but first
            if (parameterAddedSoFar)
                operation.Parameters.Remove(parameter);

            // Do not populate default value for api-version while specification generation is going on at build time.
            // while populate for local testing.
            // first element only
            else if (
                //!this.config.IsSwaggerSpecGenerationInProgress &&
                parameter.Schema.Default == null)
                parameter.Schema.Default = new OpenApiString(description?.DefaultValue?.ToString());

            parameterAddedSoFar = true;
        }
    }
}