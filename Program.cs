using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

#region Add services to the container.
builder.Services.AddControllers();
#endregion

#region Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersioning(setup =>
{
    setup.DefaultApiVersion = new ApiVersion(2, 0);
    setup.AssumeDefaultVersionWhenUnspecified = true;
    setup.ReportApiVersions = true;
});

//builder.Services.AddVersionedApiExplorer(setup =>
//{
//    setup.GroupNameFormat = "'v'VVV";
//    setup.SubstituteApiVersionInUrl = true;
//});
#endregion

builder.Services.AddSwaggerGen(option =>
{
    //option.SwaggerDoc("v2", new OpenApiInfo { Title = "V2 API C#", Version = "v2" });

    option.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "API de Gerenciamento de Usuários.",
        Description = "Gerenciar e acompahar os acessos",
        TermsOfService = new Uri(uriString: "http://localhost:5299/TermsOfService"),
        Contact = new OpenApiContact
        {
            Name = "Contrato",
            Url = new Uri(uriString: "http://localhost:5299/Contact")
        },
        License = new OpenApiLicense
        {
            Name = "Licença",
            Url = new Uri(uriString: "http://localhost:5299/License")
        }
    });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Insira o token validado aqui",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer",
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

//builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

var app = builder.Build();

#region Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseSwaggerUI(c =>
    //{
    //    c.RoutePrefix = "v2";
    //    var basePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
    //    c.SwaggerEndpoint($"{basePath}/swagger/{c.RoutePrefix}/swagger.json", "V2 API C#");
    //});
}
#endregion

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
