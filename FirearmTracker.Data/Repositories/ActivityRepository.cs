using FirearmTracker.Core.Interfaces;
using FirearmTracker.Core.Models;
using FirearmTracker.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FirearmTracker.Data.Repositories
{
    public class ActivityRepository(FirearmTrackerContext context) : IActivityRepository
    {
        private readonly FirearmTrackerContext _context = context;

        public async Task<List<Activity>> GetAllForFirearmAsync(int firearmId)
        {
            return await _context.Activities
                .Where(a => a.FirearmId == firearmId)
                .OrderByDescending(a => a.ActivityDate)
                .ToListAsync();
        }

        public async Task<Activity?> GetByIdAsync(int id)
        {
            return await _context.Activities.FindAsync(id);
        }

        public async Task<Activity> AddAsync(Activity activity)
        {
            activity.CreatedDate = DateTime.UtcNow;
            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();
            return activity;
        }

        public async Task<Activity> UpdateAsync(Activity activity)
        {
            activity.ModifiedDate = DateTime.UtcNow;
            _context.Activities.Update(activity);
            await _context.SaveChangesAsync();
            return activity;
        }

        public async Task DeleteAsync(int id)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity != null)
            {
                activity.IsDeleted = true;
                activity.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}