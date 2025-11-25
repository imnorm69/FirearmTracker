using FirearmTracker.Core.Models;

namespace FirearmTracker.Core.Interfaces
{
    public interface IFirearmAmmunitionLinkRepository
    {
        Task<List<FirearmAmmunitionLink>> GetAllAsync();
        Task<FirearmAmmunitionLink?> GetByIdAsync(int id);
        Task<List<FirearmAmmunitionLink>> GetByFirearmIdAsync(int firearmId);
        Task<List<FirearmAmmunitionLink>> GetByAmmunitionIdAsync(int ammunitionId);
        Task<FirearmAmmunitionLink?> GetLinkAsync(int firearmId, int ammunitionId);
        Task<FirearmAmmunitionLink> AddAsync(FirearmAmmunitionLink link);
        Task DeleteAsync(int id);
    }
}
