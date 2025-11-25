namespace FirearmTracker.Core.Models
{
    public class TransferActivityData
    {
        public string TransferType { get; set; } = string.Empty; // "Gift", "Inheritance", "Trade", "Other"
        public string FromWhom { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}