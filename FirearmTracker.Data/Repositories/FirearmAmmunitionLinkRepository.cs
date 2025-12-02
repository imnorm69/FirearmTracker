using FirearmTracker.Core.Interfaces;
using FirearmTracker.Core.Models;
using FirearmTracker.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FirearmTracker.Data.Repositories
{
    public class FirearmAmmunitionLinkRepository(FirearmTrackerContext context) : IFirearmAmmunitionLinkRepository
    {
        private readonly FirearmTrackerContext _context = context;

        public async Task<List<FirearmAmmunitionLink>> GetAllAsync()
        {
            return await _context.FirearmAmmunitionLinks
                .Include(l => l.Firearm)
                .Include(l => l.Ammunition)
                .IgnoreQueryFilters()
                .ToListAsync();
        }

        public async Task<FirearmAmmunitionLink?> GetByIdAsync(int id)
        {
            return await _context.FirearmAmmunitionLinks
                .Include(l => l.Firearm)
                .Include(l => l.Ammunition)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<List<FirearmAmmunitionLink>> GetByFirearmIdAsync(int firearmId)
        {
            return await _context.FirearmAmmunitionLinks
                .Include(l => l.Ammunition)
                .ThenInclude(a => a.Transactions)
                .Where(l => l.FirearmId == firearmId)
                .ToListAsync();
        }

        public async Task<List<FirearmAmmunitionLink>> GetByAmmunitionIdAsync(int ammunitionId)
        {
            return await _context.FirearmAmmunitionLinks
                .Include(l => l.Firearm)
                .Where(l => l.AmmunitionId == ammunitionId)
                .ToListAsync();
        }

        public async Task<FirearmAmmunitionLink?> GetLinkAsync(int firearmId, int ammunitionId)
        {
            return await _context.FirearmAmmunitionLinks
                .FirstOrDefaultAsync(l => l.FirearmId == firearmId && l.AmmunitionId == ammunitionId && !l.IsDeleted);
        }

        public async Task<FirearmAmmunitionLink> AddAsync(FirearmAmmunitionLink link)
        {
            _context.FirearmAmmunitionLinks.Add(link);
            await _context.SaveChangesAsync();
            return link;
        }

        public async Task DeleteAsync(int id)
        {
            var link = await GetByIdAsync(id);
            if (link != null)
            {
                link.IsDeleted = true;
                _context.FirearmAmmunitionLinks.Update(link);
                await _context.SaveChangesAsync();
            }
        }
    }
}