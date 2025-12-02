using FirearmTracker.Core.Enums;

namespace FirearmTracker.Core.Models.Reports
{
    /// <summary>
    /// Base class for all report configurations
    /// </summary>
    public abstract class ReportConfiguration
    {
        public ReportType ReportType { get; set; }
        public List<SortOption> SortOptions { get; set; } = [];
        public List<FilterOption> Filters { get; set; } = [];
        public List<ExportFormat> SupportedExports { get; set; } = [];
        public bool SupportsOnScreenView { get; set; } = true;
        public bool SupportsPrint { get; set; } = true;
    }

    /// <summary>
    /// Configuration for tabular reports (like Firearm Inventory)
    /// </summary>
    public class TabularReportConfiguration : ReportConfiguration
    {
        public List<string> AvailableColumns { get; set; } = [];
        public List<string> SelectedColumns { get; set; } = [];
        public int PageSize { get; set; } = 50;
    }

    /// <summary>
    /// Configuration for document-style reports (like Firearm Detail)
    /// </summary>
    public class DocumentReportConfiguration : ReportConfiguration
    {
        public List<string> HeaderFields { get; set; } = [];
        public List<string> AvailableRelatedTables { get; set; } = [];
        public List<string> SelectedRelatedTables { get; set; } = [];
        public bool PageBreakBetweenRecords { get; set; } = true;
        public bool PreventLineBreaks { get; set; } = true;
    }

    /// <summary>
    /// Represents a sort option for reports
    /// </summary>
    public class SortOption
    {
        public string FieldName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public bool IsDescending { get; set; }
        public int Priority { get; set; }  // Higher number = higher priority (applied last)
    }

    /// <summary>
    /// Represents a filter option for reports
    /// </summary>
    public class FilterOption
    {
        public string FieldName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public FilterOperator Operator { get; set; }
        public object? Value { get; set; }
        public object? Value2 { get; set; } // For "Between" operations
    }

    /// <summary>
    /// Represents a date range filter
    /// </summary>
    public class DateFilter : FilterOption
    {
        public DateFilterType DateFilterType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    /// <summary>
    /// Represents a multi-select filter (like Manufacturer, Caliber)
    /// </summary>
    public class MultiSelectFilter : FilterOption
    {
        public List<string> AvailableValues { get; set; } = [];
        public List<string> SelectedValues { get; set; } = [];
    }
}