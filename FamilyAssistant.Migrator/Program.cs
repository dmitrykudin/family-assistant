using Microsoft.Extensions.Configuration;

namespace FamilyAssistant.Migrator;

public static class Program
{
    public static void Main(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var appSettingsName = string.IsNullOrEmpty(environment)
            ? "appsettings.json"
            : $"appsettings.{environment}.json";

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(appSettingsName)
            .Build();

        var connectionString = configuration["DbSettings:ConnectionString"];
        var migrationRunner = new MigrationRunner(connectionString);

        migrationRunner.MigrateDatabase();
    }
}

