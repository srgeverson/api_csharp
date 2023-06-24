using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class TagDescriptionsDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var todasTags = new List<OpenApiTag> {
                new OpenApiTag { Name = "Usuario", Description = "Controlador responsável pela autenticação e controle de acesso da API." },
                new OpenApiTag { Name = "Documento", Description = "Controlador responsável pela disponibilização de ducumentos da aplicação." },
            };
        var tagsRespectivaVersao = new List<OpenApiTag>();
        todasTags.ForEach(tag =>
        {
            if (swaggerDoc.Paths.Where(p => p.Key.StartsWith(string.Concat("/", context.DocumentName, "/", tag.Name, "s"))).Any())
                tagsRespectivaVersao.Add(tag);
        });
        swaggerDoc.Tags = tagsRespectivaVersao;
    }
}