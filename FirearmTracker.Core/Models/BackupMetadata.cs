using FirearmTracker.Core.Enums;

namespace FirearmTracker.Core.Models
{
    public class BackupMetadata
    {
        public string Version { get; set; } = "0.6.0";
        public DateTime CreatedDate { get; set; }
        public DatabaseType DatabaseType { get; set; }
        public int FirearmCount { get; set; }
        public int ActivityCount { get; set; }
        public int AccessoryCount { get; set; }
        public int AmmunitionCount { get; set; }
        public int DocumentCount { get; set; }
        public int UserCount { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }
}