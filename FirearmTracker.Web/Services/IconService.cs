using FirearmTracker.Core.Enums;
using FirearmTracker.Core.Interfaces;

namespace FirearmTracker.Web.Services
{
    public class IconService : IIconService
    {
        // Maps ActivityType to SVG filename (without .svg extension)
        public string GetActivityIcon(ActivityType activityType)
        {
            return activityType switch
            {
                ActivityType.Purchase => "purchase",
                ActivityType.Sale => "sale",
                ActivityType.RepairDIY => "repair-diy",
                ActivityType.RepairVendor => "repair-professional",
                ActivityType.AppraisalSelf => "appraisal-self",
                ActivityType.AppraisalProfessional => "appraisal-professional",
                ActivityType.RangeSession => "range-session",
                ActivityType.Modification => "modification",
                ActivityType.Insurance => "insurance",
                ActivityType.Transfer => "transfer",
                ActivityType.Malfunction => "malfunction",
                _ => "activity-generic"
            };
        }

        // Maps AmmunitionTransactionType to SVG filename
        public string GetAmmunitionTransactionIcon(AmmunitionTransactionType transactionType)
        {
            return transactionType switch
            {
                AmmunitionTransactionType.Purchase => "purchase",
                AmmunitionTransactionType.ManualConsumption => "consumption",
                AmmunitionTransactionType.Sale => "sale",
                AmmunitionTransactionType.RangeSession => "range-session",
                _ => "activity-generic"
            };
        }

        // Tab icons
        public string GetTabIcon(string tabName)
        {
            return tabName.ToLower() switch
            {
                "transactions" => "transactions",
                "maintenance" => "maintenance",
                "valuations" => "valuations",
                "usage" => "usage",
                "modifications" => "modifications",
                "documents" => "documents",
                "accessories" => "accessories",
                "ammunition" => "ammunition",
                "firearms" => "firearms",
                _ => "activity-generic"
            };
        }
    }
}