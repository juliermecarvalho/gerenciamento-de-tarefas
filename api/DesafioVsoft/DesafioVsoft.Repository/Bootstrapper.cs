using DesafioVsoft.Domain.Repositories;
using DesafioVsoft.Repository.Data;
using DesafioVsoft.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace DesafioVsoft.Repository;



/// <summary>
/// Classe responsável por registrar as dependências do repositório
/// </summary>
public static class Bootstrapper
{
    /// <summary>
    /// Registra todos os serviços relacionados a dados (DbContext e repositórios)
    /// </summary>
    /// <param name="services">Container de serviços</param>
    /// <param name="connectionString">String de conexão do banco SQLite</param>
    public static void AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        var temp = Path.GetTempPath();
        string migrationsPath = Path.Combine(temp, "DesafioVsoft.Migrations");
        Directory.CreateDirectory(migrationsPath);
        var database = Path.Combine(migrationsPath, "database.db");
        connectionString = $"Data Source={database}";
        // Configurar o contexto
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite(connectionString, b =>
            b.MigrationsAssembly("DesafioVsoft.Migrations"));

        services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite(connectionString, b => b.MigrationsAssembly("DesafioVsoft.Migrations")));


        // Registrar repositórios genéricos e específicos
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITaskRepository, TaskRepository>();
    }
}
