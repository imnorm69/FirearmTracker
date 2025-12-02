using FirearmTracker.Core.Enums;

namespace FirearmTracker.Core.Models
{
    public class Firearm
    {
        public int Id { get; set; }
        public string Manufacturer { get; set; } = string.Empty;
        public FirearmType FirearmType { get; set; }
        public string Model { get; set; } = string.Empty;
        public string? Caliber { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
    }
}