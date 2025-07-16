using DesafioVsoft.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Registra serviços
builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

// Configura o pipeline e banco
app.UseApiConfiguration();

app.Run();
