using FirearmTracker.Core.Models;

namespace FirearmTracker.Core.Interfaces
{
    public interface IDatabaseConfigurationService
    {
        Task<DatabaseConfiguration?> LoadConfigurationAsync();

        Task SaveConfigurationAsync(DatabaseConfiguration configuration);

        Task<bool> ConfigurationExistsAsync();

        Task<bool> TestPostgresConnectionAsync(PostgresConfiguration config);
    }
}