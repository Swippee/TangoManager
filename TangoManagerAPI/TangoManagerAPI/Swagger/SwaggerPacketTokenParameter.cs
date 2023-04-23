using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;

namespace TangoManagerAPI.Swagger
{
    internal sealed class SwaggerPacketTokenParameter : IOperationFilter
    {

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "PacketToken",
                In = ParameterLocation.Header,
                Required = false, // set to false if this is optional
                AllowEmptyValue = false,
                Description = "PacketToken guid"
            });
        }
    }
}
