namespace FirearmTracker.Core.Models
{
    public class MalfunctionActivityData
    {
        public string MalfunctionType { get; set; } = string.Empty;
        public int? RoundsFiredBefore { get; set; }
        public string? AmmunitionType { get; set; }
        public string? WeatherConditions { get; set; }
        public string? ResolutionAction { get; set; }
    }
}