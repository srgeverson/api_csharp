using api_csharp.API.exceptionhandler;
using domain.DAO;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddGlobalExceptionHandlerMiddleware();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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
