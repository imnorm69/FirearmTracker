namespace FirearmTracker.Core.Enums
{
    public enum ReportType
    {
        FirearmInventory,
        FirearmDetail,
        AmmunitionInventory,
        ShootingSessionHistory,
        MaintenanceHistory,
        ValuationHistory
    }

    public enum ExportFormat
    {
        Pdf,
        Excel,
        Csv
    }

    public enum FilterOperator
    {
        Equals,
        NotEquals,
        Contains,
        StartsWith,
        EndsWith,
        GreaterThan,
        LessThan,
        Between,
        In
    }

    public enum DateFilterType
    {
        Before,
        After,
        Between,
        Equals
    }
}