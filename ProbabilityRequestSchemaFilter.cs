using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Any;
using CalculatorAPI.Contracts;

namespace CalculatorAPI.Swagger
{
    public class ProbabilityRequestSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(ProbabilityRequest))
            {
                schema.Description ??= "Request payload for probability calculations. PA and PB are probabilities between 0.0 and 1.0.";
                schema.Example = new OpenApiObject
                {
                    ["PA"] = new OpenApiDouble(0.5),
                    ["PB"] = new OpenApiDouble(0.5),
                    ["Operation"] = new OpenApiString("Either")
                };
            }
        }
    }
}