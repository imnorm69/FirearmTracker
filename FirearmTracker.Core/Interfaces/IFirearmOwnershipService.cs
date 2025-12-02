namespace FirearmTracker.Core.Interfaces
{
    public interface IFirearmOwnershipService
    {
        /// <summary>
        /// Determines if a firearm is currently sold based on transaction history.
        /// A firearm is sold if the most recent Sale activity is more recent than the most recent Purchase activity.
        /// </summary>
        Task<bool> IsFirearmSoldAsync(int firearmId);

        /// <summary>
        /// Gets the most recent purchase date for a firearm.
        /// </summary>
        Task<DateTime?> GetLatestPurchaseDateAsync(int firearmId);

        /// <summary>
        /// Gets the most recent purchase price for a firearm.
        /// </summary>
        Task<decimal?> GetLatestPurchasePriceAsync(int firearmId);

        /// <summary>
        /// Gets purchase information (date and price) from the most recent purchase activity.
        /// </summary>
        Task<(DateTime? Date, decimal? Price)> GetLatestPurchaseInfoAsync(int firearmId);
    }
}