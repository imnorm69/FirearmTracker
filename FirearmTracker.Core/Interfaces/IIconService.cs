using FirearmTracker.Core.Enums;

namespace FirearmTracker.Core.Interfaces
{
    public interface IIconService
    {
        /// <summary>
        /// Maps ActivityType to SVG filename (without .svg extension)
        /// </summary>
        string GetActivityIcon(ActivityType activityType);

        /// <summary>
        /// Maps AmmunitionTransactionType to SVG filename
        /// </summary>
        string GetAmmunitionTransactionIcon(AmmunitionTransactionType transactionType);

        /// <summary>
        /// Gets tab icon filename based on tab name
        /// </summary>
        string GetTabIcon(string tabName);
    }
}