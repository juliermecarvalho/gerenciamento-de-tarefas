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

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite("Data Source=../DesafioVsoft.Api/Data/database.db");

        return new AppDbContext(optionsBuilder.Options);
    }
}
