using System.Text.RegularExpressions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Artemis.Extensions.Rpc;

public class SomeFilter2 : ISchemaFilter
{
    #region Implementation of ISchemaFilter

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var regex = new Regex(@"\[(.*?)\]");

        var des = schema.Description;

        if (des is not null)
        {
            var matches = regex.Matches(des);

            if (matches.Any(item => item.Value.Contains("required")))
            {

            }

            var newdes = regex.Replace(des, "");
        }
    }



    #endregion
}