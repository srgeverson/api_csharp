using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class TagDescriptionsDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        swaggerDoc.Tags = new List<OpenApiTag> {
                new OpenApiTag { Name = "Autenticacao", Description = "Controlador responsável pela autenticação e controle de acesso da API." },
                new OpenApiTag { Name = "Documentacao", Description = "Controlador responsável pela disponibilização de ducumentos da aplicação." },
            };
    }
}