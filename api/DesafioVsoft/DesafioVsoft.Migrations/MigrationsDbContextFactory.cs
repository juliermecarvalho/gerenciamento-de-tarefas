using DesafioVsoft.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioVsoft.Migrations;



/// <summary>
/// Fábrica de contexto para geração de migrations
/// </summary>
public class MigrationsDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var temp = Path.GetTempPath();
        string migrationsPath = Path.Combine(temp, "DesafioVsoft.Migrations");
        Directory.CreateDirectory(migrationsPath);
        var database = Path.Combine(migrationsPath, "database.db");

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite($"Data Source={database}");

        return new AppDbContext(optionsBuilder.Options);
    }
}
