using FirearmTracker.Core.Interfaces;

namespace FirearmTracker.Web.Services
{
    public class CaliberService : ICaliberService
    {
        private readonly List<string> _commonCalibers =
        [
            // Pistol Calibers
            ".22 LR",
            ".25 ACP",
            ".32 ACP",
            ".380 ACP",
            "9mm",
            ".38 Special",
            ".357 Magnum",
            ".40 S&W",
            "10mm",
            ".44 Magnum",
            ".45 ACP",
            ".45 Colt",
            ".50 AE",

            // Rifle Calibers
            ".17 HMR",
            ".22 WMR",
            ".223 Remington",
            "5.56x45mm NATO",
            ".243 Winchester",
            "6.5 Creedmoor",
            ".270 Winchester",
            ".30-30 Winchester",
            ".308 Winchester",
            "7.62x39mm",
            "7.62x51mm NATO",
            ".30-06 Springfield",
            ".300 Winchester Magnum",
            ".338 Lapua Magnum",
            ".50 BMG",

            // Shotgun Gauges
            "12 Gauge",
            "16 Gauge",
            "20 Gauge",
            "28 Gauge",
            ".410 Bore"
        ];

        private readonly HashSet<string> _customCalibers = [];

        public List<string> GetAllCalibers()
        {
            var allCalibers = _commonCalibers.Concat(_customCalibers).Distinct().OrderBy(c => c).ToList();
            return allCalibers;
        }

        public void AddCustomCaliber(string caliber)
        {
            if (!string.IsNullOrWhiteSpace(caliber) && !_commonCalibers.Contains(caliber, StringComparer.OrdinalIgnoreCase))
            {
                _customCalibers.Add(caliber);
            }
        }
    }
}