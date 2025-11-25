using FirearmTracker.Core.Models;

namespace FirearmTracker.Core.Interfaces
{
    public interface IAccessoryRepository
    {
        Task<List<Accessory>> GetAllAsync();

        Task<Accessory?> GetByIdAsync(int id);

        Task<List<Accessory>> GetByFirearmIdAsync(int firearmId);

        Task<Accessory> AddAsync(Accessory accessory);

        Task UpdateAsync(Accessory accessory);
    }
}