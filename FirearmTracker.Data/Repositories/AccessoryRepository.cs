using FirearmTracker.Core.Interfaces;
using FirearmTracker.Core.Models;
using FirearmTracker.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FirearmTracker.Data.Repositories
{
    public class AccessoryRepository : IAccessoryRepository
    {
        private readonly FirearmTrackerContext _context;

        public AccessoryRepository(FirearmTrackerContext context)
        {
            _context = context;
        }

        public async Task<List<Accessory>> GetAllAsync()
        {
            return await _context.Accessories
                .Include(a => a.LinkedFirearm)
                .IgnoreQueryFilters()
                .ToListAsync();
        }

        public async Task<Accessory?> GetByIdAsync(int id)
        {
            return await _context.Accessories
                .Include(a => a.LinkedFirearm)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Accessory>> GetByFirearmIdAsync(int firearmId)
        {
            var accessories = await _context.Accessories
                .Where(a => a.LinkedFirearmId == firearmId)
                .ToListAsync();

            return accessories.Where(a => !a.IsDeleted).ToList();
        }

        public async Task<Accessory> AddAsync(Accessory accessory)
        {
            _context.Accessories.Add(accessory);
            await _context.SaveChangesAsync();
            return accessory;
        }

        public async Task UpdateAsync(Accessory accessory)
        {
            _context.Accessories.Update(accessory);
            await _context.SaveChangesAsync();
        }
    }
}