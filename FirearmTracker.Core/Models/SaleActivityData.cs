namespace FirearmTracker.Core.Models
{
    public class SaleActivityData
    {
        public string SoldToType { get; set; } = string.Empty; // "Dealer" or "Private Party"
        public string SoldToName { get; set; } = string.Empty;
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}