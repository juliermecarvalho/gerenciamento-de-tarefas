using DesafioVsoft.Api.Validators;
using DesafioVsoft.Repository;
using DesafioVsoft.Repository.Data;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace DesafioVsoft.Api.Extensions;

/// <summary>
/// Classe de extensão para registrar configurações da aplicação
/// </summary>
public static class ApiBootstrapper
{
    /// <summary>
    /// Registra serviços principais da aplicação
    /// </summary>
    public static void AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Controllers e validação Fluent
        services.AddControllers();

        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssemblyContaining<UserInputDtoValidator>();

        // Banco de dados SQLite
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddInfrastructure(connectionString);

        // Swagger com versionamento
        services.AddEndpointsApiExplorer();

        services.AddApiVersioning(opt =>
        {
            opt.DefaultApiVersion = new ApiVersion(1, 0);
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.ReportApiVersions = true;
            opt.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("x-api-version"),
                new MediaTypeApiVersionReader("x-api-version"));
        });

        services.AddVersionedApiExplorer(opt =>
        {
            opt.GroupNameFormat = "'v'VVV";
            opt.SubstituteApiVersionInUrl = true;
        });

        services.AddSwaggerGen();
        services.ConfigureOptions<ConfigureSwaggerOptions>();

    }

    /// <summary>
    /// Configura Swagger e criação do banco
    /// </summary>
    public static void UseApiConfiguration(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var desc in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", $"API {desc.GroupName.ToUpperInvariant()}");
                }
            });
        }

        //// Garante que o banco existe
        //using (var scope = app.Services.CreateScope())
        //{
        //    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        //    db.Database.EnsureCreated();
        //}

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
    }
}
