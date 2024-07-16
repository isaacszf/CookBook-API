using Dapper;
using Infra.DataAccess;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace Infra.DbMigrations;

public static class DatabaseMigration
{
    public static void MigrateDatabase(string connection)
    {
        var connectionBuilder = new MySqlConnectionStringBuilder(connection);
        var dbName = connectionBuilder.Database;
        
        // "Removendo" propriedade Database para que não de erro ao conectar
        connectionBuilder.Remove("Database");
        
        using var dbConnection = new MySqlConnection(connectionBuilder.ConnectionString);
        
        var parameters = new DynamicParameters();
        parameters.Add("name", dbName);
        
        var records = 
            dbConnection.Query(
                "SELECT * FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @name", 
                parameters);

        // Se db não existir, crie ele
        if (!records.Any()) dbConnection.Execute($"CREATE DATABASE {dbName}");
    }

    public static void RunMigrations(string connection)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CookBookDbContext>();
        optionsBuilder.UseMySql(connection, new MariaDbServerVersion(new Version(11, 4, 2)));
        
        using var context = new CookBookDbContext(optionsBuilder.Options);
        context.Database.Migrate();
    }
}