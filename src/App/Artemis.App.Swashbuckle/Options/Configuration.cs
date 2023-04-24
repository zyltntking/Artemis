using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace Artemis.App.Swashbuckle.Options;

/// <summary>
/// OpenApi document configuration.
/// </summary>
internal class Configuration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Configuration"/> class.
    /// </summary>
    /// <param name="info">Document info section details.</param>
    internal Configuration(OpenApiDocumentInfo info)
    {
        _info = info ?? throw new ArgumentNullException(nameof(info), "Can not be null.");
    }

    /// <summary>
    /// OpenAPI document info section properties.
    /// </summary>
    private readonly OpenApiDocumentInfo _info;

    /// <summary>
    /// Return api version.
    /// </summary>
    /// <returns>Api version.</returns>
    public string GetVersion()
    {
        return _info.Version;
    }

    /// <summary>
    /// Populate openAPI decoument 'info' section.
    /// </summary>
    /// <returns>OpenApiInfo.</returns>
    public OpenApiInfo GetInfo()
    {
        return new OpenApiInfo
        {
            Title = _info.Title,
            Version = _info.Version,
            Description = _info.Description,
            Extensions =
                {
                    ["x-ms-code-generation-settings"] = new OpenApiObject
                    {
                        ["name"] = new OpenApiString(_info.ClientName),
                    },
                },
        };
    }

    ///// <summary>
    ///// Populate openAPI decoument 'security' section.
    ///// </summary>
    ///// <returns>OpenApiSecurityRequirement object.</returns>
    //public OpenApiSecurityRequirement GetOpenApiSecurityRequirement()
    //{
    //    return new OpenApiSecurityRequirement
    //        {
    //            {
    //                new OpenApiSecurityScheme
    //                {
    //                    Reference = new OpenApiReference
    //                    {
    //                        Type = ReferenceType.SecurityScheme,
    //                        Id = "azure_auth",
    //                    },
    //                },
    //                new List<string>() { "user_impersonation" }
    //            },
    //        };
    //}

    ///// <summary>
    ///// Populate openAPI decoument 'securityDefinitions' section.
    ///// </summary>
    ///// <returns>OpenApiSecurityScheme object.</returns>
    //public OpenApiSecurityScheme GetAzureSecurityDefinition()
    //{
    //    return new OpenApiSecurityScheme
    //    {
    //        Type = SecuritySchemeType.OAuth2,
    //        Flows = new OpenApiOAuthFlows()
    //        {
    //            Implicit = new OpenApiOAuthFlow()
    //            {
    //                AuthorizationUrl = new Uri("https://login.microsoftonline.com/common/oauth2/authorize"),
    //                Scopes = new Dictionary<string, string> { ["user_impersonation"] = "impersonate your user account" },
    //            },
    //        },
    //        Description = "Azure Active Directory OAuth2 Flow",
    //    };
    //}
}