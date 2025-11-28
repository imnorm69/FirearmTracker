namespace FirearmTracker.Core.Models
{
    public class PostgresConfiguration
    {
        public string Host { get; set; } = "localhost";
        public int Port { get; set; } = 5432;
        public string Database { get; set; } = "firearmtracker";
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string GetConnectionString()
        {
            return $"Host={Host};Port={Port};Database={Database};Username={Username};Password={Password}";
        }
    }
}