using api_csharp.API.exceptionhandler;
using AppClassLibraryDomain.DAO;
using AppClassLibraryDomain.DAO.SQL;
using AppClassLibraryDomain.facade;
using AppClassLibraryDomain.service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddGlobalExceptionHandlerMiddleware();

#region DI

//Others
//builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//builder.Services.AddSingleton<ISessaoUsuarioHelper, SessaoUsuarioHelper>();

//DAO
var conexao = Environment.GetEnvironmentVariable("CONNECTION_STRING_AspNetMVC");
//ConexaoDAO.URLCONEXAO = app.Configuration.GetSection("ConnectionString")["DefaultConnection"];
builder.Services.AddSingleton<IPermissaoDAO>(new PermissaoSQLDAO() { UrlConnection = conexao });
builder.Services.AddSingleton<ISistemaDAO>(new SistemaSQLDAO() { UrlConnection = conexao });
builder.Services.AddSingleton<IUsuarioDAO>(new UsuarioSQLDAO() { UrlConnection = conexao });
builder.Services.AddSingleton<IUsuarioPermissaoDAO>(new UsuarioPermissaoSQLDAO() { UrlConnection = conexao });

//Sevices
builder.Services.AddSingleton<IPermissaoService, PermissaoService>();
builder.Services.AddSingleton<ISistemaService, SistemaService>();
builder.Services.AddSingleton<IUsuarioPermissaoService, UsuarioPermissaoService>();
builder.Services.AddSingleton<IUsuarioService, UsuarioService>();

//Facades
builder.Services.AddSingleton<IAuthorizationServerFacade, AuthorizationServerFacade>();

#endregion

builder.Services.AddControllers();

#region Open API

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    var urlAPI = builder.Configuration.GetSection("OAuth")["Secret"];

    option.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "API de Gerenciamento de Usuários.",
        Description = "Gerenciar e acompahar os acessos",
        TermsOfService = new Uri(builder.Configuration.GetSection("OpenApiInfo")["UrlTermsOfService"]),
        Contact = new OpenApiContact
        {
            Name = "Contrato",
            Url = new Uri(builder.Configuration.GetSection("OpenApiInfo")["UrlContact"])
        },
        License = new OpenApiLicense
        {
            Name = "Licença",
            Url = new Uri(builder.Configuration.GetSection("OpenApiInfo")["UrlLicense"])
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
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

#endregion

#region Authorization

var secret = builder.Configuration.GetSection("OAuth")["Secret"];
var key = Encoding.ASCII.GetBytes(secret);

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.RequireHttpsMetadata = false;
    option.SaveToken = true;
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseGlobalExceptionHandlerMiddleware();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAuthorization();

app.MapControllers();

app.Run();
