using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Artemis.Extensions.Rpc;

public class SomeFilter : IOperationFilter
{
    #region Implementation of IOperationFilter

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Responses.Remove("default");
    }

    #endregion
}