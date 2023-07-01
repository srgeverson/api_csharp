using api_csharp.Core;
using AppClassLibraryDomain.DAO.SQL;
using AppClassLibraryDomain.DAO;
using AppClassLibraryDomain.facade;
using AppClassLibraryDomain.service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using api_csharp.API.exceptionhandler;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

#region Swagger 3.0
// GitHub do ASP.NET API Versioning:
// https://github.com/microsoft/aspnet-api-versioning

// GitHub do projeto que utilizei como base para a
// a implementacaoo desta aplicacaoo:
// https://github.com/microsoft/aspnet-api-versioning/tree/master/samples/aspnetcore/SwaggerSample

// Algumas referencias sobre ASP.NET API Versioning:
// https://devblogs.microsoft.com/aspnet/open-source-http-api-packages-and-tools/
// https://www.hanselman.com/blog/aspnet-core-restful-web-api-versioning-made-easy

builder.Services.AddApiVersioning(options =>
{
    // Retorna os headers "api-supported-versions" e "api-deprecated-versions"
    // indicando versoes suportadas pela API e o que esta como deprecated
    options.ReportApiVersions = true;

    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(2, 0);
});

builder.Services.AddVersionedApiExplorer(options =>
{
    // Agrupar por numero de versao
    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
    // note: the specified format code will format the version as "'v'major[.minor][-status]"
    options.GroupNameFormat = "'v'VVV";

    // Necessario para o correto funcionamento das rotas
    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
    // can also be used to control the format of the API version in route templates
    options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
#endregion

#region DAOs
var conexao = Environment.GetEnvironmentVariable("CONNECTION_STRING_api_csharp");
//ConexaoDAO.URLCONEXAO = app.Configuration.GetSection("ConnectionString")["DefaultConnection"];
builder.Services.AddSingleton<IPermissaoDAO>(new PermissaoSQLDAO() { UrlConnection = conexao });
builder.Services.AddSingleton<ISistemaDAO>(new SistemaSQLDAO() { UrlConnection = conexao });
builder.Services.AddSingleton<IUsuarioDAO>(new UsuarioSQLDAO() { UrlConnection = conexao });
builder.Services.AddSingleton<IUsuarioPermissaoDAO>(new UsuarioPermissaoSQLDAO() { UrlConnection = conexao });
#endregion

#region Servços
builder.Services.AddSingleton<IContatoService, ContatoService>();
builder.Services.AddSingleton<IPermissaoService, PermissaoService>();
builder.Services.AddSingleton<ISistemaService, SistemaService>();
builder.Services.AddSingleton<IUsuarioPermissaoService, UsuarioPermissaoService>();
builder.Services.AddSingleton<IUsuarioService, UsuarioService>();
#endregion

#region Fachadas
builder.Services.AddSingleton<IAuthorizationServerFacade, AuthorizationServerFacade>();
#endregion

#region
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddGlobalExceptionHandlerMiddleware();
#endregion

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = "Description",
        Name = "Name",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme()
            {
                Reference=new OpenApiReference()
                {
                    Type = ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
    options.DocumentFilter<TagDescriptionsDocumentFilter>();
    options.OperationFilter<SwaggerDefaultValues>();
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var key = string.Empty;

builder.Services
    .AddAuthentication(a =>
    {
        a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
.AddJwtBearer(jwt => {
    jwt.RequireHttpsMetadata = false;
    jwt.SaveToken = true;
    jwt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = null,//key,
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(options =>
{
    // Geracaoo de um endpoint do Swagger para cada versao descoberta
    foreach (var description in
        app.Services.GetRequiredService<IApiVersionDescriptionProvider>().ApiVersionDescriptions)
        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();