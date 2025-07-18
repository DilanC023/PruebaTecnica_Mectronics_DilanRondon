using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EstudiantesApp.API.SwaggerDocumentation
{
    // Clase para mostrar requerimientos de rol en Swagger
    public class FiltroRequerimientoRolesDocumentacion : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var path in swaggerDoc.Paths.Values)
            {
                foreach (var operation in path.Operations.Values)
                {
                    var authAttributes = context.ApiDescriptions
                        .FirstOrDefault(x => x.HttpMethod == operation.OperationId)?
                        .ActionDescriptor?.EndpointMetadata
                        .OfType<AuthorizeAttribute>();

                    if (authAttributes != null && authAttributes.Any())
                    {
                        var roles = authAttributes
                            .SelectMany(a => a.Roles?.Split(',') ?? Array.Empty<string>())
                            .Distinct();

                        if (roles.Any())
                        {
                            operation.Description += $"<p><strong>Roles requeridos:</strong> {string.Join(", ", roles)}</p>";
                        }
                    }
                }
            }
        }
    }
}
