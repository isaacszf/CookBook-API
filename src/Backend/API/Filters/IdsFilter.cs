using API.Binders;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.Filters;

public class IdsFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var encryptedIds = context
            .ApiDescription
            .ParameterDescriptions
            .Where(x => x.ModelMetadata.BinderType == typeof(CookBookIdBinder))
            .ToDictionary(d => d.Name, d => d);

        foreach (var parameter in operation.Parameters)
        {
            if (!encryptedIds.TryGetValue(parameter.Name, out var apiParameter)) continue;
            parameter.Schema.Format = string.Empty;
            parameter.Schema.Type = "string";
        }

        foreach (var schema in context.SchemaRepository.Schemas.Values)
        {
            foreach (var property in schema.Properties)
            {
                if (!encryptedIds.TryGetValue(property.Key, out var apiParameter)) continue;
                property.Value.Format = string.Empty;
                property.Value.Type = "string";
            }
        }
    }
}