using FirearmTracker.Core.Enums;
using FirearmTracker.Core.Interfaces;
using FirearmTracker.Core.Models;
using FirearmTracker.Core.Models.Reports;
using Microsoft.EntityFrameworkCore;

namespace FirearmTracker.Web.Services
{
    public class ReportService : IReportService
    {
        private readonly IFirearmRepository _firearmRepository;
        private readonly IActivityRepository _activityRepository;
        private readonly IAccessoryRepository _accessoryRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly FirearmOwnershipService _ownershipService;

        public ReportService(
            IFirearmRepository firearmRepository,
            IActivityRepository activityRepository,
            IAccessoryRepository accessoryRepository,
            IDocumentRepository documentRepository,
            FirearmOwnershipService ownershipService)
        {
            _firearmRepository = firearmRepository;
            _activityRepository = activityRepository;
            _accessoryRepository = accessoryRepository;
            _documentRepository = documentRepository;
            _ownershipService = ownershipService;
        }

        #region Configuration Methods

        public async Task<TabularReportConfiguration> GetFirearmInventoryConfigurationAsync()
        {
            var config = new TabularReportConfiguration
            {
                ReportType = ReportType.FirearmInventory,
                SupportsOnScreenView = true,
                SupportsPrint = true,
                SupportedExports = new List<ExportFormat> { ExportFormat.Pdf, ExportFormat.Excel, ExportFormat.Csv }
            };

            // Define available columns
            config.AvailableColumns = new List<string>
            {
                "Manufacturer",
                "Model",
                "Caliber",
                "SerialNumber",
                "FirearmType",
                "DatePurchased",
                "PurchasePrice",
                "Notes"
            };

            // Default selected columns
            config.SelectedColumns = new List<string>
            {
                "Manufacturer",
                "Model",
                "Caliber",
                "SerialNumber",
                "DatePurchased",
                "PurchasePrice"
            };

            return config;
        }

        public async Task<DocumentReportConfiguration> GetFirearmDetailConfigurationAsync()
        {
            var config = new DocumentReportConfiguration
            {
                ReportType = ReportType.FirearmDetail,
                SupportsOnScreenView = true,
                SupportsPrint = true,
                SupportedExports = new List<ExportFormat> { ExportFormat.Pdf },
                PageBreakBetweenRecords = true,
                PreventLineBreaks = true
            };

            // Define header fields
            config.HeaderFields = new List<string>
            {
                "Manufacturer",
                "Model",
                "Caliber",
                "SerialNumber",
                "DatePurchased",
                "PurchasePrice",
                "DateSold",
                "SoldPrice"
            };

            // Define available related tables
            config.AvailableRelatedTables = new List<string>
            {
                "Accessories",
                "ShootingSessions",
                "Transactions",
                "Maintenance",
                "Photos"
            };

            // Default: all tables selected
            config.SelectedRelatedTables = new List<string>(config.AvailableRelatedTables);

            return config;
        }

        #endregion

        #region Data Generation Methods

        public async Task<TabularReportResult> GenerateFirearmInventoryReportAsync(TabularReportConfiguration config)
        {
            // TODO: Implement actual data generation with filters and sorting
            var result = new TabularReportResult
            {
                ReportTitle = "Firearm Inventory Report",
                GeneratedDate = DateTime.UtcNow,
                ColumnHeaders = config.SelectedColumns
            };

            var firearms = await _firearmRepository.GetAllAsync();

            // Apply filters (stub - needs implementation)
            var filteredFirearms = ApplyFilters(firearms, config.Filters);

            // Apply sorting (stub - needs implementation)
            var sortedFirearms = ApplySorting(filteredFirearms, config.SortOptions);

            // Build rows based on selected columns
            foreach (var firearm in sortedFirearms)
            {
                var row = new Dictionary<string, object?>();
                
                foreach (var column in config.SelectedColumns)
                {
                    row[column] = column switch
                    {
                        "Manufacturer" => firearm.Manufacturer,
                        "Model" => firearm.Model,
                        "Caliber" => firearm.Caliber,
                        "SerialNumber" => firearm.SerialNumber,
                        "FirearmType" => firearm.FirearmType.ToString(),
                        "DatePurchased" => await _ownershipService.GetLatestPurchaseDateAsync(firearm.Id),
                        "PurchasePrice" => await _ownershipService.GetLatestPurchasePriceAsync(firearm.Id),
                        "Notes" => firearm.Notes,
                        _ => null
                    };
                }
                
                result.Rows.Add(row);
            }

            result.TotalRecords = result.Rows.Count;
            return result;
        }

        public async Task<DocumentReportResult> GenerateFirearmDetailReportAsync(DocumentReportConfiguration config)
        {
            // TODO: Implement actual data generation with filters
            var result = new DocumentReportResult
            {
                ReportTitle = "Firearm Detail Report",
                GeneratedDate = DateTime.UtcNow
            };

            var firearms = await _firearmRepository.GetAllAsync();

            // Apply filters (stub - needs implementation)
            var filteredFirearms = ApplyFilters(firearms, config.Filters);

            foreach (var firearm in filteredFirearms)
            {
                var record = new DocumentReportRecord();

                // Build header fields
                var purchaseInfo = await _ownershipService.GetLatestPurchaseInfoAsync(firearm.Id);
                
                record.HeaderFields = new Dictionary<string, object?>
                {
                    ["Manufacturer"] = firearm.Manufacturer,
                    ["Model"] = firearm.Model,
                    ["Caliber"] = firearm.Caliber,
                    ["SerialNumber"] = firearm.SerialNumber,
                    ["DatePurchased"] = purchaseInfo.Date,
                    ["PurchasePrice"] = purchaseInfo.Price,
                    ["DateSold"] = null, // TODO: Get from latest sale activity
                    ["SoldPrice"] = null // TODO: Get from latest sale activity
                };

                // Build related tables based on selected tables
                if (config.SelectedRelatedTables.Contains("Accessories"))
                {
                    var accessories = await _accessoryRepository.GetByFirearmIdAsync(firearm.Id);
                    var accessoryRows = accessories.Select(a => new Dictionary<string, object?>
                    {
                        ["Name"] = a.Name,
                        ["Type"] = a.Type,
                        ["Manufacturer"] = a.Manufacturer,
                        ["PurchaseDate"] = a.PurchaseDate,
                        ["PurchasePrice"] = a.PurchasePrice
                    }).ToList();
                    record.RelatedTables["Accessories"] = accessoryRows;
                }

                if (config.SelectedRelatedTables.Contains("Transactions"))
                {
                    var activities = await _activityRepository.GetAllForFirearmAsync(firearm.Id);
                    var purchaseAndSaleActivities = activities
                        .Where(a => !a.IsDeleted && (a.ActivityType == ActivityType.Purchase || a.ActivityType == ActivityType.Sale))
                        .Select(a => new Dictionary<string, object?>
                        {
                            ["ActivityType"] = a.ActivityType.ToString(),
                            ["Date"] = a.ActivityDate,
                            ["Description"] = a.Description,
                            ["Amount"] = a.Amount,
                            ["Notes"] = a.Notes
                        }).ToList();
                    record.RelatedTables["Transactions"] = purchaseAndSaleActivities;
                }

                // TODO: Implement other related tables (ShootingSessions, Maintenance, Photos)

                result.Records.Add(record);
            }

            result.TotalRecords = result.Records.Count;
            return result;
        }

        #endregion

        #region Export Methods

        public async Task<byte[]> ExportToPdfAsync(ReportResult report, ReportConfiguration config)
        {
            // TODO: Implement PDF export using a library like iTextSharp or QuestPDF
            throw new NotImplementedException("PDF export will be implemented in a future update");
        }

        public async Task<byte[]> ExportToExcelAsync(TabularReportResult report)
        {
            // TODO: Implement Excel export using a library like EPPlus or ClosedXML
            throw new NotImplementedException("Excel export will be implemented in a future update");
        }

        public async Task<byte[]> ExportToCsvAsync(TabularReportResult report)
        {
            // TODO: Implement CSV export
            throw new NotImplementedException("CSV export will be implemented in a future update");
        }

        #endregion

        #region Helper Methods

        public async Task<List<string>> GetDistinctManufacturersAsync()
        {
            var firearms = await _firearmRepository.GetAllAsync();
            return firearms
                .Where(f => !string.IsNullOrEmpty(f.Manufacturer))
                .Select(f => f.Manufacturer)
                .Distinct()
                .OrderBy(m => m)
                .ToList();
        }

        public async Task<List<string>> GetDistinctCalibersAsync()
        {
            var firearms = await _firearmRepository.GetAllAsync();
            return firearms
                .Where(f => !string.IsNullOrEmpty(f.Caliber))
                .Select(f => f.Caliber!)
                .Distinct()
                .OrderBy(c => c)
                .ToList();
        }

        #endregion

        #region Private Helper Methods

        private List<Firearm> ApplyFilters(List<Firearm> firearms, List<FilterOption> filters)
        {
            // TODO: Implement filtering logic
            // For now, just return all firearms
            return firearms.Where(f => !f.IsDeleted).ToList();
        }

        private List<Firearm> ApplySorting(List<Firearm> firearms, List<SortOption> sortOptions)
        {
            // TODO: Implement multi-field sorting with priority
            // For now, just return unsorted
            return firearms;
        }

        #endregion
    }
}
