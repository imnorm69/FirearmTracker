using FirearmTracker.Core.Enums;
using FirearmTracker.Core.Models.Reports;

namespace FirearmTracker.Core.Interfaces
{
    public interface IReportService
    {
        // Configuration methods
        Task<TabularReportConfiguration> GetFirearmInventoryConfigurationAsync();
        Task<DocumentReportConfiguration> GetFirearmDetailConfigurationAsync();
        
        // Data generation methods
        Task<TabularReportResult> GenerateFirearmInventoryReportAsync(TabularReportConfiguration config);
        Task<DocumentReportResult> GenerateFirearmDetailReportAsync(DocumentReportConfiguration config);
        
        // Export methods
        Task<byte[]> ExportToPdfAsync(ReportResult report, ReportConfiguration config);
        Task<byte[]> ExportToExcelAsync(TabularReportResult report);
        Task<byte[]> ExportToCsvAsync(TabularReportResult report);
        
        // Helper methods
        Task<List<string>> GetDistinctManufacturersAsync();
        Task<List<string>> GetDistinctCalibersAsync();
    }
}
