using FirearmTracker.Core.Enums;

namespace FirearmTracker.Core.Models
{
    public class DatabaseConfiguration
    {
        public DatabaseType DatabaseType { get; set; } = DatabaseType.Sqlite;
        public string? SqliteFilePath { get; set; }
        public PostgresConfiguration? PostgresConfig { get; set; }

        public string GetConnectionString()
        {
            return DatabaseType switch
            {
                DatabaseType.Sqlite => $"Data Source={SqliteFilePath ?? "firearmtracker.db"}",
                DatabaseType.Postgres => PostgresConfig?.GetConnectionString()
                    ?? throw new InvalidOperationException("PostgreSQL configuration is missing"),
                _ => throw new NotSupportedException($"Database type {DatabaseType} is not supported")
            };
        }
    }
}