namespace FirearmTracker.Core.Models
{
    public class ModificationActivityData
    {
        public string AccessoryPartName { get; set; } = string.Empty;
        public string InstalledBy { get; set; } = string.Empty; // "Self" or "Professional"
        public string? VendorInstallerName { get; set; }
        public string? Description { get; set; }
    }
}