using FirearmTracker.Core.Enums;
using FirearmTracker.Core.Interfaces;

namespace FirearmTracker.Web.Services
{
    public class FirearmOwnershipService(IActivityRepository activityRepository) : IFirearmOwnershipService
    {
        private readonly IActivityRepository _activityRepository = activityRepository;

        /// <summary>
        /// Determines if a firearm is currently sold based on transaction history.
        /// A firearm is sold if the most recent Sale activity is more recent than the most recent Purchase activity.
        /// </summary>
        public async Task<bool> IsFirearmSoldAsync(int firearmId)
        {
            var activities = await _activityRepository.GetAllForFirearmAsync(firearmId);

            // Filter to only Purchase and Sale activities, excluding deleted ones
            var transactions = activities
                .Where(a => !a.IsDeleted && (a.ActivityType == ActivityType.Purchase || a.ActivityType == ActivityType.Sale))
                .OrderByDescending(a => a.ActivityDate)
                .ToList();

            if (transactions.Count == 0)
            {
                // No transactions means not sold (initial collection item)
                return false;
            }

            // Check the most recent transaction
            var mostRecentTransaction = transactions.First();
            return mostRecentTransaction.ActivityType == ActivityType.Sale;
        }

        /// <summary>
        /// Gets the most recent purchase date for a firearm.
        /// </summary>
        public async Task<DateTime?> GetLatestPurchaseDateAsync(int firearmId)
        {
            var activities = await _activityRepository.GetAllForFirearmAsync(firearmId);

            var latestPurchase = activities
                .Where(a => !a.IsDeleted && a.ActivityType == ActivityType.Purchase)
                .OrderByDescending(a => a.ActivityDate)
                .FirstOrDefault();

            return latestPurchase?.ActivityDate;
        }

        /// <summary>
        /// Gets the most recent purchase price for a firearm.
        /// </summary>
        public async Task<decimal?> GetLatestPurchasePriceAsync(int firearmId)
        {
            var activities = await _activityRepository.GetAllForFirearmAsync(firearmId);

            var latestPurchase = activities
                .Where(a => !a.IsDeleted && a.ActivityType == ActivityType.Purchase)
                .OrderByDescending(a => a.ActivityDate)
                .FirstOrDefault();

            return latestPurchase?.Amount;
        }

        /// <summary>
        /// Gets purchase information (date and price) from the most recent purchase activity.
        /// </summary>
        public async Task<(DateTime? Date, decimal? Price)> GetLatestPurchaseInfoAsync(int firearmId)
        {
            var activities = await _activityRepository.GetAllForFirearmAsync(firearmId);

            var latestPurchase = activities
                .Where(a => !a.IsDeleted && a.ActivityType == ActivityType.Purchase)
                .OrderByDescending(a => a.ActivityDate)
                .FirstOrDefault();

            if (latestPurchase == null)
            {
                return (null, null);
            }

            return (latestPurchase.ActivityDate, latestPurchase.Amount);
        }
    }
}