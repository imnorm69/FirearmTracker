using FirearmTracker.Core.Models;

namespace FirearmTracker.Core.Interfaces
{
    public interface IFirearmRepository
    {
        Task<List<Firearm>> GetAllAsync();

        Task<Firearm?> GetByIdAsync(int id);

        Task<Firearm> AddAsync(Firearm firearm);

        Task<Firearm> UpdateAsync(Firearm firearm);

        Task DeleteAsync(int id);
    }
}