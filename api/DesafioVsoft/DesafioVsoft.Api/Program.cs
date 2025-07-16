using DesafioVsoft.Api.Extensions;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Libera CORS para qualquer origem (para desenvolvimento)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Adiciona o appsettings.Container.json se o ambiente for "Container"
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();


// Registra serviços
builder.Services.AddApiServices(builder.Configuration);


// Adiciona o appsettings.Container.json se o ambiente for "Container"




var app = builder.Build();

// Configura o pipeline e banco
app.UseApiConfiguration();
app.UseCors("AllowAll");
app.Run();
