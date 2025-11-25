using FirearmTracker.Core.Enums;

namespace FirearmTracker.Core.Models
{
    public class Activity
    {
        public int Id { get; set; }
        public int FirearmId { get; set; }
        public ActivityType ActivityType { get; set; }
        public DateTime ActivityDate { get; set; }
        public decimal? Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;

        // JSON field for type-specific data (flexible storage)
        public string AdditionalData { get; set; } = string.Empty;

        // Audit fields
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; } = false;

        // Navigation property
        public Firearm? Firearm { get; set; }
    }
}