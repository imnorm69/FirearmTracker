namespace FirearmTracker.Core.Models.Reports
{
    /// <summary>
    /// Base class for report results
    /// </summary>
    public abstract class ReportResult
    {
        public string ReportTitle { get; set; } = string.Empty;
        public DateTime GeneratedDate { get; set; } = DateTime.UtcNow;
        public int TotalRecords { get; set; }
    }

    /// <summary>
    /// Result for tabular reports
    /// </summary>
    public class TabularReportResult : ReportResult
    {
        public List<string> ColumnHeaders { get; set; } = [];
        public List<Dictionary<string, object?>> Rows { get; set; } = [];
    }

    /// <summary>
    /// Result for document-style reports
    /// </summary>
    public class DocumentReportResult : ReportResult
    {
        public List<DocumentReportRecord> Records { get; set; } = [];
    }

    /// <summary>
    /// Represents a single record in a document report (e.g., one firearm)
    /// </summary>
    public class DocumentReportRecord
    {
        public Dictionary<string, object?> HeaderFields { get; set; } = [];
        public Dictionary<string, List<Dictionary<string, object?>>> RelatedTables { get; set; } = [];
    }

    /// <summary>
    /// Firearm Inventory Report specific data
    /// </summary>
    public class FirearmInventoryReportData
    {
        public int Id { get; set; }
        public string Manufacturer { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Caliber { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public DateTime? DatePurchased { get; set; }
        public decimal? PurchasePrice { get; set; }
        public string FirearmType { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }

    /// <summary>
    /// Firearm Detail Report specific data
    /// </summary>
    public class FirearmDetailReportData
    {
        public int Id { get; set; }
        public string Manufacturer { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Caliber { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public DateTime? DatePurchased { get; set; }
        public decimal? PurchasePrice { get; set; }
        public DateTime? DateSold { get; set; }
        public decimal? SoldPrice { get; set; }
        public string Notes { get; set; } = string.Empty;

        // Related data
        public List<AccessoryReportData> Accessories { get; set; } = [];

        public List<ActivityReportData> Transactions { get; set; } = [];
        public List<ActivityReportData> MaintenanceRecords { get; set; } = [];
        public List<ActivityReportData> ShootingSessions { get; set; } = [];
        public List<DocumentReportData> Photos { get; set; } = [];
    }

    public class AccessoryReportData
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public DateTime? PurchaseDate { get; set; }
        public decimal? PurchasePrice { get; set; }
    }

    public class ActivityReportData
    {
        public string ActivityType { get; set; } = string.Empty;
        public DateTime ActivityDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal? Amount { get; set; }
        public string Notes { get; set; } = string.Empty;
    }

    public class DocumentReportData
    {
        public string FileName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime UploadedDate { get; set; }
    }
}