namespace FirearmTracker.Core.Models
{
    public class InsuranceActivityData
    {
        public string? PolicyNumber { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string InsuranceCompany { get; set; } = string.Empty;
        public string? CompanyContact { get; set; }
    }
}