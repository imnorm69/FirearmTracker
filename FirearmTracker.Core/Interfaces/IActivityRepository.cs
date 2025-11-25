using FirearmTracker.Core.Models;

namespace FirearmTracker.Core.Interfaces
{
    public interface IActivityRepository
    {
        Task<List<Activity>> GetAllForFirearmAsync(int firearmId);

        Task<Activity?> GetByIdAsync(int id);

        Task<Activity> AddAsync(Activity activity);

        Task<Activity> UpdateAsync(Activity activity);

        Task DeleteAsync(int id);
    }
}