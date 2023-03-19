using FluentMigrator.Runner;
using FluentMigrator.Runner.VersionTableInfo;
using Microsoft.Extensions.DependencyInjection;

namespace FamilyAssistant.Migrator;

public class MigrationRunner
{
    private readonly string _connectionString;

    public MigrationRunner(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void MigrateDatabase()
    {
        using (var serviceProvider = CreateServices())
        using (var scope = serviceProvider.CreateScope())
        {
            UpdateDatabase(serviceProvider);
        }
    }

    /// <summary>
    /// Configure the dependency injection services
    /// </summary>
    private ServiceProvider CreateServices()
    {
        return new ServiceCollection()
            // Add common FluentMigrator services
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                // Add SQLite support to FluentMigrator
                .AddPostgres()
                // Set the connection string
                .WithGlobalConnectionString(_connectionString)
                // Define the assembly containing the migrations
                .ScanIn(typeof(MigrationRunner).Assembly).For.Migrations())
            // Enable logging to console in the FluentMigrator way
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .AddScoped<IVersionTableMetaData, VersionTable>()
            // Build the service provider
            .BuildServiceProvider(false);
    }

    /// <summary>
    /// Update the database
    /// </summary>
    private static void UpdateDatabase(IServiceProvider serviceProvider)
    {
        // Instantiate the runner
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        // Execute the migrations
        runner.MigrateUp();
    }
}
