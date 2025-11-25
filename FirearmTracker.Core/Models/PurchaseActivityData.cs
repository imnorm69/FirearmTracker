namespace FirearmTracker.Core.Models
{
    public class PurchaseActivityData
    {
        public string PurchasedFromType { get; set; } = string.Empty; // "Dealer" or "Private Party"
        public string PurchasedFromName { get; set; } = string.Empty;
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}