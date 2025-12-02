using FirearmTracker.Core.Interfaces;
using FirearmTracker.Core.Models;
using FirearmTracker.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FirearmTracker.Data.Repositories
{
    public class FirearmRepository(FirearmTrackerContext context) : IFirearmRepository
    {
        private readonly FirearmTrackerContext _context = context;

        public async Task<List<Firearm>> GetAllAsync()
        {
            return await _context.Firearms
                .IgnoreQueryFilters()  // This bypasses the IsDeleted filter
                .ToListAsync();
        }

        public async Task<Firearm?> GetByIdAsync(int id)
        {
            return await _context.Firearms
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Firearm> AddAsync(Firearm firearm)
        {
            firearm.CreatedDate = DateTime.UtcNow;
            _context.Firearms.Add(firearm);
            await _context.SaveChangesAsync();
            return firearm;
        }

        public async Task<Firearm> UpdateAsync(Firearm firearm)
        {
            firearm.ModifiedDate = DateTime.UtcNow;
            _context.Firearms.Update(firearm);
            await _context.SaveChangesAsync();
            return firearm;
        }

        public async Task DeleteAsync(int id)
        {
            var firearm = await _context.Firearms.FindAsync(id);
            if (firearm != null)
            {
                firearm.IsDeleted = true;
                firearm.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}