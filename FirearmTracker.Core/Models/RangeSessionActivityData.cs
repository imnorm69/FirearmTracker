namespace FirearmTracker.Core.Models
{
    public class RangeSessionActivityData
    {
        public int? RoundsFired { get; set; }
        public string? LocationName { get; set; }
        public string? AmmunitionType { get; set; }
        public string? PerformanceNotes { get; set; }
        public bool CleanedAfterSession { get; set; }
    }
}