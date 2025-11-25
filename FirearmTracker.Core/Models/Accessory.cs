namespace FirearmTracker.Core.Models
{
    public class Accessory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // Scope, Holster, Magazine, etc.
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public string? SerialNumber { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public decimal? PurchasePrice { get; set; }
        public string? Notes { get; set; }
        public AccessoryStatus Status { get; set; } = AccessoryStatus.Available;

        // Optional link to a firearm
        public int? LinkedFirearmId { get; set; }

        public Firearm? LinkedFirearm { get; set; }

        public DateTime? SaleDate { get; set; }
        public decimal? SalePrice { get; set; }
        public bool IsDeleted => SaleDate.HasValue;
    }

    public enum AccessoryStatus
    {
        Available,
        LinkedToFirearm,
        Sold
    }
}