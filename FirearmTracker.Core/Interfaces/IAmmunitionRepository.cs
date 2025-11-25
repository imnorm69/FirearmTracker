using FirearmTracker.Core.Models;

namespace FirearmTracker.Core.Interfaces
{
    public interface IAmmunitionRepository
    {
        Task<List<Ammunition>> GetAllAsync();

        Task<Ammunition?> GetByIdAsync(int id);

        Task<List<Ammunition>> GetByCaliberAsync(string caliber);

        Task<Ammunition> AddAsync(Ammunition ammunition);

        Task UpdateAsync(Ammunition ammunition);

        Task<List<string>> GetDistinctStorageLocationsAsync();
    }
}