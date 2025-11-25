namespace FirearmTracker.Core.Models
{
    public class RepairVendorActivityData
    {
        public string VendorName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}