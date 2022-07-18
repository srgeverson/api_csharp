using api_csharp.API.exceptionhandler;
using domain.DAO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

//Teste
IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.Development.json")
                .Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddGlobalExceptionHandlerMiddleware();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "API de Gerenciamento de Usuários.",
        Description = "Gerenciar e acompahar os acessos",
        TermsOfService = new Uri("https://github.com/srgeverson/api_csharp"),
        Contact = new OpenApiContact
        {
            Name = "Contrato",
            Url = new Uri("https://github.com/srgeverson/api_csharp")
        },
        License = new OpenApiLicense
        {
            Name = "Licença",
            Url = new Uri("https://github.com/srgeverson/api_csharp")
        }
    });
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var secret = configuration.GetSection("OAuth")["Secret"];
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

var app = builder.Build();

//IConfigurationRoot configuration = new ConfigurationBuilder()
//                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
//                .AddJsonFile(app.Environment.IsDevelopment() ? "appsettings.Development.json" : "appsettings.json")
//                .Build();

// Configure the HTTP request pipeline.
app.UseGlobalExceptionHandlerMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

ConexaoDAO.URLCONEXAO = configuration.GetSection("ConnectionString")["DefaultConnection"];

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
