using FirearmTracker.Core.Models;

namespace FirearmTracker.Core.Interfaces
{
    public interface IAmmunitionTransactionRepository
    {
        Task<List<AmmunitionTransaction>> GetAllAsync();

        Task<AmmunitionTransaction?> GetByIdAsync(int id);

        Task<List<AmmunitionTransaction>> GetByAmmunitionIdAsync(int ammunitionId);

        Task<List<AmmunitionTransaction>> GetByActivityIdAsync(int activityId);

        Task<AmmunitionTransaction> AddAsync(AmmunitionTransaction transaction);

        Task UpdateAsync(AmmunitionTransaction transaction);
    }
}