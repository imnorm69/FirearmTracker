using FirearmTracker.Core.Interfaces;
using FirearmTracker.Core.Models;
using FirearmTracker.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FirearmTracker.Data.Repositories
{
    public class AmmunitionRepository(FirearmTrackerContext context) : IAmmunitionRepository
    {
        private readonly FirearmTrackerContext _context = context;

        public async Task<List<Ammunition>> GetAllAsync()
        {
            return await _context.Ammunition
                .Include(a => a.Transactions)
                .IgnoreQueryFilters()
                .ToListAsync();
        }

        public async Task<Ammunition?> GetByIdAsync(int id)
        {
            return await _context.Ammunition
                .Include(a => a.Transactions)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Ammunition>> GetByCaliberAsync(string caliber)
        {
            return await _context.Ammunition
                .Include(a => a.Transactions)
                .Where(a => a.Caliber == caliber)
                .ToListAsync();
        }

        public async Task<Ammunition> AddAsync(Ammunition ammunition)
        {
            _context.Ammunition.Add(ammunition);
            await _context.SaveChangesAsync();
            return ammunition;
        }

        public async Task UpdateAsync(Ammunition ammunition)
        {
            _context.Ammunition.Update(ammunition);
            await _context.SaveChangesAsync();
        }

        public async Task<List<string>> GetDistinctStorageLocationsAsync()
        {
            return await _context.Ammunition
                .Where(a => !string.IsNullOrEmpty(a.StorageLocation))
                .Select(a => a.StorageLocation!)
                .Distinct()
                .OrderBy(l => l)
                .ToListAsync();
        }
    }
}