using FirearmTracker.Core.Interfaces;
using FirearmTracker.Core.Models;
using Npgsql;
using System.Text.Json;

namespace FirearmTracker.Web.Services
{
    public class DatabaseConfigurationService : IDatabaseConfigurationService
    {
        private readonly string _configFilePath;
        private readonly ILogger<DatabaseConfigurationService> _logger;

        public DatabaseConfigurationService(
            IWebHostEnvironment environment,
            ILogger<DatabaseConfigurationService> logger)
        {
            _configFilePath = Path.Combine(environment.ContentRootPath, "dbconfig.json");
            _logger = logger;
        }

        public async Task<DatabaseConfiguration?> LoadConfigurationAsync()
        {
            try
            {
                if (!File.Exists(_configFilePath))
                {
                    _logger.LogInformation("Database configuration file not found at {Path}", _configFilePath);
                    return null;
                }

                var json = await File.ReadAllTextAsync(_configFilePath);
                var config = JsonSerializer.Deserialize<DatabaseConfiguration>(json);
                _logger.LogInformation("Loaded database configuration: {Type}", config?.DatabaseType);
                return config;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading database configuration");
                return null;
            }
        }

        public async Task SaveConfigurationAsync(DatabaseConfiguration configuration)
        {
            try
            {
                var json = JsonSerializer.Serialize(configuration, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                await File.WriteAllTextAsync(_configFilePath, json);
                _logger.LogInformation("Saved database configuration: {Type}", configuration.DatabaseType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving database configuration");
                throw;
            }
        }

        public Task<bool> ConfigurationExistsAsync()
        {
            return Task.FromResult(File.Exists(_configFilePath));
        }

        public async Task<bool> TestPostgresConnectionAsync(PostgresConfiguration config)
        {
            try
            {
                var connectionString = config.GetConnectionString();
                await using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();
                _logger.LogInformation("PostgreSQL connection test successful");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "PostgreSQL connection test failed");
                return false;
            }
        }
    }
}