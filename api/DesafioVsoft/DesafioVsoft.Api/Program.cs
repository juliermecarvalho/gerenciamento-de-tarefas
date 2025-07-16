using DesafioVsoft.Api.Extensions;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);



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

app.Run();
