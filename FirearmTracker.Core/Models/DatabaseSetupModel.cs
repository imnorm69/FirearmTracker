using FirearmTracker.Core.Enums;

namespace FirearmTracker.Core.Models
{
    public class DatabaseSetupModel
    {
        public DatabaseType DatabaseType { get; set; } = DatabaseType.Sqlite;
        public string SqliteFilePath { get; set; } = "firearmtracker.db";
        public string PostgresHost { get; set; } = "localhost";
        public int PostgresPort { get; set; } = 5432;
        public string PostgresDatabase { get; set; } = "firearmtracker";
        public string PostgresUsername { get; set; } = string.Empty;
        public string PostgresPassword { get; set; } = string.Empty;
    }
}