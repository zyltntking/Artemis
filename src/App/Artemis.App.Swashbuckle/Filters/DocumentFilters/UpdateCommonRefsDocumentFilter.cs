﻿using Artemis.App.Swashbuckle.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Artemis.App.Swashbuckle.Filters.DocumentFilters;

/// <summary>
/// Removes definitions and parameters to be used from external sources and updates it.
/// </summary>
public class UpdateCommonRefsDocumentFilter : IDocumentFilter
{
    public const string DefinitionsPrefix = "#/definitions/";
    public const string ParametersPrefix = "#/parameters/";
    private readonly IGeneratorConfig _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateCommonRefsDocumentFilter"/> class.
    /// </summary>
    /// <param name="swaggerConfig">Swagger config.</param>
    public UpdateCommonRefsDocumentFilter(IGeneratorConfig swaggerConfig)
    {
        _config = swaggerConfig;
    }

    /// <summary>
    /// Applies filter.
    /// </summary>
    /// <param name="swaggerDoc">swaggerDoc.</param>
    /// <param name="context">context.</param>
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        swaggerDoc.Paths.ToList().ForEach(path =>
        {
            path.Value.Operations.ToList().ForEach(operation =>
            {
                CommonizeOperationParameter(operation);
            });
        });

        // add type=Object to all AllOf schema
        foreach (var def in swaggerDoc.Components.Schemas)
        {
            var props = def.Value.Properties.Where(prop => prop.Value.AllOf != null && prop.Value.Type == null);
            var list = props.ToList();
            list.ForEach(prop => prop.Value.Type = "object");
        }

        var fixingActions = new List<Action>();

        foreach (var schema in swaggerDoc.Components.Schemas)
        {
            var commonizePropertiesFixingActions = CommonizeSchemaProperties(schema);
            fixingActions.AddRange(commonizePropertiesFixingActions);

            // Commonize Schemas to RP ReusableParameters
            var rpResusableParam = _config.ResourceProviderReusableParameters.FirstOrDefault(param => param.Key.Equals(schema.Key, StringComparison.InvariantCultureIgnoreCase));
            if (rpResusableParam.Key != null)
            {
                // the name of the Reusable parameter can be different than the one in the common file
                var nameOfResusableParameter = rpResusableParam.Value;
                // setting external references to common definitions
                fixingActions.Add(() =>
                {
                    var refSchema = new OpenApiSchema
                    {
                        Reference = new OpenApiReference { ExternalResource = $"{_config.RPCommonFilePath}{ParametersPrefix}{nameOfResusableParameter}" },
                        Description = schema.Value.Description
                    };
                    swaggerDoc.Components.Schemas.Remove(schema.Key);
                    swaggerDoc.Components.Schemas.Add(schema.Key, refSchema);
                });
            }

            // Commonize Schemas to Version ReusableParameters
            if (_config.VersionCommonReusableDefinitions.ContainsKey(schema.Key))
            {
                // setting external references to common definitions
                fixingActions.Add(() =>
                {
                    var refSchema = new OpenApiSchema
                    {
                        Reference = new OpenApiReference { ExternalResource = $"{_config.VersionCommonFolderPath}{_config.VersionCommonReusableDefinitions[schema.Key]}.json{DefinitionsPrefix}{schema.Key}" },
                        Description = schema.Value.Description
                    };
                    swaggerDoc.Components.Schemas.Remove(schema.Key);
                    swaggerDoc.Components.Schemas.Add(schema.Key, refSchema);
                });
            }
        }

        fixingActions.ForEach(fixAction => fixAction());

        RemoveReusedEntitiesFromDocument(swaggerDoc);
    }

    private List<Action> CommonizeSchemaProperties(KeyValuePair<string, OpenApiSchema> schema)
    {
        List<Action> fixingActions = new List<Action>();
        // Handle all schemas that have properties which are common
        foreach (var prop in schema.Value.Properties)
        {
            // if one of the properties is a reference to a common defination, clean it up and leave only the reference+description
            // used to commonize enums
            var id = prop.Value.Reference?.Id ?? prop.Value.AllOf?.FirstOrDefault()?.Reference?.Id;

            var rpResusableParam = _config.ResourceProviderReusableParameters.FirstOrDefault(param => param.Key.Equals(id, StringComparison.InvariantCultureIgnoreCase));
            if (id != null &&
                (rpResusableParam.Key != null ||
                _config.VersionCommonReusableDefinitions.ContainsKey(id)))
            {
                //commonize property of schema
                fixingActions.Add(() =>
                {
                    var referenceLink = _config.VersionCommonReusableDefinitions.ContainsKey(id) ?
                        $"{_config.VersionCommonFolderPath}{_config.VersionCommonReusableDefinitions[id]}.json{DefinitionsPrefix}{id}" :
                        $"{DefinitionsPrefix}{id}";

                    var refSchema = new OpenApiSchema
                    {
                        Extensions = new Dictionary<string, IOpenApiExtension>
                            {
                                    { "$ref", new OpenApiString(referenceLink) } // adding ref as extension cause according to JSON standards $ref shouldne have any other properties
                            },
                        Description = prop.Value.Description,
                        ReadOnly = prop.Value.ReadOnly,
                        Required = prop.Value.Required,
                        Type = prop.Value.Type,
                        Example = prop.Value.Example,
                    };
                    schema.Value.Properties.Remove(prop.Key);
                    schema.Value.Properties.Add(prop.Key, refSchema);
                });
            }
        }
        return fixingActions;
    }

    private void CommonizeOperationParameter(KeyValuePair<OperationType, OpenApiOperation> op)
    {
        // commonize parameters to global common
        foreach (var resuseParam in _config.GlobalCommonReusableParameters)
        {
            var reuseParamName = resuseParam.Key;
            var opParam = op.Value.Parameters.FirstOrDefault(p => p.Name == reuseParamName || p.Reference?.Id == reuseParamName);
            if (opParam != null)
            {
                opParam.Reference = new OpenApiReference { ExternalResource = $"{_config.GlobalCommonFilePath}{ParametersPrefix}{reuseParamName}" };
            }
        }

        // commonize parameters to resource provider common
        foreach (var resuseParamKeyValue in _config.ResourceProviderReusableParameters)
        {
            var reuseParamName = resuseParamKeyValue.Key;
            var commonParamName = resuseParamKeyValue.Value;
            var opParam = op.Value.Parameters.FirstOrDefault(p => reuseParamName.Equals(p.Name, StringComparison.OrdinalIgnoreCase) || p.Reference?.Id == reuseParamName);
            if (opParam != null)
            {
                opParam.Reference = new OpenApiReference { ExternalResource = $"{_config.RPCommonFilePath}{ParametersPrefix}{commonParamName}" };
            }
        }

        // commonize version parameters
        foreach (var resuseParam in _config.VersionCommonReusableDefinitions)
        {
            var reuseParamName = resuseParam.Key;
            var reuseParamFileName = resuseParam.Value;
            var opParam = op.Value.Parameters.FirstOrDefault(p => reuseParamName.Equals(p.Name, StringComparison.OrdinalIgnoreCase) || p.Reference?.Id == reuseParamName);
            if (opParam != null)
            {
                opParam.Reference = new OpenApiReference { ExternalResource = $"{_config.VersionCommonFolderPath}{reuseParamFileName}.json{ParametersPrefix}{reuseParamName}" };
            }
        }
    }

    private void RemoveReusedEntitiesFromDocument(OpenApiDocument swaggerDoc)
    {
        foreach (var reuseParam in _config.GlobalCommonReusableParameters)
        {
            swaggerDoc.Components.Parameters.Remove(reuseParam.Key);
        }

        foreach (var reuseParam in _config.ResourceProviderReusableParameters.Select(kvp => kvp.Key))
        {
            swaggerDoc.Components.Parameters.Remove(reuseParam);
        }

        foreach (var reuseParam in _config.VersionCommonReusableDefinitions)
        {
            swaggerDoc.Components.Schemas.Remove(reuseParam.Key);
        }
    }
}