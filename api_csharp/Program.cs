using api_csharp.API.exceptionhandler;
using domain.DAO;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddGlobalExceptionHandlerMiddleware();
//builder.Services.AddProblemDetails(setup =>
//{
//    setup.Map<FaultException<BusinessFault>>((context, exception) =>
//    {
//        // resolve logger
//        var logger = context.RequestServices.GetRequiredService<ILogger<ProblemDetails>>();

//        // log exception to Seq
//        logger.LogError(exception, "{@Exception} occurred.", exception);

//        // return the problem details map   
//        return new ProblemDetails
//        {
//            Title = exception.Message,
//            Detail = exception.Detail.FaultMessage,
//            Status = exception.Detail.FaultType.ToHttpStatus(),
//            Type = exception.Detail.FaultType.ToString(),
//            Instance = exception.Detail.FaultReference
//        };
//    });
//});

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

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseGlobalExceptionHandlerMiddleware();
if (app.Environment.IsDevelopment())
{
    IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.Development.json")
                .Build();
    ConexaoDAO.URLCONEXAO = configuration.GetSection("ConnectionString")["DefaultConnection"];
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
    ConexaoDAO.URLCONEXAO = configuration.GetSection("ConnectionString")["DefaultConnection"];
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
